using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Spyro_Web_App_v1
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["ErrorMsg"] != null)
            {
                Response.Write("<br/><h2 style=\"background-color:red; text-align:center\" >" + Session["ErrorMsg"] + "!!!</h2>");
                Response.Write("<br/><p style=\"background-color:black; text-align:center; color:white;\" >Application works when the link is generated from PowerBI");
            }
        }
    }
}