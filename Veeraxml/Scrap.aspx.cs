using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HtmlAgilityPack;

namespace Veeraxml
{
    public partial class Scrap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Scrap_OnClick(object sender, EventArgs e)
        {
            HtmlWeb website = new HtmlWeb();
            website.AutoDetectEncoding = false;
            website.OverrideEncoding = Encoding.Default;
            HtmlDocument Doc = website.Load("http://veerabooking.com/login");

            //get the first node that fit the table type that has a product-list class attribute
            XHTML.Text = Doc.ParsedText;

        }
}
}