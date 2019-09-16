using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Veerabook
{
    public class Rh
    {
        Xtools xtools = new Xtools();

        public string Apicall (string command, string callstring)
        {

            string requestUrl = LiveApi + command + "/";
            // Start Send request to the API



            WebRequest request = WebRequest.Create(requestUrl);
            request.Method = "POST";

            //Add Custom Header Values
            request.Headers.Add("Authorization" , "Basic MTgxOTo3ZjUzM2I5Yi1hYmJkLTQ2MGItOGQ0Mi03MDhhYzM2OWUzOWY=");




            string postData =    callstring  ;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
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


        public string ApiCallGet(string command, string callstring)
        {



            string html = string.Empty;
            string url =   LiveApi + command ;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Headers.Add("Authorization", "Basic MTgxOTo3ZjUzM2I5Yi1hYmJkLTQ2MGItOGQ0Mi03MDhhYzM2OWUzOWY=");


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }


            return html;


          
        }



        public string GetLoginsession()
        {
            string postData =   "<Login><LoginName>" + LoginName + "</LoginName><Passw>" + LoginPassw +"</Passw></Login>";
            string callresult = Apicall("login", postData);

         return xtools.GeTXMLResult("SessionId", callresult);
           
        }



        public string GetRegionByName(string RegionName)
        {

            string ApiPostdata = "multicomplete?data={\"query\":\"Mos\",\"format\":\"json\",\"lang\":\"en\"}";

            ApiPostdata = ApiPostdata.Replace("Mos", RegionName);


            String APIResponse =  ApiCallGet(ApiPostdata,"");


            XmlDocument doc = JsonConvert.DeserializeXmlNode(APIResponse, "response");

            XmlNodeList list = doc.SelectNodes("//regions");

            



          





            return xtools.GeTXMLResult("id", list[0].InnerXml);

        }



        public XmlNodeList GetHotelInfo(string hotelname)
        {

            string ApiPostdata = "hotel/list?ids=Mos&lang=en";

            ApiPostdata = ApiPostdata.Replace("Mos", hotelname.Replace(" " ,"_").ToString());


            String APIResponse = ApiCallGet(ApiPostdata, "");


            XmlDocument doc = JsonConvert.DeserializeXmlNode(APIResponse, "response");

            XmlNodeList list = doc.SelectNodes("//result");


            return list;

        }




        public string htlsrchpostdata(string sessionId, string cityname, string checkin, string checkout, string room1, string room2, string romm3, string room4, string room5)
        {


            //Split the Comma Seperated Values 







            string postData =
                "{\"region_id\":regionid,\"checkin\":\"check_in\",\"checkout\":\"check_out\",\"adults\":[Adults],\"children\":[Children],\"lang\":\"en\",\"currency\":\"USD\"}";

            postData = postData.Replace("regionid",GetRegionByName(cityname));

            postData = postData.Replace("check_in", checkin);

            postData = postData.Replace("check_out", checkout);

            postData = postData.Replace("[Adults]", RF(room1)[0].ToString());

            postData = postData.Replace("[Children]", ChildStr(RF(room1)[1].ToString(), RF(room1)[2].ToString(), RF(room1)[3].ToString()));







            return postData;
        }


        public string[] RF(string fields)
        {

            fields = fields.Replace("[", "").Replace("]", "");
            string[] Comma_Fields = null;
            Comma_Fields = fields.Split(',');
            return Comma_Fields;
        }



        public string ChildStr(string child1, string child2, string child3)
        {




            if (child1 == "undefined" || child1 == "0")
            {
                child1 = "";
                return "[]";
            }
           

            if (child2 == "undefined" || child2 == "0")
            {
                child2 = "";
            }
           
            if (child3 == "undefined" || child3 == "0")
            {
                child3 = "";
            }


            // Case Solo Child 
            if (child2 == "" && child3 == "")
            {
                return "[" + child1 + "]";
            }

            // Case Solo Child 
            if (child2 == "" || child3 == "")
            {
                return "[" + child1 + "]";
            }

            //Case only First and Second 
            if (child3 == "" && child2 != "")
            {
                return "[" + child1 + "," + child2 + "]";
            }









            String chidlstr = "[" + child1 + "," + child2  + "," + child3 + "]";



            return chidlstr;

        }








        public string Search(String SessionId, string htlSrchPostData)
        {
            String APIResponse = Apicall("hotel/rates", htlSrchPostData);



            //Convert Json Response to Formatted XML 

            XmlDocument doc = JsonConvert.DeserializeXmlNode(APIResponse,"hotels");


            //Save the XML 
            doc.Save(AppDomain.CurrentDomain.BaseDirectory + "Cache/" + SessionId + "_RH_Original.XML");



            //Split Rooms Using XML Nodes
            XmlDocument xdoc = new XmlDocument();
            xdoc = doc ;


            //  // split into elements
            XmlNodeList list = xdoc.SelectNodes("//rates");

            //Get a Replica of Merger final DT
            DataTable dt = FinalDT();

            //
            for (int i = 0; i < list.Count; i++)
            {

                DataRow newRow = dt.NewRow();
                newRow["Hotel_ID"] = xtools.GeTXMLResult("id", list[i].ParentNode.OuterXml);
                newRow["Hotel_Name"] = xtools.GeTXMLResult("id", list[i].ParentNode.OuterXml).Replace("_"," ");
                newRow["Hotel_Category"] = "4";
                newRow["Hotel_Address"] = "RH";
                newRow["Hotel_Phone"] = "RH";
                newRow["Hotel_City_Name"] = "RH";
                newRow["Hotel_City_Code"] = "RH";


                newRow["Hotel_Location_LAT"] = "1222";
                newRow["Hotel_Location_LNG"] = "32323232";



                newRow["Room_ID"] = xtools.GeTXMLResult("book_hash", list[i].InnerXml);
                newRow["Room_Name"] = xtools.GeTXMLResult("room_name", list[i].InnerXml);
                newRow["Meal_Code"] = xtools.GeTXMLResult("meal", list[i].InnerXml);
                newRow["Meal_Name"] = xtools.GeTXMLResult("meal", list[i].InnerXml);
                newRow["Cancellation_Deadline"] = xtools.GeTXMLResult("cancellation_info", list[i].InnerXml);
                newRow["Cancellation_Policy"] = xtools.GeTXMLResult("cancellation_info", list[i].InnerXml);
                //newRow["Room_BasePrice"] = xtools.GeTXMLNODE("payment_types", "amount", list[i].InnerXml);
                //newRow["Room_TotalPrice"] = xtools.GeTXMLNODE("payment_types", "amount", list[i].InnerXml);
                //newRow["Room_BasePrice_Currency"] = xtools.GeTXMLNODE("payment_types", "currency_code", list[i].InnerXml);
                newRow["Room_BasePrice"] = xtools.GeTXMLResult("b2b_recommended_price", list[i].InnerXml);
                newRow["Room_TotalPrice"] = xtools.GeTXMLResult("b2b_recommended_price", list[i].InnerXml);
                newRow["Room_BasePrice_Currency"] = "USD";




                //Set Source Supplier 
                newRow["Supplier"] = "RH";

                //Session ID and SearchToken 
                newRow["SessionId"] = SessionId ;
                newRow["SearchToken"] = "" ;

                dt.Rows.Add(newRow);
            }


            dt.TableName = "Veera";

            //Force Create the Text file , fix the Glitch ! 
            //StringWriter sw = new StringWriter();
            // dt.WriteXml(AppDomain.CurrentDomain.BaseDirectory + "Cache/" + SessionId + "_RH.XML");

            using (var bulkCopy = new SqlBulkCopy(System.Configuration.ConfigurationManager.ConnectionStrings["Connstr"].ToString(), SqlBulkCopyOptions.KeepIdentity))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                foreach (DataColumn col in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }

                bulkCopy.BulkCopyTimeout = 600;
                bulkCopy.DestinationTableName = "Veera_Result";
                bulkCopy.WriteToServer(dt);
            }


            //using (FileStream fs = File.Create())
            //{
            //    byte[] info = new UTF8Encoding(true).GetBytes(sw.ToString());

            //    fs.Write(info, 0, info.Length);
            //}


            ////Get Session SearchToken to Cache With
            string SessionSearchToken  = "111223324456";





            return SessionSearchToken;




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
                return "https://partner.ostrovok.ru/api/b2b/v2/";
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


    public class RecordDataTableConverter : Newtonsoft.Json.Converters.DataTableConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            if (reader.TokenType == JsonToken.StartObject)
            {
                var token = JToken.Load(reader);
                token = new JArray(token.SelectTokens("records[*].record"));
                using (var subReader = token.CreateReader())
                {
                    while (subReader.TokenType == JsonToken.None)
                        subReader.Read();
                    return base.ReadJson(subReader, objectType, existingValue, serializer); // Use base class to convert
                }
            }
            else
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
        }
    }


}