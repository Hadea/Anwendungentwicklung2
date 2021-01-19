using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVVM_ViewModel;

namespace MVVM_Test
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void Creation()
        {
            Assert.IsNotNull(new MainViewModel());
        }
    }
}
