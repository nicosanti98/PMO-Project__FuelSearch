using Microcharts;
using System;
using System.Collections.Generic;
using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;
namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedaImpianto : ContentPage
    {
        private string idImpianto;
        private string Tipo;
        private string isSelf;
        GeneralItem item = new GeneralItem();

        //Costruttore che riceve un idImpianto, il Tipo di Carburante e il Servuizio
        //In maniera tale da IDENTIFICARE UNIVOCAMENTE l'impianto associato
        //A quel carburante e a quel servizio
        public SchedaImpianto(string idImpianto, string tipo, string isSelf)
        {

            this.idImpianto = idImpianto;
            this.Tipo = tipo;
            this.isSelf = isSelf;
            InitializeComponent();
            Refresh.IsRefreshing = true;
            SetPage();

        }

        //Ricava la path del logo in base alla compagnia gestore dell'impianto
        private string SelectImageSource(string bandiera)
        {


            if (bandiera.Contains("Api-Ip"))
            {
                return "ip.png";
            }
            else if (bandiera.Contains("Eni"))
            {
                return "eni.png";
            }
            else if (bandiera.Contains("Erg"))
            {
                return "totalerg.png";
            }
            else if (bandiera.Contains("Esso"))
            {
                return "esso.png";
            }
            else if (bandiera.Contains("coop"))
            {
                return "enercoop.jpg";
            }
            else if (bandiera.Contains("Tamoil"))
            {
                return "tamoil.png";
            }
            else if (bandiera.Contains("Q8"))
            {
                return "q8.png";
            }
            else if (bandiera.Equals("Pompe Bianche"))
            {
                return "pompebianche.png";

            }
            else if (bandiera.Equals("Repsol"))
            {
                return "repsol.png";
            }
            else
            {
                return "unknown.png";
            }


        }

        //Metodo per ottenere l'anagrafica dell'item che ha le caratteristiche del costruttore
        private GeneralItem GetItem()
        {


            string Query = "SELECT * FROM Rilevazioni, AnagraficaImpianto WHERE AnagraficaImpianto.idImpianto = " + this.idImpianto + " AND AnagraficaImpianto.idImpianto = Rilevazioni.idImpianto AND descCarburante = '" + this.Tipo + "' AND isSelf = '" + this.isSelf + "'";
            RemoteDBConnection conn = new RemoteDBConnection(Query);

            List<GeneralItem> List = conn.ExecuteQueryWithResponse(Query);
            //Ritorna la lista abbinata contenenti sia gli attributi della classe Rilevazioni sia gli attributi della classe AnagraficaImpianto
            return List[0];

        }

        //Metodo che di fatto setta i parametri dei componenti 
        //Visuali della pagina Xamarin
        private void SetPage()
        {
            GeneralItem Item = new GeneralItem();
            Item = GetItem();

            string Img = SelectImageSource(Item.Bandiera);
            logo.Source = Img;

            title.Text = Item.NomeImpianto;
            subtitle.Text = "Di: " + Item.Gestore;
            Indirizzo.Text = Item.Indirizzo + ", " + Item.Comune + "(" + Item.Provincia + ")";
            Servizio.Text = (Item.isSelf.Equals("1")) ? "Self-Service" : "Servito";
            Prezzo.Text = Item.prezzo + " €";
            string Date = Item.dtComu.Substring(8, 2) + "/" + Item.dtComu.Substring(5, 2) + "/" + Item.dtComu.Substring(0, 4);
            Rilevazione.Text = Date;
            SetChart(Item);
        }

        //Metodo che setta i parametri del grafico
        private void SetChart(GeneralItem item)
        {
            List<Entry> Entries = new List<Entry>();
            List<GeneralItem> List = new List<GeneralItem>();
            WebClient wc = new WebClient();
            string query = "SELECT DISTINCT prezzo, dtComu FROM StoricoRilevazioni, AnagraficaImpianto WHERE StoricoRilevazioni.idImpianto = '" + item.idImpianto + "' AND StoricoRilevazioni.idImpianto = AnagraficaImpianto.idImpianto AND descCarburante = '" + item.descCarburante + "' AND isSelf = '" + item.isSelf + "'";
            RemoteDBConnection conn = new RemoteDBConnection(query);
            List = conn.ExecuteQueryWithResponse(query);



            //Ciclo per inserire nel grafico solo le rilevazioni degli ultimi 15 giorni
            for (int i = 0; i < List.Count; i++)
            {
                string Date;
                Date = List[i].dtComu.Substring(8, 2) + "/" + List[i].dtComu.Substring(5, 2) + "/" + List[i].dtComu.Substring(0, 4);
                Entry e = new Entry(float.Parse(List[i].prezzo));
                //Asse x
                e.Label = Date;
                //Colore linea
                e.Color = SkiaSharp.SKColor.Parse("#1E90FF");
                //Asse y
                e.ValueLabel = List[i].prezzo;

                Entries.Add(e);
            }

            //Crea il grafico
            Chart.Chart = new LineChart { Entries = Entries, LineMode = LineMode.Straight, BackgroundColor = SkiaSharp.SKColor.Parse("#FFFFFF"), PointSize = 5 };
            Refresh.IsRefreshing = false;

        }

        private async void OpenMap_Clicked(object sender, EventArgs e)
        {
            GeneralItem item = GetItem();
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");
            }
            else
            {
                await Map.OpenAsync(new Location(double.Parse(item.Latitudine.Replace(".", ",")), double.Parse(item.Longitudine.Replace(".", ","))));
            }

        }
    }
}