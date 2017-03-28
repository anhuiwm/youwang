using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorLogView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["filename"] != null)
        {
            string filename = Request.Params["filename"];

            StreamReader sr = new StreamReader(filename);
            string str = string.Empty;
            while((str = sr.ReadLine()) != null)
            {
                Response.Write(str + "<br>");
            }
            sr.Close();
        }
    }
}