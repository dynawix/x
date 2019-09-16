using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Veerabook
{
    class Xtools
    {



    





        public string JsonPrettify(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
        {
            Hashtable hTable = new Hashtable();
            ArrayList duplicateList = new ArrayList();

            //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
            //And add duplicate item value in arraylist.
            foreach (DataRow drow in dTable.Rows)
            {
                if (hTable.Contains(drow[colName]))
                    duplicateList.Add(drow);
                else
                    hTable.Add(drow[colName], string.Empty);
            }

            //Removing a list of duplicate items from datatable.
            foreach (DataRow dRow in duplicateList)
                dTable.Rows.Remove(dRow);

            //Datatable which contains unique records will be return as output.
            return dTable;
        }


        public DataTable getHotelImages(string hotelName, int countStr)
        {
            hotelName = hotelName.Replace(" ", "%20");
            var urlRequest = "https://api.cognitive.microsoft.com/bing/v7.0/images/search?subscription-key=1daaae7c7f98447785584cddf2378545&q=" + hotelName + "&count=" + countStr + "&offset=0&mkt=en-us&safeSearch=Moderate";
            Uri myUri = new Uri(urlRequest);
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
            myHttpWebRequest.Accept = "*/*";
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";

            //Get the headers associated with the request.
            WebHeaderCollection myWebHeaderCollection = myHttpWebRequest.Headers;
            //Add the Accept-Language header (for Danish) in the request.
            myWebHeaderCollection.Add("Accept-Language:en-US");
            //    myWebHeaderCollection.Add("User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36");
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            WebHeaderCollection header = myHttpWebResponse.Headers;
            var encoding = ASCIIEncoding.ASCII;
            string responseText = "";
            using (var reader = new System.IO.StreamReader(myHttpWebResponse.GetResponseStream(), encoding))
            {
                responseText = reader.ReadToEnd();
            }
            DataTable dataTable = GetDataTableFromJsonString(responseText);
            return dataTable;


        }

        public DataTable GetDataTableFromJsonString(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }






        public string GeTXMLResult(string XMLNODE, string XMLSTRING)
        {

            string XMLNODE_OPEN = "<" + XMLNODE + ">";
            string XMLNODE_CLOSE = "</" + XMLNODE + ">";

            int posA = XMLSTRING.IndexOf(XMLNODE_OPEN);
            int posB = XMLSTRING.LastIndexOf(XMLNODE_CLOSE);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + XMLNODE_OPEN.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return XMLSTRING.Substring(adjustedPosA, posB - adjustedPosA);
        }


        public string GeTXMLNODE(string XMLNODE, string XMLCHILD, string XMLSTRING)
        {

            string XMLNODE_OPEN = "<" + XMLNODE + ">";
            string XMLNODE_CLOSE = "</" + XMLNODE + ">";

            int posA = XMLSTRING.IndexOf(XMLNODE_OPEN);
            int posB = XMLSTRING.LastIndexOf(XMLNODE_CLOSE);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + XMLNODE_OPEN.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }


            return GeTXMLResult(XMLCHILD, XMLSTRING.Substring(adjustedPosA, posB - adjustedPosA)); ;
        }



        public double distance_calculate(double lat1, double lon1, double lat2, double lon2, char unit)
        {

            double theta = lon1 - lon2;
            double dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        internal static void sqlread(string v)
        {
            throw new NotImplementedException();
        }

        public double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        public double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        public double GetDistanceInKMeters(double latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            return (distance_calculate(latitude1, Longitude1, Latitude2, Longitude2, 'K') / 1000);
        }
        public double GetDistanceInnauticalmiles(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            double _status = 0.0;

            Console.WriteLine("nautical miles : " + distance_calculate(Latitude1, Longitude1, Latitude2, Longitude2, 'N'));
            return _status;
        }
        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string sIPAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(sIPAddress))
            {
                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                string[] ipArray = sIPAddress.Split(new Char[] { ',' });
                return ipArray[0];
            }
        }

        //Data Access Layer
        public DataSet SQLREAD(string SQLQUERY)
        {
            DataSet functionReturnValue = null;
            try
            {
                SqlConnection thisConnection = null;
                thisConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connstr"].ToString());
                string sql = SQLQUERY;
                thisConnection.Open();
                DataSet DS = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(sql, thisConnection);
                da.Fill(DS);
                functionReturnValue = DS;
                thisConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return functionReturnValue;
        }
        public string SQLINSERT(string SQLQUERY)
        {
            string functionReturnValue = null;
            try
            {
                SqlConnection thisConnection = null;
                thisConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connstr"].ToString());
                string sql = SQLQUERY;

                thisConnection.Open();

                dynamic objCmd = null;
                objCmd = new System.Data.SqlClient.SqlCommand();
                var _with1 = objCmd;
                _with1.Connection = thisConnection;
                _with1.CommandType = CommandType.Text;
                _with1.CommandText = sql;
                objCmd.ExecuteNonQuery();




                functionReturnValue = "Data insertion Successful !";
                thisConnection.Close();

            }
            catch (Exception ex)
            {
                //  Lookups.LogErrorToText("Web", "Settings.vb", "FillMenus", ex.Message)

            }
            return functionReturnValue;
        }
        public int GetRandom(int Min, int Max)
        {
            System.Random Generator = new System.Random();
            return Generator.Next(Min, Max);
        }

        public List<string> FindEMailAddressesInStr(string StringWithEmails)
        {
            List<string> emailList = new List<string>();
            System.Text.RegularExpressions.MatchCollection regExMatch = System.Text.RegularExpressions.Regex.Matches(StringWithEmails, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

            for (int i = 0; i <= regExMatch.Count - 1; i++)
            {
                if (emailList.Contains(regExMatch[i].Value) == false)
                {
                    emailList.Add(regExMatch[i].Value);
                }
            }

            return emailList;
        }


        public int MyMod(int TotalNum, int MaxNum)
        {
            int n = 0;
            int m = 0;
            n = 1;
            m = MaxNum * n;
            while (!(TotalNum - m < MaxNum))
            {
                m = MaxNum * n;
                n = n + 1;
            }
            return TotalNum - m;
        }

        public List<string> ListRemoteFiles(string ftpAddress, string ftpUser, string ftpPassword, ref Exception ExceptionInfo)
        {
            List<string> functionReturnValue = null;

            List<string> ListOfFilesOnFTPSite = new List<string>();

            FtpWebRequest ftpRequest = null;
            FtpWebResponse ftpResponse = null;

            StreamReader strReader = null;
            string sline = "";

            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(ftpAddress);

                var _with3 = ftpRequest;
                _with3.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                _with3.Method = WebRequestMethods.Ftp.ListDirectory;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();

                strReader = new StreamReader(ftpResponse.GetResponseStream());

                if (strReader != null)
                    sline = strReader.ReadLine();

                while (sline != null)
                {
                    ListOfFilesOnFTPSite.Add(sline);
                    sline = strReader.ReadLine();
                }

            }
            catch (Exception ex)
            {
                ExceptionInfo = ex;

            }
            finally
            {
                if (ftpResponse != null)
                {
                    ftpResponse.Close();
                    ftpResponse = null;
                }

                if (strReader != null)
                {
                    strReader.Close();
                    strReader = null;
                }
            }

            functionReturnValue = ListOfFilesOnFTPSite;

            ListOfFilesOnFTPSite = null;
            return functionReturnValue;
        }
        public object InsertInto(string Fields, string TableName)
        {

            DataTable dt = new DataTable();
            dt = SQLREAD("SELECT * FROM " + TableName).Tables[0];
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();



            string InsertStatement = null;
            InsertStatement = "INSERT INTO " + TableName + " (";




            //Split the Comma Seperated Values 
            string[] Comma_Fields = null;
            Comma_Fields = Fields.Split(',');






            // Create a Dictionary of Input Values
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (string element in Comma_Fields)
            {
                string[] words = element.Split('=');
                dictionary.Add(words[0], words[1].ToLower());

            }




            //Prepare the First Half of Insert Statement
            foreach (string element in columnNames)
            {
                InsertStatement = InsertStatement + element + ",";
            }



            //Remove the Last Comma
            InsertStatement = InsertStatement.Remove(InsertStatement.Length - 1, 1);

            //Construct 
            InsertStatement = InsertStatement + ") VALUES (";







            //Prepare the Second Half of Insert Statement
            foreach (string element in columnNames)
            {
                string Fixer = null;
                Fixer = dictionary[element.ToLower()];
                if (IsNumeric(Fixer) == false)
                {
                    Fixer = "'" + Fixer + "'";
                }
                InsertStatement = InsertStatement + Fixer + ",";
            }



            //Remove the Last Comma
            InsertStatement = InsertStatement.Remove(InsertStatement.Length - 1, 1);






            return InsertStatement + ")";



        }

        //Smart Methods for CRUD
        public DataTable Findany(string Tablename, string searchkey)
        {



            DataTable dt = new DataTable();
            dt = SQLREAD("SELECT * FROM " + Tablename).Tables[0];
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(y => y.ColumnName).ToArray();

            string QUERY = null;
            QUERY = "SELECT * FROM " + Tablename + " WHERE " + columnNames[0] + " LIKE '%" + searchkey + "%' OR ";

            int x;
            for (x = 1; x <= columnNames.Length - 1; x++)
            {
                QUERY = QUERY + columnNames[x] + " LIKE '%" + searchkey + "%' OR ";
            }
            return SQLREAD(QUERY.Remove(QUERY.Length - 1, 4)).Tables[0];



        }

        public DataTable FindExact(string Tablename, string searchkey)
        {


            DataTable dt = new DataTable();
            dt = SQLREAD("SELECT * FROM " + Tablename).Tables[0];
            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(y => y.ColumnName).ToArray();



            string Fixer = null;
            Fixer = "'" + searchkey + "'";
            //Detect if it's Numeric
            if (IsNumeric(searchkey) == true)
            {
                Fixer = searchkey;
            }
            //Detect if it's a Date 
            if (Regex.IsMatch(searchkey, "((0?[13578]|10|12)(-|\\/)((0[0-9])|([12])([0-9]?)|(3[01]?))(-|\\/)((\\d{4})|(\\d{2}))|(0?[2469]|11)(-|\\/)((0[0-9])|([12])([0-9]?)|(3[0]?))(-|\\/)((\\d{4}|\\d{2})))") == true)
            {
                Fixer = searchkey;
            }





            string QUERY = null;
            QUERY = "SELECT * FROM " + Tablename + " WHERE " + columnNames[0] + " = " + Fixer + " OR ";

            int x;
            for (x = 1; x <= columnNames.Length - 1; x++)
            {
                QUERY = QUERY + columnNames[x] + " = " + searchkey + " OR ";
            }




            return SQLREAD(QUERY.Remove(QUERY.Length - 1, 4)).Tables[0];



        }


        public string UpdateAny(string Tablename, string UpdateStatement, string searchkey)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = SQLREAD("SELECT * FROM " + Tablename).Tables[0];
                string[] columnNames = dt.Columns.Cast<DataColumn>().Select(y => y.ColumnName).ToArray();

                string QUERY = null;
                QUERY = "UPDATE " + Tablename + " SET ";






                //Split the Comma Seperated Values 
                string[] Comma_Fields = null;
                Comma_Fields = UpdateStatement.Split(',');



                // Create a Dictionary of Input Values
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                foreach (string element in Comma_Fields)
                {
                    string[] words = element.Split('=');
                    dictionary.Add(words[0], words[1]);

                }

                foreach (KeyValuePair<string, string> kvp in dictionary)
                {
                    string Fixer = null;
                    Fixer = "'" + kvp.Value + "'";
                    //Detect if it's Numeric
                    if (IsNumeric(kvp.Value) == true)
                    {
                        Fixer = kvp.Value;
                    }
                    //Detect if it's a Date 
                    if (Regex.IsMatch(kvp.Value, "((0?[13578]|10|12)(-|\\/)((0[0-9])|([12])([0-9]?)|(3[01]?))(-|\\/)((\\d{4})|(\\d{2}))|(0?[2469]|11)(-|\\/)((0[0-9])|([12])([0-9]?)|(3[0]?))(-|\\/)((\\d{4}|\\d{2})))") == true)
                    {
                        Fixer = kvp.Value;
                    }

                    QUERY = QUERY + kvp.Key + "=" + Fixer + ",";
                }


                QUERY = QUERY.Remove(QUERY.Length - 1, 1);


                QUERY = QUERY + " WHERE " + columnNames[0] + " LIKE '%" + searchkey + "%' OR ";



                int x;
                //Build the Dynamic Full Text Search Feature 
                for (x = 1; x <= columnNames.Length - 1; x++)
                {
                    QUERY = QUERY + columnNames[x] + " LIKE '%" + searchkey + "%' OR ";
                }





                QUERY = safeoutput(QUERY);







                return SQLINSERT(QUERY.Substring(0, QUERY.Length - 4));






            }
            catch (Exception ex)
            {
                return Convert.ToString(ex);
            }



        }

        public string UpdateExact(string Tablename, string UpdateStatement, string searchkey)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = SQLREAD("SELECT * FROM " + Tablename).Tables[0];
                string[] columnNames = dt.Columns.Cast<DataColumn>().Select(y => y.ColumnName).ToArray();

                string QUERY = null;
                QUERY = "UPDATE " + Tablename + " SET ";




                //Split the Comma Seperated Values 
                string[] Comma_Fields = null;
                Comma_Fields = UpdateStatement.Split(',');



                // Create a Dictionary of Input Values
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                foreach (string element in Comma_Fields)
                {
                    string[] words = element.Split('=');
                    dictionary.Add(words[0], words[1]);
                }



                //Prepare the Final Statement 
                foreach (KeyValuePair<string, string> kvp in dictionary)
                {
                    string Fixer = null;
                    Fixer = "'" + kvp.Value + "'";
                    //Detect if it's Numeric
                    if (IsNumeric(kvp.Value) == true)
                    {
                        Fixer = kvp.Value;
                    }
                    //Detect if it's a Date 
                    if (Regex.IsMatch(kvp.Value, "((0?[13578]|10|12)(-|\\/)((0[0-9])|([12])([0-9]?)|(3[01]?))(-|\\/)((\\d{4})|(\\d{2}))|(0?[2469]|11)(-|\\/)((0[0-9])|([12])([0-9]?)|(3[0]?))(-|\\/)((\\d{4}|\\d{2})))") == true)
                    {
                        Fixer = "'" + kvp.Value + "'";
                    }




                    QUERY = QUERY + kvp.Key + "=" + Fixer + ",";
                }


                QUERY = QUERY.Remove(QUERY.Length - 1, 1);


                QUERY = QUERY + " WHERE " + columnNames[0] + " = " + searchkey + " OR ";



                int x;
                //Build the Dynamic Full Text Search Feature 
                for (x = 1; x <= columnNames.Length - 1; x++)
                {
                    QUERY = QUERY + columnNames[x] + " = " + searchkey + " OR ";
                }





                QUERY = safeoutput(QUERY);




                return SQLINSERT(QUERY.Substring(0, QUERY.Length - 4));










            }
            catch (Exception ex)
            {
                return Convert.ToString(ex);
            }



        }



        //RCMS Helper Methods
        public string Get_RCMS_XML(string ID)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = SQLREAD("SELECT * FROM RCMS_XML WHERE ID=" + ID + "").Tables[0];
                using (StreamWriter sw = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "/XML/" + dt.Rows[0]["ID"] + ".xml"))
                {
                    sw.Write(Convert.ToString(dt.Rows[0]["XML"]));

                }

                return dt.Rows[0]["ID"] + ".XML";

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public bool BitToBoolean(int Eval)
        //{

        //		if (Eval == 1)
        //			return true;
        //		if (Eval == 0)
        //			return false;


        //}

        //public bool fixNull(sItem)
        //{
        //	if (object.ReferenceEquals(sItem, System.DBNull.Value)) {
        //		return false;
        //	} else {
        //		return true;
        //	}
        //}
        //fix null values

        public string GetAny(string key, string TB, string Column, string Getval)
        {
            try
            {
                DataTable RESULT = new DataTable();
                RESULT = SQLREAD("SELECT * FROM " + TB + " WHERE " + Column + "='" + key + "'").Tables[0];
                return Convert.ToString(RESULT.Rows[0][Getval]);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //Safe Entry

        public string safeinput(string strinput)
        {

            try
            {
                string myString = null;
                string finalString = null;

                myString = strinput;


                finalString = myString.Replace("@", "at_email");
                myString = finalString;
                finalString = myString.Replace(",", "__");
                myString = finalString;
                finalString = myString.Replace("=", "equals_value");



                return finalString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string safeoutput(string strinput)
        {

            try
            {
                string myString = null;
                string finalString = null;


                myString = strinput;


                finalString = myString.Replace("at_email", "@").Replace("__", ",").Replace("equals_value", "=");


                return finalString;

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        //Login Logic
        public string DoLogin(string UserName, string PASSWORD)
        {
            try
            {
                DataTable Result = new DataTable();
                Result = SQLREAD("SELECT * FROM RCMS_USER WHERE RCMS_UID='" + UserName + "'").Tables[0];
                if (Result.Rows[0]["RCMS_PWD"] == PASSWORD)
                {
                    return Convert.ToString(Result.Rows[0]["ROLE_ID"]);
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public DataTable SecurityLayer(string AccessToken)
        {
            try
            {
                return SQLREAD("SELECT * FROM RCMS_USER WHERE ID=" + AccessToken + "").Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DateTime GetEgyptDate()
        {

            DateTime Mydate = default(DateTime);
            double addh = 0;
            addh = 2;
            Mydate = DateTime.UtcNow;
            Mydate = Mydate.AddHours(addh);


            return Mydate;
        }



        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }



    }
}


