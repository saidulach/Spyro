using Antlr.Runtime;
using Microsoft.AspNet.FriendlyUrls.ModelBinding;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;


namespace Spyro_Web_App_v1
{
    public partial class _Default : Page
    {
        //Settting constants for Spyro exe and input file location
        private static bool INITIALIZED = false;
        private static string SPYRO_LOC = ConfigurationManager.AppSettings["SCFS_PATH"].ToString();
        private static string INPUT_LOC = ConfigurationManager.AppSettings["INPUT_FILE_PATH"].ToString();

        private static double CH4;
        private static double T_ADJUST;
        private static string F12_CONTENTS;
        private static string CURR_USER;
        private static string TST_STRING;
        private static string TITLE;
        private static string FILE_PATH; 

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblUser.Text = CURR_USER;
                    lblTitle.Text = TITLE;
                    lblCH4.Text = CH4.ToString();
                    txtTst.Text = TST_STRING;
                    
                }
            }
            catch (Exception ex)
            {
                Session["ErrorMsg"] = ex.Message;
                Response.Redirect("~/Contact.aspx?");
            }
            
        }
        protected void ExecuteTAdjust_Click(object sender, EventArgs e)
        {
            //Proceed only if values are initialized
            if (INITIALIZED)
            {
                SetEnvironments();
                LogWriter("Executed By: " + CURR_USER);
                LogWriter();
                CalculateTAdjust(CH4);
                GetTAdjust();
                txtTAdjust.Text = T_ADJUST.ToString();
                lblmsg.Text = "Process Completed";
            }
            else
            {
                lblmsg.Text = "No information to process!";
            }
            
        }

        public static void Initialize(string user, string title, string ch4, string tst)
        {
            CURR_USER = user;
            TITLE = title;
            TST_STRING = tst;
            CH4 = Convert.ToDouble(ch4);
            INITIALIZED = true;
        }

        public static void SetEnvironments()
        {
            try
            {
                FILE_PATH = INPUT_LOC + CURR_USER + "\\" + TITLE + "-" + DateTime.UtcNow.ToString("dd.MM.yyyy HH.mm.ss");
               
                //Check directory in network drive and create folders
                if (!Directory.Exists(FILE_PATH))
                {
                    Directory.CreateDirectory(FILE_PATH);
                }
                
                File.WriteAllText(FILE_PATH + "\\Input.tst", TST_STRING);
            }

            catch (Exception ex)
            {
                HttpContext.Current.Session["ErrorMsg"] = ex.Message;
                HttpContext.Current.Response.Redirect("~/Contact.aspx");
            }
        }
        public static int CalculateTAdjust(double Methane)
        {
            T_ADJUST = 0;
            double OldTAdjust = -5;
            double NewTAdjust = 5;
            double OldMethane, NewMethane, Slope, Offset, Delta;

            LogWriter("T_ADJUST", T_ADJUST);
            LogWriter("Old TAdjust", OldTAdjust);
            LogWriter("New TAdjust", NewTAdjust);

            SetTAdjust(OldTAdjust);
            RunSPYRO();
            GetF12();

            OldMethane = Convert.ToDouble(GetBetween(F12_CONTENTS, "2 CH4", "3 C2H2"));
            LogWriter("Old Methane", OldMethane);
            
            if (OldMethane <= 0)
            {
                return 0;
            }

            SetTAdjust(NewTAdjust);
            RunSPYRO();
            GetF12();

            NewMethane = Convert.ToDouble(GetBetween(F12_CONTENTS, "2 CH4", "3 C2H2"));
            LogWriter("New Methane", NewMethane);

            if (NewMethane <= 0)
            {
                return 0;
            }

            int Count = 0;
            LogWriter("*************XXXXXXXXXX*************");
            LogWriter();

            do
            {
                LogWriter("Loop : ", Count + 1);
                Slope = (NewMethane - OldMethane) / (NewTAdjust - OldTAdjust);
                Offset = NewMethane - (Slope * NewTAdjust);
                OldTAdjust = NewTAdjust;
                OldMethane = NewMethane;

                LogWriter("Slope", Slope);
                LogWriter("Offset", Offset);
                LogWriter("Old TAdjust", OldTAdjust);
                LogWriter("Old Methane", OldMethane);

                if (Slope > 0)
                {
                    NewTAdjust = (Methane - Offset) / Slope;
                }
                else
                {
                    NewTAdjust = 5;
                }

                LogWriter("New TAdjust", NewTAdjust);

                if (Math.Abs(NewTAdjust) > 1000)
                {
                    return 0;
                }

                SetTAdjust(NewTAdjust);
                RunSPYRO();
                GetF12();

                NewMethane = Convert.ToDouble(GetBetween(F12_CONTENTS, "2 CH4", "3 C2H2"));
                LogWriter("New Methane", NewMethane);

                if (NewMethane <= 0)
                {
                    return 0;
                }

                Delta = Math.Abs(Methane - NewMethane);

                if (Count > 30)
                {
                    break;
                }

                LogWriter("Delta", Delta);
                Count++;
                LogWriter();

            } while (Delta > 0.009);

            T_ADJUST = NewTAdjust;
            LogWriter("T_ADJUST",T_ADJUST);

            return 1;

        }
        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        public static void LogWriter()
        {
            File.AppendAllText(FILE_PATH + "\\Log.txt",Environment.NewLine);
        }
        public static void LogWriter(string attr)
        {
            File.AppendAllText(FILE_PATH + "\\Log.txt", attr + Environment.NewLine);
        }
        public static void LogWriter(string attr, double value)
        {
            File.AppendAllText(FILE_PATH + "\\Log.txt", attr + ":" +value + Environment.NewLine);
        }
        public static void GetF12()
        {
            F12_CONTENTS = File.ReadAllText(FILE_PATH + "\\Results\\INPUT.EFPS.F12");
        }
        public static void RunSPYRO()
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = SPYRO_LOC;
            p.StartInfo.Arguments = FILE_PATH + "\\Input.tst";
            p.Start();
            p.WaitForExit();
  
        }
        
        public static int SetTAdjust(double TAdjust)
        {
            string Contents = File.ReadAllText(FILE_PATH + "\\Input.tst");

            if(Contents == "")
            {
                return 0;
            }
            int p = Contents.IndexOf("TADJ=") + 5;

            if (p == 0)
            {
                return 0;
            }

            string tempString = GetBetween(Contents, "TADJ=", "KEYW=&REACT");
            string s1 = Contents.Substring(0, p);
            string s2 = Contents.Substring(p + tempString.Length);
            
            TAdjust = Math.Round(TAdjust,5);
            LogWriter("T_Adjust", TAdjust);

            String s = s1 + Convert.ToString(TAdjust) + "\n" + s2;
            File.WriteAllText(FILE_PATH+ "\\Input.tst", s);
            return 1;

        }
        public static void GetTAdjust()
        {
            FILE_PATH = "\\\\dmke-amslab.shell.com\\EinsteinPTC_Test\\SPYRO\\Users\\Saidul.Choudhury\\AtH2020-41 80646 088-840_W3 06.01.2023 22.08.49";
            string Contents = File.ReadAllText(FILE_PATH + "\\Results\\INPUT.EFPS.F12");
            string tempString = GetBetween(Contents, "2 CH4", "3 C2H2");    
            T_ADJUST = Convert.ToDouble(tempString.Trim());
        }



    }
}
