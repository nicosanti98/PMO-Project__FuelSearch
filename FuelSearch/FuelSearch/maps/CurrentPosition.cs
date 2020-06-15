using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace FuelSearch.Index
{
    class CurrentPosition
    {
        private Position attuale;

        public CurrentPosition()
        {
            TakePosition();
        }

        private async void TakePosition()
        {
            Location l = await Geolocation.GetLocationAsync();
            Position p = new Position(l.Latitude, l.Longitude);
            this.attuale = p;
        }

        public Position GetLastPosition()
        {
            return this.attuale;
        }
    }
}
