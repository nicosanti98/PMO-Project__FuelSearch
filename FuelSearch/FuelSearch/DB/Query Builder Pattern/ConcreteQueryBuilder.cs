using System.Collections.Generic;

namespace FuelSearch.DB
{
    class ConcreteQueryBuilder : IQueryBuilder
    {
        private Query query;

        public ConcreteQueryBuilder()
        {
            this.Reset();
        }
        private void Reset()
        {
            this.query = new Query();
        }

        public void BuildQuery(List<string> list)
        {
            this.query.AddConditions(list);
        }

        public string TakeQuery()
        {
            Query res = query;
            this.Reset();
            return res.TakeQuery();

        }
    }
}
