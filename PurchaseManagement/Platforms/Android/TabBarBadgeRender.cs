using List = Android.Widget;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Platform;
using Android.Content;
using Android.Widget;

namespace PurchaseManagement
{
    //public partial class AccountPage : ContentPage
    //{
    //    partial void ChangedHandler(object sender, EventArgs e)
    //    {
    //        ListView listView = sender as ListView;
    //        var list = listView.Handler.PlatformView as List.Ab+sListView;
            
    //        list.ItemLongClick += List_ItemLongClick;
    //        list.LongClick += List_LongClick;
    //        list.ItemSelected += List_ItemSelected;
    //    }

    //    private void List_ItemSelected(object sender, Android.Widget.AdapterView.ItemSelectedEventArgs e)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private void List_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private void List_ItemLongClick(object sender, Android.Widget.AdapterView.ItemLongClickEventArgs e)
    //    {
            
    //    }
    //}
    public class CustomListView : ListViewRenderer
    {
        public CustomListView(Context context) : base(context)
        {
        }
        protected override Android.Widget.ListView CreateNativeControl()
        {
            var listView = base.CreateNativeControl();
            listView.ChoiceMode = ChoiceMode.Single;
            listView.SetSelector(Android.Resource.Color.Transparent);
            listView.ItemSelected += ListView_ItemSelected;
            return listView;
        }

        private void ListView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var listView = sender as List.ListView;
            if(listView != null)
            {
                for (int i = 0; i < listView.ChildCount; i++)
                {
                    var view = listView.GetChildAt(i);
                    if (view != null)
                    {
                        view.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    }
                }

                if (e.Parent != null && e.Position != AdapterView.InvalidPosition)
                {
                    var selectedView = listView.GetChildAt(e.Position);
                    selectedView?.SetBackgroundColor(Android.Graphics.Color.LightGray);
                }
            }
            
        }
    }
}
