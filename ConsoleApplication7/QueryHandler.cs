using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class QueryHandler
    {
        Func<string, string> stringOp;
        List<Query> queries;
        KeyWordFinder kwf;
		DataAccess dA;

        public QueryHandler(
							DataAccess dA,
							KeyWordFinder kwf, 
							Func<string, string> stringOp
							) //later associate results with
        {                                                                               //query
            this.dA = dA;
            this.kwf = kwf;
			this.stringOp = stringOp;
            this.queries = new List<Query>();
        }

        private void genQueries(string[] sentences)
        {
            QueryGenerator queryGen = new QueryGenerator(kwf);
            foreach (string sentence in sentences)
            {
                queries.Add(queryGen.queryGen(sentence));
            }
        }

        private async Task<Tuple<string,string>[]> sendOff()
        {
            List<Task<Tuple<string,string>>> issuedQueries = new List<Task<Tuple<string,string>>>();
            foreach (Query q in queries)
            {
				foreach(Attrib a in q.attributeList)
				{
					issuedQueries.Add
					(
							Task<Tuple<string,string>>.Factory.StartNew
							(
								()=>{ return new Tuple<string,string>(a.value, string.Join("\n",dA.query(q).Values)); }
							)
					);
				}
               // Task.
            }
            return await Task.WhenAll(issuedQueries);
        }

        public List<Tuple<string,string>> handleQuery(string[] sentences)
        {
            genQueries(sentences);
            List<Tuple<string,string>> retList = new List<Tuple<string,string>>();
            Tuple<string,string>[] response = sendOff().Result;
            foreach (Tuple<string,string> r in response)
            {
               // string temp = r;
                //new HTMLMessager().removeFromLine(ref temp);//re encapsulate in return val
                retList.Add(new Tuple<string, string>(r.Item1, stringOp(r.Item2)));
            }
            return retList;
        }
    }
}
