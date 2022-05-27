using Microsoft.EntityFrameworkCore;
using ProntoalivioPharmacy.Data.Entities;
using ProntoalivioPharmacy.Enums;
using ProntoalivioPharmacy.Helpers;

namespace ProntoalivioPharmacy.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckMedicineTypesAsync();
            await CheckCitiesAsync();
            await CheckProducstAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Sebas", "Posada", "sebasposada@yopmail.com", "300 872 3420", "Calle Luna Calle Sol",
                "sebas.jpg", UserType.Admin);
            await CheckUserAsync("2020", "Marce", "Marin", "marcemarin@yopmail.com", "300 872 3420", "Calle Luna Calle Sol",
                "marce.jpg", UserType.User);
            await CheckUserAsync("3030", "Luz", "Arias", "luz@yopmail.com", "300 872 3420", "Calle Luna Calle Sol", 
                "luz.JPG", UserType.User);
            await CheckUserAsync("4040", "Juan", "Mejia", "mejia@yopmail.com", "300 872 3420", "Calle Luna Calle Sol",
                "juan.jpg", UserType.User);
        }

        private async Task CheckProducstAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Acetaminofen", 1000M, 100F, new List<string>() {"Pastillas"}, 
                    new List<string>() {"acetaminofen.png"});
                await AddProductAsync("Noxpirin", 1500M, 200F, new List<string>() { "Pastillas" },
                    new List<string>() { "noxpirin.png", "noxpirin1.png", "noxpirin2.png" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> medicinetypes, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (string? medicinetype in medicinetypes)
            {
                prodcut.ProductCategories.Add(new ProductCategory 
                { MedicineType = await _context.MedicineTypes.FirstOrDefaultAsync(c => c.Name == medicinetype) });
            }


            foreach (string? image in images)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\products\\{image}", "products");
                prodcut.ProductImages.Add(new ProductImage { ImageId = imageId });
            }

            _context.Products.Add(prodcut);
        }


        private async Task<User> CheckUserAsync(string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string image,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\" +
                    $"images\\users\\{image}", "users");
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
                    ImageId = imageId,
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
                _context.Cities.Add(new City
                {
                    Name = "Cali",
                    Neighborhoods = new List<Neighborhood>()
                    {
                        new Neighborhood {
                            Name = "Ciudad Jardin",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Pasteur 3" },
                                new Laboratory { Name = "La rebaja 3" },
                                new Laboratory { Name = "La Y 3" },
                            }
                        },
                        new Neighborhood {
                            Name = "San Fernando",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Cruz verde 3" },
                                new Laboratory { Name = "Drogueria Alemana 3" },
                                new Laboratory { Name = "Farmatodo 3" },
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
                _context.MedicineTypes.Add(new MedicineType { Name = "Higiene" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
