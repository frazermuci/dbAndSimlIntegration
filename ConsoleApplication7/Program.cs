using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
//using Trainer.

namespace Test
{
    class Program
    {
        /*public static void genModel(string filePath, string modelDest)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.Load(filePath);
            Parser.ParsedCHM parsed = new Parser.ParsedCHM(htmlDoc);
            List<List<Parser.Element>> blocks = parsed.blocks;

            using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(modelDest))
            {
                foreach (List<Parser.Element> elements in blocks)
                {
                    foreach (Parser.Element e in elements)
                    {
                        if(e.name != "img")
                        {
                            file.WriteLine("<START:" + parsed.title + "> " + e.data+" <END>");
                        }
                    }
                }
            }
        }*/

        static void Main(string[] args)
        {
            /*  var trainingFile = "C:\\Users\\Matthew\\Desktop\\train\\genModel";
              //genModel("C:\\Users\\Matthew\\Desktop\\TestCHM\\DeleteUser.html", trainingFile);

              // The number of iterations; no general rule for finding the best value, just try several!
              var iterations = 500;
              // The cut; no general rule for finding the best value, just try several!
              var cut = 200;
              // The characters which can mark an end of sentence

              var nameFind = MaximumEntropyNameFinder.TrainModel(trainingFile, iterations, cut);
              var modelPath = "C:\\Users\\Matthew\\Desktop\\train\\";
              //new PlainTextGisModelWriter.Persist(nameFind, modelPath+"genModel.nbin");
              new BinaryGisModelWriter().Persist(nameFind, modelPath + "genModel.nbin");

              //TTTTTTRYYY THIS NEXT
              //var gisModel = new SharpEntropy.GisModel(new PlainTextGisModelReader(modelPath + "genModel.nbin"));

              //MaximumEntropyNameFinder menf = new MaximumEntropyNameFinder(gisModel);
              var nameFinder = new EnglishNameFinder(modelPath); //+ "genModel.nbin");
              Console.WriteLine("after");
              var models = new[] { "genModel"};





              while (true)
              {
                  var paragraph = Console.ReadLine();
                  Console.WriteLine(nameFinder.GetNames(models, paragraph));
              }*/
            Func<string, string> func = new Func<string, string>((x) =>
            {
                var temp = x;
                new HTMLMessager().removeFromLine(ref temp);
                return temp;
            });
            KeyWordFinder kwf = new KeyWordFinderUserBot();
			DataAccess dA = new DataAccessDB();
            while (true)
            {
                var paragraph = Console.ReadLine();
                var modelPath = "..\\..\\..\\Resources\\Models";
                //var emesd = new EnglishMaximumEntropySentenceDetector(modelPath + "\\EnglishSD.nbin");
                var sentences = paragraph.Split('.');
                var qHandler = new QueryHandler(dA,kwf, func);
                foreach (Tuple<string, string> response in qHandler.handleQuery(sentences))
                {
					Console.WriteLine("Hits for query: "+"\"" +response.Item1+"\"");
                    Console.WriteLine(response.Item2);
                    Console.WriteLine();
                }   
            }
        }
    }
}
