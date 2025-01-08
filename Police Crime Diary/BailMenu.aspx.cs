using Newtonsoft.Json;
using Police_Crime_Diary.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Police_Crime_Diary
{
    public partial class BailMenu : System.Web.UI.Page
    {
        HtmlGenericControl message = null;
        public BailMenu()
        {
            message = new HtmlGenericControl("ul");
            message.Attributes.Add("style", "list-style: none;");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["usr"] == null)
            {
                Response.Redirect("~/welcomeguest");
                return;
            }
            User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies["usr"].Value));
            if (usr.Type.ToLower() != "admin")
            {
                Server.Transfer("~/Unauthorized.aspx");
                return;
            }

            try
            {
                string search = Request.QueryString["search"];
                search = (string.IsNullOrEmpty(search)) ? "" : search;

                string delete = Request.QueryString["delete"];
                delete = (string.IsNullOrEmpty(delete)) ? "" : delete;

                if (delete != "")
                {
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand($"DELETE FROM BailList WHERE ID = {delete}", sqlCon))
                        {
                            if (sqlCmd.ExecuteNonQuery() > 0)
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = $"Bail Record is deleted successfully";
                                li.Attributes.Add("style", "color: green; border: 1px solid green;  padding: 5px; background-color: white;");
                                message.Controls.Add(li);
                            }
                            else Response.Redirect("~/BailMenu.aspx");
                        }
                    }
                }

                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT b.ID, c.Name, b.BailAmount, b.AmountOffered, u.Name, b.Status, c.Picture FROM BailList b " +
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
                                    bail_crimes.InnerHtml += $"<tr data-id=\"{reader.GetValue(0).ToString()}\"><td>{reader.GetValue(1).ToString()}</td><td><img src=\"/uploads/suspects/{reader.GetValue(6).ToString()}\" style=\"height: 100px; width: 100px;\" /></td><td>{reader.GetValue(2).ToString()}</td><td>{reader.GetValue(3).ToString()}</td><td>{reader.GetValue(4).ToString()}</td><td>{reader.GetValue(5).ToString()}</td><td><a class=\"btn btn-danger\" href=\"?delete={reader.GetValue(0).ToString()}\">Delete</a></td></tr>";
                                }
                            }
                            else
                                bail_crimes.InnerHtml = "<tr><td colspan=\"7\" style=\"text-align: center; font-weight: bold; \">No Bail Recored Yet!</td></tr>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.InnerText = ex.Message;
                li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                message.Controls.Add(li);
            }
            if (IsPostBack)
            {
                if (message.Controls.Count > 0)
                {
                    display_message.Controls.Add(message);
                }
            }
        }
    }
}