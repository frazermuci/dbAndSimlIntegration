using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public abstract class SentenceBoiler
    {
        //Tuple<string, bool> boilDown(string sentecnce);
        public abstract string boilDown(string sentecnce);

        public static SentenceBoiler sentenceBoilerFactory(string reify)
        {
            if (reify == "UserBot")
            {
                return new SentenceBoilerUserBot();
            }
            else { return null; }
        }
    }
}
