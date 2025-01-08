using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace Police_Crime_Diary.Service
{
    [JsonObject("Bail")]
    public class Bail
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("SuspectID")]
        public string SuspectID { get; set; }

        [JsonProperty("BailAmount")]
        public string BailAmount { get; set; }

        [JsonProperty("AmountOffered")]
        public string AmountOffered { get; set; }

        [JsonProperty("OfficerIncharge")]
        public string OfficereIncharge { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }

    [JsonObject("Suspect")]
    public class Suspect
    {
        public Suspect()
        {
            RelatedDocuments = new List<string>();
        }

        [JsonProperty("ID")]
        public int ID { get; set; }
        
        [JsonProperty("ReportedBy")]
        public string ReportedBy { get; set; }

        [StringLength(50, MinimumLength = 2)]
        [Required]
        [RegularExpression(@"[a-zA-Z]\w")]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 4)]
        [RegularExpression("[Male|Female]")]
        [JsonProperty("Gender")]
        public string Gender { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [JsonProperty("CrimeType")]
        public string CrimeType { get; set; }

        [StringLength(20, MinimumLength = 2)]
        [Required]
        [JsonProperty("Evidence")]
        public string Evidence { get; set; }

        [Required]
        [JsonProperty("CrimeDate")]
        public DateTime CrimeDate { get; set; }
        
        [Required]
        [JsonProperty("CrimeTime")]
        public TimeSpan CrimeTime { get; set; }

        [StringLength(1000, MinimumLength = 0)]
        [JsonProperty("Description")]
        public string Description { get; set; }

        [Required]
        [JsonProperty("Picture")]
        public string Picture { get; set; }

        [JsonProperty("RelatedDocuments")]
        public List<string> RelatedDocuments { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }
    }

    [JsonObject("Mode")]
    public class ModeID
    {
        [JsonProperty("Mode")]
        public string Mode { get; set; }
        [JsonProperty("Number")]
        public string Number { get; set; }
    }
    [JsonObject("CreateUser")]
    public class CreateUser
    {
        [JsonProperty("Address")]
        public string Address { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        [JsonProperty("Mode")]
        public ModeID Mode { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Password")]
        public string Password { get; set; }
        [JsonProperty("PCID")]
        public string PCID { get; set; }
        [JsonProperty("Phone")]
        public string Phone { get; set; }
        [JsonProperty("Picture")]
        public string Picture { get; set; }
        [JsonProperty("Sex")]
        public string Sex { get; set; }
        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }
    }

    [JsonObject("User")]
    public class User
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }
    }

    [JsonObject("TransferObejct")]
    public class TransferObejct
    {
        [JsonProperty("CrimeID")]
        public int CrimeID { get; set; }
        [JsonProperty("From")]
        public string From { get; set; }
        [JsonProperty("To")]
        public string To { get; set; }
        [JsonProperty("Date")]
        public DateTime Date { get; set; }
        [JsonProperty("Reason")]
        public string Reason { get; set; }
    }

    [JsonObject("ChargeCrime")]
    public class ChargeCrime
    {
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Suspect")]
        public string Suspect { get; set; }

        [JsonProperty("Officer")]
        public string Officer { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Details")]
        public string Details { get; set; }

        [JsonProperty("Date")]
        public DateTime Date { get; set; }

        [JsonProperty("Evidence")]
        public List<string> Evidence { get; set; }

        [JsonProperty("CourtName")]
        public string CourtName { get; internal set; }

        [JsonProperty("CourtDate")]
        public DateTime CourtDate { get; internal set; }

        [JsonProperty("Status")]
        public string Status { get; internal set; }
    }

    public static class PoliceDiaryEncription
    {
        public static string Encrypt(string text)
        {
            text = text.Trim();
            if (text == "")
                return text;
            string key = "Umar2PDProject";
            string salt = "BZNmKji?15TzK";
            byte[] text_bytes = Encoding.Unicode.GetBytes(text);
            string clear_text = "";
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, Encoding.UTF32.GetBytes(salt.ToCharArray()));
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(text_bytes, 0, text_bytes.Length);
                        cs.Close();
                    }
                    clear_text = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clear_text;
        }

        public static string Decrypt(string cypherText)
        {
            cypherText = cypherText.Trim();
            if (cypherText.Trim() == "")
                return cypherText;

            string key = "Umar2PDProject";
            string salt = "BZNmKji?15TzK";
            byte[] text_bytes = Convert.FromBase64String(cypherText);
            string clear_text = "";
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, Encoding.UTF32.GetBytes(salt.ToCharArray()));
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(text_bytes, 0, text_bytes.Length);
                        cs.Close();
                    }
                    clear_text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return clear_text;
        }
    }


    // For Singulton Design Pattern: Ensure one opened connection to the database
    public class DatabaseConnection
    {
        private DatabaseConnection() { }
        private static SqlConnection _instance = null;
        public static string connection_string = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"{Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), "PoliceCrimeDiaryDB.mdf")}\";Integrated Security=True";
        
        public static SqlConnection GetDBConnection()
        {
            if(_instance == null) _instance = new SqlConnection();

            if(string.IsNullOrEmpty(_instance.ConnectionString)) _instance.ConnectionString = connection_string;

            if (_instance.State == System.Data.ConnectionState.Closed) _instance.Open();

            return _instance;
        }

        public static void OnDB()
        {
            if(_instance == null)
            {
                _instance = new SqlConnection(connection_string);
                _instance.Open();
            }
        }
    }

    public class InboxNotify
    {
        public void SetInbox(User usr, string message, string secret = "NULL", string onlyBy = "NULL")
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(DatabaseConnection.connection_string))
                {
                    sqlCon.Open();
                    using (SqlCommand sqlCmd = new SqlCommand($"INSERT INTO Inbox(SenderID, PostMessage, IsSecret, OnlyBy) " +
                                                            $"VALUES({usr.ID}, '{message}', '{secret}', '{onlyBy}')", sqlCon))
                    {
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " Inbox");
            }
        }
    }
}