using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroFeel.AuthenticationService
{
    public class ApplicationDbContext : IdentityDbContext<AnUser>
    {
        public ApplicationDbContext()
            : base("PIDCAuthDB", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}