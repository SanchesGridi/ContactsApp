using System.Collections.Generic;

using ContactsApp.Infrastructure.Data.Sql.Entities;

namespace ContactsApp.Infrastructure.Data.Sql
{
    public class ApplicationInitializer
    {
        public void CreateStorageData(ApplicationContext context)
        {
            var adminRole = new Role { Access = RoleDefinition.Admin };
            var userRole = new Role { Access = RoleDefinition.User };
            var guestRole =  new Role { Access = RoleDefinition.Guest };

            context.Roles.AddRange(adminRole, userRole, guestRole);
            context.SaveChanges();

            var guests = new[]
            {
                new User { Login = "Guest1", Password = "123******", Email = "someGx1@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest2", Password = "1234*****", Email = "someGx2@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest3", Password = "12345****", Email = "someGx3@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest4", Password = "123456***", Email = "someGx4@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest5", Password = "1234567**", Email = "someGx5@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest6", Password = "12345678*", Email = "someGx6@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest7", Password = "123456789", Email = "someGx7@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest8", Password = "12345678x", Email = "someGx8@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest9", Password = "12345678y", Email = "someGx9@gmail.com", RoleId = guestRole.Id },
                new User { Login = "Guest10", Password = "12345678z", Email = "someGx10@gmail.com", RoleId = guestRole.Id }
            };
            var admin = new User
            {
                Name = "Alexander",
                Surname = "Gridin",
                Email = "ap233281xg@gmail.com",
                Login = "FdX",
                Password = "1_1x1_1",
                RoleId = adminRole.Id
            };
            var user = new User
            {
                Name = "Tom",
                Surname = "Jackson",
                Email = "tom@g.com",
                Login = "TJackson",
                Password = "123",
                RoleId = userRole.Id
            };
            var contacts = this.InitializeContacts();
            user.Contacts.AddRange(contacts);

            context.AddRange(guests);
            context.Add(admin);
            context.Add(user);
            context.SaveChanges();
        }

        private IEnumerable<Contact> InitializeContacts()
        {
            var benNumbers = new[] { "+375 33 123-123-2", "+375 29 123-123-2", "+375 44 123-123-2", "+375 25 123-123-2" };
            return new[]
            {
                new Contact 
                { 
                    Name = "Ben", Surname = "Carter", Email = "carterxxx@gk.com", PhoneNumber = benNumbers[0],
                    ContactSnAccounts = new List<ContactSnAccount>
                    {
                        new TelegramAccount { DisplayUserName = "Tom1", UserName = "@tomB1", AccountIdentifierUserName = benNumbers[1] },
                        new TelegramAccount { DisplayUserName = "Tom2", UserName = "@tomB2", AccountIdentifierUserName = benNumbers[2] },
                        new TelegramAccount { DisplayUserName = "Tom3", UserName = "@tomB3", AccountIdentifierUserName = benNumbers[3] },
                    }
                },
                new Contact { Name = "Angela", Surname = "Wang", Email = "wangangi@gmail.com", PhoneNumber = "+375 44 164-374-3" },
                new Contact { Name = "Chris", Surname = "Jackson", Email = "x_jackson@gmail.com", PhoneNumber = "+375 25 333-333-7" },
                new Contact { Name = "Jasmine", Surname = "Brooks", Email = "brooks_on_fire@mail.ru", PhoneNumber = "+375 29 123-123-2" },
                new Contact { Name = "Jessi", Surname = "Holt", Email = "holt_bolt@323232.com", PhoneNumber = "+375 25 443-333-1" },
                new Contact { Name = "x", Surname = "y", Email = "z@gmail.com", PhoneNumber = "+375 25 456-789-0" },
                new Contact { Name = "xx", Surname = "yy", Email = "zz@gmail.com", PhoneNumber = "+375 33 222-333-4" },
                new Contact { Name = "Karen", Surname = "Winslow", Email = "tytyzd@kx.by", PhoneNumber = "+375 123-321-9" },
                new Contact { Name = "Lindsay", Surname = "Connor", Email = "x234ff@gmail.comcomcom", PhoneNumber = "+375 123-111-1" },
                new Contact { Name = "Stephanie", Surname = "Johnson", Email = "zabyl@debil.ru", PhoneNumber = "+375 33 111-311-2" },
                new Contact { Name = "50", Surname = "Cent", Email = "east_cost@usa.com", PhoneNumber = "+375 33 333-444-1" },
                new Contact { Name = "Dr.", Surname = "DRE", Email = "west_cost@usa.com", PhoneNumber = "+375 44 322-111-3" },
                new Contact { Name = "BOBik", Surname = "Marley", Email = "smoke@weed.everyday", PhoneNumber = "+375 33 333-444-5" },
                new Contact { Name = "Arnold", Surname = "bio_bot", Email = "like_human@and_more.west_world", PhoneNumber = "+375 29 001-110-1" },
                new Contact { Name = "Dragons", Surname = "Mother", Email = "yByVdul@25.raz", PhoneNumber = "+375 44 525-252-5" },
                new Contact { Name = "xyz", Surname = "XYZ", Email = "xyz@xyz.com", PhoneNumber = "+375 44 111-911-1" },
                new Contact { Name = "vash", Surname = "spisok", Email = "contactov", PhoneNumber = "+375 33 375-375-1" },
                new Contact { Name = "Tetka", Surname = "Ira", Email = "13232ffs3@mail.ru", PhoneNumber = "+375 44 111-222-3" },
                new Contact { Name = "sssfsf", Surname = "Ivan", Email = "ivan@mail.ru", PhoneNumber = "+375 33 444-555-1" },
                new Contact { Name = "xp", Surname = "vt", Email = "rt@gmail.com", PhoneNumber = "+375 29 323-323-1" },
                new Contact { Name = "Oleg", Surname = "Gazmanov", Email = "emaixl@mail.ru", PhoneNumber = "+375 29 117-222-3" },
                new Contact { Name = "Elvis", Surname = "Presley", Email = "elvis_pr@mail.ru", PhoneNumber = "+375 33 652-353-6" },
                new Contact { Name = "Francis", Surname = "Sinatra", Email = "sinatraFrank@tut.by", PhoneNumber = "+375 29 762-356-5" },
                new Contact { Name = "Louis", Surname = "Armstrong", Email = "louee547@mail.ru", PhoneNumber = "+375 29 452-658-5" },
                new Contact { Name = "Marilyn ", Surname = "Monroe", Email = "Norma_Jeane36@tut.by", PhoneNumber = "+375 33 347-523-0" },
                new Contact { Name = "Egor", Surname = "Petrov", Email = "egorT@mail.ru", PhoneNumber = "+375 33 295-664-7" },
                new Contact { Name = "Mary", Surname = "Simpson", Email = "maryyy@tut.by", PhoneNumber = "+375 33 365-956-5" },
                new Contact { Name = "John", Surname = "Doe", Email = "DJ_lok@mail.ru", PhoneNumber = "+375 33 399-452-1" },
                new Contact { Name = "Endrew ", Surname = "Lower", Email = "Rewento@tut.by", PhoneNumber = "+375 25 995-631-5" },
                new Contact { Name = "Tony", Surname = "Moll", Email = "Ton_Oll@mail.ru", PhoneNumber = "+375 33 612-323-2" },
                new Contact { Name = "Kate", Surname = "Avdeeva", Email = "KAte@tut.by", PhoneNumber = "+375 29 754-220-0" },
                new Contact { Name = "Victor", Surname = "Selitskiy", Email = "Sel_ba@mail.ru", PhoneNumber = "+375 25 852-633-2" },
                new Contact { Name = "Oleg ", Surname = "Vasnetsov", Email = "Oleg5447@tut.by", PhoneNumber = "+375 33 322-151-5" },
                new Contact { Name = "Mike", Surname = "Edison", Email = "Ed_M$$@mail.ru", PhoneNumber = "+375 29 852-966-3" },
                new Contact { Name = "Avraam ", Surname = "Lincoln", Email = "Avraam_manager@tut.by", PhoneNumber = "+375 25 996-931-5" },
                new Contact { Name = "Tony", Surname = "Stark", Email = "Tonytony@mail.ru", PhoneNumber = "+375 33 612-843-2" },
                new Contact { Name = "Stiv", Surname = "Jankins", Email = "Jann_k@tut.by", PhoneNumber = "+375 29 754-228-0" },
                new Contact { Name = "Tedd", Surname = "Boundy", Email = "Teddy@mail.ru", PhoneNumber = "+375 25 852-693-2" },
                new Contact { Name = "Olga ", Surname = "Pastulatova", Email = "Ola_Hola@tut.by", PhoneNumber = "+375 33 322-701-5", },
                new Contact { Name = "xvatit", Surname = "contactov", Email = "redactiryi@cut.shto", PhoneNumber = "+375 44 231-231-1" }
            };
        }
    }
}