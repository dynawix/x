using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Veerabook;

namespace Veeraxml
{
    public partial class Rooms : System.Web.UI.Page
    {
        Merger _merger = new Merger();
        private Multi _multi = new Multi();
        Xtools _xtools = new Xtools();
        Rh _rh = new Rh();
        protected void Page_Load(object sender, EventArgs e)
        {
            string hotelname = Request.QueryString["H"];
            String hotelcity = Request.QueryString["C"];

            this.hotelname.Text = hotelname;
        



            //Get Hotel Images 
           // HtlImages.DataSource = _xtools.getHotelImages(hotelname + " " + hotelcity, 6);
           // HtlImages.DataBind();

            //Get Hotel Data According to Supplier 

            XmlNodeList hotelData = _rh.GetHotelInfo(hotelname);

            Hotel_Description.Text = _xtools.GeTXMLResult("description_short", hotelData[0].InnerXml);
            hoteladdress.Text = _xtools.GeTXMLResult("address", hotelData[0].InnerXml);





        }
    }
}