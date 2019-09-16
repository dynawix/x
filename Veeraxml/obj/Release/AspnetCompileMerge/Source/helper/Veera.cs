using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Veerabook
{
    public class Veera
    {
        //Imports
        Multi multi = new Multi();
        Xtools xtools = new Xtools() ; 
        public string VeeraLogin()
        {

            string VeeraSession;
            Guid id = Guid.NewGuid();


            VeeraSession = id.ToString();

            //Login To MultiReisen
            String MR_Session;
            MR_Session = multi.GetLoginsession();

            //Login to Bookingee 

            //Login to TBO

        //    xtools.InsertInto(VeeraSession + "@V_session", "VeeraSession");

            return VeeraSession;
        }







    }
}