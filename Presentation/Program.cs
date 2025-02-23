using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddDbContext<DataContext>(options => options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Projects\\DataStorageAssignment\\Data\\Databases\\local_database.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"))
    .AddScoped<IProjectRepository, ProjectRepository>()
    .AddScoped<IProjectService, ProjectService>()
    .BuildServiceProvider();

var projectService = serviceProvider.GetRequiredService<IProjectService>();
var dataContext = serviceProvider.GetRequiredService<DataContext>();

while (true)
{
    Console.Clear();
    Console.WriteLine("*** Projektapplikation Mattin-Lassei AB ***");
    Console.WriteLine(" ");
    Console.WriteLine("1. Lista projekt");
    Console.WriteLine("2. Lägg upp projekt");
    Console.WriteLine("3. Redigera existerande projekt");
    Console.WriteLine("4. Radera ett projekt");
    Console.WriteLine("5. Avsluta applikation");
    Console.WriteLine(" ");
    Console.Write("Ditt val: ");

    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            Console.Clear();
            var projects = await projectService.GetProjectsAsync();
            foreach (var project in projects)
            {
                Console.WriteLine($"#{project.ProjectNumber} - {project.Name} - (Status: {project.StatusTypeName})");
            }

            // Begär projektnummer för att visa detaljer
            Console.WriteLine(" ");
            Console.Write("Ange projektnummer för att visa detaljer: ");
            int selectedProjectNumber = int.Parse(Console.ReadLine() ?? "0");

            // Hämta det specifika projektet baserat på projektnumret
            var selectedProject = await projectService.GetProjectByIdAsync(selectedProjectNumber);

            if (selectedProject != null)
            {
                // Visa projektets detaljer
                Console.Clear();
                Console.WriteLine("*** Projektdetaljer ***");
                Console.WriteLine(" ");
                Console.WriteLine($"Projektnamn: {selectedProject.Name}");
                Console.WriteLine($"Startdatum: {selectedProject.StartDate.ToShortDateString()}");
                Console.WriteLine($"Slutdatum: {selectedProject.EndDate.ToShortDateString()}");
                Console.WriteLine($"Kundnamn: {selectedProject.CustomerName}");
                Console.WriteLine($"Status: {selectedProject.StatusTypeName}");
                Console.WriteLine($"Pris per timme: {selectedProject.PricePerHour} SEK");
                Console.WriteLine($"Totalt antal timmar: {selectedProject.TotalHours}");
                Console.WriteLine($"Totalt pris: {selectedProject.PricePerHour * selectedProject.TotalHours} SEK");
            }
            else
            {
                Console.WriteLine("Projektet finns inte.");
            }

            Console.WriteLine(" ");
            Console.WriteLine("Tryck enter för att gå tillbaka..");
            Console.ReadKey();
            break;

        case "2":
            Console.Clear();
            // Begär projektdetaljer från användaren
            Console.Write("Ange projektnamn: ");
            string projectName = Console.ReadLine() ?? "Okänt projekt";

            Console.Write("Ange startdatum (YYYY-MM-DD): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());

            Console.Write("Ange slutdatum (YYYY-MM-DD): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.AddMonths(6).ToString());

            Console.Write("Ange kundens namn: ");
            string customerName = Console.ReadLine() ?? "Okänd kund";

            Console.Write("Ange tjänstens namn: ");
            string productName = Console.ReadLine() ?? "Standardtjänst";

            Console.Write("Ange status-ID (1 = Ej påbörjat, 2 = Pågående, 3 = Avslutat): ");
            int statusId;
            bool isValidStatus = int.TryParse(Console.ReadLine(), out statusId);

            if (!isValidStatus)
            {
                Console.WriteLine("Ogiltigt status-ID, sätts till standardvärde 1.");
                statusId = 1; // Standardvärde om användaren inte anger ett korrekt ID
            }

            Console.Write("Ange pris per timme: ");
            decimal pricePerHour = decimal.Parse(Console.ReadLine() ?? "0");

            Console.Write("Ange totalt antal timmar: ");
            int totalHours = int.Parse(Console.ReadLine() ?? "0");


            // Projektansvarig
            Console.WriteLine(" ");
            Console.WriteLine("Lägg upp en projektansvarig för detta projekt");
            Console.WriteLine(" ");

            // Begär användardetaljer
            Console.Write("Ange förnamn: ");
            string firstName = Console.ReadLine() ?? "Förnamn";
            Console.Write("Ange efternamn: ");
            string lastName = Console.ReadLine() ?? "Efternamn";
            Console.Write("Ange e-postadress: ");
            string email = Console.ReadLine() ?? "email@domain.com";

            // Skapa och spara användaren
            var user = new UserEntity
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();
            int userId = user.Id; // Tilldela värdet för userId

            // Kontrollera om produkten redan finns eller skapa en ny
            var existingProduct = await dataContext.Products
                .Where(p => p.ProductName == productName)
                .FirstOrDefaultAsync();

            int productId;
            if (existingProduct != null)
            {
                productId = existingProduct.Id;
            }
            else
            {
                // Skapa och spara produkt om den inte finns
                var newProduct = new ProductEntity
                {
                    ProductName = productName,
                };
                dataContext.Products.Add(newProduct);
                await dataContext.SaveChangesAsync();
                productId = newProduct.Id;
            }

            // Skapa och lägg till projektet
            var newProject = await projectService.AddProjectWithCustomerAsync(projectName, startDate, endDate, customerName, productName, statusId, pricePerHour, totalHours, userId);

            Console.WriteLine($"Projektet {newProject.Name} har nu skapats!");
            Console.WriteLine(" ");
            Console.WriteLine("Tryck enter för att fortsätta...");
            Console.ReadKey();
            break;

        case "3":
            Console.Clear();
            // Begär projektnummer för redigering
            Console.Write("Ange projektnummer för att redigera: ");
            int projectNumber = int.Parse(Console.ReadLine() ?? "0");

            // Hämta det projekt som ska redigeras
            var projectToEdit = await projectService.GetProjectByIdAsync(projectNumber);
            if (projectToEdit == null)
            {
                Console.WriteLine("Projektet finns inte.");
                Console.WriteLine(" ");
                Console.WriteLine("Tryck enter för att fortsätta...");
                Console.ReadKey();
                break;
            }

            // Begär nya detaljer för projektet
            Console.WriteLine($"Redigera projektet: {projectToEdit.Name}");

            Console.Write("Ange nytt projektnamn (nuvarande: " + projectToEdit.Name + "): ");
            string newProjectName = Console.ReadLine() ?? projectToEdit.Name;

            Console.Write("Ange nytt startdatum (nuvarande: " + projectToEdit.StartDate.ToShortDateString() + "): ");
            string? startDateInput = Console.ReadLine();
            DateTime newStartDate = string.IsNullOrEmpty(startDateInput) ? projectToEdit.StartDate : DateTime.Parse(startDateInput);

            Console.Write("Ange nytt slutdatum (nuvarande: " + projectToEdit.EndDate.ToShortDateString() + "): ");
            string? endDateInput = Console.ReadLine();
            DateTime newEndDate = string.IsNullOrEmpty(endDateInput) ? projectToEdit.EndDate : DateTime.Parse(endDateInput);

            Console.Write("Ange nytt kundnamn (nuvarande: " + projectToEdit.CustomerName + "): ");
            string newCustomerName = Console.ReadLine() ?? projectToEdit.CustomerName;

            Console.Write("Ange nytt produktnamn (nuvarande: " + projectToEdit.Name + "): ");
            string newProductName = Console.ReadLine() ?? projectToEdit.Name;

            Console.Write("Ange nytt status-ID (nuvarande: " + projectToEdit.StatusTypeId + "): ");
            string? statusIdInput = Console.ReadLine();
            int newStatusId = string.IsNullOrEmpty(statusIdInput) ? projectToEdit.StatusTypeId : int.Parse(statusIdInput);

            Console.Write("Ange nytt pris per timme (nuvarande: " + projectToEdit.PricePerHour + "): ");
            string? pricePerHourInput = Console.ReadLine();
            decimal newPricePerHour = string.IsNullOrEmpty(pricePerHourInput) ? projectToEdit.PricePerHour : decimal.Parse(pricePerHourInput);

            Console.Write("Ange nytt totalt antal timmar (nuvarande: " + projectToEdit.TotalHours + "): ");
            string? totalHoursInput = Console.ReadLine();
            int newTotalHours = string.IsNullOrEmpty(totalHoursInput) ? projectToEdit.TotalHours : int.Parse(totalHoursInput);

            var updatedProject = await projectService.UpdateProjectAsync(projectNumber, new ProjectModel
            {
                Name = newProjectName,
                StartDate = newStartDate,
                EndDate = newEndDate,
                CustomerName = newCustomerName,
                StatusTypeId = newStatusId,
                PricePerHour = newPricePerHour,
                TotalHours = newTotalHours
            });

            if (updatedProject != null)
            {
                Console.WriteLine($"Projektet {updatedProject.Name} har uppdaterats!");
            }
            else
            {
                Console.WriteLine("Projektet kunde inte uppdateras.");
            }

            Console.WriteLine(" ");
            Console.WriteLine("Tryck enter för att fortsätta...");
            Console.ReadKey();
            break;


        case "4":
            Console.Clear();
            // Begär projektnummer för att radera
            Console.Write("Ange projektnummer för att radera: ");
            int projectNumberToDelete = int.Parse(Console.ReadLine() ?? "0");

            // Hämta det projekt som ska raderas
            var projectToDelete = await projectService.GetProjectByIdAsync(projectNumberToDelete);
            if (projectToDelete == null)
            {
                Console.WriteLine("Projektet finns inte.");
            }
            else
            {
                // Bekräfta innan radering
                Console.WriteLine($"Är du säker på att du vill radera projektet: {projectToDelete.Name}? (ja/nej)");
                string confirmation = Console.ReadLine()?.ToLower();

                if (confirmation == "ja")
                {
                    // Anropa en metod för att radera projektet
                    var isDeleted = await projectService.DeleteProjectAsync(projectNumberToDelete);

                    if (isDeleted)
                    {
                        Console.WriteLine($"Projektet {projectToDelete.Name} har raderats!");
                    }
                    else
                    {
                        Console.WriteLine("Projektet kunde inte raderas.");
                    }
                }
                else
                {
                    Console.WriteLine("Radering avbröts.");
                }
            }

            Console.WriteLine(" ");
            Console.WriteLine("Tryck enter för att fortsätta...");
            Console.ReadKey();
            break;

        case "5":
            return;
    }
}
