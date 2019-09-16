using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Veerabook
{
    public class Merger
    {


         Xtools _xtools = new Xtools();

        public DataTable FinalSearchData(string sessionSearchToken ,string sessionId)
        {



            DataTable dt = _xtools.SQLREAD("SELECT * FROM VEERA_RESULT WHERE SESSIONID='" + sessionId + "'").Tables[0];
            return _xtools.RemoveDuplicateRows(dt, "Hotel_Name");



        }

        public DataTable FinalSearchDataRooms(string sessionSearchToken, string sessionId,string hotelname)
        {

            DataSet ds = new DataSet("dataSet");

            DataTable dt = _xtools.SQLREAD("SELECT * FROM VEERA_RESULT WHERE SESSIONID='" + sessionId + "' AND Hotel_Name Like '%" + hotelname + "%' ORDER BY Room_BasePrice ASC").Tables[0];
            return dt;

        }


    }
}