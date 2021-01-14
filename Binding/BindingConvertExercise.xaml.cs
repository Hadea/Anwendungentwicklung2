using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Binding
{
    /// <summary>
    /// Interaction logic for BindingConvertExercise.xaml
    /// </summary>
    public partial class BindingConvertExercise : Page
    {
        public BindingConvertExercise()
        {
            InitializeComponent();
            (FindResource("DoubleToHeightConverter") as DoubleToHeightConverter).parentCanvas = cnvGraph;
            sldA.Value = 1000;
            sldB.Value = 1000;
            sldC.Value = 1000;
            sldD.Value = 1000;
        }

    }
    public class DoubleToHeightConverter : IValueConverter
    {
        public Canvas parentCanvas;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false) throw new ArgumentException();
            return (parentCanvas != null ? parentCanvas.Height : 100) - (double)value / 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
