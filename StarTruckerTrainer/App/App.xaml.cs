#nullable enable

using StarTruckerTrainer.Games;
using TrainerCommon.Games;

namespace StarTruckerTrainer.App;

public partial class App {

    protected override Game game { get; } = new StarTrucker();

}