using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PageRedirect.Aspx
{
    public partial class Redirect : System.Web.UI.Page
    {
        private string [] redirectSites;
        private string [] redirectSitesNames;
        private Dictionary<string, string> redirectSiteInfo;
        private string parameterForUsername;
        private string loginName;
        private bool showButtons;
        public Redirect()
        {
            showButtons = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowButtons"]);
            redirectSites = ConfigurationManager.AppSettings["RedirectTo"].Split(',');
            redirectSitesNames = ConfigurationManager.AppSettings["RedirectToPageNames"].Split(',');
            parameterForUsername = ConfigurationManager.AppSettings["ParameterForUsername"];
            redirectSiteInfo = BindKeysAndValues(redirectSitesNames, redirectSites);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            loginName = System.Web.HttpContext.Current.User.Identity.Name.Contains("\\") ?
                System.Web.HttpContext.Current.User.Identity.Name.Split('\\').Last() : System.Web.HttpContext.Current.User.Identity.Name;

            Username.InnerText = loginName;
            if (showButtons)
            {
                foreach (var redirectSite in redirectSiteInfo)
                {
                    ShowRedirectButtons.InnerHtml += $"<input type=button value=\"{redirectSite.Key}\" onclick=\"window.location.href = '{redirectSite.Value}?{parameterForUsername}={loginName}'; \"/> <br /><br />";
                }
            }
            else
            {
                ShowRedirectButtons.InnerText = "NOT Showing Buttons";
                Response.Redirect($"{redirectSites[0]}{parameterForUsername}={loginName}");
            }
        }
        private Dictionary<string, string> BindKeysAndValues(string [] keys, string [] values)
        {
            var map = new Dictionary<string, string>();
            for (int i = 0; i < keys.Length; i++)
            {
                map.Add(keys[i], values[i]);
            }
            return map;
        }
    }
}