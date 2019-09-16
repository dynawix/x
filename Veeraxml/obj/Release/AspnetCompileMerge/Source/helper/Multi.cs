using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Veerabook
{
    public class Multi
    {
        Xtools xtools = new Xtools();

        public string Apicall (string command, string callstring)
        {

            string requestUrl = LiveApi + command + "/";
            // Start Send request to the API
            WebRequest request = WebRequest.Create(requestUrl);
            request.Method = "POST";
            string postData = "xml=" + "<MultireisenAPI_Request>" +  callstring + "</MultireisenAPI_Request>";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            return responseFromServer;
        } 

        public string GetLoginsession()
        {
            string postData =   "<Login><LoginName>" + LoginName + "</LoginName><Passw>" + LoginPassw +"</Passw></Login>";
            string callresult = Apicall("login", postData);

         return xtools.GeTXMLResult("SessionId", callresult);
           
        }



        public string Search(String SessionId, string htlSrchPostData)
        {
            String APIResponse = Apicall("hotelsearch",htlSrchPostData);
            
            //Cache to Binary Text File XML 
            using (FileStream fs = File.Create(AppDomain.CurrentDomain.BaseDirectory + "Cache/" + SessionId + "_Original.XML"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(APIResponse);
                fs.Write(info, 0, info.Length);
            }



            //Get Session SearchToken to Cache With
            string SessionSearchToken = xtools.GeTXMLResult("SearchToken", APIResponse);


            //Split Rooms Using XML Nodes
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(AppDomain.CurrentDomain.BaseDirectory + "Cache/" + SessionId + "_Original.XML");

            //  // split into elements
            XmlNodeList list = xdoc.SelectNodes("//RoomType");

            //Get a Replica of Merger final DT
            DataTable dt = FinalDT();

            //
            for (int i = 0; i < list.Count; i++)
            {

                DataRow newRow = dt.NewRow();
                newRow["Hotel_ID"] = list[i].ParentNode.ParentNode.ChildNodes[0].InnerText;
                newRow["Hotel_Name"] = list[i].ParentNode.ParentNode.ChildNodes[1].InnerText;
                newRow["Hotel_Category"] = xtools.GeTXMLResult("Category", list[i].ParentNode.ParentNode.OuterXml);
                newRow["Hotel_Address"] = list[i].ParentNode.ParentNode.ChildNodes[3].InnerText;
                newRow["Hotel_Phone"] = list[i].ParentNode.ParentNode.ChildNodes[4].InnerText;
                newRow["Hotel_City_Name"] = list[i].ParentNode.ParentNode.ChildNodes[5].ChildNodes[0].InnerText;
                newRow["Hotel_City_Code"] = list[i].ParentNode.ParentNode.ChildNodes[5].ChildNodes[1].InnerText;


                newRow["Hotel_Location_LAT"] = xtools.GeTXMLResult("Latitude", list[i].ParentNode.ParentNode.OuterXml);
                newRow["Hotel_Location_LNG"] = xtools.GeTXMLResult("Longitude", list[i].ParentNode.ParentNode.OuterXml);



                newRow["Room_ID"] = list[i].ChildNodes[0].InnerText;
                newRow["Room_Name"] = list[i].ChildNodes[1].InnerText;
                newRow["Meal_Code"] = xtools.GeTXMLResult("code", list[i].OuterXml);
                newRow["Meal_Name"] = xtools.GeTXMLResult("name", list[i].OuterXml);
                newRow["Cancellation_Deadline"] = xtools.GeTXMLResult("Deadline", list[i].OuterXml);
                newRow["Cancellation_Policy"] = xtools.GeTXMLResult("Policy", list[i].OuterXml);
                newRow["Room_BasePrice"] = xtools.GeTXMLResult("baseprice", list[i].OuterXml);
                newRow["Room_TotalPrice"] = xtools.GeTXMLResult("total", list[i].OuterXml);
                newRow["Room_BasePrice_Currency"] = xtools.GeTXMLResult("Currency", list[i].OuterXml);

                //Set Source Supplier 
                newRow["Supplier"] = "Multi";

                //Session ID and SearchToken 
                newRow["SessionId"] = list[i].ParentNode.ParentNode.ParentNode.ParentNode.ChildNodes[0].InnerText;
                newRow["SearchToken"] = list[i].ParentNode.ParentNode.ParentNode.ParentNode.ChildNodes[1].InnerText;

                dt.Rows.Add(newRow);
            }


            dt.TableName = "Veera";

            //Force Create the Text file , fix the Glitch ! 
            StringWriter sw = new StringWriter();
            dt.WriteXml(sw);



            using (FileStream fs = File.Create(AppDomain.CurrentDomain.BaseDirectory + "Cache/" + SessionSearchToken + ".XML"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(sw.ToString());

                fs.Write(info, 0, info.Length);
            }




            return SessionSearchToken;




        }



        public string GetHotelData(string Hotelid, string sessionID, string searchtoken)
        {

            string postData = "<RequestHeader><LoginName>" + LoginName + "</LoginName>" +
               "<SessionId>" + sessionID + "</SessionId>" +
               "<SearchToken>" + searchtoken + "</SearchToken>" +
               "<Language>" + "en" + "</Language>" + "</RequestHeader>" +
               "<GetItemDetails>" + "<Hotel>" + "<ItemID>" + Hotelid + "</ItemID>" +
                 "</Hotel>" + "</GetItemDetails>";


            string callresult = Apicall("getitemdetails", postData);

            //Load Up the Document for XML Processing
            XmlDocument xdocItem = new XmlDocument();
            xdocItem.LoadXml(callresult);


           XmlNodeList listItem = xdocItem.SelectNodes("//HotelResult");

            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("Hotel_Location");
            dtItem.Columns.Add("Hotel_Description");
            dtItem.Columns.Add("Hotel_Amenities");









            using (FileStream fs = File.Create(AppDomain.CurrentDomain.BaseDirectory + "Cache/Hotel_" + Hotelid + "_Details.XML"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(callresult);

                fs.Write(info, 0, info.Length);
            }





            //Cache the Over All Result to XML File 
            //dtItem.TableName = "Veera";
            //dtItem.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "Cache/Hotel_"+ Hotelid  + "_Details.XML");


            return "Hotel_" + Hotelid + "_Details.XML";

        }







        public string GetRoomData(string Hotelid,string RoomId,string sessionID,string searchtoken)
        {


            string postData = "<RequestHeader><LoginName>" + LoginName + "</LoginName>" +
                "<SessionId>" + sessionID + "</SessionId>" +
                "<SearchToken>" + searchtoken + "</SearchToken>" +
                "<Language>" + "en" + "</Language>" + "</RequestHeader>" +
                "<SelectData>" + "<Hotel>" + "<ItemID>" + Hotelid + "</ItemID>" +
                "<RoomTypeID>" + RoomId + "</RoomTypeID>" + "</Hotel>" + "</SelectData>";

            string callresult = Apicall("select", postData);

            //Check for Failed Calls
          

            return callresult;


            System.Threading.Thread.Sleep(1000);


        }


        public string initbooking(string Hotelid, string RoomId, string sessionID, string searchtoken,string Passengers_Str , string pay_Mode)
        {




            //contact information
            string contactAdd = "Teszt utca 11 ";
            string contactCit = "Cairo";
            string contactZipCode = "1234";
            string contactPhon = "123456789";
            string contactEmails = "test.user@gmail.com";
            string contactCountries = "EG";


            // company data
            string companyAdd = "Teszt utca 11 ";
            string companyCit = "Cairo";
            string companyZipCode = "1234";
            string companyNames = "Veera Booking";
            string companyCountries = "EG";

            // agent booking data
            string agentTitle = "Mr";
            string agentFirstName = "Mohamed";
            string agentLastName = "Hussein";
            string agentCountry = "EG";
            string agentEmail = "test.user@gmail.com";
            string agentPhon = "123456789";
            string agentZipCode = "1234";
            string agentCity = "Cairo";
            string agentAddress = "Teszt utca 11";


            string PassengersStr = "<Passengers>" + Passengers_Str + "</Passengers>";



            string InitBasic = "<InitBookingData><Contact><Title>" + agentTitle + "</Title><Lastname>"
                + agentLastName + "</Lastname><Firstname>"
                + agentFirstName + "</Firstname><Email>"
                + agentEmail + "</Email><Phone>"
                + agentPhon + "</Phone><Country>"
                + agentCountry + "</Country><Zip>"
                + agentZipCode + "</Zip><City>"
                + agentCity + "</City><Address>"
                + agentAddress + "</Address></Contact><Invoice><Name>"
                + companyNames + "</Name><Country>"
                + companyCountries + "</Country><Zip>"
                + companyZipCode + "</Zip><City>"
                + companyCit + "</City><Address>"
                + companyAdd + "</Address></Invoice>"
                + PassengersStr + "<Paymode>" + pay_Mode + "</Paymode></InitBookingData>"; 







            string postData = "<RequestHeader><LoginName>" + LoginName + "</LoginName>" +
                "<SessionId>" + sessionID + "</SessionId>" +
                "<SearchToken>" + searchtoken + "</SearchToken>" +"</RequestHeader>" +
                InitBasic ;



            string callresult = Apicall("initbooking", postData);
            return callresult;
        }
















        //Static Values

        public string LoginName
        {
            get
            {

                return "admin@veraxml.com";
            }
        }

        public string LoginPassw
        {
            get
            {

                return "CBL2zTupj";
            }
        }

        public string LiveApi
        {
            get
            {
                return "http://api.multireisen.com/";
            }
        }

        public string TestApi
        {
            get
            {
                return "http://api-test.multireisen.com/";
            }
        }

        public DataTable FinalDT()
        {
            var dt = new DataTable();

            dt.Columns.Add("Hotel_ID");
            dt.Columns.Add("Hotel_Name");
            dt.Columns.Add("Hotel_Category");
            dt.Columns.Add("Hotel_Address");
            dt.Columns.Add("Hotel_Phone");
            dt.Columns.Add("Hotel_City_Name");
            dt.Columns.Add("Hotel_City_Code");
            dt.Columns.Add("Hotel_Location_LAT");
            dt.Columns.Add("Hotel_Location_LNG");
            dt.Columns.Add("Room_ID");
            dt.Columns.Add("Room_Name");
            dt.Columns.Add("Meal_Code");
            dt.Columns.Add("Meal_Name");
            dt.Columns.Add("Cancellation_Deadline");
            dt.Columns.Add("Cancellation_Policy");
            dt.Columns.Add("Room_TotalPrice");
            dt.Columns.Add("Room_BasePrice");
            dt.Columns.Add("Room_BasePrice_Currency");
            dt.Columns.Add("Supplier");
            dt.Columns.Add("SessionId");
            dt.Columns.Add("SearchToken");

            return dt; 


        }



    }
}