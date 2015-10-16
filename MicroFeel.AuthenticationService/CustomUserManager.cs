using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroFeel.AuthenticationService
{
    public class CustomUserManager : UserManager<AnUser>
    {
        public CustomUserManager(IUserStore<AnUser> store)
            : base(store)
        {
        }

        public static CustomUserManager Create()
        {
            var manager = new CustomUserManager(new UserStore<AnUser>(new ApplicationDbContext()));
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<AnUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // 配置密码的验证逻辑
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // 配置用户锁定默认值
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
            // 你可以编写自己的提供程序并将其插入到此处。
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<AnUser>
            {
                MessageFormat = "你的安全代码是 {0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<AnUser>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<AnUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
            return manager;
        }
    }
}