using System.Threading.Tasks;
using Syn.Bot.Siml;
using System.IO;
using System;

namespace Test
{
    public class SentenceBoilerUserBot : SentenceBoiler
    {
        public override string boilDown(string sentence)
        {
            int max = -1;
            string maxFile = "";
            string path = "C:\\Users\\Matthew\\Desktop\\2016-2017 school\\2017 Winter\\191A\\MockedQueryGenerator\\SynBotDir";
            foreach (string fileName in System.IO.Directory.EnumerateFiles(path)) //maybe don't hard code
            {

                SimlBot Chatbot = new SimlBot();
                Chatbot.PackageManager.LoadFromString(File.ReadAllText(fileName));
                var result = Chatbot.Chat(sentence);
                string[] outPut = result.BotMessage.Split(':');
                int test = Convert.ToInt32(outPut[0]);
                maxFile = max < test ? outPut[1] : maxFile;
                max = max < test ? test : max;
                //Console.WriteLine(result.BotMessage);
            }
            //return new Tuple<string,bool>(maxFile, true);
            return maxFile;
        }
    }
}