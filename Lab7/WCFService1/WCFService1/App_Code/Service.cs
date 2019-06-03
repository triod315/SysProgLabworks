using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    public string GetData(int value)
    {
        return string.Format("You entered: {0}", value);
    }

    public string F4_WebService(int x)
    {
        return Library_4.KI3_Class_4.F4(x, 3).ToString();
    }

    public System.Data.DataTable GetAllElements()
    {
        string DB = "Data Source=DESKTOP-5582JAK\\SQLEXPRESS;Initial Catalog=TradeDB;Integrated Security=True";
        System.Data.DataTable dt =new System.Data.DataTable("TradingEnterprises");
        SqlConnection Connection = new SqlConnection(DB);
        SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from Trading_enterprisessUkraine", Connection);
        dataAdapter.Fill(dt);
        return dt;
    }

    public System.Data.DataTable GetElementByID(int ID)
    {
        string DB = "Data Source=DESKTOP-5582JAK\\SQLEXPRESS;Initial Catalog=TradeDB;Integrated Security=True";
        System.Data.DataTable dt = new System.Data.DataTable("TradingEnterprises");
        SqlConnection Connection = new SqlConnection(DB);
        SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Trading_enterprisessUkraine where ID="+ID, Connection);
        dataAdapter.Fill(dt);
        return dt;
    }

    public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}
}
