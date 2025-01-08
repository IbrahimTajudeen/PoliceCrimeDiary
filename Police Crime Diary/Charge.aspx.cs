using Newtonsoft.Json;
using Police_Crime_Diary.Service;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Police_Crime_Diary
{
    public partial class Charge : System.Web.UI.Page
    {
        HtmlGenericControl message = null;
        public Charge()
        {
            message = new HtmlGenericControl("ul");
            message.Attributes.Add("style", "list-style: none;");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            message.Controls.Clear();
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
                if (!IsPostBack)
                {
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand($"SELECT ID, Name FROM CrimeReport WHERE ID NOT IN(SELECT CrimeID FROM Transfers)", sqlCon))
                        {
                            using (SqlDataReader reader = sqlCmd.ExecuteReader())
                            {
                                suspects.Items.Clear();
                                suspects.Items.Add(new ListItem { Enabled = true, Selected = false, Text = "", Value = "" });
                                while (reader.Read())
                                {
                                    suspects.Items.Add(new ListItem { Enabled = true, Text = reader.GetValue(1).ToString(), Value = reader.GetValue(0).ToString() });
                                }
                            }
                        }

                        using (SqlCommand sqlCmd = new SqlCommand($"SELECT ID, Name, Username FROM Users WHERE LOWER(UserType) = 'police'", sqlCon))
                        {
                            using (SqlDataReader reader = sqlCmd.ExecuteReader())
                            {
                                officers.Items.Clear();
                                officers.Items.Add(new ListItem { Enabled = true, Selected = false, Text = "", Value = "" });
                                while (reader.Read())
                                {
                                    officers.Items.Add(new ListItem { Enabled = true, Text = reader.GetValue(1).ToString(), Value = reader.GetValue(0).ToString() });
                                }
                            }
                        }
                    }
                }

                string edit = Request.QueryString["edit"];
                edit = (string.IsNullOrEmpty(edit)) ? "" : edit;
                if(edit != "")
                {
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand($"SELECT TOP 1 r.Name, u.Name, c.ChargeDate, c.CourtName,  c.CourtDate, r.Description, c.ChargeDetails, c.Status FROM ChargedCrimes c JOIN CrimeReport r ON r.ID = c.CrimeID JOIN Users u ON u.ID = c.OfficerInCharge WHERE c.ID = {edit}", sqlCon))
                        {
                            using (SqlDataReader reader = sqlCmd.ExecuteReader())
                            {
                                if(reader.HasRows)
                                {
                                    suspects.SelectedItem.Selected = false;
                                    officers.SelectedItem.Selected = false;
                                    status.SelectedItem.Selected = false;
                                    while (reader.Read())
                                    {
                                        foreach (ListItem item in suspects.Items)
                                        {
                                            if (item.Text == reader.GetValue(0).ToString())
                                            {
                                                item.Selected = true;
                                                break;
                                            }
                                        }

                                        foreach (ListItem item in officers.Items)
                                        {
                                            if (item.Text == reader.GetValue(1).ToString())
                                            {
                                                item.Selected = true;
                                                break;
                                            }
                                        }

                                        foreach (ListItem item in status.Items)
                                        {
                                            if (item.Text == reader.GetValue(7).ToString())
                                            {
                                                item.Selected = true;
                                                break;
                                            }
                                        }
                                        suspects.Enabled = false;
                                        files.Enabled = false;
                                        
                                        date.Value = DateTime.Parse(reader.GetValue(2).ToString()).ToShortDateString();//.ToString("MM/dd/yyyy");
                                        date.Disabled = true;

                                        description.Value = reader.GetValue(5).ToString();
                                        description.Disabled = true;

                                        court_name.Value = reader.GetValue(3).ToString();
                                        court_date.Value = DateTime.Parse(reader.GetValue(4).ToString()).ToShortDateString();//.ToString("MM/dd/yyyy");
                                        
                                        details.Value = reader.GetValue(6).ToString();
                                        submit.Text = "Update Charge";
                                        break;
                                    }
                                }
                                else
                                {
                                    HtmlGenericControl li = new HtmlGenericControl("li");
                                    li.InnerText = "Could not find Charge Crime Record";
                                    li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                                    message.Controls.Add(li);
                                }
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
            if (message.Controls.Count > 0)
            {
                display_message.Controls.Add(message);
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["usr"] == null)
                    return;
                User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies["usr"].Value));
                message.Controls.Clear();

                string edit = Request.QueryString["edit"];
                edit = (string.IsNullOrEmpty(edit)) ? "" : edit;

                if(edit != "")
                {
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand($"UPDATE ChargedCrimes SET OfficerInCharge = {officers.SelectedValue}, CourtName = '{court_name.Value}', CourtDate = '{court_date.Value}', ChargeDetails = '{details.Value}', Status = '{status.SelectedItem.Text}'", sqlCon))
                        {
                            if (sqlCmd.ExecuteNonQuery() > 0)
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = "Charge updated successfully";
                                li.Attributes.Add("style", "color: green; border: 1px solid green;  padding: 5px; background-color: white;");
                                message.Controls.Add(li);
                            }
                            else
                            {
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = "Failed to update charge";
                                li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                                message.Controls.Add(li);
                            }
                        }
                    }
                }
                else
                {
                    string errors = "";
                    if (int.Parse((suspects.SelectedValue == "" ? "0" : suspects.SelectedValue)) <= 0)
                        errors = "Suspect";

                    if (int.Parse((officers.SelectedValue == "" ? "0" : officers.SelectedValue)) <= 0)
                        errors += ((errors == "") ? "Officer" : ", Officer");


                    if (details.Value.Trim() == "")
                        errors += ((errors == "") ? "Details" : ", Details");

                    if (date.Value.Trim() == "")
                        errors += ((errors == "") ? "Date" : ", Date");

                    if (!files.HasFiles)
                        errors += ((errors == "") ? "Evidence" : ", Evidence");

                    if (errors != "")
                    {
                        errors = "Invalid " + errors + "Details";
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.InnerText = errors;
                        li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                        message.Controls.Add(li);
                    }
                    if (errors == "")
                    {
                        ChargeCrime ccrime = new ChargeCrime();
                        ccrime.Suspect = suspects.SelectedValue;
                        ccrime.Officer = officers.SelectedValue;
                        ccrime.Details = details.Value;
                        ccrime.Date = DateTime.Parse(date.Value);
                        ccrime.Evidence = new List<string>();
                        ccrime.CourtName = court_name.Value;
                        ccrime.CourtDate = DateTime.Parse(court_date.Value);
                        ccrime.Status = status.SelectedItem.Text;

                        if (files.HasFiles)
                        {
                            foreach (HttpPostedFile rd in files.PostedFiles)
                            {
                                string path = "", extension = Path.GetExtension(rd.FileName);

                                string file_name = PoliceDiaryEncription.Encrypt(Path.GetFileNameWithoutExtension(rd.FileName));
                                file_name = file_name.Replace("/", "").Replace("\\", "").Replace("?", "").Replace("#", "").Replace(",", "").Replace("+", "");
                                if (file_name.Length > 20)
                                    file_name = file_name.Substring(0, 19);
                                file_name += extension;

                                int file_counter = 0;
                                while (File.Exists(Path.Combine(Server.MapPath("~/uploads/suspects/documents"), file_name)))
                                {
                                    file_counter++;
                                    file_name = file_counter + "_" + file_name;
                                }

                                ccrime.Evidence.Add(file_name);
                                path = Path.Combine(Server.MapPath("~/uploads/suspects/evidence"), file_name);

                                rd.SaveAs(path);
                            }
                        }

                        using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                        {
                            sqlCon.Open();
                            using (SqlCommand sqlCmd = new SqlCommand($"INSERT INTO ChargedCrimes(CrimeID, OfficerInCharge, ChargeDetails, ChargeDate, ChargeEvidence, CourtName, CourtDate, Status) " +
                                                                                              $"VALUES({ccrime.Suspect}, '{ccrime.Officer}', '{ccrime.Details}', '{ccrime.Date}', '{string.Join(",", ccrime.Evidence)}', '{ccrime.CourtName}', '{ccrime.CourtDate}', '{ccrime.Status}')", sqlCon))
                            {
                                if (sqlCmd.ExecuteNonQuery() > 0)
                                {
                                    new InboxNotify().SetInbox(usr, $"{suspects.SelectedItem.Text} Has been charge to court with the {ccrime.Evidence.Count} Evidence");
                                    HtmlGenericControl li = new HtmlGenericControl("li");
                                    li.InnerText = "Charge Made Successfully";
                                    li.Attributes.Add("style", "color: green; border: 1px solid green;  padding: 5px; background-color: white;");
                                    message.Controls.Add(li);
                                }
                                else
                                {
                                    HtmlGenericControl li = new HtmlGenericControl("li");
                                    li.InnerText = "Charge not made successfully";
                                    li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                                    message.Controls.Add(li);
                                }
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