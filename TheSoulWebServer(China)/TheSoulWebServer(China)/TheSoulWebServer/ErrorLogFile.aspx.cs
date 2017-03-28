using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorLogFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string MyWriteFile = Request.PhysicalApplicationPath + @"log\";
        DirectoryInfo FileRead = new DirectoryInfo(MyWriteFile);
        foreach (FileInfo flist in FileRead.GetFiles())
        {
            Response.Write("<a href='ErrorLogView.aspx?filename=" + flist.FullName + "'>" + flist.FullName + "</a><br>");
        }
    }
}