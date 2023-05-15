using bustop_app.ViewModel;

namespace bustop_app;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		Title = "";
	}
}