using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Veerabook;

namespace Veeraxml
{
    public partial class Hotel : System.Web.UI.Page
    {
        private Multi _multi = new Multi();
        VeeraMaster _veera = new  VeeraMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
          

            login();
          
        }

        protected void login()
        {

            Session["SessionId"] = _veera.VeeraLogin();
            SessionId.Value = Session["SessionId"].ToString();

        }


    }
}