using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Veerabook;

namespace Veeraxml
{
    public partial class image_hotel : System.Web.UI.Page
    {
        Xtools _xtools = new Xtools();

        protected void Page_Load(object sender, EventArgs e)
        {

            string hotelname = Request.QueryString["hotelname"];
            string hotelcity = Request.QueryString["hotelcity"];


            try
            {
                var result = _xtools.getHotelImages(hotelname + " " + hotelcity, 1)
                    .Rows[0]["contentUrl"].ToString();
                Htlimg.Src = result;

            }
            catch (Exception exception)
            {

            }

          



        }
    }
}