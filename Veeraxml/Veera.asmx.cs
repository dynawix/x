using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Veerabook;

namespace Veeraxml
{

    /// <summary>
    /// Summary description for Veera
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Veera : System.Web.Services.WebService
    {
        Xtools _xtools = new Xtools();
        private Multi _multi = new Multi();
        Merger _merger = new Merger();
        private Rh _Rh = new Rh();

        [WebMethod]
        public string Search(string sessionId, string cityname, string checkin, string checkout, string room1, string room2, string room3, string room4, string room5)
        {

            // Search on Rate Hawk 
            _Rh.Search(sessionId, _Rh.htlsrchpostdata(sessionId, cityname, checkin, checkout, room1, room2, room3, room4, room5));


            //Get Session Search Token Based On what's Sent 
            string sessionSearchToken = _multi.SearchAsync(sessionId, _multi.htlsrchpostdata(sessionId, cityname, checkin, checkout, room1, room2, room3, room4, room5));



            _merger.FinalSearchData(sessionSearchToken, sessionId);


            return "Ok";

        }
    }
}
