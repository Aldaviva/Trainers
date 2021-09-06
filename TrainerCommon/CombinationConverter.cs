using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;

#nullable enable

namespace TrainerCommon.App {

    public class CombinationConverter: IValueConverter {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Combination combination) {
                // Like Gma.System.MouseKeyHook.Combination.ToString() but it prints Ctrl instead of Control like a normal menu hotkey
                return string.Join("+", combination.Chord.Select(key => key switch {
                    Keys.Control => "Ctrl",
                    _            => key.ToString()
                }).Append(combination.TriggerKey.ToString()));
            } else {
                throw new ArgumentException("value is not an instance of Combination", nameof(value));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }

    }

}