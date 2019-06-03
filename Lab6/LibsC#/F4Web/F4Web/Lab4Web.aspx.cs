using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library_4;

namespace F4Web
{
    public partial class Lab4Web : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "Result: "+KI3_Class_4.F4(double.Parse(TextBox1.Text), double.Parse(TextBox2.Text)).ToString();
        }
    }
}