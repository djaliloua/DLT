namespace ManagPassWord.Triggers
{
    public class OnFocusTriggerAction : TriggerAction<SearchBar>
    {
        protected override async void Invoke(SearchBar sender)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
            
        }
    }
}
