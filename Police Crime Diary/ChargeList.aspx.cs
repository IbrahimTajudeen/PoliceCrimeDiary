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
    public partial class ChargeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["usr"] == null)
            {
                Response.Redirect("~/welcomeguest");
                return;
            }
            User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies["usr"].Value));
            if (usr.Type.ToLower() != "police")
            {
                Server.Transfer("~/Unauthorized.aspx");
                return;
            }

            try
            {
                bail_crimes.InnerHtml = "<tr><td colspan=\"7\" style=\"text-align: center; font-weight: bold; \">No Charge Crime Yet!</td></tr>";
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT c.ID, r.Name, r.Picture, c.CourtName, c.CourtDate, u.Name, c.Status FROM ChargedCrimes c JOIN CrimeReport r ON r.ID = c.CrimeID JOIN Users u ON u.ID = c.OfficerInCharge", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                bail_crimes.InnerHtml = "";
                                while (reader.Read())
                                {
                                    bail_crimes.InnerHtml += $"<tr data-id=\"{reader.GetValue(0).ToString()}\"><td>{reader.GetValue(1).ToString()}</td><td><img src=\"/uploads/suspects/{reader.GetValue(2).ToString()}\" style=\"height: 100px; width: 100px;\" /></td><td>{reader.GetValue(3).ToString()}</td><td>{reader.GetValue(4).ToString()}</td><td>{reader.GetValue(5).ToString()}</td><td>{reader.GetValue(6).ToString()}</td><td><a class=\"btn btn-primary\" href=\"Charge.aspx?edit={reader.GetValue(0).ToString()}\">Edit</a></td></tr>";
                                }
                            }
                            else
                                bail_crimes.InnerHtml = "<tr><td colspan=\"7\" style=\"text-align: center; font-weight: bold; \">No Charge Crime Yet!</td></tr>";
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