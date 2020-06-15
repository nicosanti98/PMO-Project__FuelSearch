using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace FuelSearch
{
    //La classe Custom Map, è derivata della classe Map
    public class CustomMap : Map
    {
        //Attributo pubblico Lista di oggetti CustomPin
        public List<CustomPin> CustomPins { get; set; }
    }
}
