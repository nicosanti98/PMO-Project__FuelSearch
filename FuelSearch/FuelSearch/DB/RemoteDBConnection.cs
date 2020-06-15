using FuelSearch.DB;
using FuelSearch.Parsers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace FuelSearch
{
    class RemoteDBConnection : IDb
    {
        //Link all'endpoint che permette di eseguire la query
        private readonly string ENDPOINT = "https://www.kingofthecage.it/App/EndPoints/ContactDB.php?query=";

        //Contiene la/e queries
        private string query;
        //Contiene i dati scaricati
        private string Data;
        public RemoteDBConnection(string query)
        {
            //Sostituisce il carattere spazio e il carattere ' con i codici 
            //Affinchè siano leggibili come parametri di una URL
            this.query = query.Replace(" ", "%20").Replace("'", "%27");
        }



        //Metodi non implementati perchè gestiti dal server
        public int Connect()
        {
            try
            {
                WebClient wc = new WebClient();
                //Variabile contenente la stringa scaricata
                string res = wc.DownloadString(ENDPOINT + this.query);
                this.Data = res;
                return 1;
            }
            catch (Exception ex)
            {
                //Errore nel download. Ritorno 0
                return 0;
            }

        }

        public void DestroyConnection()
        {
            throw new NotImplementedException();
        }

        public void ExecuteQueryWithoutResponse(string query)
        {

        }

        public List<GeneralItem> ExecuteQueryWithResponse(string query)
        {
            List<GeneralItem> List = new List<GeneralItem>();
            this.query = query;
            Connect();
            //Creo l'oggetto parser per parsare la stringa in un oggetto JSON
            JSONParser Parser = new JSONParser(this.Data);
            JArray Obj = Parser.TakeJSON();


            //Riempio la lista di item con i valori ricavati dalla stringa
            for (int i = 0; i < Obj.Count; i++)
            {
                GeneralItem Item = new GeneralItem
                {
                    idImpianto = TryParse("idImpianto", Obj[i]),
                    Bandiera = TryParse("Bandiera", Obj[i]),
                    Comune = TryParse("Comune", Obj[i]),
                    descCarburante = TryParse("descCarburante", Obj[i]),
                    dtComu = TryParse("dtComu", Obj[i]),
                    Gestore = TryParse("Gestore", Obj[i]),
                    Indirizzo = TryParse("Indirizzo", Obj[i]),
                    isSelf = TryParse("isSelf", Obj[i]),
                    NomeImpianto = TryParse("NomeImpianto", Obj[i]),
                    prezzo = TryParse("prezzo", Obj[i]),
                    Provincia = TryParse("Provincia", Obj[i]),
                    TipoImpianto = TryParse("TipoImpianto", Obj[i]),
                    Latitudine = TryParse("Latitudine", Obj[i]),
                    Longitudine = TryParse("Longitudine", Obj[i])
                };

                List.Add(Item);


            }
            return List;
        }

        //Prova a ritornare la stringa contneuta nel campo 
        private string TryParse(string Field, JToken Obj)
        {
            try
            {
                return Obj[Field].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
