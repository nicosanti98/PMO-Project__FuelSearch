using Newtonsoft.Json.Linq;
using System;

namespace FuelSearch.Parsers
{
    class JSONParser : Parser
    {
        //Variabile contenente l'oggetto json
        private JArray obj;
        public override void BuildObject(string[] parsedString)
        {
            throw new NotImplementedException();
        }

        public override string[] SplitString(string data)
        {
            throw new NotImplementedException();
        }

        //Metodo che riceve una stringa e la parse in un oggetto JSON
        public JSONParser(string JSONstring)
        {
            JArray obj = JArray.Parse(JSONstring);
            this.obj = obj;

        }
        public JArray TakeJSON()
        {
            return this.obj;
        }


    }
}
