#nullable enable

using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;

namespace TrainerCommon.App;

public class CombinationConverter: IValueConverter {

    private static readonly IComparer<Keys> HOTKEY_SORTER = new HotkeySorter();

    /// <summary>
    /// Like Gma.System.MouseKeyHook.Combination.ToString() but it prints Ctrl instead of Control like a normal menu hotkey
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch {
        null => string.Empty,
        Combination combination => string.Join("+", combination.Chord.OrderBy(key => key, HOTKEY_SORTER)
            .Select(key => key switch {
                Keys.Control           => "Ctrl",
                Keys.LWin or Keys.RWin => "Win",
                _                      => key.ToString()
            })
            .Append(combination.TriggerKey.ToString())),
        _ => value
    };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// <para>Sort Shift after Ctrl and Alt, since this is how hotkeys appear in menus in Windows.</para>
    /// <para>The sorting order is Win, Ctrl, Alt, Shift</para>
    /// </summary>
    private class HotkeySorter: IComparer<Keys> {

        public int Compare(Keys x, Keys y) {
            if (x is Keys.Shift && y is Keys.Alt or Keys.Control or Keys.LWin or Keys.RWin) {
                return 1;
            } else if (y is Keys.Shift && x is Keys.Alt or Keys.Control or Keys.LWin or Keys.RWin) {
                return -1;
            } else {
                return x.CompareTo(y);
            }
        }

    }

}