﻿using MauiNavigationHelper.NavigationLib.Utility;
using MauiNavigationHelper.NavigationLib.Models;
using MauiNavigationHelper.NavigationLib.Abstractions;

namespace MauiNavigationHelper.NavigationLib.Services
{
    public class NavigationService : INavigationService
    {
        #region Core navigation methods

        public async Task Push<T>() where T : Page
        {
            await Push<T>(new NavigationParameters());
        }

        public async Task Push<T>(NavigationParameters navigationParameters) where T : Page
        {
            await HandleNavigation<T>(async () =>
            {
                var pageToNavigateTo = ServiceResolver.Resolve<T>();

                if (navigationParameters.UseModalNavigation)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(pageToNavigateTo, navigationParameters.UseAnimatedNavigation);
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(pageToNavigateTo, navigationParameters.UseAnimatedNavigation);
                }
            },
                navigationParameters);
        }

        public async Task Pop()
        {
            await Pop(new NavigationParameters());
        }

        public async Task Pop(NavigationParameters navigationParameters)
        {
            await HandleNavigation<Page>(async () =>
            {
                if (navigationParameters.UseModalNavigation)
                {
                    _ = await Application.Current.MainPage.Navigation.PopModalAsync(navigationParameters.UseAnimatedNavigation);
                }
                else
                {
                    _ = await Application.Current.MainPage.Navigation.PopAsync(navigationParameters.UseAnimatedNavigation);
                }
            },
                navigationParameters);
        }

        public async Task PopToRoot()
        {
            await PopToRoot(new NavigationParameters());
        }

        public async Task PopToRoot(NavigationParameters navigationParameters)
        {
            await HandleNavigation<Page>(async () =>
            {
                await Application.Current.MainPage.Navigation.PopToRootAsync(navigationParameters.UseAnimatedNavigation);
            },
                navigationParameters);
        }

        #endregion Core navigation methods

        #region Advanced navigation methods

        public async Task GoBack()
        {
            await GoBack(new NavigationParameters());
        }

        public async Task GoBack(NavigationParameters navigationParameters)
        {
            await HandleNavigation<Page>(async () =>
            {
                if (navigationParameters.UseModalNavigation)
                {
                    _ = await Application.Current.MainPage.Navigation.PopModalAsync(navigationParameters.UseAnimatedNavigation);
                }
                else
                {
                    if (Application.Current.MainPage.Navigation.NavigationStack.Count <= 1)
                    {
                        // quit the app as this page is the last one
                        Application.Current.Quit();
                    }
                    else
                    {
                        _ = await Application.Current.MainPage.Navigation.PopAsync(navigationParameters.UseAnimatedNavigation);
                    }
                }
            },
                navigationParameters);
        }

        public async Task ReplaceTopPage<T>()
            where T : Page
        {
            await ReplaceTopPage<T>(new NavigationParameters());
        }

        public async Task ReplaceTopPage<T>(NavigationParameters navigationParameters)
            where T : Page
        {
            await HandleNavigation<Page>(async () =>
            {
                var navigation = Application.Current.MainPage.Navigation;
                var pageToNavigateTo = ServiceResolver.Resolve<T>();

                navigation.InsertPageBefore(pageToNavigateTo, navigation.NavigationStack.Last());
                await navigation.PopAsync(navigationParameters.UseAnimatedNavigation);
            },
                navigationParameters);
        }

        public async Task ResetStackAndPush<T>()
            where T : Page
        {
            await ResetStackAndPush<T>(new NavigationParameters());
        }

        public async Task ResetStackAndPush<T>(NavigationParameters navigationParameters)
            where T : Page
        {
            await HandleNavigation<Page>(async () =>
            {
                var navigation = Application.Current.MainPage.Navigation;
                var pageToNavigateTo = ServiceResolver.Resolve<T>();

                if (navigation.NavigationStack.Count > 0)
                {
                    // insert page as the new root page
                    navigation.InsertPageBefore(pageToNavigateTo, navigation.NavigationStack.Last());
                }
                else
                {
                    // the stack was already empty
                    await navigation.PushAsync(pageToNavigateTo, navigationParameters.UseAnimatedNavigation);
                }

                await navigation.PopToRootAsync(navigationParameters.UseAnimatedNavigation);
            },
                navigationParameters);
        }

        #endregion Advanced navigation methods

        #region URI navigation methods

        public async Task Navigate(string uri)
        {
            await Navigate(uri, new NavigationParameters());
        }

        public async Task Navigate(string uri, NavigationParameters navigationParameters)
        {
            // todo: how to consider modal navigation/animations/parameters etc. at each segment of the navigation
            // todo: need to consider what query parameter should and shouldn't be.
            // should they be only for passing simple variables (strings, bools etc.) rather than complex json objects
            var navigation = Application.Current.MainPage.Navigation;

            var segments = UriUtility.GetUriSegments(uri);
            var instructions = segments.Select(UriUtility.ParseUriSegment)
                .ToList();

            var pagesToRemove = new List<Page>();

            if (UriUtility.IsUriAbsolute(uri))
            {
                if (instructions.Any(instruction => instruction.PageType == typeof(GoBackUriSegment)))
                {
                    throw new Exception("You can't perform 'go back' actions during absolute URI navigation");
                }

                // add every page as a page to be removed
                pagesToRemove.AddRange(navigation.NavigationStack);

                foreach (var page in navigation.NavigationStack)
                {
                    await LifecycleEventUtility.TriggerOnNavigatingFrom(page?.BindingContext, navigationParameters);
                }
            }

            if (instructions.Count > 1)
            {
                // handle all but the last instruction
                for (int i = 0; i < instructions.Count() - 1; i++)
                {
                    if (instructions[i].PageType == typeof(GoBackUriSegment))
                    {
                        // handle "Go Back" URI segments
                        var pageToRemove = navigation.NavigationStack[^(i + 1)];
                        pagesToRemove.Add(pageToRemove);

                        await LifecycleEventUtility.TriggerOnNavigatingFrom(
                            pageToRemove?.BindingContext,
                            instructions[i].QueryParameters.MergeNavigationParameters(navigationParameters));
                    }
                    else
                    {
                        // push pages relatively onto the stack
                        var pageToNavigateTo = ServiceResolver.Resolve(instructions[i].PageType) as Page;
                        var pushNavigationParameters = instructions[i].QueryParameters.MergeNavigationParameters(navigationParameters);

                        if (pushNavigationParameters.UseModalNavigation)
                        {
                            await Application.Current.MainPage.Navigation.PushModalAsync(pageToNavigateTo, navigationParameters.UseAnimatedNavigation);
                        }
                        else
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(pageToNavigateTo, navigationParameters.UseAnimatedNavigation);
                        }

                        await LifecycleEventUtility.TriggerOnNavigatedTo(
                            pageToNavigateTo?.BindingContext,
                            pushNavigationParameters);
                    }
                }
            }

            // handle final instruction
            var lastInstruction = instructions.Last();

            if (lastInstruction.PageType == typeof(GoBackUriSegment))
            {
                // remove all the pages that needed removed
                foreach (var pageToRemove in pagesToRemove)
                {
                    navigation.RemovePage(pageToRemove);
                    await LifecycleEventUtility.TriggerOnNavigatedFrom(
                        pageToRemove?.BindingContext,
                        lastInstruction.QueryParameters.MergeNavigationParameters(navigationParameters));
                }

                var pageToPop = navigation.NavigationStack.Last();
                var popNavigationParameters = lastInstruction.QueryParameters.MergeNavigationParameters(navigationParameters);

                // pop final page
                if (popNavigationParameters.UseModalNavigation)
                {
                    _ = await Application.Current.MainPage.Navigation.PopModalAsync(navigationParameters.UseAnimatedNavigation);
                }
                else
                {
                    _ = await Application.Current.MainPage.Navigation.PopAsync(navigationParameters.UseAnimatedNavigation);
                }

                await LifecycleEventUtility.TriggerOnNavigatedFrom(
                    pageToPop?.BindingContext,
                    popNavigationParameters);
            }
            else
            {
                // push page relatively onto the stack
                var pageToNavigateTo = ServiceResolver.Resolve(lastInstruction.PageType) as Page;
                var pushNavigationParameters = lastInstruction.QueryParameters.MergeNavigationParameters(navigationParameters);

                if (pushNavigationParameters.UseModalNavigation)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(pageToNavigateTo, navigationParameters.UseAnimatedNavigation);
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(pageToNavigateTo, navigationParameters.UseAnimatedNavigation);
                }

                // remove all the pages that needed removed
                foreach (var pageToRemove in pagesToRemove)
                {
                    navigation.RemovePage(pageToRemove);
                    await LifecycleEventUtility.TriggerOnNavigatedFrom(
                        pageToRemove?.BindingContext,
                        pushNavigationParameters);
                }
            }

            var lastNavigationParameters = lastInstruction.QueryParameters.MergeNavigationParameters(navigationParameters);

            var toBindingContext = MauiPageUtility.GetTopPageBindingContext();
            await LifecycleEventUtility.TriggerOnNavigatedTo(toBindingContext, lastNavigationParameters);

            // if the SelectTab parameter is used, we will switch tab
            SelectTabFromParameters(lastNavigationParameters);
        }

        #endregion URI navigation methods

        #region Internal implementation

        private async Task HandleNavigation<T>(Func<Task> navigationAction, NavigationParameters navigationParameters)
            where T : Page
        {
            var fromBindingContext = MauiPageUtility.GetTopPageBindingContext();

            await LifecycleEventUtility.TriggerOnNavigatingFrom(fromBindingContext, navigationParameters);

            await navigationAction.Invoke();

            await LifecycleEventUtility.TriggerOnNavigatedFrom(fromBindingContext, navigationParameters);

            var toBindingContext = MauiPageUtility.GetTopPageBindingContext();
            await LifecycleEventUtility.TriggerOnNavigatedTo(toBindingContext, navigationParameters);

            // if the SelectTab parameter is used, we will switch tab
            SelectTabFromParameters(navigationParameters);
        }

        private void SelectTabFromParameters(NavigationParameters navigationParameters)
        {
            if (string.IsNullOrWhiteSpace(navigationParameters.SelectTab))
            {
                // no tab to select
                return;
            }

            var tabType = UriUtility.FindPageType(navigationParameters.SelectTab);
            SelectTabWithType(tabType);
        }

        /// <summary>
        /// This method allows the <see cref="SelectTab{T}()"/> to be called with reflection.
        /// </summary>
        private void SelectTabWithType(Type tabType)
        {
            var selectTabMethod = GetType()
                .GetMethod(nameof(SelectTab))
                .MakeGenericMethod(tabType);
            selectTabMethod.Invoke(this, null);
        }

        /// <summary>
        /// This method searches for and tries to find a TabbedPage that is visible to the user.
        /// </summary>
        /// <param name="page">Page to search for a TabbedPage in</param>
        /// <returns>A tabbeed page if found</returns>
        private TabbedPage FindVisibleTabbedPage(Page page)
        {
            return page switch
            {
                TabbedPage tabbedPage => tabbedPage,
                FlyoutPage { Detail: TabbedPage flyoutTabbedPage } => flyoutTabbedPage,
                FlyoutPage { Detail: var detail } => GetTabbedPageFromNavigationPage(detail),
                _ => GetTabbedPageFromNavigationPage(page)
            };
        }

        private TabbedPage GetTabbedPageFromNavigationPage(Page page)
        {
            if (page is NavigationPage { CurrentPage: TabbedPage tabbedPage })
            {
                return tabbedPage;
            }

            return null;
        }

        #endregion Internal implementation

        #region Tab navigation methods

        public void SelectTab<T>()
            where T : Page
        {
            var topPage = MauiPageUtility.GetTopPage();
            var tabbedPage = FindVisibleTabbedPage(topPage);

            if (tabbedPage == null)
            {
                // todo: warn about this in https://github.com/BurkusCat/Burkus.Mvvm.Maui/issues/17 ?
                return;
            }

            foreach (var childPage in tabbedPage.Children)
            {
                if (childPage.GetType() == typeof(T))
                {
                    tabbedPage.CurrentPage = childPage;
                    return;
                }

                if (childPage is NavigationPage)
                {
                    if (((NavigationPage)childPage).CurrentPage.GetType() == typeof(T))
                    {
                        tabbedPage.CurrentPage = childPage;
                        return;
                    }
                }
            }
        }

        #endregion Tab navigation methods

        #region Flyout navigation methods

        public void SwitchFlyoutDetail(Type detailPage)
        {
            var switchFlyoutDetailMethod = GetType()
                .GetMethods()
                .First(methodInfo => methodInfo.Name == nameof(SwitchFlyoutDetail) && methodInfo.IsGenericMethod)
                .MakeGenericMethod(detailPage);
            switchFlyoutDetailMethod.Invoke(this, null);
        }

        public void SwitchFlyoutDetail<T>()
            where T : Page
        {
            var flyoutPage = MauiPageUtility.GetTopPage() as FlyoutPage;

            if (flyoutPage == null)
            {
                // todo: warn about this in https://github.com/BurkusCat/Burkus.Mvvm.Maui/issues/17 ?
                return;
            }

            var pageToNavigateTo = ServiceResolver.Resolve<T>();

            // wrap the detail in a NavigationPage
            flyoutPage.Detail = new NavigationPage(pageToNavigateTo);

            // close the flyout
            flyoutPage.IsPresented = false;
        }

        // TODO: push to flyout, push to tab, push to navigation page?
        // push to navigation page might be the most versatile since it would allow you to add to tabs, add to flyouts
        // then if you wanted to push above a tabbedpage you would use the normal push
        // a parameter? ToNavigationPage could be used so it could be used with URL navigation or the existing methods

        #endregion Flyout navigation methods
    }
}
