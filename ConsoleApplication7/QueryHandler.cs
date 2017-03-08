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
        SentenceBoiler sb;
        DataAccess dA;

        public QueryHandler(
                            DataAccess dA,
                            SentenceBoiler sb,
                            Func<string, string> stringOp
                            ) //later associate results with
        {                                                                               //query
            this.dA = dA;
            this.sb = sb;
            this.stringOp = stringOp;
            this.queries = new List<Query>();
        }

        private void genQueries(string[] sentences)
        {
            QueryGenerator queryGen = new QueryGenerator(sb);
            foreach (string sentence in sentences)
            {
                queries.Add(queryGen.queryGen(sentence));
            }
        }

        private async Task<List<Tuple<string, string[]>>[]> sendOff()
        {
            List<Task<List<Tuple<string, string[]>>>> issuedQueries = new List<Task<List<Tuple<string, string[]>>>>();
            foreach (Query q in queries)
            {
               // foreach (Attrib a in q.attributeList)
                //{
                    
                    issuedQueries.Add
                    (
                            Task<List<Tuple<string, string[]>>>.Factory.StartNew
                            (                          
                             
                                () => {
                                        List<Tuple<string, string[]>> ltup = new List<Tuple<string, string[]>>();
                                        
                                        foreach (var kvpair in dA.query(q))
                                        {
                                            List<string> temp = new List<string>();
                                            foreach (var s in kvpair.Value)
                                            {
                                                temp.Add(s);
                                            }
                                            ltup.Add(new Tuple<string, string[]>(q.originalSentence, temp.ToArray()));
                                        }
                                        return ltup;
                                       }
                            )
                    );
                //}
                // Task.
            }
            return await Task.WhenAll(issuedQueries);
        }

        public List<List<Tuple<string, string[]>>> handleQuery(string[] sentences)
        {
            genQueries(sentences);
            List<List<Tuple<string, string[]>>> retList = new List<List<Tuple<string, string[]>>>();
            List<Tuple<string, string[]>>[] response = sendOff().Result;
            foreach (List<Tuple<string, string[]>> l in response)
            {
                List<Tuple<string, string[]>> lList = new List<Tuple<string, string[]>>();
                foreach (var r in l)
                {
                    //refactor
                    List<string> tempList = new List<string>();
                    foreach (string s in r.Item2)
                    {
                        tempList.Add(stringOp(s));
                    }
                    //refactor
                    lList.Add(new Tuple<string, string[]>(r.Item1, tempList.ToArray()));//stringOp(r.Item2)));
                }
                retList.Add(lList);
            }
            return retList;
        }
    }
}
