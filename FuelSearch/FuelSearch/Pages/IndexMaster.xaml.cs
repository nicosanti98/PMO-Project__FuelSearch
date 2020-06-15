using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexMaster : ContentPage
    {
        public ListView ListView;

        public IndexMaster()
        {
            InitializeComponent();


            BindingContext = new IndexMasterViewModel();
            ListView = MenuItemsListView;
        }

        class IndexMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<IndexMenuItem> MenuItems { get; set; }

            public IndexMasterViewModel()
            {
                //Riempio la list view con gli item rappresentanti
                //le pagine con il loro titolo e l'immagine associata
                MenuItems = new ObservableCollection<IndexMenuItem>(new[]
                {
                    new IndexMenuItem { Id = 0, Title = "Home", ImageSource = "home.png"},
                    new IndexMenuItem{Id = 1, Title = "Intorno a Me", ImageSource = "bussola.png"},
                    new IndexMenuItem{Id=2, Title = "Ricerca Impianti", ImageSource="search.png" },
                    new IndexMenuItem { Id = 3, Title = "Contatti", ImageSource = "contatti.png"}


                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            bool red = await DisplayAlert("Continuare?", "Scaricare gli ultimi dati disponibili?", "Ok", "Annulla");

            //Nel caso in cui l'utente abbia scelto di scaricare gli aggiornamenti, la data di scrittura del file
            //viene spostata a 10 giorni indietro, in modo tale da superare il controllo nella MainPage
            //Potendo cosi scaricare i dati più aggiornati
            if (red == true)
            {
                File.SetLastWriteTime(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/FuelSearch.db", DateTime.Today.Subtract(TimeSpan.FromDays(10)));

            }


        }
    }
}