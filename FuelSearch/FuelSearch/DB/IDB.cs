using System.Collections.Generic;

namespace FuelSearch.DB
{
    //Interfaccia IDb che fornisce 4 metodi base per l'interazione
    //con dei DB. In questo progetto viene usata solamente per la comunicazione con
    //un DB remoto, ma grazie a questa interfaccia è possibile l'implementazione 
    //di una classe per la comunicazione con un db locale
    interface IDb
    {
        List<GeneralItem> ExecuteQueryWithResponse(string query);
        void ExecuteQueryWithoutResponse(string query);
        int Connect();
        void DestroyConnection();
    }
}
