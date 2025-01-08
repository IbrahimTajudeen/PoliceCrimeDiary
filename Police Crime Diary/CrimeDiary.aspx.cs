using Newtonsoft.Json;
using Police_Crime_Diary.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Police_Crime_Diary
{
    public partial class CrimeDiary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["usr"] == null)
                {
                    Response.Redirect("~/welcomeguest");
                    return;
                }
                User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies["usr"].Value));
                if (usr.Type.ToLower() != "user")
                {
                    Server.Transfer("~/Unauthorized.aspx");
                    return;
                }
                string search = Request.QueryString["search"];
                search = (string.IsNullOrEmpty(search)) ? "" : search;
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT ID, Name, CrimeType, Location, Evidence, Description, DateofCrime, TimeOfCrime, DateReported FROM CrimeReport " +
                                                                $"WHERE Name LIKE '%{search}%' OR Evidence LIKE '%{search}%' OR CrimeType LIKE '%{search}%' OR Location LIKE '%{search}%' ORDER BY ID DESC", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                crime_list.InnerHtml = "";
                                while (reader.Read())
                                {
                                    crime_list.InnerHtml += $"<tr data-id=\"{reader.GetValue(0).ToString()}\"><td>{reader.GetValue(1).ToString()}</td><td>{reader.GetValue(2).ToString()}</td><td>{reader.GetValue(3).ToString()}</td>"+
                                        $"<td>{reader.GetValue(4).ToString()}</td><td>{reader.GetValue(5).ToString()}</td><td>{DateTime.Parse(reader.GetValue(6).ToString()).ToShortDateString()}</td><td>{reader.GetValue(7).ToString()}</td><td>{DateTime.Parse(reader.GetValue(8).ToString()).ToShortDateString()}</td></tr>"; ;
                                }
                            }
                            else
                                crime_list.InnerHtml = "<tr><td colspan=\"8\" style=\"text-align: center; font-weight: bold; \">No Crime Reported Yet!</td></tr>";

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}