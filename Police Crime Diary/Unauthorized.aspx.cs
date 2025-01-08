using Newtonsoft.Json;
using Police_Crime_Diary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Police_Crime_Diary
{
    public partial class Unauthorized : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["usr"] == null)
            {
                Response.Redirect("~/welcomeguest");
                return;
            }
            User usr = JsonConvert.DeserializeObject<User>(PoliceDiaryEncription.Decrypt(Request.Cookies["usr"].Value));
            role.InnerText = usr.Type;
            the_page.InnerText = Request.Url.ToString();
        }
    }
}