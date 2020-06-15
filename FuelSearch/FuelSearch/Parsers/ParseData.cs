namespace FuelSearch
{
    class ParseData : Parser
    {
        private string data;
        private string datainverted;


        //Riceve un array di stringhe composto rispettivamente da gg, mm, aaaa
        //Invertendone poi l'ordine affinche la stringa sia confrontabile con un'altra
        //della stessa tipologia
        //allo stesso modo di un numero intero
        public override void BuildObject(string[] parsedString)
        {
            string gg = parsedString[0];
            string mm = parsedString[1];
            string aaaa = parsedString[2];

            if (mm.Length == 1)
            {
                mm = "0" + mm;
            }
            if (gg.Length == 1)
            {
                gg = "0" + gg;
            }

            this.data = gg + mm + aaaa;
            this.datainverted = aaaa + mm + gg;
        }

        //Riceve una data nel formato aaaa/mm/gg e la splitta
        //secondo il carattere /
        public override string[] SplitString(string data)
        {
            string[] splitted = data.Split('/');
            return splitted;

        }

        public string TakeDataGGMMAAAA()
        {
            return this.data;
        }

        public string TakeDataAAAAMMGG()
        {
            return this.datainverted;
        }
    }
}
