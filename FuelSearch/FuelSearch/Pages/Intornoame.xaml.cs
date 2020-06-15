using SQLite;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Intornoame : ContentPage
    {
        //Contiene il raggio
        private double Raggio;
        //Contiene la posizione attuale, memorizzata all'evento OnAppearing della pagina
        private Position CurrentPosition;

        private MapSpan MapSpan;
        //Contiene la lista di tipi di carburante ottenuti leggendo tutto il DB
        private List<GeneralItem> ListaTipiCarburanti = new List<GeneralItem>();
        public Intornoame()
        {
            WebClient wc = new WebClient();
            InitializeComponent();

            SetPage();

        }
        private async void SetPage()
        {
            GetPosition();

            if (this.CurrentPosition.Latitude == 0 && this.CurrentPosition.Longitude == 0)
            {
                btn.IsVisible = true;
                btncerca.IsEnabled = false;
                slider.IsEnabled = false;
            }
            //Per default setta la mappa centrandola nella posizione attuale
            //per un raggio di 0,5 km
            this.MapSpan = MapSpan.FromCenterAndRadius(this.CurrentPosition, Distance.FromKilometers(0.5));
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(this.CurrentPosition.Latitude, this.CurrentPosition.Longitude), Distance.FromKilometers(0.5)));

            //Aggiungo alla lista dei carburanti possibili tutti i carburanti presenti nell'elenco
            string query = "SELECT DISTINCT descCarburante FROM Rilevazioni";
            RemoteDBConnection conn = new RemoteDBConnection(query);
            if (conn.Connect() == 0)
            {
                await DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");
                btn.IsVisible = true;
                btn.IsEnabled = true;
            }
            else
            {
                btn.IsVisible = false;
                List<GeneralItem> List = conn.ExecuteQueryWithResponse(query);
                descCarburante.Items.Add("--");
                for (int i = 0; i < List.Count; i++)
                {
                    descCarburante.Items.Add(List[i].descCarburante.ToString());
                }
            }
        }
        protected override void OnAppearing()
        {
            GetPosition();
        }

        private async Task GetPosition()
        {
            //Tenta di ricavare la posizione attuale, valore di default con latitudine = 0 e longitudine = 0
            try
            {
                Location Loc = await Geolocation.GetLastKnownLocationAsync();
                this.CurrentPosition = new Position(Loc.Latitude, Loc.Longitude);
            }
            catch (Exception ex)
            {
                this.CurrentPosition = new Position(0, 0);
            }

        }

        //Metodo che avvicina o allontana la mappa in base al valore dello slider
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {

            slider.Value = Math.Round(e.NewValue, 1);
            labl.Text = "Raggio: " + slider.Value + " km";
            this.Raggio = slider.Value;
            MapSpan M = MapSpan.FromCenterAndRadius(CurrentPosition, Distance.FromKilometers(this.Raggio));
            this.MapSpan = M;
            Map.MoveToRegion(M);

        }

        private void Btncerca_Clicked(object sender, EventArgs e)
        {
            for (int i = 0; Map.Pins.Count != 0; i++)
            {
                Map.Pins.RemoveAt(i);
                i--;
            }

            //Passato una ListView e una query che produce i risultati da inserire
            //nella ListView, chiamo il metodo AddToListView per riempire la ListView
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");
                btn.IsVisible = true;
                btn.IsEnabled = true;
            }
            else
            {
                ListViewFiller L = new ListViewFiller(AddToMap(), lista);
                L.AddToListView();
                Count.Text = "Risultati ottenuti: " + L.TakeListLenght();
            }

        }

        private void DescCarburante_SelectedIndexChanged(object sender, EventArgs e)
        {
            btncerca.IsEnabled = true;
        }


        //Metodo che di fatto riempie la mappa con i valori ricercati, allo stesso modo del metodo in IndexDetail.cs
        private string AddToMap()
        {

            SQLiteConnection Conn = new SQLiteConnection(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/FuelSearch.db");
            SQLiteCommand Cmd = new SQLiteCommand(Conn);
            string latin, latfin;
            string longin, longfin;

            latin = (MapSpan.Center.Latitude - (MapSpan.LatitudeDegrees / 2)).ToString().Replace(",", ".");
            latfin = (MapSpan.Center.Latitude + (MapSpan.LatitudeDegrees / 2)).ToString().Replace(",", ".");

            longin = (MapSpan.Center.Longitude - (MapSpan.LongitudeDegrees / 2)).ToString().Replace(",", ".");
            longfin = (MapSpan.Center.Longitude + (MapSpan.LongitudeDegrees / 2)).ToString().Replace(",", ".");

            string query = "SELECT " +
                "*" +
                "FROM Rilevazioni, AnagraficaImpianto " +
                "WHERE (AnagraficaImpianto.idImpianto = Rilevazioni.idImpianto) " +
                "AND (descCarburante = '" + descCarburante.Items[descCarburante.SelectedIndex] + "') " +
                "AND (latitudine BETWEEN " + latin + " AND " + latfin + ")" +
                "AND (longitudine BETWEEN " + longin + " AND " + longfin + ")";
            List<GeneralItem> List;
            RemoteDBConnection conn = new RemoteDBConnection(query);
            if (conn.Connect() == 0)
            {
                DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");
                btn.IsVisible = true;
                btn.IsEnabled = true;
            }
            else
            {
                List = conn.ExecuteQueryWithResponse(query);
                SetMap SetMap = new SetMap(Map, List);
                SetMap.AddPin();
            }


            return query;

        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Info", "Ricaricare la pagina?", "Ok", "Annulla");
            if (res == true)
            {
                SetPage();
                btn.IsVisible = false;
            }

        }

        private async void lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Apri scheda impianto 
            //Apri nelle mappe
            GeneralListViewItem item = (GeneralListViewItem)lista.SelectedItem;
            try
            {

                await Navigation.PushModalAsync(new SchedaImpianto(item.idImpianto, item.tipo.Substring(6), (item.isSelf.Equals("Servito") ? "0" : "1")));

            }
            catch (Exception ex)
            {
                await DisplayAlert("Errore", ex.ToString(), "Ok");
            }

        }




    }
}