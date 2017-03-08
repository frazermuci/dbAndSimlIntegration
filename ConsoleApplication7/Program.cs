using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

//using Trainer.

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Func<string, string> func = new Func<string, string>((x) =>
            {
                var temp = x;
                new HTMLMessager().removeFromLine(ref temp);
                return temp;
            });
            SentenceBoiler sb = SentenceBoiler.sentenceBoilerFactory("UserBot");
			DataAccess dA = new DataAccessDB();
            while (true)
            {
                var paragraph = Console.ReadLine();
                var modelPath = "..\\..\\..\\Resources\\Models";
                var sentences = paragraph.Split('.');
                var qHandler = new QueryHandler(dA,sb, func);
                int max = -99;
                string[] maxStrings = new string[] {""};
                foreach (var response in qHandler.handleQuery(sentences))
                {
                    string query = "";
                    foreach (var block in response)
                    {
                        query = block.Item1;
                        var kwList = KeyWordFinder.handleLineKeyWords(5, block.Item1);
                        int occurance = 0;
                        foreach (string s in block.Item2)
                        {

                            foreach (string kw in kwList)
                            {
                                occurance = occurance + Regex.Matches(s, kw).Count;
                            }

                        }
                        maxStrings = occurance > max ? block.Item2 : maxStrings;//average hits per element
                        max = occurance > max ? occurance : max;
                    }
                    Console.WriteLine("Hits for query: " + "\"" + query + "\"");
                    foreach (string s in maxStrings)
                    {
                        Console.WriteLine(s);
                    }
                    Console.WriteLine();
                }   
            }
        }
    }
}
