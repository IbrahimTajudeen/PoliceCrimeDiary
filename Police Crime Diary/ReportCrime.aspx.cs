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
    public partial class ReportCrime : System.Web.UI.Page
    {
        HtmlGenericControl _message = null;

        public ReportCrime()
        {
            _message = new HtmlGenericControl("ul");
            _message.Attributes.Add("style", "list-style: none;");
        }

        protected void Page_Load(object sender, EventArgs e)
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
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            _message.Controls.Clear();
            if (Request.Cookies.AllKeys.Contains("usr"))
            {
                Suspect suspect = new Suspect();
                User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies.Get("usr").Value));
                suspect.ReportedBy = usr.ID.ToString();
                suspect.Name = suspect_name.Value;
                suspect.Gender = suspect_gender.Value;
                suspect.CrimeType = suspect_crime_type.Value;
                suspect.Evidence = evidence.Value;
                suspect.CrimeDate = DateTime.Parse(crime_date.Value);
                suspect.CrimeTime = TimeSpan.Parse(crime_time.Value);
                suspect.Description = description.Value;
                suspect.Location = location.Value;

                if (suspect_picture.HasFile)
                {
                    string path = "", extension = Path.GetExtension(suspect_picture.FileName);

                    string file_name = PoliceDiaryEncription.Encrypt(Path.GetFileNameWithoutExtension(suspect_picture.FileName));
                    file_name = file_name.Replace("/", "").Replace("\\", "").Replace("?", "").Replace("#", "").Replace(",", "").Replace("+", "");
                    if (file_name.Length > 20)
                        file_name = file_name.Substring(0, 19);
                    file_name += extension;

                    int file_counter = 0;
                    while (File.Exists(Path.Combine(Server.MapPath("~/uploads/suspects"), file_name)))
                    {
                        file_counter++;
                        file_name = file_counter + "_" + file_name;
                    }

                    suspect.Picture = file_name;
                    path = Path.Combine(Server.MapPath("~/uploads/suspects"), file_name);

                    HttpPostedFile f = suspect_picture.PostedFile;
                    f.SaveAs(path);
                }
                else suspect.Picture = "";

                if (related_document.HasFiles)
                {
                    int i = 0;
                    foreach (HttpPostedFile rd in related_document.PostedFiles)
                    {
                        string path = "", extension = Path.GetExtension(rd.FileName);

                        string file_name = PoliceDiaryEncription.Encrypt(Path.GetFileNameWithoutExtension(rd.FileName));
                        file_name = file_name.Replace("/", "").Replace("\\", "").Replace("?", "").Replace("#", "").Replace(",","").Replace("+", "");
                        if (file_name.Length > 20)
                            file_name = file_name.Substring(0, 19);
                        file_name += extension;

                        int file_counter = 0;
                        while (File.Exists(Path.Combine(Server.MapPath("~/uploads/suspects/documents"), file_name)))
                        {
                            file_counter++;
                            file_name = file_counter + "_" + file_name;
                        }

                        suspect.RelatedDocuments.Add(file_name);
                        path = Path.Combine(Server.MapPath("~/uploads/suspects/documents"), file_name);

                        rd.SaveAs(path);
                        i++;
                    }
                }
                try
                {
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        string _insert = $"INSERT INTO CrimeReport(ReportedBy, Name, Sex, CrimeType, Evidence, DateofCrime, TimeOfCrime, Description, Picture, RelatedDocuments, Location, Status) " +
                                         $"VALUES({suspect.ReportedBy}, '{suspect.Name}', '{suspect.Gender}', '{suspect.CrimeType}', '{suspect.Evidence}', '{suspect.CrimeDate}', '{suspect.CrimeTime}', '{suspect.Description}', '{suspect.Picture}', '{string.Join(",", suspect.RelatedDocuments)}', '{suspect.Location}', 'Reported'); ";
                        using (SqlCommand sqlCmd = new SqlCommand(_insert, sqlCon))
                        {

                            if (sqlCmd.ExecuteNonQuery() > 0)
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = "Crime Reported Successful";
                                li.Attributes.Add("style", "color: green; border: 1px solid darkgreen; padding: 5px; background-color: white;");
                                _message.Controls.Add(li);
                            }
                            else
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = "Crime Report Failed";
                                li.Attributes.Add("style", "color: red; border: 1px solid red; padding: 5px; background-color: white;");
                                _message.Controls.Add(li);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    HtmlGenericControl li = new HtmlGenericControl("li");
                    li.InnerText = ex.Message;
                    li.Attributes.Add("style", "color: red; border: 1px solid red; padding: 5px; background-color: white;");
                    _message.Controls.Add(li);
                }
            }

            if (IsPostBack)
            {
                if (_message.Controls.Count > 0)
                {
                    display_message.Controls.Add(_message);
                }
            }

        }
    }
}