﻿#nullable enable

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using KoKo.Property;
using TrainerCommon.Cheats;
using TrainerCommon.Games;

namespace TrainerCommon.Trainer;

public interface TrainerService: IDisposable {

    Property<AttachmentState> attachmentState { get; }

    void attachToGame(Game game);

}

public class TrainerServiceImpl: TrainerService {

    private readonly StoredProperty<AttachmentState> _attachmentState = new();
    public Property<AttachmentState> attachmentState { get; }

    private readonly CancellationTokenSource cancellationTokenSource = new();

    private Task? monitorTask;

    public TrainerServiceImpl() {
        attachmentState                  =  _attachmentState;
        _attachmentState.PropertyChanged += (_, args) => Trace.WriteLine($"Trainer state: {args.NewValue}");
    }

    public void attachToGame(Game game) {
        if (monitorTask is not null) {
            throw new ApplicationException("Cannot attach the same TrainerServiceImpl instance to a game more than once.");
        }

        monitorTask = Task.Factory.StartNew(async () => {
            Process?       gameProcess        = null;
            ProcessHandle? gameProcessHandle  = null;
            string?        gameExecutableHash = null;
            string?        gameVersionCode    = null;

            while (!cancellationTokenSource.IsCancellationRequested) {
                await Task.Delay(_attachmentState.Value switch {
                    AttachmentState.TRAINER_STOPPED                  => 0,
                    AttachmentState.ATTACHED                         => 100, // same as Cheat Engine's default Freeze Interval (General Settings)
                    AttachmentState.MEMORY_ADDRESS_NOT_FOUND         => 1000,
                    AttachmentState.MEMORY_ADDRESS_COULD_NOT_BE_READ => 1000,
                    AttachmentState.PROGRAM_NOT_RUNNING              => 3000,
                    AttachmentState.UNSUPPORTED_PROGRAM_VERSION      => 10000,
                    _                                                => throw new ArgumentOutOfRangeException()
                }, cancellationTokenSource.Token);

                gameProcess ??= Process.GetProcessesByName(game.processName).FirstOrDefault();
                if (gameProcess == null || gameProcess.HasExited) {
                    cleanUpGameProcess();
                    continue;
                }

                gameProcessHandle ??= MemoryEditor.openProcess(gameProcess);
                if (gameProcessHandle == null) {
                    cleanUpGameProcess();
                    continue;
                }

                if (gameExecutableHash == null) {
                    try {
                        gameExecutableHash = readFileHash(gameProcess.MainModule!.FileName);
                    } catch (InvalidOperationException) {
                        cleanUpGameProcess();
                        continue;
                    }

                    gameVersionCode = game.getVersion(gameExecutableHash);
                    if (gameVersionCode == null) {
                        _attachmentState.Value = AttachmentState.UNSUPPORTED_PROGRAM_VERSION;
                    }
                }

                if (_attachmentState.Value == AttachmentState.UNSUPPORTED_PROGRAM_VERSION) {
                    continue;
                }

                try {
                    foreach (Cheat cheat in game.cheats) {
                        cheat.applyIfNecessary(gameProcessHandle, gameVersionCode!);
                    }

                    _attachmentState.Value = AttachmentState.ATTACHED;
                } catch (ApplicationException e) {
                    Trace.WriteLine("ApplicationException: " + e);
                    _attachmentState.Value = AttachmentState.MEMORY_ADDRESS_NOT_FOUND;
                } catch (Win32Exception e) {
                    Trace.WriteLine($"Win32Exception: (NativeErrorCode = {e.NativeErrorCode}) " + e);
                    _attachmentState.Value = AttachmentState.MEMORY_ADDRESS_COULD_NOT_BE_READ;
                    if (e.NativeErrorCode != 299) {
                        Console.WriteLine(e);
                    }

                    Trace.WriteLine("Memory address could not be read");
                    Trace.WriteLine(e);
                }
            }

            gameProcess?.Dispose();
            gameProcessHandle?.Dispose();

            void cleanUpGameProcess() {
                _attachmentState.Value = AttachmentState.PROGRAM_NOT_RUNNING;
                gameProcess?.Dispose();
                gameProcess = null;
                gameProcessHandle?.Dispose();
                gameProcessHandle  = null;
                gameExecutableHash = null;
                gameVersionCode    = null;
            }
        }, cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    private static string readFileHash(string filename) {
        using FileStream fileStream = File.OpenRead(filename);
        using SHA256     sha256     = SHA256.Create();
        return string.Join(string.Empty, sha256.ComputeHash(fileStream).Select(b => b.ToString("x2")));
    }

    public void Dispose() {
        cancellationTokenSource.Cancel();
        try {
            monitorTask?.GetAwaiter().GetResult();
        } catch (TaskCanceledException) {
            //cancellation is how this task normally ends
        }

        _attachmentState.Value = AttachmentState.TRAINER_STOPPED;
    }

}