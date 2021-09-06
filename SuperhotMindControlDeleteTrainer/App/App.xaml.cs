using SuperhotMindControlDeleteTrainer.Games;
using TrainerCommon.Games;

#nullable enable

namespace SuperhotMindControlDeleteTrainer.App {

    public partial class App {

        protected override Game game { get; } = new SuperhotMindControlDelete();

    }

}