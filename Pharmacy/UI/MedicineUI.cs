using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Enums;
using Pharmacy.UI.Helpers;

namespace Pharmacy.UI
{
    public sealed class MedicineUI
    {
        private readonly MedicineService _medicineService;
        private readonly CategoryUI _categoryUI;

        public MedicineUI(
            MedicineService medicineService,
            CategoryUI categoryUI)
        {
            ArgumentNullException.ThrowIfNull(medicineService);
            ArgumentNullException.ThrowIfNull(categoryUI);

            _medicineService = medicineService;
            _categoryUI = categoryUI;
        }

        public void ShowMenu()
        {
            ConsoleMenu.ShowMenu(
                "Medicine Management",
                "Add Medicine",
                "Search Medicine",
                "Remove Medicine",
                "Display All Medicines",
                "Back");
        }

        public void AddMedicine()
        {
            string name = ConsoleInput.ReadRequiredText("Medicine Name: ");
            string company = ConsoleInput.ReadRequiredText("Manufacturer: ");
            string barcode = ConsoleInput.ReadRequiredText("Barcode: ");
            string activeIngredient = ConsoleInput.ReadRequiredText("Active Ingredient: ");
            string strength = ConsoleInput.ReadRequiredText("Strength: ");

            decimal purchasePrice = ConsoleInput.ReadDecimal("Purchase Price: ");
            decimal salePrice = ConsoleInput.ReadDecimal("Sale Price: ");

            int reorderLevel = ConsoleInput.ReadNonNegativeInteger(
                "Reorder Level: ");

            DosageForm dosageForm = DosageForm.Tablet;

            Category category = _categoryUI.GetCategory();

            _medicineService.Add(
                name,
                company,
                barcode,
                activeIngredient,
                strength,
                purchasePrice,
                salePrice,
                reorderLevel,
                dosageForm,
                category);

            Console.WriteLine("Medicine added successfully.");
        }

        public void SearchMedicine()
        {
            string name = ConsoleInput.ReadRequiredText(
                "Search for a medicine by name: ");

            List<Medicine> medicines =
                _medicineService.SearchMedicineByName(name);

            if (medicines.Count == 0)
            {
                Console.WriteLine("No medicines found with that name.");
                return;
            }

            DisplayMedicine(medicines);
        }

        public void DisplayMedicine(List<Medicine> medicines)
        {
            if (medicines.Count == 0)
            {
                Console.WriteLine("No medicines to display.");
                return;
            }

            foreach (Medicine medicine in medicines)
            {
                Console.WriteLine("--------------------------------");
                DisplayMedicineDetails(medicine);
            }

            Console.WriteLine("--------------------------------");
        }

        private void DisplayMedicineDetails(Medicine medicine)
        {
            Console.WriteLine($"ID                : {medicine.Id}");
            Console.WriteLine($"Name              : {medicine.Name}");
            Console.WriteLine($"Manufacturer      : {medicine.Company}");
            Console.WriteLine($"Barcode           : {medicine.Barcode}");
            Console.WriteLine($"Active Ingredient : {medicine.ActiveIngredient}");
            Console.WriteLine($"Strength          : {medicine.Strength}");
            Console.WriteLine($"Purchase Price    : {medicine.PurchasePrice:C}");
            Console.WriteLine($"Sale Price        : {medicine.SalePrice:C}");
            Console.WriteLine($"Reorder Level     : {medicine.ReorderLevel}");
            Console.WriteLine($"Dosage Form       : {medicine.DosageForm}");
            Console.WriteLine($"Category          : {medicine.Category?.Name}");
        }

        public void DisplayAll()
        {
            IReadOnlyList<Medicine> medicines = _medicineService.GetAll();

            Console.WriteLine("========== ALL MEDICINES ==========");

            DisplayMedicine(medicines.ToList());
        }

        public void RemoveMedicine()
        {
            IReadOnlyList<Medicine> medicines = _medicineService.GetAll();

            if (medicines.Count == 0)
            {
                Console.WriteLine("No medicines available to remove.");
                return;
            }

            DisplayMedicine(medicines.ToList());

            int inputId = ConsoleInput.ReadNonNegativeInteger(
                "Enter the ID of the medicine to remove: ");

            Medicine? medicine = _medicineService.GetById(inputId);

            if (medicine is null)
            {
                Console.WriteLine($"No medicine found with ID: {inputId}");
                return;
            }

            _medicineService.RemoveMedicine(medicine.Id);

            Console.WriteLine("Medicine removed successfully.");
        }

        public void RunMenu()
        {
            while (true)
            {
                Console.Clear();

                ShowMenu();

                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        AddMedicine();
                        break;

                    case "2":
                        SearchMedicine();
                        break;

                    case "3":
                        RemoveMedicine();
                        break;

                    case "4":
                        DisplayAll();
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine(
                            "Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}