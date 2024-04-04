using ManagPassWord.Models;
using ManagPassWord.ViewModels.Password;

namespace ManagPassWord.CustomClasses
{
    public class PasswordSearchHandler:SearchHandler
    {
        public IList<User> Passwords { get; set; }
        public Type SelectedItemNavigationTarget { get; set; }
        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            Passwords = ServiceLocator.GetService<MainPageViewModel>().Users;
            await Task.Delay(1000);
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                ItemsSource = Passwords
                    .Where(data => data.Username.ToLower().Contains(newValue.ToLower())
                    || data.Site.ToLower().Contains(newValue.ToLower())
                    )
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
            var navigationParameter = new Dictionary<string, object>
                        {
                            { "user", (User)item },
                            { "isedit", false },
                        };
            await Shell.Current.GoToAsync(nameof(DetailPage), navigationParameter);
        }
    }
}
