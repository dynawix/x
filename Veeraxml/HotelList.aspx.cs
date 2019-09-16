using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Veerabook;

namespace Veeraxml
{
    public partial class HotelList : System.Web.UI.Page
    {
        Xtools _xtools = new Xtools();
        private Multi _multi = new Multi();
        Merger _merger = new Merger();
        private Rh _Rh = new Rh();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


                if (Request.QueryString["Load"] != null)
                {
                    Session["XCOUNT"] = Convert.ToInt32(Request.QueryString["LOAD"]) + 10;
                    
                    //If it exists
                    BindHotelLv(Convert.ToInt32(Session["XCOUNT"]));
                    return;
                }
                if (Request.QueryString["H"] != null)
                {

                    //If it exists
                    BindHotelLvNameFilter(Request.QueryString["H"].ToString());
                }
                else
                {
                    BindHotelList();
                }
            }
        }

        void BindHotelList()
        {

            string cityname = Request.QueryString["cityname"];
            string checkin = Request.QueryString["checkin"];
            string checkout = Request.QueryString["checkout"];
            string room1 = Request.QueryString["room1"];
            string room2 = Request.QueryString["room2"];
            string room3 = Request.QueryString["room3"];
            string room4 = Request.QueryString["room4"];
            string room5 = Request.QueryString["room5"];

            string sessionid = Session["SessionId"].ToString();



            //Get Session Search Token Based On what's Sent 
           //   Session["SessionSearchToken"] = _multi.SearchAsync(sessionid, _multi.htlsrchpostdata(sessionid, cityname, checkin, checkout, room1, room2, room3, room4, room5));

            // Search on Rate Hawk 
            _Rh.Search(Session["SessionId"].ToString(),_Rh.htlsrchpostdata(Session["SessionId"].ToString(),cityname,checkin,checkout,room1,room2,room3,room4,room5));




          


            //Call the Binding Line 
               BindHotelLv(10);
        }

        void BindHotelLv(int xcount)
        {

            DataTable dt = _merger.FinalSearchData("MR", Session["SessionId"].ToString());

            Hotel_Count.Text = dt.Rows.Count.ToString();


            DataTable dtresult = dt.AsEnumerable().Take(xcount).CopyToDataTable();


            lvHotelsList.DataSource = dtresult;
            lvHotelsList.DataBind();


        }



        void BindHotelLvNameFilter(string Hotelname)
        {

            DataTable dt = _merger.FinalSearchData("MR", Session["SessionId"].ToString());

            Hotel_Count.Text = dt.Rows.Count.ToString();

            dt.DefaultView.RowFilter = "Hotel_name LIKE '%" + Hotelname + "%'";
            dt.DefaultView.Sort = "Room_BasePrice ASC";

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