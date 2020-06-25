using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FuelSearch
{
    public partial class App : Application
    {
        public App()
        {
            if (!System.IO.File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "setting.txt"))
            {
                System.IO.File.Create(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "setting.txt");
            }
            InitializeComponent();
            MainPage = new Index.Index(25);
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
