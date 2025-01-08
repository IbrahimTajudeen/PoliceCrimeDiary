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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
            {
                sqlCon.Open();
                User usr = new User();
                string _select = $"SELECT TOP 1 ID Username FROM tblUser WHERE Username = '{username.Value}' AND Password = '{PoliceDiaryEncription.Encrypt(password.Value)}'";
                using (SqlCommand sqlCmd = new SqlCommand(_select, sqlCon))
                {
                    using (SqlDataReader reader = sqlCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            usr.ID = int.Parse(reader.GetValue(0).ToString());
                            usr.Username = reader.GetValue(1).ToString();
                            HttpCookie cok = new HttpCookie("user");
                            cok.Value = PoliceDiaryEncription.Encrypt(JsonConvert.SerializeObject(usr));
                            cok.HttpOnly = true;
                            cok.Secure = true;
                            Response.Cookies.Add(cok);

                            Response.Redirect("~/Default.aspx");
                        }
                    }       
                }
            }
            if(IsPostBack)
            {
                Response.Write("<div class=\"btn btn-sm mb-2 btn-danger border d-none align-items-center justify-content-between\" runat=\"server\" id=\"message\"> " +
                                $"<span class=\"btn fw-bold text-light\">Invalid Log in credentials</span>" +
                                "<span class=\"btn fw-bold text-light close\">X</span>" +
                                "</div>");
            }
        }
    }
}