using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class QueryGenerator
    {
        private SentenceBoiler sb;

        public QueryGenerator(SentenceBoiler sb)
        {
            this.sb = sb;
        }

        public Query queryGen(string sentence)
        {
            List<Attrib> attributeList = new List<Attrib>();
            attributeList.Add(new Attrib("filePath", sb.boilDown(sentence),false));
            return new Query(attributeList, new Tuple<int, bool>(0, false), "text",sentence);
        }
    }
}
