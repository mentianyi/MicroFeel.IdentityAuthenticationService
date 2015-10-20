using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient
{
    internal class CustomIdentity : ClaimsIdentity
    {
        public string AuthenticationType { get; set; }
        public DateTime Expires { get; internal set; }
        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public string ticket { get; set; }
    }
}
