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
    public partial class ReplyBail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["usr"] == null)
            {
                Response.Redirect("~/welcomeguest");
                return;
            }
            User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies["usr"].Value));
            if (usr.Type.ToLower() != "admin" && usr.Type.ToLower() != "police")
            {
                Server.Transfer("~/Unauthorized.aspx");
                return;
            }

            try
            {
                string search = Request.QueryString["search"];
                search = (string.IsNullOrEmpty(search)) ? "" : search;

                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT b.ID, c.Name, b.BailAmount, b.AmountOffered, u.Name, b.Status FROM BailList b " +
                                                                $"LEFT JOIN CrimeReport c " +
                                                                $"ON c.ID = b.SuspectID LEFT JOIN Users u ON b.OfficerInCharge = u.ID " +
                                                                $"WHERE c.Name LIKE '%{search}%' OR u.Name LIKE '%{search}%' ORDER BY ID DESC", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                bail_crimes.InnerHtml = "";
                                while (reader.Read())
                                {
                                    bail_crimes.InnerHtml += $"<tr data-id=\"{reader.GetValue(0).ToString()}\"><td><div class=\"btn btn-info view-btn\">View&nbsp;Info</div></td><td>{reader.GetValue(1).ToString()}</td><td>{reader.GetValue(2).ToString()}</td><td>{reader.GetValue(3).ToString()}</td><td>{reader.GetValue(4).ToString()}</td><td>{reader.GetValue(5).ToString()}</td></tr>";
                                }
                            }
                            else
                                bail_crimes.InnerHtml = "<tr><td colspan=\"6\" style=\"text-align: center; font-weight: bold; \">No Bail Recored Yet!</td></tr>";
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