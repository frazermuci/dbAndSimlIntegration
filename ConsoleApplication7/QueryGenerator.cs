using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class QueryGenerator
    {
        private KeyWordFinder kwf;

        public QueryGenerator(KeyWordFinder kwf)
        {
            this.kwf = kwf;
        }

        public Query queryGen(string sentence)
        {
            List<Attrib> attributeList = new List<Attrib>();
            attributeList.Add(new Attrib("filePath", kwf.boilDown(sentence),false));
            return new Query(attributeList, new Tuple<int, bool>(0, false), "text");
        }
    }
}
