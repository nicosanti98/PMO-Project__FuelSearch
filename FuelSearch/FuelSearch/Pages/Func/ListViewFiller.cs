using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace FuelSearch.Index
{
    class ListViewFiller
    {
        ObservableCollection<GeneralListViewItem> Listviewitems = new ObservableCollection<GeneralListViewItem>();
        private string Query;
        private ListView Listview;

        private int len;

        //Costruttore che riceve una ListView e una Query
        public ListViewFiller(string query, ListView list)
        {
            this.Query = query;
            this.Listview = list;
        }


        //Metodo per scegliere il logo in base alla compagnia del distributore
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
                return "enercoop.png";
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

        private List<GeneralItem> TakeList(string query)
        {
            RemoteDBConnection conn = new RemoteDBConnection(query);
            if (conn.Connect() == 0)
            {
                return null;
            }
            else
            {
                List<GeneralItem> List = conn.ExecuteQueryWithResponse(query);
                return List;
            }

        }


        //Aggiunge la lista di GeneralItem alla grafica
        public void AddToListView()
        {
            for (int i = 0; Listviewitems.Count != 0; i++)
            {
                Listviewitems.RemoveAt(i);
                i--;
            }
            this.Listview.ItemsSource = Listviewitems;
            List<GeneralItem> List;
            List = TakeList(Query);

            //Variabili contenenti l'indice contenente prezzo massimo e prezzo minimo 
            //contenuti nella lista
            int MaxIndex = 0;
            int MinIndex = 0;

            ObservableCollection<GeneralListViewItem> ItemsTopAndFlop = new ObservableCollection<GeneralListViewItem>();
            //Rilevo gli indici massimi e minimi

            for (int i = 0; i < List.Count; i++)
            {
                if (Double.Parse(List[i].prezzo) > Double.Parse(List[MaxIndex].prezzo))
                {
                    MaxIndex = i;

                }
                if (Double.Parse(List[i].prezzo) < Double.Parse(List[MinIndex].prezzo))
                {
                    MinIndex = i;
                }
            }

            for (int i = 0; i < List.Count; i++)
            {
                if (i == MinIndex)
                {
                    string Date;
                    Date = List[i].dtComu.Substring(8, 2) + "/" + List[i].dtComu.Substring(5, 2) + "/" + List[i].dtComu.Substring(0, 4);
                    string img = SelectImageSource(List[i].Bandiera);
                    //Aggiunge l'oggetto GeneralListViewItem così inizializzato
                    ItemsTopAndFlop.Add(new GeneralListViewItem { background = Color.FromHex("80FF80"), idImpianto = List[i].idImpianto, tipo = "Tipo: " + List[i].descCarburante, prezzo = List[i].prezzo, bandiera = List[i].Bandiera.ToString(), rilevazione = "Ultima rilevazione: " + Date, indirizzo = List[i].Indirizzo, isSelf = (List[i].isSelf.Equals("1")) ? "Self-Service" : "Servito", logo = img, latitudine = List[i].Latitudine.ToString(), longitudine = List[i].Longitudine.ToString() });

                }
                else
                {
                    if (i == MaxIndex)
                    {
                        string Date;
                        Date = List[i].dtComu.Substring(8, 2) + "/" + List[i].dtComu.Substring(5, 2) + "/" + List[i].dtComu.Substring(0, 4);
                        string img = SelectImageSource(List[i].Bandiera);
                        //Aggiunge l'oggetto GeneralListViewItem così inizializzato
                        ItemsTopAndFlop.Add(new GeneralListViewItem
                        {
                            background = Color.FromHex("FF8080"),
                            idImpianto = List[i].idImpianto,
                            tipo = "Tipo: " + List[i].descCarburante,
                            prezzo = List[i].prezzo,
                            bandiera = List[i].Bandiera.ToString(),
                            rilevazione = "Ultima rilevazione: " + Date,
                            indirizzo = List[i].Indirizzo,
                            isSelf = (List[i].isSelf.Equals("1")) ? "Self-Service" : "Servito",
                            logo = img,
                            latitudine = List[i].Latitudine.ToString(),
                            longitudine = List[i].Longitudine.ToString()
                        });
                    }
                    else
                    {
                        string Date;
                        Date = List[i].dtComu.Substring(8, 2) + "/" + List[i].dtComu.Substring(5, 2) + "/" + List[i].dtComu.Substring(0, 4);
                        string img = SelectImageSource(List[i].Bandiera);
                        //Aggiunge l'oggetto GeneralListViewItem così inizializzato
                        Listviewitems.Add(new GeneralListViewItem
                        {
                            background = Color.Default,
                            idImpianto = List[i].idImpianto,
                            tipo = "Tipo: " + List[i].descCarburante,
                            prezzo = List[i].prezzo,
                            bandiera = List[i].Bandiera.ToString(),
                            rilevazione = "Ultima rilevazione: " + Date,
                            indirizzo = List[i].Indirizzo,
                            isSelf = (List[i].isSelf.Equals("1")) ? "Self-Service" : "Servito",
                            logo = img,
                            latitudine = List[i].Latitudine.ToString(),
                            longitudine = List[i].Longitudine.ToString()
                        });

                    }
                }
            }
            //Ciclo che mette all'inizio l'impianto migliore e l'impianto peggiore
            for (int i = 0; i < Listviewitems.Count; i++)
            {
                ItemsTopAndFlop.Add(Listviewitems[i]);
            }
            this.Listview.ItemsSource = ItemsTopAndFlop;
            this.len = ItemsTopAndFlop.Count;
        }


        //Ritorna il numero di elementi della lista
        public int TakeListLenght()
        {
            return len;
        }


    }
}
