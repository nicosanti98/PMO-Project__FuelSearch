using FuelSearch.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IndexDetail : ContentPage
    {
        //Attributi privati della classe IndexDetail


        //Variabile contenente il raggio che verrà riempita mediante il costruttore
        private double Radius;
        //Variabile contenente la Posizione Attuale, verrà inizializzata mediante il costruttore
        private Position CurrentPosition;


        //Variabile contenente la data massima entro la quale cercare le rilevazioni per la pagina principale
        string Date;

        //Costruttore
        public IndexDetail(double Radius)
        {

            InitializeComponent();
            Refresh.IsRefreshing = true;
            this.Radius = Radius;

            //Setta i valori iniziali dello slider e del testo associato in base al raggio passato per parametro
            //Al costruttore
            slider.Value = this.Radius / 10;
            labl.Text = "Raggio: " + this.Radius + " km";

            /*Poichè successivamente l'app baserà il suo funzionamento in base all'orario di ultima modifica
             * del file contenente i dati, setto a ora la data di ultima modifica di quel file*/


            //All'avvio sposto la mappa in un punto al centro dell'Italia con un raggio di 750 km
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(41.650160, 12.808320), Distance.FromKilometers(750)));

            //Questo gruppo di istruzioni permette di parsare la data in formato ggmmaaaa, 
            //In maniera tale da corrispondere alla data presente nel campo dtComu della classe rilevazioni, 
            //anch'essa di questo formato. Tale formato permette di confrontare le date come se fossero numeri
            //(ad es. 12042020 > 12032020, quindi la prima data è successiva alla seconda
            this.Date = DateTime.Today.Subtract(TimeSpan.FromDays(3)).Year + "-" + DateTime.Today.Subtract(TimeSpan.FromDays(3)).Month + "-" + DateTime.Today.Subtract(TimeSpan.FromDays(3)).Day;


            //Rimozione segnaposti ancora presenti nella mappa
            for (int i = 0; Map.Pins.Count != 0; i++)
            {
                Map.Pins.RemoveAt(i);
                i--;
            }
            SetMapAndList();


        }


        //Metodo fondamentale che costruisce la mappa e le liste
        //Ricavando i dati dal database e organizzandoli in maniera tale
        //Da ottenere la grafica
        private async void SetMapAndList()
        {

            //Tenta di ricavare la posizione attuale, valore di default con latitudine = 0 e longitudine = 0
            try
            {
                Location Loc = await Geolocation.GetLocationAsync();
                this.CurrentPosition = new Position(Loc.Latitude, Loc.Longitude);
            }
            catch (Exception ex)
            {
                this.CurrentPosition = new Position(0, 0);
            }


            //Setta la mappa centrandola nella posizione attuale facendo vedere un raggio pari a raggio
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(CurrentPosition.Latitude, CurrentPosition.Longitude), Distance.FromKilometers(this.Radius)));
            MapSpan Span = MapSpan.FromCenterAndRadius(this.CurrentPosition, Distance.FromKilometers(this.Radius));


            if ((this.CurrentPosition.Longitude == 0 && this.CurrentPosition.Latitude == 0))
            {
                //Facciamo si che l'utente possa ricaricare la pagina mostradogli
                //l'apposito bottone
                btn.IsVisible = true;
            }
            else
            {



                //Lista contenente le rilevazioni da aggiungere alla mappa
                List<GeneralItem> ListToAdd = new List<GeneralItem>();

                /*****************************/
                /*Ricavo le latitudine e le longitudini 
                 * minime e massime, partendo dal centro*/
                /*****************************/
                string latin, latfin, longin, longfin;
                latin = (Span.Center.Latitude - (Span.LatitudeDegrees / 2)).ToString().Replace(",", ".");
                latfin = (Span.Center.Latitude + (Span.LatitudeDegrees / 2)).ToString().Replace(",", ".");

                longin = (Span.Center.Longitude - (Span.LongitudeDegrees / 2)).ToString().Replace(",", ".");
                longfin = (Span.Center.Longitude + (Span.LongitudeDegrees / 2)).ToString().Replace(",", ".");
                try
                {
                    List<string> list = new List<string>();

                    string query = BuildQuery("MAX", "2.7", Date, longin, longfin, latin, latfin, "Gasolio");
                    list.Add(query);
                    query = BuildQuery("MAX", "2.7", Date, longin, longfin, latin, latfin, "Benzina");
                    list.Add(query);
                    query = BuildQuery("MAX", "1.5", Date, longin, longfin, latin, latfin, "Metano");
                    list.Add(query);
                    query = BuildQuery("MAX", "1.5", Date, longin, longfin, latin, latfin, "GPL");
                    list.Add(query);
                    string[] queries = list.ToArray();

                    //Funzione che di fatto è deputata a settare tutte le componenti della Home Page
                    SetHomePage(queries, listFlop, latin, longin, latfin, longfin);


                }
                catch (Exception ex)
                {
                    await DisplayAlert("Attenzione", "Non sono disponibili prezzi migliori per tutte le categorie di carburante in un raggio così ridotto", "Ok");
                }
                try
                {
                    List<string> list = new List<string>();

                    string query = BuildQuery("MIN", "1", Date, longin, longfin, latin, latfin, "Gasolio");
                    list.Add(query);
                    query = BuildQuery("MIN", "1", Date, longin, longfin, latin, latfin, "Benzina");
                    list.Add(query);
                    query = BuildQuery("MIN", "0.3", Date, longin, longfin, latin, latfin, "Metano");
                    list.Add(query);
                    query = BuildQuery("MIN", "0.3", Date, longin, longfin, latin, latfin, "GPL");
                    list.Add(query);

                    string[] queries = list.ToArray();

                    SetHomePage(queries, listTop, latin, longin, latfin, longfin);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Attenzione", "Non sono disponibili prezzi peggiori per tutte le categorie di carburante in un raggio così ridotto", "Ok");
                }


                //Faccio si che la mappa mostri la posizione attuale
                Map.IsShowingUser = true;
                Refresh.IsRefreshing = false;
            }


        }

        private async void SetHomePage(string[] queries, ListView list, string latin, string longin, string latfin, string longfin)
        {
            //Lista contenente le rilevazioni da aggiungere alla mappa
            List<GeneralItem> ListToAdd = new List<GeneralItem>();
            ObservableCollection<GeneralListViewItem> listviewitems = new ObservableCollection<GeneralListViewItem>();
            //Aggiunge a una lista la lista dei migliori distributori nel range latitudine-longitudine selezionato

            for (int i = 0; i < queries.Length; i++)
            {
                List<GeneralItem> Item = new List<GeneralItem>();
                RemoteDBConnection conn = new RemoteDBConnection(queries[i]);
                if (conn.Connect() == 0)
                {
                    await DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");
                    btn.IsVisible = true;
                    btn.IsEnabled = true;
                    SetMapAndList();
                    break;


                }
                else
                {
                    try
                    {
                        Item = conn.ExecuteQueryWithResponse(queries[i]);
                        ListToAdd.Add(Item[0]);
                    }
                    catch
                    {
                        await DisplayAlert("Errore", "Non sono presenti dati sufficienti per effettuare un confronto in un raggio così piccolo", "Ok");
                    }

                }

            }

            for (int i = 0; i < ListToAdd.Count; i++)
            {
                //Richiamo la funzione che ritorna la path dell'immagine a seconda del tipo
                //di carburante della rilevazione
                string path = GetLogo(ListToAdd[i].descCarburante);

                //Riempo l'item con tutti i valori richiesti affinchè la lista abbia
                //Elementi sufficienti per riempire la scheda dell'impianto e aprire le mappe
                GeneralListViewItem item = new GeneralListViewItem()
                {
                    idImpianto = ListToAdd[i].idImpianto,
                    logo = path,
                    prezzo = ListToAdd[i].prezzo,
                    indirizzo = ListToAdd[i].Bandiera + " - " + ListToAdd[i].Comune + " (" + ListToAdd[i].Provincia + ")",
                    latitudine = ListToAdd[i].Latitudine.ToString(),
                    longitudine = ListToAdd[i].Longitudine.ToString(),
                    tipo = ListToAdd[i].descCarburante,
                    isSelf = ListToAdd[i].isSelf

                };

                listviewitems.Add(item);
            }
            list.ItemsSource = listviewitems;

            //Classe e metodo per aggiungere, a partire da una lista,
            //I pin in una mappa
            SetMap setMap = new SetMap(Map, ListToAdd);
            setMap.AddPin();
        }

        //Metodo che restituisce la path al giusto logo in base al tipo di carburante
        private string GetLogo(string tipo)
        {
            if (tipo == "Benzina")
            {
                return "logobenzina.png";
            }
            else if (tipo == "Gasolio")
            {
                return "logodiesel.png";
            }
            else if (tipo == "Metano")
            {
                return "logometano.jpg";
            }
            else return "logogpl.png";
        }

       
        //Metodo che costruisce le query per ricevere gli elementi della pagina principale
        private string BuildQuery(string MaxOMin, string PrezzoLim, string Date, string longin, string longfin, string latin, string latfin, string descCarburante)
        {
            string query = ""; 
            if(MaxOMin.Equals("MAX"))
            {
                query = "SELECT " +
                            "* " +
                            "FROM Rilevazioni, AnagraficaImpianto " +
                            "WHERE (AnagraficaImpianto.idImpianto = Rilevazioni.idImpianto)" +
                            "AND prezzo=(SELECT MAX(prezzo) FROM Rilevazioni, AnagraficaImpianto WHERE" +
                            "               (AnagraficaImpianto.idImpianto = Rilevazioni.idImpianto)" +
                                            "AND (Latitudine BETWEEN " + latin + " AND " + latfin + ")" +
                                            "AND (Longitudine BETWEEN " + longin + " AND " + longfin + ")" +
                                            "AND (dtComu > '" + Date + "')" +
                                            "AND (prezzo < "+PrezzoLim+") " +
                                            "AND (descCarburante = '" + descCarburante + "'))" +
                            "AND (Latitudine BETWEEN " + latin + " AND " + latfin + ")" +
                            "AND (Longitudine BETWEEN " + longin + " AND " + longfin + ")" +
                            "AND (dtComu > '" + Date + "')" +
                            "AND (prezzo < " + PrezzoLim + ") " +
                            "AND (descCarburante = '" + descCarburante + "')";
            }
            else
            {
                query = "SELECT " +
                            "* " +
                            "FROM Rilevazioni, AnagraficaImpianto " +
                            "WHERE (AnagraficaImpianto.idImpianto = Rilevazioni.idImpianto)" +
                            "AND prezzo = (SELECT MIN(prezzo) FROM Rilevazioni, AnagraficaImpianto WHERE" +
                            "               (AnagraficaImpianto.idImpianto = Rilevazioni.idImpianto)" +
                            "               AND (Latitudine BETWEEN " + latin + " AND " + latfin + ")" +
                                            "AND (Longitudine BETWEEN " + longin + " AND " + longfin + ")" +
                                            "AND (dtComu > '" + Date + "')" +
                                            "AND (prezzo > " + PrezzoLim + ") " +
                                            "AND (descCarburante = '" + descCarburante + "'))" +
                            "AND (latitudine BETWEEN " + latin + " AND " + latfin + ")" +
                            "AND (longitudine BETWEEN " + longin + " AND " + longfin + ")" +
                            "AND (dtComu > '" + Date + "')" +
                            "AND (prezzo > " + PrezzoLim + ") " +
                            "AND (descCarburante = '" + descCarburante + "')";
            }

            return query;
        }






        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Info", "Ricaricare la pagina?", "Ok", "Annulla");
            if (res == true)
            {
                SetMapAndList();
            }

        }

        private async void ListTop_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Apri mappe
            //Apri scheda impianto

            GeneralListViewItem item = (GeneralListViewItem)listTop.SelectedItem;
            try
            {

                await Navigation.PushModalAsync(new SchedaImpianto(item.idImpianto, item.tipo, item.isSelf));

            }
            catch (Exception ex)
            {
                await DisplayAlert("Errore", ex.ToString(), "Ok");
            }
        }

        private async void ListFlop_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Apri mappe
            //Apri scheda impianto

            GeneralListViewItem item = (GeneralListViewItem)listFlop.SelectedItem;
            try
            {
                await Navigation.PushModalAsync(new SchedaImpianto(item.idImpianto, item.tipo, item.isSelf));

            }
            catch (Exception ex)
            {
                await DisplayAlert("Errore", "Impossibile aprire Scheda Impianto", "Ok");
            }


        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Refresh.IsRefreshing = true;
            for (int i = 0; Map.Pins.Count != 0; i++)
            {
                Map.Pins.RemoveAt(i);
                i--;
            }
            listFlop.ItemsSource = null;
            listTop.ItemsSource = null;
            SetMapAndList();

        }

        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            slider.Value = Math.Round(e.NewValue, 1);
            labl.Text = "Raggio: " + slider.Value * 10 + " km";
            this.Radius = slider.Value * 10;
            MapSpan m = MapSpan.FromCenterAndRadius(CurrentPosition, Distance.FromKilometers(this.Radius));
            Map.MoveToRegion(m);
        }


    }

}







