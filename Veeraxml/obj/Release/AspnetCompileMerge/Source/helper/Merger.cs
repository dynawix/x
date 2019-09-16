using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;

namespace Veerabook
{
    public class Merger
    {
        public DataTable FinalSearchData(string SessionSearchToken)
        {
            string text = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "Cache/" + SessionSearchToken + ".XML");

            StringReader theReader = new StringReader(text);
            DataSet DS = new DataSet("dataSet");
            DS.ReadXml(theReader);

            return DS.Tables[0];
        }
    }
}