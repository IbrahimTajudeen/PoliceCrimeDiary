using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace Police_Crime_Diary.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class PoliceCrimeDiaryAPIService
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        
        [OperationContract]
        public string Login(string username, string password)
        {
            try
            {
                if(username == "UmarFarooq" && password == "UmarProject")
                {
                    using (SqlConnection sc = new SqlConnection(DatabaseConnection.connection_string))
                    {
                        sc.Open();
                        string _select = $"SELECT TOP 1 * FROM Users WHERE Username = '{PoliceDiaryEncription.Encrypt(username)}' AND Password = '{PoliceDiaryEncription.Encrypt(password)}'";
                        using (SqlCommand scmd = new SqlCommand(_select, sc))
                        {
                            bool _adminExists = false;
                            using (SqlDataReader r = scmd.ExecuteReader())
                            {
                                _adminExists = r.HasRows;
                            }
                            if(!_adminExists)
                            {
                                string _insert = $"INSERT INTO Users(Name, Sex, Address, Email, Picture, Phone, ModeOfID, PCID, JoinDate, Password, Username, UserType) " +
                                                $"VALUES('Umar Farooq', 'Male', 'My Personal Address', 'umarfarooq89@gmail.com', 'umar_pic.jpg', '08134230057', " +
                                                $"'{JsonConvert.SerializeObject(new ModeID { Mode = "National ID Card", Number="12345678981" })}', '1111', '{DateTime.Now}', '{PoliceDiaryEncription.Encrypt("UmarProject")}', '{PoliceDiaryEncription.Encrypt("UmarFarooq")}', 'Admin') ";
                                scmd.CommandText = _insert;
                                scmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT TOP 1 ID, Username, UserType FROM Users WHERE Username = '{PoliceDiaryEncription.Encrypt(username)}' AND Password = '{PoliceDiaryEncription.Encrypt(password)}'", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                User usr = new User();
                                usr.ID = int.Parse(reader.GetValue(0).ToString());
                                usr.Username = PoliceDiaryEncription.Decrypt(reader.GetValue(1).ToString());
                                usr.Type = reader.GetValue(2).ToString();
                                HttpContext.Current.Request.Cookies.Clear();
                                HttpContext.Current.Response.Cookies.Clear();
                                HttpCookie user_cookie = new HttpCookie("usr");
                                user_cookie.HttpOnly = true;
                                user_cookie.Secure = true;
                                user_cookie.Value = PoliceDiaryEncription.Encrypt(JsonConvert.SerializeObject(usr));
                                HttpContext.Current.Response.Cookies.Add(user_cookie);
                            }
                            else return "Invalid Login Credentials";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Log in successful";
        }

        [OperationContract]
        public string Logout()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["usr"] != null)
                {
                    HttpCookie cook = new HttpCookie("usr");
                    cook.Value = ""; cook.HttpOnly = true; cook.Secure = true; cook.Expires = DateTime.Now.AddDays(-1);

                    HttpContext.Current.Response.Cookies.Add(cook);
                    
                    return "Log out Successful";
                }
                return "Log out not successful. You need to login first";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [OperationContract]
        public string GetCrimeDescription(int id)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"SELECT Description FROM CrimeReport WHERE ID = {id}", sqlCon))
                    {
                        using (SqlDataReader reader = sqlCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return reader.GetValue(0).ToString();
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [OperationContract]
        public string ReplyBail(string action, int id, string suspect_name, int amount, int offer)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["usr"] == null)
                    throw new Exception("Invalid Request. You're not logged in yet.");
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    string _update = $"UPDATE BailList SET Status = '{action}' WHERE ID = {id}";
                    using (SqlCommand sqlCmd = new SqlCommand(_update, sqlCon))
                    {
                        if (sqlCmd.ExecuteNonQuery() > 0)
                        {
                            User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(HttpContext.Current.Request.Cookies["usr"].Value));
                            new InboxNotify().SetInbox(usr, $"The suspect \"{suspect_name}\" was {action} with the offer amount of \"{amount.ToString("N2")}\"");
                            return action;
                        }
                        return "";
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [OperationContract]
        public string ApplyBail(int id, string offer)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies["usr"] == null)
                    throw new Exception("Invalid Request. You're not logged in yet.");
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    string _update = $"UPDATE BailList SET Status = 'Pending', AmountOffered = '{offer}' WHERE ID = {id}";
                    using (SqlCommand sqlCmd = new SqlCommand(_update, sqlCon))
                    {
                        if (sqlCmd.ExecuteNonQuery() > 0)
                        {
                            User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(HttpContext.Current.Request.Cookies["usr"].Value));
                            new InboxNotify().SetInbox(usr, $"A bail offer of \"{offer}\" was made and now the Bail application is now pending.");
                            return "Pending";
                        }
                            
                        return "";
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Add more operations here and mark them with [OperationContract]


    }
}
