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
    public partial class AddBail : System.Web.UI.Page
    {
        HtmlGenericControl message = null;

        public AddBail()
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
            if (usr.Type.ToLower() != "admin")
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
                                officer.Items.Clear();
                                officer.Items.Add(new ListItem { Enabled = true, Selected = false, Text = "", Value = "" });
                                while (reader.Read())
                                {
                                    officer.Items.Add(new ListItem { Enabled = true, Text = reader.GetValue(1).ToString(), Value = reader.GetValue(0).ToString() });
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

        protected void add_bail_Click(object sender, EventArgs e)
        {
            Bail bail = new Bail();
            bail.SuspectID = suspects.SelectedValue;
            bail.OfficereIncharge = officer.SelectedValue;
            bail.BailAmount = bail_amount.Value;
            
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    string _insert = $"IF (SELECT TOP 1 ID FROM BailList WHERE {bail.SuspectID} IN(SELECT ID FROM BailList WHERE Status != 'Accepted')) IS NULL " +
                                        $"BEGIN " +
                                            $"INSERT INTO BailList(SuspectID, OfficerInCharge, BailAmount) VALUES({bail.SuspectID}, {bail.OfficereIncharge}, '{bail.BailAmount}') " +
                                        $"END " +
                                    $"ELSE " +
                                        $"BEGIN " +
                                            $"UPDATE BailList SET OfficerInCharge = {bail.OfficereIncharge}, BailAmount = '{bail.BailAmount}' WHERE SuspectID = {bail.SuspectID} " +
                                        $"END";
                    using (SqlCommand sqlCmd = new SqlCommand(_insert, sqlCon))
                    {
                        if (sqlCmd.ExecuteNonQuery() > 0)
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            li.InnerText = "Bail Added Successfully";
                            li.Attributes.Add("style", "color: green; border: 1px solid green;  padding: 5px; background-color: white;");
                            message.Controls.Add(li);
                        }
                        else
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            li.InnerText = "Bail not added. Maybe Suspect bail is already accepted";
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