using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Pharmacy.UI.Helpers;

namespace Pharmacy
{
    public sealed class SupplierUI
    {
        private readonly SupplierService _supplierService;

        public SupplierUI(SupplierService supplierService)
        {
            ArgumentNullException.ThrowIfNull(supplierService);
            _supplierService = supplierService;
        }

        // Display the supplier management menu
        public void ShowMenu()
        {
            ConsoleMenu.ShowMenu(
                "Supplier Management",
                "Add Supplier",
                "Search Supplier",
                "Update Supplier",
                "Delete Supplier",
                "Display All Suppliers",
                "Back");
        }

        // Add a supplier
        public void AddSupplier()
        {
            Console.WriteLine("========== Add New Supplier ==========");

            string name = ConsoleInput.ReadRequiredText(
                "Supplier Name: ");

            string phone = ConsoleInput.ReadRequiredText(
                "Phone Number: ");

            Console.Write("Email (optional): ");
            string? email = Console.ReadLine();

            Console.Write("Address (optional): ");
            string? address = Console.ReadLine();

            Console.Write("Tax Number (optional): ");
            string? taxNumber = Console.ReadLine();

            Console.Write("Commercial Registration Number (optional): ");
            string? commercialReg = Console.ReadLine();

            _supplierService.Add(
                name,
                phone,
                email,
                address,
                taxNumber,
                commercialReg);

            Console.WriteLine();
            Console.WriteLine("Supplier added successfully.");
        }

        // Search for a supplier by name or phone number
        public void SearchSupplier()
        {
            Console.WriteLine("========== Search Supplier ==========");
            Console.WriteLine("1. Search by Name");
            Console.WriteLine("2. Search by Phone Number");

            Console.Write("Select search method: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    {
                        string name = ConsoleInput.ReadRequiredText(
                            "Enter supplier name: ");

                        IReadOnlyList<Supplier> suppliers =
                            _supplierService.SearchByName(name);

                        DisplaySuppliers(suppliers);
                        break;
                    }

                case "2":
                    {
                        string phone = ConsoleInput.ReadRequiredText(
                            "Enter phone number: ");

                        IReadOnlyList<Supplier> suppliers =
                            _supplierService.SearchByPhone(phone);

                        DisplaySuppliers(suppliers);
                        break;
                    }

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        // Update supplier information
        public void UpdateSupplier()
        {
            IReadOnlyList<Supplier> suppliers =
                _supplierService.GetAll();

            if (suppliers.Count == 0)
            {
                Console.WriteLine("There are no suppliers to update.");
                return;
            }

            DisplaySuppliers(suppliers);

            int id = ConsoleInput.ReadNonNegativeInteger(
                "Enter the ID of the supplier to update: ");

            Supplier? supplier = _supplierService.GetById(id);

            if (supplier is null)
            {
                Console.WriteLine("Supplier not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Leave a field empty to keep its current value.");

            Console.Write($"Supplier Name [{supplier.Name}]: ");
            string? name = Console.ReadLine();

            Console.Write($"Phone Number [{supplier.Phone}]: ");
            string? phone = Console.ReadLine();

            Console.Write($"Email [{supplier.Email ?? "Not specified"}]: ");
            string? email = Console.ReadLine();

            Console.Write($"Address [{supplier.Address ?? "Not specified"}]: ");
            string? address = Console.ReadLine();

            Console.Write($"Tax Number [{supplier.TaxNumber ?? "Not specified"}]: ");
            string? taxNumber = Console.ReadLine();

            Console.Write(
                $"Commercial Registration Number [{supplier.CommercialRegistrationNumber ?? "Not specified"}]: ");
            string? commercialReg = Console.ReadLine();

            _supplierService.Update(
                id,
                name: string.IsNullOrWhiteSpace(name) ? null : name,
                phone: string.IsNullOrWhiteSpace(phone) ? null : phone,
                email: string.IsNullOrWhiteSpace(email) ? null : email,
                address: string.IsNullOrWhiteSpace(address) ? null : address,
                taxNumber: string.IsNullOrWhiteSpace(taxNumber) ? null : taxNumber,
                commercialReg: string.IsNullOrWhiteSpace(commercialReg)
                    ? null
                    : commercialReg);

            Console.WriteLine();
            Console.WriteLine("Supplier information updated successfully.");
        }

        // Delete a supplier
        public void DeleteSupplier()
        {
            IReadOnlyList<Supplier> suppliers =
                _supplierService.GetAll();

            if (suppliers.Count == 0)
            {
                Console.WriteLine("There are no suppliers to delete.");
                return;
            }

            DisplaySuppliers(suppliers);

            int id = ConsoleInput.ReadNonNegativeInteger(
                "Enter the ID of the supplier to delete: ");

            Supplier? supplier = _supplierService.GetById(id);

            if (supplier is null)
            {
                Console.WriteLine("Supplier not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Supplier: {supplier.Name}");
            Console.Write("Are you sure you want to delete this supplier? (y/n): ");

            string confirmation = Console.ReadLine() ?? string.Empty;

            if (!confirmation.Equals(
                    "y",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = _supplierService.Delete(id);

            Console.WriteLine(
                deleted
                    ? "Supplier deleted successfully."
                    : "Unable to delete the supplier.");
        }

        // Display all suppliers
        public void DisplayAll()
        {
            IReadOnlyList<Supplier> suppliers =
                _supplierService.GetAll();

            Console.WriteLine("========== All Suppliers ==========");

            DisplaySuppliers(suppliers);
        }

        // Display a list of suppliers
        private void DisplaySuppliers(
            IReadOnlyList<Supplier> suppliers)
        {
            if (suppliers.Count == 0)
            {
                Console.WriteLine("No results to display.");
                return;
            }

            foreach (Supplier supplier in suppliers)
            {
                Console.WriteLine("----------------------------------------");
                DisplaySupplierDetails(supplier);
            }

            Console.WriteLine("----------------------------------------");
        }

        // Display the details of a single supplier
        private void DisplaySupplierDetails(Supplier supplier)
        {
            Console.WriteLine($"ID                          : {supplier.Id}");
            Console.WriteLine($"Supplier Name               : {supplier.Name}");
            Console.WriteLine($"Phone Number                : {supplier.Phone}");
            Console.WriteLine($"Email                       : {supplier.Email ?? "Not specified"}");
            Console.WriteLine($"Address                     : {supplier.Address ?? "Not specified"}");
            Console.WriteLine($"Tax Number                  : {supplier.TaxNumber ?? "Not specified"}");
            Console.WriteLine(
                $"Commercial Registration No. : {supplier.CommercialRegistrationNumber ?? "Not specified"}");
        }

        // Run the supplier menu
        public void RunMenu()
        {
            while (true)
            {
                Console.Clear();

                ShowMenu();

                string choice = Console.ReadLine() ?? string.Empty;

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddSupplier();
                            break;

                        case "2":
                            SearchSupplier();
                            break;

                        case "3":
                            UpdateSupplier();
                            break;

                        case "4":
                            DeleteSupplier();
                            break;

                        case "5":
                            DisplayAll();
                            break;

                        case "6":
                            return;

                        default:
                            Console.WriteLine(
                                "Invalid option. Please try again.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Invalid data: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Unable to complete the operation: {ex.Message}");
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine($"Item not found: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}