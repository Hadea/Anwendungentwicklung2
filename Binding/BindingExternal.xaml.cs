using System;
using System.Collections.Generic;
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
    /// Interaction logic for BindingExternal.xaml
    /// </summary>
    public partial class BindingExternal : Page
    {
        public BindingExternal()
        {
            InitializeComponent();
        }
        public BindingExternal(Window bindingTarget = null)
        {
            InitializeComponent();
            DataContext = bindingTarget;
            WindowContext = bindingTarget;
            //sldC.DataContext = this;
            //sldD.DataContext = this;
        }

        public Window WindowContext { get; set; }
    }
}
