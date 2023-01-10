# Spyro

A Web Application built in ASP.NET (C#)

This application is developed to run a legacy application named SPYRO from the network drive.

SPYRO - Requires an input file with .tst extension for it to execute the process.

This application will work only from PowerBI as it generates the required input file via the query string.

To run it outside of PowerBI, the exact query string has to be generated along with the URL.

Pages:
  1. Load:  Takes the query string and initializes the properties of Default and redirects to Default. 
  2. Default:  Loads the form with the initialized properties.
  3. Contact: Contact page to reach support team.

Event Trigger function: protected void ExecuteTAdjust_Click(object sender, EventArgs e)

Static Functions:
  1. public static void Initialize(string user, string title, string ch4, string tst) //Function to intialize of Default.
  
  2. public static void SetEnvironments() //Create folder and input file
  
  3. public static int CalculateTAdjust(double Methane) //Main logic implementation for TAdjust
  
  4. public static string GetBetween(string strSource, string strStart, string strEnd) //Get values 2 between strings.
  
  5. An Overloading writer function: // Function to write to text file
          a. public static void LogWriter()
          b. public static void LogWriter(string attr)
          c. public static void LogWriter(string attr, double value)
  
  6. public static void GetF12() //Get contents of F12 file

  7. public static void RunSPYRO() //Execute the spyro application.
  
  8. public static int SetTAdjust() //Gets the new TAdjusted value and updates it in the input file
  
  9. public static void GetTAdjust() //Gets final TAdjust value from the file. 


