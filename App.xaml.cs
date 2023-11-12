using Ejer2_3.Controllers;

namespace Ejer2_3
{
    public partial class App : Application
    {
        static BDAudio BDAudio;

        public static BDAudio BDAudios
        {
            get
            {
                if (BDAudio == null)
                {
                    BDAudio = new BDAudio(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Audios.db"));

                }
                return BDAudio;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}