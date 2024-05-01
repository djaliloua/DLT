using PurchaseManagement.MVVM.Models;

namespace PurchaseManagement.BusinessLogic
{
    public class AccountBL
    {
        public bool Validate(Account account)
        {
            if(account == null)
            {
                return false;
            }
            if(account.Money==null) { return false; }
            return true;
        }
    }
}
