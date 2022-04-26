namespace Net.UI.Sandbox;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        button.Pressed += Button_Pressed;
        button.Released += Button_Released;
        button.Clicked += Button_Clicked;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        label.Text += "\nClicked";
    }

    private void Button_Released(object sender, EventArgs e)
    {
        label.Text += "\nReleased";
    }

    private void Button_Pressed(object sender, EventArgs e)
    {
        label.Text += "\nPressed";
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        label.Text += "\nGrid Tapped";
    }
}

