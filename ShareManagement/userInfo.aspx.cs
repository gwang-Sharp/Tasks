using Management;
using ServiceDAL;
using System;
using System.Data;
using System.IO;
using System.Web;

namespace ShareManagement
{
    public partial class Products_List : System.Web.UI.Page
    {
        protected DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            dt = DBContent.Select();
        }
    }
}