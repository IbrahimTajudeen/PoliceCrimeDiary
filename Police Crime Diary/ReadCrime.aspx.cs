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
    public partial class ReadCrime : System.Web.UI.Page
    {
        HtmlGenericControl message = null;
        public ReadCrime()
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
            if (usr.Type.ToLower() != "police")
            {
                Server.Transfer("~/Unauthorized.aspx");
                return;
            }

            try
            {
                string _page = Request.QueryString["page"];
                _page = (string.IsNullOrEmpty(_page)) ? "0" : _page;

                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT u.ID, u.Username, c.Name, c.Picture, u.Address, c.Sex, u.Phone, c.CrimeType, " +
                                                          $"c.Description, c.Evidence, c.DateofCrime, c.TimeOfCrime, c.DateReported, c.ID, (SELECT COUNT(*) FROM CrimeReport) From Users u " +
                                                          $"JOIN CrimeReport c " +
                                                          $"ON c.ReportedBy = u.ID " +
                                                          $"ORDER BY c.ID OFFSET {_page} ROWS FETCH NEXT 2 ROWS ONLY; ", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                crimes.InnerHtml = "";
                                int max = 0;
                                while (reader.Read())
                                {
                                    crimes.InnerHtml += $"<tr data-id=\"{reader.GetValue(13).ToString()}\"><td><div class=\"btn btn-primary\">Take&nbsp;Case</div></td>" +
                                                            $"<td>{PoliceDiaryEncription.Decrypt(reader.GetValue(1).ToString())}</td>" +
                                                            $"<td>{reader.GetValue(0).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(2).ToString()}</td>" +
                                                            $"<td><img src=\"/uploads/suspects/{reader.GetValue(3).ToString()}\" style=\"height: 100px; width: 100px;\"/></td>" +
                                                            $"<td>{reader.GetValue(4).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(5).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(6).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(7).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(8).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(9).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(10).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(11).ToString()}</td>" +
                                                            $"<td>{reader.GetValue(12).ToString()}</td></tr>";
                                    max = int.Parse(reader.GetValue(14).ToString());
                                }
                                showing.InnerText = $"Showing {((_page == "0") ? "1" : _page)} to {Math.Min(int.Parse(_page) + 5, max)} of {max} entries";

                                prev_page.Attributes["href"] = ((int.Parse(_page) <= 0) ? "#" : "?page=" + (int.Parse(_page) - 5));
                                nxt_page.Attributes["href"] = (((int.Parse(_page) + 5) >= max) ? "#" : "?page=" + (int.Parse(_page) + 5));

                                if (_page != "0")
                                {
                                    int start = 5, number = 1;
                                    page_count.InnerText = number.ToString();
                                    while (!Enumerable.Range(0, start).Contains(int.Parse(_page)))
                                    {
                                        number++;
                                        page_count.InnerText = number.ToString();
                                        start += 5;
                                    }
                                }
                                else page_count.InnerText = "1";

                                prev_page.Attributes["class"] = ((int.Parse(_page) <= 0) ? "btn btn-secondary disabled" : "btn btn-info");
                                nxt_page.Attributes["class"] = (((int.Parse(_page) + 5) >= max) ? "btn btn-secondary disabled" : "btn btn-info");
                            }
                            else
                            {
                                crimes.InnerHtml = "<tr><td colspan=\"14\" style=\"text-align: center; font-weight: bold; \">No Crime Report Yet</td></tr>";

                                page_count.InnerText = "0";
                                prev_page.Attributes["class"] = "btn btn-secondary disabled";
                                nxt_page.Attributes["class"] = "btn btn-secondary disabled";
                            }
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