
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Index : MasterDetailPage
    {
        //Attributo privato contenente il raggio dalla posizione attuale
        //Con il quale inizializzare alcune pagine
        private double Raggio;

        //Costruttore
        public Index(double Radius)
        {

            this.Raggio = Radius;
            InitializeComponent();


            //Aggiunge un listener per l'evento di selezione di un item della list view
            //Contenente le voci del menu di navigazione
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }


        //Metodo che descrive il comportamento al click di un item nel menu
        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Variabile contenente l'item del menu selezionato
            var item = e.SelectedItem as IndexMenuItem;

            //**********************************//
            /*In base alla selezione vengono inizializzate
             * Delle pagine come pagine di tipo Detail, figlie 
             * della pagina Master. Ciò farà si che da ogni pagina
             * Sia visibile e accessibile il menu di navigazione*/
            //**********************************//
            if (item == null)
                return;

            if (item.Title.Equals("Home"))
            {
                await DependencyService.Get<IAdmobInterstitialAds>().Display("ca-app-pub-9362856343758559/5817800981");
                Detail = new NavigationPage(new IndexDetail(this.Raggio));

            }
            else if (item.Title.Equals("Contatti"))
            {
                Detail = new NavigationPage(new Contatti());
            }
            else if (item.Title.Equals("Intorno a Me"))
            {
                Detail = new NavigationPage(new Intornoame());
            }
            else if (item.Title.Equals("Ricerca Impianti"))
            {
                Detail = new NavigationPage(new Search());
            }
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }


    }
}