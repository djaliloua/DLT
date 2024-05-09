using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using PurchaseManagement.MVVM.ViewModels;

namespace PurchaseManagement.Test.Fitness
{
    public class HelperClass
    {
        public Architecture Architecture = new ArchLoader().LoadAssembly(typeof(App).Assembly).Build();
    }
}
