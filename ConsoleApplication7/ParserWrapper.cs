using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


////maybe store all these private methods in a helper module
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Test
{
    class ParserWrapper
    {
        Dictionary<string, ParsedCHM> parsedCHMs;
        //ActiveAIDDB db;

        public ParserWrapper(string filePath)
        {
            parsedCHMs = new Dictionary<string, ParsedCHM>();
            parsedCHMs[filePath] = new ParsedCHM(filePath);
        }

        public ParserWrapper(List<string> filePaths)
        {
            foreach (string file in filePaths)
            {
                parsedCHMs[file] = new ParsedCHM(file);
            }
        }

      /* private void insertBlocksIntoDB(string filePath, List<List<Element>> blocks)
        {
            foreach (List<Element> block in blocks)
            {
                int blockCount = 0;
                foreach (Element element in block)
                {
                    if (element.name == "img")
                    {
                        //NEED A MEANS TO RETRIEVE ID
                        ;//db.insertIntoImages();
                    }
                    else
                    {
                        //NEED A MEANS TO RETRIEVE ID
                        db.insertIntoElements(filePath, blockCount, element.data);
                    }
                    ++blockCount;
                }
            }
        }

      private void insertHREFSOIntoDB(string filePath, List<string> hrefs)
        {
            foreach (string href in hrefs)
            {
                //NEED A MEANS TO RETRIEVE ID
                db.insertIntoHyperlinks(filePath, href);
            }
        }

        public void persistData()
        {
            ActiveAIDDB db = new ActiveAIDDB();
            db.insertIntoFiles(title, "");

            foreach (KeyValuePair<string, ParsedCHM> pair in parsedCHMs)
            {
                insertBlocksIntoDB(pair.Key, pair.Value.blocks);
                insertHREFSOIntoDB(pair.Value.title, pair.Value.hrefs);
            }
        }*/

        private void insertIntoOccuranceDict(ref Dictionary<string, int> occuranceOfWord, string[] data, int weight)
        {
            foreach (string word in data)//really need to fix this
            {
                if (occuranceOfWord.ContainsKey(word))
                {
                    occuranceOfWord[word.Trim().ToLower()] = occuranceOfWord[word] + 1 * weight;//500 for title
                }
                else
                {
                    occuranceOfWord[word.Trim().ToLower()] = 1 * weight;
                }
             }
        }

        private string getMaxWord(Dictionary<string, int> dict)
        {
            Tuple<string, int> tup = new Tuple<string, int>("",-9999);
            foreach (var pair in dict)
            {
                if (pair.Value > tup.Item2)
                {
                    tup = new Tuple<string, int>(pair.Key, pair.Value);
                }
            }
            return tup.Item1;
        }

        private bool throwOutWord(string check, List<string> titleKeyWords)
        {
            string regex = "&.*?;|^\\s*$|^and$|^or$|^not$|^the$|^a$|^an$|^be$|^on$|^being$|^been$|^but$|^in$|^of$|^will$|";
            regex = string.Concat(new string[] {regex, "^under$|^from$|^with$|^without$|^for$|", "^to$|^too$|^so$|^have$|^has$|^had$"});
            Regex regexThrowOut = new Regex(regex);
            return regexThrowOut.IsMatch(check) || check == "" || titleKeyWords.Contains(check);
        }

        private List<string> getTopKeyWords(Dictionary<string, int> occuranceOfWord, List<string> titleKeyWords)
        {
            List<string> keywords = new List<string>();
            for (var i = 0; i < 5 && occuranceOfWord.Count() != 0;)
            {
                string maxWord = getMaxWord(occuranceOfWord);
                if (!throwOutWord(maxWord, titleKeyWords))
                {
                    keywords.Add(maxWord);
                    ++i;
                }
                occuranceOfWord.Remove(maxWord);
            }
            return keywords;
        }

        private List<string> getKeyWords(List<List<Element>> blocks, List<string> titleKeyWords)
        {
            Dictionary<string, int> occuranceOfWord = new Dictionary<string, int>();       
            foreach (List<Element> block in blocks)
            {
                foreach (Element element in block)
                {
                    if (element.name != "img")
                    {
                        insertIntoOccuranceDict(ref occuranceOfWord, element.data.Split(' '), 1);
                    }
                }
            }
            return getTopKeyWords(occuranceOfWord, titleKeyWords);            
        }

        private List<string> handleTitleKeyWords(string title)
        {
            Dictionary<string, int> occuranceOfWord = new Dictionary<string, int>();
            insertIntoOccuranceDict(ref occuranceOfWord, title.Split(' '), 1);
            return getTopKeyWords(occuranceOfWord, new List<string>());
        }

        private List<string> aggregateKeyWords(List<List<string>> fileKeyWords)
        {
            Dictionary<string, int> occuranceOfWord = new Dictionary<string, int>();
            foreach (var words in fileKeyWords)
            {
                int range = words.Count() - 5;
                insertIntoOccuranceDict(ref occuranceOfWord, words.Take(range).ToArray(), 500);
                insertIntoOccuranceDict(ref occuranceOfWord, words.Skip(range).ToArray(), 1);
            }
            return getTopKeyWords(occuranceOfWord, new List<string>());
        }

        public void genModel()
        {
            List<List<string>> fileKeyWords = new List<List<string>>();
            List<string> responseFileNames = new List<string>();
            string response;
            foreach (var pair in parsedCHMs)
            {
                List<string> keywords = new List<string>(); 
                responseFileNames.Add(pair.Key);
                keywords.AddRange(handleTitleKeyWords(pair.Value.title));//keywords weighted by ordering in which they appear in list
                keywords.AddRange(getKeyWords(pair.Value.blocks, keywords));
                fileKeyWords.Add(keywords);
            }
            response = String.Join(";", responseFileNames.ToArray());
            //aggregateKeyWords(fileKeyWords)
            //Console.WriteLine(response);
            /*foreach (string keyword in keywords)
            {
                Console.WriteLine(keyword);
            }*/
        }
    }
}

