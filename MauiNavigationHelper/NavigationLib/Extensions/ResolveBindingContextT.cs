
namespace MauiNavigationHelper.NavigationLib.Extensions
{
    public class ResolveBindingContext<T> : IMarkupExtension<T>
    where T : class
    {
        //public object ProvideValue(IServiceProvider serviceProvider)
        //{
        //    return ServiceResolver.Resolve<T>();
        //}

        //T IMarkupExtension<T>.ProvideValue(IServiceProvider serviceProvider)
        //{
        //    return ServiceResolver.Resolve<T>();
        //}
        public T ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
