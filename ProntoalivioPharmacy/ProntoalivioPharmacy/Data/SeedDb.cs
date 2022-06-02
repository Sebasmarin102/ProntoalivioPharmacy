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
                await AddProductAsync("Dihidrocodeina", 3500M, 50F, new List<string>() { "Jarabes" },
                    new List<string>() { "dihidrocodeina.png" });
                await AddProductAsync("Pedyalite", 9500M, 20F, new List<string>() { "Jarabes", "Sueros" },
                    new List<string>() { "pedyalite.png" });
                await AddProductAsync("Jabon Intimo", 10000M, 150F, new List<string>() { "Higiene" },
                    new List<string>() { "jabonintimo.png" });
                await AddProductAsync("Electrolit", 5000M, 500F, new List<string>() { "Sueros", "Jarabes" },
                    new List<string>() { "electrolic.png" });
                await AddProductAsync("Adorem", 5900M, 200F, new List<string>() { "Jarabes" },
                    new List<string>() { "adorem.png" });
                await AddProductAsync("Aquasol x30", 5000M, 500F, new List<string>() { "Pastillas" },
                    new List<string>() { "aquasol.png" });
                await AddProductAsync("Cepillo dientes", 2000M, 500F, new List<string>() { "Higiene" },
                    new List<string>() { "cepillo.png" });
                await AddProductAsync("Crema N4", 1500M, 500F, new List<string>() { "Higiene" },
                    new List<string>() { "creman4.png" });
                await AddProductAsync("Pañales huggies", 5000M, 500F, new List<string>() { "Higiene" },
                    new List<string>() { "huggies.png" });
                await AddProductAsync("Tapabocas", 500M, 1000F, new List<string>() { "Higiene" },
                    new List<string>() { "mascarilla.png" });
                await AddProductAsync("Medicasp", 10000M, 100F, new List<string>() { "Higiene" },
                    new List<string>() { "medicasp.png" });
                await AddProductAsync("Shampoo H&S", 21000M, 50F, new List<string>() { "Higiene" },
                    new List<string>() { "shampoo.png" });
                await AddProductAsync("Desodorante", 15900M, 20F, new List<string>() { "Higiene" },
                    new List<string>() { "desodorante.png" });
                await AddProductAsync("Tramadol Inyección", 3900M, 20F, new List<string>() { "Inyecciones" },
                    new List<string>() { "trama.png" });
                await AddProductAsync("Betametasona Inyección", 3900M, 100F, new List<string>() { "Inyecciones" },
                    new List<string>() { "BETAME.png" });
                await AddProductAsync("Diclofenaco Inyección", 3900M, 200F, new List<string>() { "Inyecciones" },
                    new List<string>() { "diclofenaco.png" });
                await AddProductAsync("Dexametasona Inyección", 3900M, 150F, new List<string>() { "Inyecciones" },
                    new List<string>() { "unnamed.png" });
                await AddProductAsync("Suero Papeleta", 500M, 150F, new List<string>() { "Sueros", "Jarabes" },
                    new List<string>() { "suero.png" });
                await AddProductAsync("SueroX", 6900M, 250F, new List<string>() { "Sueros", "Jarabes" },
                    new List<string>() { "suerox.png" });
                await AddProductAsync("Aquilea Tos", 10900M, 10F, new List<string>() { "Jarabes" },
                    new List<string>() { "tos.png" });
                await AddProductAsync("Desloratadina", 4900M, 250F, new List<string>() { "Jarabes" },
                    new List<string>() { "desloratadina.png" });
                await AddProductAsync("Advil x10", 4900M, 250F, new List<string>() { "Pastillas" },
                    new List<string>() { "advil.png" });
                await AddProductAsync("Noraver x10", 11900M, 250F, new List<string>() { "Pastillas" },
                    new List<string>() { "noraver.png" });
                await AddProductAsync("Aspirina x10", 10000M, 250F, new List<string>() { "Pastillas" },
                    new List<string>() { "aspirina.png" });


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
                                new Laboratory { Name = "Farmacias Montevideo" },
                                new Laboratory { Name = "Farmacias A Buen Precio" },
                                new Laboratory { Name = "Farmacias San José" },
                            }
                        },
                        new Neighborhood {
                            Name = "Villahermosa",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Cruz verde" },
                                new Laboratory { Name = "Drogueria Alemana" },
                                new Laboratory { Name = "Farmatodo" },
                                new Laboratory { Name = "Botica Trébol Verde" },
                                new Laboratory { Name = "Botica Paraguay" },
                                new Laboratory { Name = "Farmacias Hernández" },
                            }
                        },
                        new Neighborhood {
                            Name = "Caicedo",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Tres esquinas" },
                                new Laboratory { Name = "Farmacia la estrechura" },
                                new Laboratory { Name = "Farmacias Para Todos" },
                                new Laboratory { Name = "Farmacias El Especialista" },
                                new Laboratory { Name = "Farmacia De la Ocho" },
                            }
                        },
                        new Neighborhood {
                            Name = "Poblado",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "La Esquina Natural" },
                                new Laboratory { Name = "Dietética Pensamiento Natural" },
                                new Laboratory { Name = "Esencia Verde" },
                                new Laboratory { Name = "Farm Fresh" },
                                new Laboratory { Name = "Farma Integral" },
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
                                new Laboratory { Name = "Farmacia Élite" },
                                new Laboratory { Name = "Farmacia HomeMedica" },
                                new Laboratory { Name = "Farmacia MediCare" },
                                new Laboratory { Name = "Farmacia Remedio Total" },
                                new Laboratory { Name = "Farmacia La Central" },
                            }
                        },
                        new Neighborhood {
                            Name = "Bosa",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Sinergia Farm" },
                                new Laboratory { Name = "Farmacia Génesis" },
                                new Laboratory { Name = "Farmacia Medicheckn" },
                                new Laboratory { Name = "Focus Farm" },
                                new Laboratory { Name = "Dakota Farma" },
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
                                new Laboratory { Name = "Farma Milenium" },
                                new Laboratory { Name = "Farmacia Ámbar" },
                                new Laboratory { Name = "Farmacia La Pacífica" },
                                new Laboratory { Name = "Farmacia Cura Y Bienestar" },
                                new Laboratory { Name = "Farmacia Néctar Salud" },
                                new Laboratory { Name = "La Casa De La Medicina" },
                            }
                        },
                        new Neighborhood {
                            Name = "San Fernando",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Farmacia La Real" },
                                new Laboratory { Name = "Farmacia La Imperial" },
                                new Laboratory { Name = "Farmacia Prisma" },
                                new Laboratory { Name = "Farmacia Vitality" },
                                new Laboratory { Name = "Farmacia Atlas" },
                                new Laboratory { Name = "ExtraFarma" },
                            }
                        }
                    }
                });
                _context.Cities.Add(new City
                {
                    Name = "Barranquilla",
                    Neighborhoods = new List<Neighborhood>()
                    {
                        new Neighborhood {
                            Name = "Ciudad Jardin",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Sinergia Farm" },
                                new Laboratory { Name = "Farmacia Génesis" },
                                new Laboratory { Name = "Farmacia Medicheckn" },
                                new Laboratory { Name = "Focus Farm" },
                                new Laboratory { Name = "Dakota Farma" },
                            }
                        },
                        new Neighborhood {
                            Name = "San Fernando",
                            Laboratories = new List<Laboratory>()
                            {
                                new Laboratory { Name = "Tres esquinas" },
                                new Laboratory { Name = "Farmacia la estrechura" },
                                new Laboratory { Name = "Farmacias Para Todos" },
                                new Laboratory { Name = "Farmacias El Especialista" },
                                new Laboratory { Name = "Farmacia De la Ocho" },
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
