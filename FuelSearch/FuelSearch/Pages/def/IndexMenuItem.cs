using System;

namespace FuelSearch.Index
{
    //Classe che definisce i campi di un IndexMenuItem
    public class IndexMenuItem
    {
        public IndexMenuItem()
        {
            TargetType = typeof(IndexDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageSource { get; set; }

        public Type TargetType { get; set; }
    }
}