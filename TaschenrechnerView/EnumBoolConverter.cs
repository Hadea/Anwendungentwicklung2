using System;
using System.Globalization;
using System.Windows.Data;

namespace TaschenrechnerView
{
    class EnumBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter); // vergleicht den value und den parameter miteinander. dadurch weiss der radiobutton ob er selektiert oder nicht selektiert ist
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing; // gibt das enum zurück an den radiobutton
        }
    }
}
