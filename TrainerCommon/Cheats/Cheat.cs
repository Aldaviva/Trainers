using Gma.System.MouseKeyHook;
using KoKo.Property;
using TrainerCommon.Trainer;

#nullable enable

namespace TrainerCommon.Cheats {

    public interface Cheat {

        string name { get; }

        Combination keyboardShortcut { get; }

        SettableProperty<bool> isEnabled { get; }

        void applyIfNecessary(ProcessHandle processHandle);

    }

    public abstract class BaseCheat: Cheat {

        public abstract string name { get; }
        public abstract Combination keyboardShortcut { get; }
        protected abstract void apply(ProcessHandle processHandle);

        public SettableProperty<bool> isEnabled { get; } = new StoredProperty<bool>();

        public void applyIfNecessary(ProcessHandle processHandle) {
            if (isEnabled.Value) {
                apply(processHandle);
            }
        }

    }

}