using MVVM;
using System.Windows.Input;

namespace ManagPassWord.MVVM.ViewModels.Debt
{
    public class DebtSettingViewModel:BaseViewModel
    {
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
