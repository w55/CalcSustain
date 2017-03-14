using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
namespace CalcSustain.WEB.Models
{
    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем 3 роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };
            var role3 = new IdentityRole { Name = "manager" };

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            // создаем пользователя:
            // cat@cat.cat

            string user_name1 = "cat@cat.cat";
            string pass1 = "cat123";
            var user1 = new ApplicationUser() { UserName = user_name1, Email = user_name1 };
            var result1 = userManager.Create(user1, pass1);

            // если создание пользователя прошло успешно
            if (result1.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(user1.Id, role2.Name);
                userManager.AddToRole(user1.Id, role3.Name);
            }

            // создаем пользователя:
            // dog@dog.dog
            //
            string admin_name = "dog@dog.dog";
            string admin_pass2 = "dog123";
            var admin = new ApplicationUser { Email = admin_name, UserName = admin_name };
            var result = userManager.Create(admin, admin_pass2);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
            }

            base.Seed(context);
        }
    }
}