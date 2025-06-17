#nullable enable

using Gma.System.MouseKeyHook;
using KoKo.Property;
using System;
using TrainerCommon.Trainer;

namespace TrainerCommon.Cheats;

public interface Cheat: IDisposable {

    string name { get; }

    Combination? keyboardShortcut { get; }

    SettableProperty<bool> isEnabled { get; }

    void applyIfNecessary(ProcessHandle processHandle, string gameVersionCode);

}

public abstract class BaseCheat: Cheat {

    public abstract string name { get; }
    public abstract Combination? keyboardShortcut { get; }

    protected abstract void apply(ProcessHandle processHandle, string gameVersionCode);

    public SettableProperty<bool> isEnabled { get; } = new StoredProperty<bool>();

    public void applyIfNecessary(ProcessHandle processHandle, string gameVersionCode) {
        if (isEnabled.Value) {
            apply(processHandle, gameVersionCode);
        }
    }

    protected virtual void Dispose(bool disposing) { }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}