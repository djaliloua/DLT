namespace MauiNavigationHelper.NavigationLib.Utility
{
    public static class MauiPageUtility
    {
        public static Page GetTopPage()
        {
            var navigationStack = Application.Current.MainPage?.Navigation.NavigationStack;

            var modalStack = Application.Current.MainPage?.Navigation.ModalStack;

            if (modalStack != null && modalStack.Any())
            {
                // return a modal as the modals are on top
                return modalStack.Last();
            }

            if (navigationStack != null && navigationStack.Any())
            {

                return navigationStack.Last();
            }

            return null;
        }

        public static object GetTopPageBindingContext()
        {
            return GetTopPage()?.BindingContext;
        }
    }
}
