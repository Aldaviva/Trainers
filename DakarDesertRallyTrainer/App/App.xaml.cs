#nullable enable

using DakarDesertRallyTrainer.Games;
using TrainerCommon.Games;

namespace DakarDesertRallyTrainer.App;

public partial class App {

    protected override Game game { get; } = new DakarDesertRally();

}