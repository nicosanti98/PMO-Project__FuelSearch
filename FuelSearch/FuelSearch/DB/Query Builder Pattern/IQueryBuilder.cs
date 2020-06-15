using System.Collections.Generic;

namespace FuelSearch
{
    //Interfaccia contenente i metodi per costruire la query
    interface IQueryBuilder
    {
        //Gli viene passato un array di stringhe rappresentanti le condizioni della query
        void BuildQuery(List<string> arr);
    }
}
