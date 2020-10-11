#nullable enable

namespace SuperhotMindControlDeleteTrainer.Cheats {

    public interface Cheat {

        void apply(bool isEnabled, ProcessHandle processHandle, MemoryEditor memoryEditor);

    }

}