using System.Collections.Generic;

namespace FuelSearch.DB
{
    //Classe query contenente la definizione dell'oggetto query
    //implementato mediante il pattern Builder per la creazione 
    //Delle query
    class Query
    {
        private const string BASE_QUERY = "SELECT * FROM Rilevazioni, AnagraficaImpianto WHERE (AnagraficaImpianto.idImpianto = Rilevazioni.idImpianto) AND ";
        private string query = "";

        //Metodo che riempie la query
        public void AddConditions(List<string> condizioni)
        {
            query = BASE_QUERY;

            for (int i = 0; i < condizioni.Count; i++)
            {
                query += condizioni[i];
                if (i < condizioni.Count - 1)
                {
                    query += "AND";
                }
            }
        }


        public string TakeQuery()
        {
            return this.query;
        }
    }
}
