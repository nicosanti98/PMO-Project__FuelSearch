using System.Drawing;

namespace FuelSearch
{
    //Attributi presenti in un oggetto di una ListView completa
    class GeneralListViewItem
    {
        //Gli attributi possono anche non essere inizializzati, non modificando il 
        //comportamento della ListView
        public string logo { get; set; }
        public string prezzo { get; set; }
        public string bandiera { get; set; }
        public string isSelf { get; set; }
        public string indirizzo { get; set; }
        public string rilevazione { get; set; }
        public string tipo { get; set; }
        public string idImpianto { get; set; }
        public string latitudine { get; set; }
        public string longitudine { get; set; }

        public Color background { get; set; }
    }
}
