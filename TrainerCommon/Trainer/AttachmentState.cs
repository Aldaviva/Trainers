#nullable enable

namespace TrainerCommon.Trainer;

public enum AttachmentState {

    TRAINER_STOPPED,
    PROGRAM_NOT_RUNNING,
    MEMORY_ADDRESS_NOT_FOUND,
    MEMORY_ADDRESS_COULD_NOT_BE_READ,
    ATTACHED,
    UNSUPPORTED_PROGRAM_VERSION

}