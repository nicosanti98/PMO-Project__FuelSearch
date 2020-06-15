using System.Collections.Generic;

namespace FuelSearch.DB
{
    class QueryDirector
    {
        private IQueryBuilder builder;
        public IQueryBuilder Builder { set { builder = value; } }



        public void BuildQueryWithConditions(List<string> conditions) { builder.BuildQuery(conditions); }

    }
}
