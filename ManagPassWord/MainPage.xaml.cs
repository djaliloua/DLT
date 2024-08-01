namespace ManagPassWord;

public partial class MainPage : ContentPage
{
    FingerPrintAuthentification _authentification;
    public MainPage()
	{
		InitializeComponent();
        _authentification = new FingerPrintAuthentification();
        //_ = _authentification.Authenticate();
    }
}