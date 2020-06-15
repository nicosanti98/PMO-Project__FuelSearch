using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace FuelSearch
{
    /***********************************************************************/
    /***********************************************************************
       Classe che permette, passatole una mappa e un item di inserire nella
       Mappa il segnaposto corrispondente a quell'item

     **********************************************************************/
    /**********************************************************************/
    class SetMap
    {
        //Attributo contenente la mappa passata mediante il costruttore
        private CustomMap map;

        //Attributo contenente la lista di item passata mediante il costruttore
        private List<GeneralItem> item;

        //Costruttore
        public SetMap(CustomMap map, List<GeneralItem> item)
        {
            this.map = map;
            this.item = item;
        }

        //Metodo pubblico che aggiunge i segnaposto alla mappa
        public void AddPin()
        {
            List<CustomPin> PinList = new List<CustomPin>();
            for (int i = 0; i < this.item.Count; i++)
            {
                CustomPin pin = new CustomPin()
                {
                    Label = this.item[i].prezzo + ", " + this.item[i].descCarburante,
                    Address = this.item[i].Bandiera + ",  " + this.item[i].Comune + " (" + this.item[i].Provincia + "), " + ((this.item[i].isSelf.Equals("0")) ? "Servito" : "Self Service"),
                    Position = new Position(double.Parse(this.item[i].Latitudine.Replace(".", ",")), double.Parse(this.item[i].Longitudine.Replace(".", ","))),
                    Type = PinType.Generic,
                    Bandiera = this.item[i].Bandiera
                };

                PinList.Add(pin);
                //Metodi che di fatto aggiungono alla mappa i segnaposto customizzati
                this.map.CustomPins = PinList;
                this.map.Pins.Add(pin);
            }



        }


    }
}
