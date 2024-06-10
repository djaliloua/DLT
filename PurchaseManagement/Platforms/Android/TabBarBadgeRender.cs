using BottomNavigation = Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Google.Android.Material.Badge;
using Microsoft.Maui.Controls.Platform;

namespace PurchaseManagement
{
    public class TabBarBadgeRender: ShellRenderer
    {
        protected override IShellBottomNavViewAppearanceTracker 
            CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new BadgeShellBottomNavAppearanceTracker(this, shellItem);
        }
    }
    class BadgeShellBottomNavAppearanceTracker : ShellBottomNavViewAppearanceTracker
    {
        private BadgeDrawable badgeDrawable;
        const int cartTabItemIndex = 0;
        public BadgeShellBottomNavAppearanceTracker(IShellContext shellContext, ShellItem shellItem):base(shellContext, shellItem) 
        {
            BadgeCounterService.CounterChanged += BadgeCounterService_CounterChanged;
        }
        private void BadgeCounterService_CounterChanged(object sender, int e)
        {
            UpdateBadge(e);
        }

        public override void SetAppearance(BottomNavigation.BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            //bottomView.SetPadding(0,50,0,0);
            if(badgeDrawable is null)
            {
                badgeDrawable = bottomView.GetOrCreateBadge(cartTabItemIndex);
            }
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            BadgeCounterService.CounterChanged -= BadgeCounterService_CounterChanged;
        }
        private void UpdateBadge(int count)
        {
            if(badgeDrawable is not null)
            {
                if(count >= 0)
                {
                    badgeDrawable.Number = count;
                    badgeDrawable.BackgroundColor = Colors.Red.ToPlatform();
                    badgeDrawable.BadgeTextColor = Colors.White.ToPlatform();
                    badgeDrawable.SetVisible(true);
                    badgeDrawable.SetBadgeWithoutTextShapeAppearanceOverlay(0);
                }
            }
        }
    }
    public static class BadgeCounterService
    {
        private static int _count;
        public static int Count
        {
            get => _count;
            private set
            {
                _count = value;
                CounterChanged?.Invoke(null, value);
            }
        }
        public static int SetCount(int count) => Count = count;
        public static event EventHandler<int> CounterChanged;
    }
}
