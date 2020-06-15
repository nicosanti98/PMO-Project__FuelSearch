using Xamarin.Forms.Maps;

namespace FuelSearch
{
    //La classe CustomPin è una classe derivata della classe Pin
    public class CustomPin : Pin
    {
        //Gli attributi che differenziano il Pin dal Custom Pin sono:

        //La compagnia che gestisce il distributore rappresentato dal segnaposto
        public string Bandiera { get; set; }
    }
}
