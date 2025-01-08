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
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        HtmlGenericControl generic_message = null;

        public Site_Mobile()
        {
            generic_message = new HtmlGenericControl("ul");
            generic_message = new HtmlGenericControl("ul");
            generic_message.Attributes.Add("style", "list-style: none;");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack || !IsPostBack)
                {
                    generic_message.Controls.Clear();
                    string path = Request.CurrentExecutionFilePath;
                    List<string> allowed_path = new List<string> { "register", "todaycrime", "crimediary", "welcomeguest" };
                    path = Path.GetFileNameWithoutExtension(path.Substring(path.LastIndexOf("/") + 1).ToLower());
                    
                    if (Request.Cookies["usr"] == null)
                    {
                        low_logout.Attributes.Add("style", "display: none;");
                        high_logout.Attributes.Add("style", "display: none;");
                    }

                    if ((Request.Cookies["usr"] == null || string.IsNullOrEmpty(Request.Cookies["usr"].Value)) && !allowed_path.Contains(path))
                    {
                        Response.Redirect("~/welcomeguest.aspx");
                    }
                    else
                    {
                        if (Request.Cookies.Get("usr") != null)
                        {
                            User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies.Get("usr").Value));
                            curr_user.Attributes["class"] = "text-success fs-4";
                            curr_user.InnerText = usr.Username;
                            unregistered.Attributes.Add("style", "display: none;");
                            registered.Attributes["style"] = "";

                            if (usr.Type.ToLower() != "user")
                            {
                                low_users.Attributes.Add("style", "display: none;");
                                high_users.Attributes["style"] = "";
                            }
                            if (usr.Type.ToLower() != "admin")
                            {
                                police_btn.Attributes.Add("style", "display: none;");
                                bail.Attributes.Add("style", "display: none;");
                            }
                        }
                    }
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand("SELECT COUNT(*) FROM CrimeReport", sqlCon))
                        {
                            using (SqlDataReader reader = sqlCmd.ExecuteReader())
                            {
                                reader.Read();
                                crime_count.InnerText = reader.GetValue(0).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.InnerText = ex.Message;
                li.Attributes.Add("style", "color: red; border: 1px solid red; padding: 5px; background-color: white;");
                generic_message.Controls.Add(li);
            }

            if (IsPostBack)
            {
                if (generic_message.Controls.Count > 0)
                {
                    master_message.Controls.Add(generic_message);
                }
            }
        }
    }
}