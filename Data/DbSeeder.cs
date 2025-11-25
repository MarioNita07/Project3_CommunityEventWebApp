using Microsoft.AspNetCore.Identity;
using CommunityEvents.Models;

namespace CommunityEvents.Data
    {
        public static class DbSeeder
        {
            public static async Task SeedRolesAndUsersAsync(IServiceProvider service)
            {
                
                var userManager = service.GetService<UserManager<IdentityUser>>();
                var roleManager = service.GetService<RoleManager<IdentityRole>>();

                
                string[] roleNames = { "Admin", "Organizer", "Participant" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

               
                var adminEmail = "admin@test.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    var newAdmin = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true 
                    };

                    
                    await userManager.CreateAsync(newAdmin, "Admin123!");

                    
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }

                
                var organizerEmail = "organizer@test.com";
                var organizerUser = await userManager.FindByEmailAsync(organizerEmail);

                if (organizerUser == null)
                {
                    var newOrganizer = new IdentityUser
                    {
                        UserName = organizerEmail,
                        Email = organizerEmail,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newOrganizer, "Organizer123!");
                    await userManager.AddToRoleAsync(newOrganizer, "Organizer");
                }

                
                var participantEmail = "user@test.com";
                var participantUser = await userManager.FindByEmailAsync(participantEmail);

                if (participantUser == null)
                {
                    var newUser = new IdentityUser
                    {
                        UserName = participantEmail,
                        Email = participantEmail,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(newUser, "User123!");
                    await userManager.AddToRoleAsync(newUser, "Participant");
                }
            }
        }
    }

