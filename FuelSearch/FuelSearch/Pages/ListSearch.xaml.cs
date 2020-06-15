using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelSearch.Index
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListSearch : ContentPage
    {

        string query;

        //Costruttore, a cui viene passata una query
        public ListSearch(string query)
        {
            this.query = query;
            InitializeComponent();


        }


        //Riempie la lista con glie elementi ottenuti dalla query
        protected override void OnAppearing()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DisplayAlert("Errore", "Errore di connessione. Connettiti alla rete e riprova.", "Ok");

            }
            else
            {
                ListViewFiller l = new ListViewFiller(this.query, lista);
                l.AddToListView();

                Count.Text = "Risultati ottenuti: " + l.TakeListLenght();
            }


        }

        private async void Lista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            ///Apri scheda impianto 
            //Apri nelle mappe
            GeneralListViewItem item = (GeneralListViewItem)lista.SelectedItem;
            try
            {

                await Navigation.PushModalAsync(new SchedaImpianto(item.idImpianto, item.tipo.Substring(6), (item.isSelf.Equals("Servito") ? "0" : "1")));

            }
            catch (Exception ex)
            {
                await DisplayAlert("Errore", ex.ToString(), "Ok");
            }

        }




    }
}