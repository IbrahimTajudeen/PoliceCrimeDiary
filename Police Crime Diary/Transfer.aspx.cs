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
    public partial class Transfer : System.Web.UI.Page
    {
        HtmlGenericControl message = null;
        public Transfer()
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
                if(!IsPostBack)
                {
                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand($"SELECT ID, Name FROM CrimeReport WHERE ID NOT IN(SELECT CrimeID FROM Transfers)", sqlCon))
                        {
                            using (SqlDataReader reader = sqlCmd.ExecuteReader())
                            {
                                suspects.Items.Add(new ListItem { Enabled = true, Text = "Select Suspect", Value = "" });
                                while (reader.Read())
                                {
                                    suspects.Items.Add(new ListItem { Enabled = true, Selected = false, Text = reader.GetValue(1).ToString(), Value = reader.GetValue(0).ToString() });
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
                string errors = "";
                if (int.Parse(((string.IsNullOrEmpty(suspects.SelectedValue)) ? "0" : suspects.SelectedValue)) <= 0)
                    errors = "Suspect";

                if (from.Value.Trim() == "")
                    errors += ((errors == "") ? "Tranfer From" : ", Tranfer From");
                

                if (to.Value.Trim() == "")
                    errors += ((errors == "") ?  "Tranfer To" : ", Tranfer To");

                if (date.Value.Trim() == "")
                    errors += ((errors == "") ? "Tranfer Date" : ", Tranfer Date");

                if (reason.Value.Trim() == "")
                    errors += ((errors == "") ? "Tranfer Reason" : ", Tranfer Reason");

                if (errors != "")
                {
                    errors = "Invalid " + errors + " Datas";
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    li.InnerText = errors;
                    li.Attributes.Add("style", "color: red; border: 1px solid red;  padding: 5px; background-color: white;");
                    message.Controls.Add(li);
                }
                if (errors == "")
                {
                    TransferObejct tf = new TransferObejct();
                    tf.CrimeID = int.Parse(suspects.SelectedValue);
                    tf.Date = DateTime.Parse(date.Value);
                    tf.From = from.Value;
                    tf.To = to.Value;
                    tf.Reason = reason.Value;

                    using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sqlCon.Open();
                        using (SqlCommand sqlCmd = new SqlCommand($"INSERT INTO Transfers(CrimeID, TransferFrom, TransferTo, TransferDate, Reason) " +
                                                                                     $"VALUES({tf.CrimeID}, '{tf.From}', '{tf.To}', '{tf.Date}', '{tf.Reason}')", sqlCon))
                        {
                            if (sqlCmd.ExecuteNonQuery() > 0)
                            {
                                new InboxNotify().SetInbox(usr, $"{suspects.SelectedItem.Text} Has been transfer from: {tf.From} to: {tf.To} with the following reason: \"{tf.Reason}\"");
                                HtmlGenericControl li = new HtmlGenericControl("li");
                                li.InnerText = "Transfer Made Successfully";
                                li.Attributes.Add("style", "color: green; border: 1px solid green;  padding: 5px; background-color: white;");
                                message.Controls.Add(li);
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