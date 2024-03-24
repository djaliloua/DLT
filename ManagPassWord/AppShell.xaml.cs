using ManagPassWord.Pages;
using ManagPassWord.Pages.Debt;

namespace ManagPassWord;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        //Password pages
        Routing.RegisterRoute(nameof(AddPassworPage), typeof(AddPassworPage)); 
		Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        // Debt pages
        Routing.RegisterRoute(nameof(DebtDetailsPage), typeof(DebtDetailsPage));
        // Shared pages
        Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
        Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
    }
}
