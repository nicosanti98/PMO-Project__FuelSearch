using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contatti : ContentPage
    {
        public Contatti()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Al click del bottone viene aperta l'applicazione per mandare una mail
                //Al mio indirizzo personale
                var message = new EmailMessage
                {
                    Subject = Oggetto.Text,
                    Body = Text.Text,
                    To = { "nicolo_santini@live.it" }
                };
                await Email.ComposeAsync(message);

            }
            catch (Exception ex)
            {
                DisplayAlert("Errore", ex.ToString(), "Ok");
            }

        }

        private void Oggetto_Focused(object sender, FocusEventArgs e)
        {
            Oggetto.Text = "";
        }


        //**********************************************//
        /*Metodi necessari per ripristinare le descrizioni
         * Dei campi compilabili sulle azioni di focus/unfocus*/
        //***********************************************//
        private void Oggetto_Unfocused(object sender, FocusEventArgs e)
        {
            if (Oggetto.Text.Equals(""))
            {
                Oggetto.Text = "Oggetto";
            }
        }

        private void Text_Focused(object sender, FocusEventArgs e)
        {
            Text.Text = "";

        }

        private void Text_Unfocused(object sender, FocusEventArgs e)
        {
            if (Text.Text.Equals(""))
            {
                Text.Text = "Messaggio";
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}