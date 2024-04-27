using FluentAssertions;
using Gma.System.MouseKeyHook;
using System.Globalization;
using System.Windows.Forms;
using TrainerCommon.App;

namespace Tests;

public class CombinationConverterTest {

    [Fact]
    public void sortModifiersLikeWindows() {
        string actual = (string) new CombinationConverter().Convert(Combination.TriggeredBy(Keys.Escape).Control().Alt().Shift().With(Keys.LWin), typeof(string), null, CultureInfo.CurrentCulture)!;
        actual.Should().Be("Win+Ctrl+Alt+Shift+Escape");

    }

}