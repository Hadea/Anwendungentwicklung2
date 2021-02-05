using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace MultiThreading
{
    /// <summary>
    /// Interaction logic for pgeThreadingAwaitSelfmade.xaml
    /// </summary>
    public partial class pgeThreadingAwaitSelfmade : Page
    {
        public pgeThreadingAwaitSelfmade()
        {
            InitializeComponent();
        }

        private async void btnStartAwaitableClass_Click(object sender, RoutedEventArgs e)
        {
            tblAusgabe.Text = "running";
            LogicClass mySlowLogic = new();

            mySlowLogic.SlowReturn();
            await mySlowLogic;

            tblAusgabe.Text = mySlowLogic.GetResult();
        }

        private void btnStartTaskCompletionSource_Click(object sender, RoutedEventArgs e)
        {
            TaskCompletionSource<string> tcs = new();
            // TODO: beispiel für TCS fertig bauen!
            throw new NotImplementedException();
        }
    }

    class LogicClass : INotifyCompletion
    {
        private Action continuation = null;
        private string result = string.Empty;

        // Make this class awaitable
        public LogicClass GetAwaiter() { return this; }

        // Implementation of INotifyCompletion for the self-awaiter
        public bool IsCompleted { get; set; }
        public string GetResult() { return result; }
        public void OnCompleted(Action continuation)
        {
            // Store continuation delegate
            this.continuation = continuation;
        }
        public void SlowReturn()
        {
            Task.Delay(3000).Wait();
            result = "Hello World";
            IsCompleted = true;
            continuation?.Invoke();
         }
    }
}
