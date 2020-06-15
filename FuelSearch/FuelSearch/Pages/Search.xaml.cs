
using FuelSearch.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        List<GeneralItem> List = new List<GeneralItem>();


        public Search()
        {
            WebClient wc = new WebClient();

            InitializeComponent();

            AdMob.AdUnitId = "ca-app-pub-9362856343758559/4142290062";

            //Rileva dal DB tutte le provincie contenute
            string query = "SELECT DISTINCT Provincia FROM AnagraficaImpianto";
            RemoteDBConnection conn = new RemoteDBConnection(query);
            if (conn.Connect() == 0)
            {
                DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");
            }
            else
            {
                List<GeneralItem> List = conn.ExecuteQueryWithResponse(query);
                for (int i = 0; i < List.Count; i++)
                {
                    pkrProvincia.Items.Add(List[i].Provincia.ToString());
                }
                pkrProvincia.Items.Add("--");
                //Rileva dal DB tutte le tipologie di carburante
                query = "SELECT DISTINCT descCarburante FROM Rilevazioni";
                List = conn.ExecuteQueryWithResponse(query);
                for (int i = 0; i < List.Count; i++)
                {
                    pkrTipo.Items.Add(List[i].descCarburante.ToString());
                }
                pkrTipo.Items.Add("--");

                query = "SELECT DISTINCT Bandiera FROM AnagraficaImpianto";
                pkrBandiera.Items.Add("--");
                List = conn.ExecuteQueryWithResponse(query);
                for (int i = 0; i < List.Count; i++)
                {
                    pkrBandiera.Items.Add(List[i].Bandiera.ToString());
                }
                pkrSel.Items.Add("--");
                pkrSel.Items.Add("Self-Service");
                pkrSel.Items.Add("Servito");
            }
        }

        //Quando il PkrProvincia viene selezionato, 
        //Viene rilevata la Lista di comuni associati a quella provincia e inseriti nel PkrComune
        private void PkrProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 0; pkrComune.Items.Count() != 0; i++)
            {
                pkrComune.Items.RemoveAt(i);
                i--;
            }
            string query = "SELECT DISTINCT Comune FROM AnagraficaImpianto WHERE Provincia = '" + pkrProvincia.Items[pkrProvincia.SelectedIndex] + "'";
            pkrComune.Items.Add("--");
            RemoteDBConnection conn = new RemoteDBConnection(query);
            if (conn.Connect() == 0)
            {
                DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");
            }
            else
            {
                List<GeneralItem> List = conn.ExecuteQueryWithResponse(query);
                for (int i = 0; i < List.Count; i++)
                {
                    pkrComune.Items.Add(List[i].Comune.ToString());
                }
            }


        }


        //Metodo che sceglie la query in base a quali dei 4 pkr sia selezionato
        //E apre una scheda ListSearch passandogli la query scelta
        private async void Btncerca_Clicked(object sender, EventArgs e)
        {
            //Vista la complessità nella costruzione delle query in base ai picker selezionato, 
            //viene in questo caso utilizzato il builder pattern
            var director = new QueryDirector();
            var builder = new ConcreteQueryBuilder();
            List<string> parameters = new List<string>();
            string query;
            director.Builder = builder;

            if (pkrProvincia.SelectedIndex == -1 || pkrProvincia.Items[pkrProvincia.SelectedIndex].Equals("--"))
            {
                DisplayAlert("Errore", "Scegliere almeno la Provincia", "Ok");

            }
            else
            {
                parameters.Add("(Provincia = '" + pkrProvincia.Items[pkrProvincia.SelectedIndex] + "')");
                if (pkrComune.SelectedIndex != -1 && !(pkrComune.Items[pkrComune.SelectedIndex].ToString().Equals("--")))
                {
                    parameters.Add("(Comune = '" + pkrComune.Items[pkrComune.SelectedIndex] + "')");
                }
                if (pkrTipo.SelectedIndex != -1 && !(pkrTipo.Items[pkrTipo.SelectedIndex].ToString().Equals("--")))
                {
                    parameters.Add("(descCarburante = '" + pkrTipo.Items[pkrTipo.SelectedIndex].ToString() + "')");
                }
                if (pkrSel.SelectedIndex != -1 && !(pkrSel.Items[pkrSel.SelectedIndex].ToString().Equals("--")))
                {
                    parameters.Add("(isSelf = '" + (pkrSel.Items[pkrSel.SelectedIndex].Equals("Self-Service") ? ("1") : ("0")) + "')");
                }
                if (pkrBandiera.SelectedIndex != -1 && !(pkrBandiera.Items[pkrBandiera.SelectedIndex].ToString().Equals("--")))
                {
                    parameters.Add("(Bandiera = '" + (pkrBandiera.Items[pkrBandiera.SelectedIndex].ToString()) + "')");
                }

                //Viene creata la query grazie al BUILDER PATTERN
                builder.BuildQuery(parameters);
                query = builder.TakeQuery();

                await Navigation.PushModalAsync(new ListSearch(query));

            }

           


        }



    }

}