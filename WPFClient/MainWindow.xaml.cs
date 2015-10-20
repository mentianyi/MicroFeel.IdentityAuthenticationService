using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Login()
        {
            var c = new AuthService.AuthenticationServiceClient();

            using (new OperationContextScope(c.InnerChannel))
            {
                var id = System.Threading.Thread.CurrentPrincipal.Identity as ClaimsIdentity;
                if (id != null && id.IsAuthenticated && !string.IsNullOrWhiteSpace(id.Claims.First(v => v.Type == ClaimTypes.UserData).Value) && DateTime.Now < DateTime.Parse(id.Claims.First(v => v.Type == ClaimTypes.Expiration).Value))
                {
                    HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                    request.Headers[HttpResponseHeader.SetCookie] = id.Claims.First(v => v.Type == ClaimTypes.UserData).Value;
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;
                    c.Open();
                    MessageBox.Show("You have been loggedin ," + id.Name);
                    return;
                }
                var loginform = new Login();
                if (loginform.ShowDialog() ?? false)
                {
                    if (c.ValidateUser(loginform.UserName, loginform.Password, string.Empty) && c.Login(loginform.UserName, loginform.Password, string.Empty, true))
                    {
                        var responseMessageProperty = (HttpResponseMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpResponseMessageProperty.Name];

                        string cookie = responseMessageProperty.Headers.Get("Set-Cookie");

                        //ASP.NET_SessionId=wx5ygqht1vdc4antol5pvq0l; path=/; HttpOnly,.ASPXAUTH=BC10A5A7CB7B83AE8B555F0ECD2DD0778272AA12C78B4A44A2D5203F96363DFA2D4EEF638F553C0B552F7D2BD414F024CE49A089D21CA1546867FBCC0FD5D0233CDAB5EF1FF990BFAA9777524364B57C6EA8E8862AE7C700E49F3013B34F9B5D306F36644684648F9D3EBE64B9017FEF; expires=Fri, 16-Oct-2015 04:30:22 GMT; path=/
                        var Currentticket = Regex.Match(cookie, ".ASPXAUTH=(\\w+);").Groups[1].Value;
                        var CurrentExpiresTime = DateTime.ParseExact(Regex.Match(cookie, "expires=(.+GMT);").Groups[1].Value, "ddd, dd-MMM-yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture).ToLocalTime();
                        //var ticket = FormsAuthentication.Decrypt(Currentticket);
                        var roles = Regex.Match(cookie, "Roles=(.+);").Groups[1].Value;
                        var rolelist = UTF8Encoding.Default.GetString(Convert.FromBase64String(roles)).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        var list = new List<Claim>();
                        list.Add(new Claim(ClaimTypes.Name, loginform.UserName));
                        foreach (var role in rolelist)
                        {
                            list.Add(new Claim(ClaimTypes.Role, role));
                        }
                        list.Add(new Claim(ClaimTypes.Expiration, CurrentExpiresTime.ToString()));
                        list.Add(new Claim(ClaimTypes.UserData, Currentticket));
                        var id2 = new ClaimsIdentity(list, "WCF AuthService");
                        System.Threading.Thread.CurrentPrincipal = new ClaimsPrincipal(id2);
                        MessageBox.Show("you loggedin :" + System.Threading.Thread.CurrentPrincipal.Identity.Name);
                    }
                    else
                    {
                        MessageBox.Show("Login failed!");
                    }
                }
                else
                {
                    MessageBox.Show("You are not loggedin.");
                }
            }
        }
    }
}
