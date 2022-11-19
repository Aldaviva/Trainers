#nullable enable

using SuperhotMindControlDeleteTrainer.Games;
using TrainerCommon.Games;

namespace SuperhotMindControlDeleteTrainer.App; 

public partial class App {

    protected override Game game { get; } = new SuperhotMindControlDelete();

}