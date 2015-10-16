using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace MicroFeel.AuthenticationService
{
    public class AnUser : IdentityUser
    {
        #region EF Properties

        [Required]
        [StringLength(20)]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        [Display(Name = "电子邮件")]
        public override string Email
        {
            get
            {
                return base.Email;
            }

            set
            {
                base.Email = value;
            }
        }

        [Display(Name = "用户名")]
        public override string UserName
        {
            get
            {
                return base.UserName;
            }

            set
            {
                base.UserName = value;
            }
        }

        [Display(Name = "电话号码")]
        public override string PhoneNumber
        {
            get
            {
                return base.PhoneNumber;
            }

            set
            {
                base.PhoneNumber = value;
            }
        }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public IList<string> CanSelectRoles
        {
            get; set;
        }

        [Display(Name = "所属角色")]
        public string role
        {
            get
            {
                //var rids = base.Roles.Select(v => v.RoleId);
                var context = new ApplicationDbContext();
                using (var usermanager = new CustomUserManager(new UserStore<AnUser>(context)))
                using (var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
                {
                    CanSelectRoles = rolemanager.Roles.Select(v => v.Name).ToList();
                    var list = usermanager.GetRoles(this.Id);
                    if (list.Count == 0)
                    {
                        return "无角色";
                    }
                    else
                    {
                        var role = list.First();
                        CanSelectRoles.Remove(role);
                        return role;
                    }
                }
            }
        }
        #endregion


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AnUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }

}