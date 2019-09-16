using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Veerabook;

namespace Veeraxml
{
    /// <summary>
    /// Summary description for Image_Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Image_Service : System.Web.Services.WebService
    {

        Xtools _xtools = new Xtools();

        [WebMethod]
        public string GetSingleImage(string hotelname,string hotelcity)
        {

            try
            {
                var result = _xtools.getHotelImages(hotelname + " " + hotelcity, 1)
                    .Rows[0]["contentUrl"].ToString();



                Context.Response.Output.Write(result);
                Context.Response.End();
                return string.Empty;

            }
            catch (Exception)
            {
                return "Noimage";
            }

        }
    }
}
