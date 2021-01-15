using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for BindingProperties.xaml
    /// </summary>
    public partial class BindingProperties : Page, INotifyPropertyChanged
    {
        public BindingProperties()
        {
            InitializeComponent();
            DataContext = this;
        }

        public double SliderValue
        {
            get
            {
                return sliderVal;
            }
            set
            {
                if (sliderVal != value)
                {
                    sliderVal = value;
                    SliderValueReversed = 100 - value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SliderValue"));
                }
            }
        }
        private double sliderVal;

        private double sliderValRef;

        public double SliderValueReversed
        {
            get { return sliderValRef; }
            set
            {
                if (sliderValRef != value)
                {
                    sliderValRef = value;
                    SliderValue = 100-value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SliderValueReversed"));
                }
            }
        }


        private void btnReadProp_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).Content = SliderValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
