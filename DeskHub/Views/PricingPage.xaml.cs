namespace DeskHub.Views;

public partial class PricingPage : ContentPage
{
	public PricingPage()
	{
		InitializeComponent();
	}

    private  void OnbtnBook380_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Sign-In Required!", "Please sign-in to book a desk.", "OK");
    }

    private void OnbtnBook60_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Sign-In Required!", "Please sign-in to book a desk.", "OK");
    }

    private void OnbtnBook8000_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Sign-In Required!", "Please sign-in to book a desk.", "OK");
    }
}