using Net.UI;

namespace NetUI.Sandbox
{
    public partial class App : Application2
    {
        public App()
        {
        }

        public override void InitializeSystems()
        {
            InitializeComponent();
            base.InitializeSystems();
        }

        public override Page GetFirstPage()
        {
            return new MainPage();
        }
    }
}