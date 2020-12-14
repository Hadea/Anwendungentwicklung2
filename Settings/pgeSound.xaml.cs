using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Settings
{
    /// <summary>
    /// Interaction logic for Sound.xaml
    /// </summary>
    public partial class pgeSound : Page
    {
        public pgeSound()
        {
            InitializeComponent();
            tbContent.Inlines.Add("Hallo");
            tbContent.Inlines.Add(" Welt");
            tbContent.Inlines.Add(new Bold(new Run("!")));
            Span colorInline = new(new Run("Ich bin hoffentlich farbig"));
            colorInline.Background = Brushes.Red;
            colorInline.Foreground = Brushes.Blue;
            tbContent.Inlines.Add(colorInline);
            
        }
    }
}
