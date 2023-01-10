using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Spyro_Web_App_v1
{
    public partial class Load : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currUser, ch4, tstString, title;
          
            if (!IsPostBack)
            {

                bool hasKeys = Request.QueryString.HasKeys();
                if (hasKeys)
                {
                    Uri theRealURL = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.RawUrl);
                    currUser = HttpUtility.ParseQueryString(theRealURL.Query).Get("name"); //Capture Logged User
                    ch4 = HttpUtility.ParseQueryString(theRealURL.Query).Get("ch4"); //Capture TST Input File
                    tstString = HttpUtility.ParseQueryString(theRealURL.Query).Get("TST"); //Capture TST Input File
                    currUser = currUser.Replace("\"", "");
                    tstString = tstString.Replace("\"", "");
                    tstString = tstString.Replace("`", System.Environment.NewLine);
                    tstString = tstString.Replace("~", "+");
                    title = _Default.GetBetween(tstString, "TITLE=&", "KEYW");
                    title = title.Trim();

                   
                    //Initialize Default Page Variables//
                    _Default.Initialize(currUser, title, ch4, tstString);
                    
                    Response.Redirect(ConfigurationManager.AppSettings["CLEANED_URL"]);
                    
                }
            }
            
        }
    }
}