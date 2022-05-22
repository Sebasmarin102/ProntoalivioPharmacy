using ProntoalivioPharmacy.Data.Entities;
using ProntoalivioPharmacy.Enums;
using ProntoalivioPharmacy.Helpers;

namespace ProntoalivioPharmacy.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckMedicineTypesAsync();
            await CheckCitiesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Sebas", "Posada", "sebasposada@yopmail.com", "300 872 3420", 
                "Calle Luna Calle Sol", UserType.Admin);
            await CheckUserAsync("2020", "Marce", "Marin", "marcemarin@yopmail.com", "300 872 3420", 
                "Calle Luna Calle Sol", UserType.User);
        }

        private async Task<User> CheckUserAsync(string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCitiesAsync()
        {
            if (!_context.Cities.Any())
            {
                _context.Cities.Add(new City
                {
                    Name = "Medellín",
                    Neighborhoods = new List<Neighborhood>()
                    {
                        new Neighborhood {
                            Name = "Buenos Aires",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Pasteur" },
                                new Laboratory { Name = "La rebaja" },
                                new Laboratory { Name = "La Y" },
                            }
                        },
                        new Neighborhood {
                            Name = "Villahermosa",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Cruz verde" },
                                new Laboratory { Name = "Drogueria Alemana" },
                                new Laboratory { Name = "Farmatodo" },
                            }
                        }
                    }
                });
                _context.Cities.Add(new City
                {
                    Name = "Bogotá",
                    Neighborhoods = new List<Neighborhood>()
                    {
                        new Neighborhood {
                            Name = "Usme",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Pasteur 2" },
                                new Laboratory { Name = "La rebaja 2" },
                                new Laboratory { Name = "La Y 2" },
                            }
                        },
                        new Neighborhood {
                            Name = "Bosa",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Cruz verde 2" },
                                new Laboratory { Name = "Drogueria Alemana 2" },
                                new Laboratory { Name = "Farmatodo 2" },
                            }
                        }
                    }
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckMedicineTypesAsync()
        {
            if (!_context.MedicineTypes.Any())
            {
                _context.MedicineTypes.Add(new MedicineType { Name = "Jarabes" });
                _context.MedicineTypes.Add(new MedicineType { Name = "Inyecciones" });
                _context.MedicineTypes.Add(new MedicineType { Name = "Sueros" });
                _context.MedicineTypes.Add(new MedicineType { Name = "Pastillas" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
