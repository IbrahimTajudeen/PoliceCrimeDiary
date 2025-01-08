using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.IO;
using Newtonsoft.Json;
using Police_Crime_Diary.Service;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

namespace Police_Crime_Diary
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        HtmlGenericControl generic_message = null;

        public SiteMaster()
        {
            generic_message = new HtmlGenericControl("ul");
            generic_message = new HtmlGenericControl("ul");
            generic_message.Attributes.Add("style", "list-style: none;");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(IsPostBack || !IsPostBack)
                {
                    generic_message.Controls.Clear();
                    string path = Request.CurrentExecutionFilePath;
                    List<string> allowed_path = new List<string> { "register", "todaycrime", "crimediary", "welcomeguest" };
                    path = Path.GetFileNameWithoutExtension(path.Substring(path.LastIndexOf("/") + 1).ToLower());
                    HttpCookie c = Request.Cookies.Get("usr");
                    
                    if(Request.Cookies["usr"] == null)
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

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }

}