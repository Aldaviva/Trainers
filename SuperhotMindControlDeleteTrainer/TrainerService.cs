using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KoKo.Property;
using SuperhotMindControlDeleteTrainer.Cheats;

#nullable enable

namespace SuperhotMindControlDeleteTrainer {

    public interface TrainerService: IDisposable {

        StoredProperty<bool> isInfiniteHealthEnabled { get; }

        Property<bool> isAttachedToGame { get; }

    }

    public class TrainerServiceImpl: TrainerService {

        private const string PROCESS_NAME = "SHMCD";

        private readonly StoredProperty<AttachmentState> attachmentState = new StoredProperty<AttachmentState>(AttachmentState.TRAINER_STOPPED);
        public Property<bool> isAttachedToGame { get; }

        private readonly StoredProperty<bool> _isInfiniteHealthEnabled = new StoredProperty<bool>();
        public StoredProperty<bool> isInfiniteHealthEnabled { get; }

        private readonly MemoryEditor            memoryEditor            = new MemoryEditorImpl();
        private readonly Cheat                   infiniteHealthCheat     = new InfiniteHealthCheat();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public TrainerServiceImpl() {
            isAttachedToGame        = DerivedProperty<bool>.Create(attachmentState, state => state == AttachmentState.ATTACHED);
            isInfiniteHealthEnabled = _isInfiniteHealthEnabled;

            attachmentState.PropertyChanged += (sender, args) => Trace.WriteLine($"Trainer state: {args.NewValue}");

            Task.Run(monitorGame, cancellationTokenSource.Token);
        }

        private async Task monitorGame() {
            Process?       gameProcess = null;
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

                gameProcess ??= Process.GetProcessesByName(PROCESS_NAME).FirstOrDefault();
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
                    infiniteHealthCheat.apply(isInfiniteHealthEnabled.Value, gameProcessHandle, memoryEditor);

                    attachmentState.Value = AttachmentState.ATTACHED;
                } catch (ApplicationException e) {
                    Trace.WriteLine("ApplicationException: "+e);
                    attachmentState.Value = AttachmentState.MEMORY_ADDRESS_NOT_FOUND;
                } catch (Win32Exception e) {
                    Trace.WriteLine($"Win32Exception: (NativeErrorCode = {e.NativeErrorCode}) "+e);
                    attachmentState.Value = AttachmentState.MEMORY_ADDRESS_COULD_NOT_BE_READ;
                    if (e.NativeErrorCode != 299) {
                        Console.WriteLine(e);
                    }
                }
            }

            gameProcess?.Dispose();
            gameProcessHandle?.Dispose();
        }

        public void Dispose() {
            cancellationTokenSource.Cancel();
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