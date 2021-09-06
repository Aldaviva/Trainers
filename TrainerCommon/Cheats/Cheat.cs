using Gma.System.MouseKeyHook;
using KoKo.Property;
using TrainerCommon.Trainer;

#nullable enable

namespace TrainerCommon.Cheats {

    public interface Cheat {

        string name { get; }

        Combination keyboardShortcut { get; }

        SettableProperty<bool> isEnabled { get; }

        void applyIfNecessary(ProcessHandle processHandle, MemoryEditor memoryEditor);

    }

    public abstract class BaseCheat: Cheat {

        public abstract string name { get; }
        public abstract Combination keyboardShortcut { get; }
        public abstract void applyIfNecessary(ProcessHandle processHandle, MemoryEditor memoryEditor);

        public SettableProperty<bool> isEnabled { get; } = new StoredProperty<bool>();

    }

}