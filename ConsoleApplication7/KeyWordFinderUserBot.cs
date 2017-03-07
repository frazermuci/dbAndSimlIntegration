using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syn.Bot.Siml;
using System.IO;

namespace Test
{
    public class KeyWordFinderUserBot : KeyWordFinder
    {
        //public Tuple<string, bool> boilDown(string sentence)
        public string boilDown(string sentence)
        {
            /*string[] splitSentence = sentence.Split(' ');
            foreach (string str in splitSentence)
            {
                if (refUserFile(str))
                {
                    return new Tuple<string, bool>(str,true);
                }
            }
            return new Tuple<string, bool>(sentence, true);*/
            int max = -1;
            string maxFile = "";
            //SynBot Chatbot;
            //Syn.Bot.Siml.SimlBot Chatbot;
            //Syn.Bot.Utiliti
            string path = "C:\\Users\\Matthew\\Desktop\\2016-2017 school\\2017 Winter\\191A\\MockedQueryGenerator\\SynBotDir";
            foreach (string fileName in System.IO.Directory.EnumerateFiles(path)) //maybe don't hard code
            {
                
                SimlBot Chatbot = new SimlBot();
                Chatbot.PackageManager.LoadFromString(File.ReadAllText(fileName));
                var result = Chatbot.Chat(sentence);
                string[] outPut = result.BotMessage.Split(':');
                //Chatbot = new SynBot(fileName);
                //Chatbot = new Syn.Bot.Siml.SimlBot(fileName);
                //Chatbot.PackageManager.LoadFromString(System.IO.File.ReadAllText(fileName));
                //var result = Chatbot.Chat(sentence);
                int test = Convert.ToInt32(outPut[0]);
                maxFile = max < test ? outPut[1] : maxFile;
                max = max < test ? test : max;
                //Console.WriteLine(result.BotMessage);
            }
            //return new Tuple<string,bool>(maxFile, true);
            Console.WriteLine(maxFile);
            return maxFile;
        }
    }
}
