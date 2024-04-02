using MVVM;
using System.Windows.Input;

namespace ManagPassWord.ViewModels.Debt
{
    public class DebtSettingViewModel:BaseViewModel
    {
        private bool _IsDebtSettingVisible = true;
        public bool IsDebtSettingVisible
        {
            get => _IsDebtSettingVisible;
            set => UpdateObservable(ref _IsDebtSettingVisible, value);
        }
        public ICommand SettingCommand { get; private set; }
        public DebtSettingViewModel()
        {
            SettingCommand = new Command(On_Setting);
        }
        private void On_Setting(object sender)
        {

        }
    }
}
