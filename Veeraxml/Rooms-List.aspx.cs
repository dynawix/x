using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Veerabook;

namespace Veeraxml
{
    public partial class Rooms_List : System.Web.UI.Page
    {
        Merger _merger = new Merger();
        Rh _rh = new Rh();
        private Multi _multi = new Multi();
        Xtools _xtools = new Xtools();
        protected void Page_Load(object sender, EventArgs e)
        {
            string hotelname = Request.QueryString["H"];
            DataTable   dt = _merger.FinalSearchDataRooms("MR",Session["SessionId"].ToString(),hotelname);

            //_rh.GetHotelInfo(hotelname.Replace(" ", "_"), Session["SessionId"].ToString());


            RoomsList.DataSource = dt ;
            RoomsList.DataBind();

        }
    }
}