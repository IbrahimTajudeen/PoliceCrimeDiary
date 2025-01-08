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
    public partial class CreatePolice : System.Web.UI.Page
    {
        HtmlGenericControl message = null;
        public CreatePolice()
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
        }

        protected void register_Click(object sender, EventArgs e)
        {
            message.Controls.Clear();
            CreateUser cusr = new CreateUser();
            List<int> _li = new List<int> { 11, 14, 22, 28, 25 };
            cusr.Name = name.Value;
            cusr.Sex = sex.Value;
            cusr.Address = address.Value;
            cusr.Email = email.Value;
            cusr.Phone = phone.Value;
            cusr.JoinDate = DateTime.Now;
            cusr.Phone += ((phone2.Value.Trim() != "") ? "/" + phone2.Value.Trim() : "");
            cusr.Mode = new ModeID();
            cusr.Mode.Mode = MOID.SelectedValue;
            cusr.Mode.Number = mode_number.Value;
            cusr.Username = PoliceDiaryEncription.Encrypt(username.Value);
            cusr.Password = PoliceDiaryEncription.Encrypt(password.Value);
            cusr.Picture = picture.FileName;
            cusr.PCID = string.Join("", new int[4] { new Random().Next(9), new Random().Next(9), new Random().Next(9), new Random().Next(9) });

            bool bad_data = false;
            if (!_li.Contains(phone.Value.Trim().Length + phone2.Value.Trim().Length))
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.InnerText = "Invalid Phone number(s)";
                li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                message.Controls.Add(li);
                bad_data = true;
            }
            if (password.Value.Length <= 5)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                li.InnerText = "Invalid password Length";
                li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                message.Controls.Add(li);
                bad_data = true;
            }

            if (bad_data)
                return;

            try
            {
                
                string _select = $"SELECT * FROM Users WHERE Username = '{cusr.Username}' OR '{cusr.Username}' = '{PoliceDiaryEncription.Encrypt("UmarFarooq")}'";
                
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(_select, sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = "Username is already taken";
                                li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                                message.Controls.Add(li);
                                return;
                            }
                        }
                        HttpPostedFile f = null;
                        string path = "", file_name = "";
                        if (picture.HasFile)
                        {
                            path = ""; string extension = Path.GetExtension(picture.FileName);

                            file_name = PoliceDiaryEncription.Encrypt(Path.GetFileNameWithoutExtension(picture.FileName));
                            file_name = file_name.Replace("/", "").Replace("\\", "").Replace("?", "").Replace("#", "").Replace("+", "");
                            if (file_name.Length > 20)
                                file_name = file_name.Substring(0, 19);
                            file_name += extension;

                            int file_counter = 0;
                            while (File.Exists(Path.Combine(Server.MapPath("~/uploads/user"), file_name)))
                            {
                                file_counter++;
                                file_name = file_counter + "_" + file_name;
                            }

                            cusr.Picture = file_name;
                            path = Path.Combine(Server.MapPath("~/uploads/user"), file_name);
                            f = picture.PostedFile;
                        }
                        else
                        {
                            cusr.Picture = "";
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            li.InnerText = "Invalid Data";
                            li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                            message.Controls.Add(li);
                            return;
                        }

                        string insert = $"INSERT INTO Users(Name, Sex, Address, Email, Picture, Phone, ModeOfID, PCID, JoinDate, Password, Username, UserType) " +
                                                $"VALUES('{cusr.Name}', '{cusr.Sex}', '{cusr.Address}', '{cusr.Email}', '{cusr.Picture}', '{cusr.Phone}', " +
                                                $"'{JsonConvert.SerializeObject(cusr.Mode)}', '{cusr.PCID}', '{cusr.JoinDate}', '{cusr.Password}', '{cusr.Username}', 'Police') ";

                        sqlCmd.CommandText = insert;
                        if (sqlCmd.ExecuteNonQuery() > 0)
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            li.InnerText = "Police Created Successful";
                            li.Attributes.Add("style", "color: green; border: 1px solid darkgreen;  padding: 5px; background-color: white;");
                            message.Controls.Add(li);
                            f.SaveAs(path);
                        }
                        else
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            li.InnerText = "Police Creation Failed";
                            li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                            message.Controls.Add(li);
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