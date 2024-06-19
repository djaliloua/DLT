namespace PurchaseManagement.NavigationLib.Extensions
{
    public class ResolveBindingContext : IMarkupExtension
    {
        public object TypeArguments { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return null;
        }
    }
}
