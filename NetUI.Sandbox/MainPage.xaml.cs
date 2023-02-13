using Net.Essentials;
using Net.UI;

namespace NetUI.Sandbox
{
    public partial class MainPage : ContentPage
    {
        public List<string> Items { get; set; } = new List<string>
        {
            "Red",
            "Black"
        };

        public MainPage()
        {
            InitializeComponent();
            button.Pressed += Button_Pressed;
            button.Released += Button_Released;
            button.Clicked += Button_Clicked;

            mp.BindingContext = this;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            label.Text += "\nClicked";
            Items = new List<string> { "A", "B", "C", "D" };
            OnPropertyChanged(nameof(Items));

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
}