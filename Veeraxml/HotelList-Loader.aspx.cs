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
    public partial class HotelList_Loader : System.Web.UI.Page
    {
     
        Xtools _xtools = new Xtools();
        private Multi _multi = new Multi();
        Merger _merger = new Merger();
        private Rh _Rh = new Rh();

        protected void Page_Load(object sender, EventArgs e)
        {

          


                    //If it exists
                    BindHotelLv(Convert.ToInt32(Request.QueryString["Load"]),Request.QueryString["sessionid"]);
               
             
           
        }


        void BindHotelLv(int xcount , string sessionid)
        {

           DataTable dt = _xtools.SQLREAD("SELECT TOP " + xcount +  " * FROM VEERA_RESULT WHERE SESSIONID='" + sessionid + "'").Tables[0];

            lvHotelsList.DataSource = dt;
            lvHotelsList.DataBind();


        }



      







        protected void lvHotelsList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                try
                {
                    //Get Hotel Image from the Api
                    Image htlImg = (Image)e.Item.FindControl("htlImg");
                    System.Data.DataRowView rowView = e.Item.DataItem as System.Data.DataRowView;
                    htlImg.ImageUrl = _xtools.getHotelImages(Convert.ToString(rowView["Hotel_Name"]) + " " + Convert.ToString(rowView["Hotel_City_Name"]), 1).Rows[0]["contentUrl"].ToString();
                    //Get Hotel Stars 

                    Literal Hotel_Category = (Literal)e.Item.FindControl("Hotel_Category");

                    Hotel_Category.Text = "<div class=\"star-rating\" data-rating=\"" + Convert.ToDouble(rowView["Hotel_Category"]) +
                                          "\"><div class=\"rating-counter\">" + Convert.ToDouble(rowView["Hotel_Category"]) + " Stars</div></div>";





                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }


            }



        }





    }

}
