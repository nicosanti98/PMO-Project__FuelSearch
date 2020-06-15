
using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using FuelSearch;

namespace FuelPrice.Droid
{
    [Activity(Label = "FuelSearch", Icon = "@drawable/icona", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation,
                Manifest.Permission.WriteExternalStorage

            };
        readonly string[] WriteStoragePermissions =
        {

        };
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);


            MobileAds.Initialize(ApplicationContext, "ca-app-pub-9362856343758559~3332382688");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);

            //Richiedo i permessi di accesso alla posizione e al FileSystem
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted && CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                {

                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    //Se il GPS è disattivato all'avvio dell'app ne richiedo l'attivazione
                    LocationManager manager = (LocationManager)GetSystemService(LocationService);
                    if (!manager.IsProviderEnabled(LocationManager.GpsProvider))
                    {
                        StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                    }
                    LoadApplication(new App());
                }

            }






        }

        protected override void OnStart()
        {

            base.OnStart();

        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {

                }
                else
                {

                    LocationManager manager = (LocationManager)GetSystemService(LocationService);
                    if (!manager.IsProviderEnabled(LocationManager.GpsProvider))
                    {
                        StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                    }
                }

            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            LoadApplication(new App());

        }
    }
}