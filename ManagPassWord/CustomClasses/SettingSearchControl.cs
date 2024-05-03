using ManagPassWord.Models;
using ManagPassWord.Pages.Debt;
using ManagPassWord.ViewModels.Debt;

namespace ManagPassWord.CustomClasses
{
    public class SettingSearchControl:SearchHandler
    {
        public IList<DebtModel> Debts { get; set; }
        public Type SelectedItemNavigationTarget { get; set; }
        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            Debts = Resolver.GetService<DebtPageViewModel>().Debts;
            await Task.Delay(1000);
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = Debts
                    .Where(animal => animal.Name.ToLower().Contains(newValue.ToLower()))
                    .ToList();
            }
        }
        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            // Let the animation complete
            await Task.Delay(1000);

            ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
            // The following route works because route names are unique in this app.
            Dictionary<string, object> navigationParameter = new Dictionary<string, object>
                        {
                            { "debt", (DebtModel)item }
                        };
            await Shell.Current.GoToAsync(nameof(DebtDetailsPage), navigationParameter);
        }
       
    }
}
