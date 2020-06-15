using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using FuelSearch;
using FuelSearch.Droid;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace FuelSearch.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }


        protected override MarkerOptions CreateMarker(Pin pin)
        {

            var marker = new MarkerOptions();
            CustomPin cp = (CustomPin)pin;

            if (cp.Bandiera.Contains("Api-Ip"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ippin));
            }
            else if (cp.Bandiera.Contains("Eni"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.enipin));
            }
            else if (cp.Bandiera.Contains("Erg"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.totalergpin));
            }
            else if (cp.Bandiera.Contains("Esso"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.essopin));
            }
            else if (cp.Bandiera.Contains("coop"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.enercoopin));
            }
            else if (cp.Bandiera.Contains("Tamoil"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.tamoilpin));
            }
            else if (cp.Bandiera.Contains("Q8"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.q8pin));
            }
            else if (cp.Bandiera.Equals("Pompe Bianche"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pompebianchepin));

            }
            else if (cp.Bandiera.Equals("Repsol"))
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.repsolpin));
            }
            else
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.unknownpin));
            }
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {

            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);

            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }

            }
            return null;
        }
    }
}