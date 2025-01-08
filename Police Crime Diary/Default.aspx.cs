using Newtonsoft.Json;
using Police_Crime_Diary.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Police_Crime_Diary
{
    public partial class _Default : Page
    {
        HtmlGenericControl message = null;
        
        public _Default()
        {
            
            message = new HtmlGenericControl("ul");
            message.Attributes.Add("style", "list-style: none;");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                notify.InnerHtml = $"<div class=\"row justify-content-center align-items-center mx-2 p-1 rounded-3 border\" style=\"background-color: #f0f5f5;\">No Recent Notification</div>";
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT TOP 5 ID, Name, CrimeType, Location FROM CrimeReport ORDER BY ID DESC", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                recent_crimes.InnerHtml = "";
                                while (reader.Read())
                                    recent_crimes.InnerHtml += $"<tr data-id=\"{reader.GetValue(0).ToString()}\"><td>{reader.GetValue(1).ToString()}</td><td>{reader.GetValue(1).ToString()}</td><td>{reader.GetValue(2).ToString()}</td></tr>";
                            }
                            else
                                recent_crimes.InnerHtml = "<tr><td colspan=\"3\" style=\"text-align: center; font-weight: bold; \">No Recent Crime</td></tr>";

                        }
                    }
                }
                if (Request.Cookies["usr"] == null)
                    return;

                User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies["usr"].Value));

                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT TOP 5 i.ID, u.Username, i.PostMessage, i.PostDate, i.PostTime FROM Inbox i JOIN Users u ON i.SenderID = u.ID WHERE {(usr.Type.ToLower() == "user" ? "IsSecret = 'NULL' " : "IsSecret = 'NULL' OR IsSecret = 'True '")} AND(OnlyBy = 'NULL' OR OnlyBy = {usr.ID}) ORDER BY i.ID DESC", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            notify.InnerHtml = "";
                            if(reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    notify.InnerHtml += $"<div class=\"row align-items-center m-2 p-1 rounded-3 border\" style=\"background-color: #f0f5f5;\"> " +
                                                       $"<div class=\"col-2\"> " +
                                                            $"<div class=\"fw-bold border-bottom\">By</div> " +
                                                            $"<div class=\"fw-italic\">{PoliceDiaryEncription.Decrypt(reader.GetValue(1).ToString())}</div> " +
                                                        $"</div> " +
                                                        $"<div class=\"col\"> " +
                                                            $"<div class=\"fw-bold border-bottom\">Message</div> " +
                                                            $"<div class=\"fw-italic\">{reader.GetValue(2).ToString()}</div> " +
                                                        $"</div> " +
                                                        $"<div class=\"col-2\"> " +
                                                            $"<div class=\"fw-bold border-bottom\">Date</div> " +
                                                            $"<div class=\"fw-italic\">{DateTime.Parse(reader.GetValue(3).ToString()).ToShortDateString()}</div> " +
                                                            $"<div class=\"fw-bold border-bottom\">Time</div> " +
                                                            $"<div class=\"fw-italic\">{DateTime.Parse(reader.GetValue(4).ToString()).ToShortTimeString()}</div></div></div>";
                                }
                            }
                            else notify.InnerHtml = $"<div class=\"row justify-content-center align-items-center mx-2 p-1 rounded-3 border\" style=\"background-color: #f0f5f5;\">No Recent Notification</div>";
                        }
                    }
                }


                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT TOP 1 Name, Username, UserType, Picture FROM Users WHERE ID = {usr.ID}", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                name.InnerText = reader.GetValue(0).ToString();
                                username.InnerText = PoliceDiaryEncription.Decrypt(reader.GetValue(1).ToString());
                                userType.InnerText = reader.GetValue(2).ToString();
                                userImg.Attributes["src"] = "/uploads/user/" + reader.GetValue(3).ToString();
                            }
                        }
                    }
                }
                
                if(usr.Type.ToLower() != "user")
                {
                    user_links.Attributes.Add("style", "display: none;");
                    high_links.Attributes["style"] = "";
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