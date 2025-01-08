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
    public partial class Police : System.Web.UI.Page
    {
        HtmlGenericControl message = null;

        public Police()
        {
            message = new HtmlGenericControl("ul");
            message.Attributes.Add("style", "list-style: none;");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                message.Controls.Clear();
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
                string search = Request.QueryString["search"];
                search = (string.IsNullOrEmpty(search)) ? "" : search;

                string delete = Request.QueryString["delete"];
                delete = (string.IsNullOrEmpty(delete)) ? "" : delete;
                
                if(delete != "")
                {
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand($"SELECT TOP 1 Name, Username, Picture FROM Users WHERE ID = {delete} AND LOWER(UserType) = 'police'", sqlCon))
                        {
                            string picture = "", name = "";
                            using (SqlDataReader reader = sqlCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    name = $"Officer Name: '{reader.GetValue(0)}' ({PoliceDiaryEncription.Decrypt(reader.GetValue(1).ToString())})";
                                    picture = reader.GetValue(2).ToString();
                                    break;
                                }
                            }
                            string del = $"DELETE FROM Users WHERE ID = {delete} AND LOWER(UserType) = 'police'";
                            sqlCmd.CommandText = del;
                            if (sqlCmd.ExecuteNonQuery() > 0)
                            {
                                if (File.Exists(Path.Combine(Server.MapPath("~/uploads/user"), picture)))
                                    File.Delete(Path.Combine(Server.MapPath("~/uploads/user"), picture));

                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = $"{name} is deleted successfully";
                                li.Attributes.Add("style", "color: green; border: 1px solid green;  padding: 5px; background-color: white;");
                                message.Controls.Add(li);
                            }
                            else Response.Redirect("~/Police.aspx");
                        }
                    }
                }

                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT ID, Name, Email, Phone, Picture, JoinDate, Username FROM Users WHERE UserType = 'Police' AND(" +
                        $"Name LIKE '%{search}%' OR Email LIKE '%{search}%' OR Phone LIKE '%{search}%') ORDER BY ID DESC", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                police_list.InnerHtml = "";
                                while (reader.Read())
                                {
                                    police_list.InnerHtml += $"<tr data-id=\"{reader.GetValue(0).ToString()}\"><td>{reader.GetValue(1).ToString()}</td>" +
                                        $"<td>{reader.GetValue(2).ToString()}</td><td>{reader.GetValue(3).ToString()}</td><td><img src=\"/uploads/user/{reader.GetValue(4).ToString()}\" style=\"width: 100px;\" /></td>" +
                                        $"<td>{DateTime.Parse(reader.GetValue(5).ToString()).ToShortDateString()}</td><td>{PoliceDiaryEncription.Decrypt(reader.GetValue(6).ToString())}</td><td><a href=\"?delete={reader.GetValue(0).ToString()}\" class=\"btn btn-danger delete\">Delete</a></td></tr>";
                                }
                            }
                            else
                                police_list.InnerHtml = "<tr><td colspan=\"6\" style=\"text-align: center; font-weight: bold; \">No Police Found</td></tr>";

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
            
            if (message.Controls.Count > 0)
            {
                display_message.Controls.Add(message);
            }
        }
    }
}