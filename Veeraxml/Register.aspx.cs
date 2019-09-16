using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Veerabook;

namespace Veeraxml
{
    public partial class Register : System.Web.UI.Page
    {
        Xtools xtools = new Xtools();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Exec_Command_OnServerClick(object sender, EventArgs e)
        {
            var sguid = System.Guid.NewGuid().ToString();

            xtools.InsertInto(office_Address.Value + "@Office_Address," + Office_Director + "@office_Director," + office_Name.Value + "@office_name"  , "Veera_Office");

        //    xtools.InsertInto("", "veera");
        }
    }
}