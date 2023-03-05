#nullable enable

using Dakar18Trainer.Games;
using TrainerCommon.Games;

namespace Dakar18Trainer.App;

public partial class App {

    protected override Game game { get; } = new Dakar18();

}