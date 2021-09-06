using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KoKo.Property;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

#nullable enable

namespace TrainerCommon.Trainer {

    public interface TrainerService: IDisposable {

        Property<bool> isAttachedToGame { get; }

        void attachToGame(Game game);

    }

    public class TrainerServiceImpl: TrainerService {

        private readonly StoredProperty<AttachmentState> attachmentState = new();
        public Property<bool> isAttachedToGame { get; }

        private readonly MemoryEditor            memoryEditor            = new MemoryEditorImpl();
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private          Task?                   monitorTask;

        public TrainerServiceImpl() {
            isAttachedToGame = DerivedProperty<bool>.Create(attachmentState, state => state == AttachmentState.ATTACHED);

            attachmentState.PropertyChanged += (_, args) => Trace.WriteLine($"Trainer state: {args.NewValue}");
        }

        public void attachToGame(Game game) {
            if (monitorTask is not null) {
                throw new ApplicationException("Cannot attach the same TrainerServiceImpl instance to a game more than once.");
            }

            monitorTask = Task.Run(async () => {
                Process?       gameProcess       = null;
                ProcessHandle? gameProcessHandle = null;

                while (!cancellationTokenSource.IsCancellationRequested) {
                    await Task.Delay(attachmentState.Value switch {
                        AttachmentState.TRAINER_STOPPED                  => 0,
                        AttachmentState.ATTACHED                         => 250,
                        AttachmentState.MEMORY_ADDRESS_NOT_FOUND         => 2000,
                        AttachmentState.MEMORY_ADDRESS_COULD_NOT_BE_READ => 2000,
                        AttachmentState.PROGRAM_NOT_RUNNING              => 10000,
                        _                                                => throw new ArgumentOutOfRangeException()
                    }, cancellationTokenSource.Token);

                    gameProcess ??= Process.GetProcessesByName(game.processName).FirstOrDefault();
                    if (gameProcess == null || gameProcess.HasExited) {
                        attachmentState.Value = AttachmentState.PROGRAM_NOT_RUNNING;
                        gameProcess?.Dispose();
                        gameProcess = null;
                        gameProcessHandle?.Dispose();
                        gameProcessHandle = null;
                        continue;
                    }

                    gameProcessHandle ??= memoryEditor.openProcess(gameProcess);
                    if (gameProcessHandle == null) {
                        attachmentState.Value = AttachmentState.PROGRAM_NOT_RUNNING;
                        continue;
                    }

                    try {
                        foreach (Cheat cheat in game.cheats) {
                            cheat.applyIfNecessary(gameProcessHandle, memoryEditor);
                        }

                        attachmentState.Value = AttachmentState.ATTACHED;
                    } catch (ApplicationException e) {
                        Trace.WriteLine("ApplicationException: " + e);
                        attachmentState.Value = AttachmentState.MEMORY_ADDRESS_NOT_FOUND;
                    } catch (Win32Exception e) {
                        Trace.WriteLine($"Win32Exception: (NativeErrorCode = {e.NativeErrorCode}) " + e);
                        attachmentState.Value = AttachmentState.MEMORY_ADDRESS_COULD_NOT_BE_READ;
                        if (e.NativeErrorCode != 299) {
                            Console.WriteLine(e);
                        }
                    }
                }

                gameProcess?.Dispose();
                gameProcessHandle?.Dispose();

            }, cancellationTokenSource.Token);
        }

        public void Dispose() {
            cancellationTokenSource.Cancel();
            monitorTask?.GetAwaiter().GetResult();
            attachmentState.Value = AttachmentState.TRAINER_STOPPED;
        }

        private enum AttachmentState {

            TRAINER_STOPPED,
            PROGRAM_NOT_RUNNING,
            MEMORY_ADDRESS_NOT_FOUND,
            MEMORY_ADDRESS_COULD_NOT_BE_READ,
            ATTACHED

        }

    }

}