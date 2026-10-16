using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Enums;
using Pharmacy.UI.Helpers;

namespace Pharmacy.UI
{
    public sealed class PurchaseUI
    {
        private readonly PurchaseService _purchaseService;
        private readonly SupplierService _supplierService;
        private readonly MedicineService _medicineService;

        public PurchaseUI(
            PurchaseService purchaseService,
            SupplierService supplierService,
            MedicineService medicineService)
        {
            _purchaseService = purchaseService
                ?? throw new ArgumentNullException(nameof(purchaseService));

            _supplierService = supplierService
                ?? throw new ArgumentNullException(nameof(supplierService));

            _medicineService = medicineService
                ?? throw new ArgumentNullException(nameof(medicineService));
        }
        public void ShowMenu()
        {

            ConsoleMenu.ShowMenu(
                    "    PURCHASE MANAGEMENT",
                    "1. Create Purchase",
                    "2. Display All Purchases",
                    "3. Search Purchase By ID",
                    "4. Search Purchase By Invoice Number",
                    "0. Back");
        }

        public void RunMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                /*
                Console.WriteLine("================================");
                Console.WriteLine("       PURCHASE MANAGEMENT");
                Console.WriteLine("================================");
                Console.WriteLine("1. Create Purchase");
                Console.WriteLine("2. Display All Purchases");
                Console.WriteLine("3. Search Purchase By ID");
                Console.WriteLine("4. Search Purchase By Invoice Number");
                Console.WriteLine("0. Back");
                Console.WriteLine("--------------------------------");
                Console.Write("Choose an option: ");
                */
                ShowMenu();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePurchase();
                        break;

                    case "2":
                        DisplayAllPurchases();
                        break;

                    case "3":
                        SearchPurchaseById();
                        break;

                    case "4":
                        SearchPurchaseByInvoiceNumber();
                        break;

                    case "0":
                        isRunning = false;
                        break;

                    default:
                        ConsoleInput.ShowMessage("Invalid option.");
                        break;
                }
            }
        }

        private void CreatePurchase()
        {
            Console.Clear();
            Console.WriteLine("========== CREATE PURCHASE ==========");

            try
            {
                IReadOnlyList<Supplier> suppliers =
                    _supplierService.GetAll();

                if (suppliers.Count == 0)
                {
                   ConsoleInput.ShowMessage(
                        "No suppliers found. Add a supplier first.");
                    return;
                }

                IReadOnlyList<Medicine> medicines =
                    _medicineService.GetAll();

                if (medicines.Count == 0)
                {
                    ConsoleInput.ShowMessage(
                        "No medicines found. Add a medicine first.");
                    return;
                }

                DisplaySuppliers(suppliers);

                int supplierId = ConsoleInput.ReadNonNegativeInteger(
                    "Enter Supplier ID: ");

                Supplier? supplier =
                    _supplierService.GetById(supplierId);

                if (supplier is null)
                {
                    ConsoleInput.ShowMessage("Supplier not found.");
                    return;
                }

                DisplayMedicines(medicines);

                int medicineId = ConsoleInput.ReadNonNegativeInteger(
                    "Enter Medicine ID: ");

                Medicine? medicine =
                    _medicineService.GetById(medicineId);

                if (medicine is null)
                {
                    ConsoleInput.ShowMessage("Medicine not found.");
                    return;
                }

                string invoiceNumber = ConsoleInput.ReadRequiredText(
                    "Enter Purchase Invoice Number: ");

                PaymentMethod paymentMethod =
                    ReadPaymentMethod();

                string batchNumber = ConsoleInput.ReadRequiredText(
                    "Enter Batch Number: ");

                int quantity = ConsoleInput.ReadNonNegativeInteger(
                    "Enter Quantity: ");

                DateTime productionDate = ConsoleInput.ReadDate(
                    "Enter Production Date (yyyy-MM-dd): ");

                DateTime expiryDate = ConsoleInput.ReadDate(
                    "Enter Expiry Date (yyyy-MM-dd): ");

                if (expiryDate <= productionDate)
                {
                    ConsoleInput.ShowMessage(
                        "Expiry date must be after production date.");
                    return;
                }

                if (expiryDate.Date <= DateTime.Today)
                {
                    ConsoleInput.ShowMessage(
                        "The batch must not be expired.");
                    return;
                }

                decimal purchasePrice = ConsoleInput.ReadDecimal(
                    "Enter Purchase Price Per Unit: ");

                Console.WriteLine();
                Console.WriteLine("========== PURCHASE SUMMARY ==========");
                Console.WriteLine($"Invoice Number : {invoiceNumber}");
                Console.WriteLine($"Supplier ID    : {supplier.Id}");
                Console.WriteLine($"Medicine ID    : {medicine.Id}");
                Console.WriteLine($"Batch Number   : {batchNumber}");
                Console.WriteLine($"Quantity       : {quantity}");
                Console.WriteLine($"Purchase Price : {purchasePrice:F2}");
                Console.WriteLine(
                    $"Total          : {quantity * purchasePrice:F2}");
                Console.WriteLine($"Payment        : {paymentMethod}");

                Console.WriteLine();
                Console.Write("Confirm purchase? (Y/N): ");

                string? confirmation = Console.ReadLine();

                if (!string.Equals(
                    confirmation?.Trim(),
                    "Y",
                    StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleInput.ShowMessage("Purchase cancelled.");
                    return;
                }

                PurchaseInvoice invoice =
                    _purchaseService.CreatePurchase(
                        invoiceNumber,
                        paymentMethod,
                        supplierId,
                        medicineId,
                        batchNumber,
                        quantity,
                        productionDate,
                        expiryDate,
                        purchasePrice);

                Console.WriteLine();
                Console.WriteLine("Purchase created successfully.");
                Console.WriteLine(
                    $"Invoice ID     : {invoice.Id}");
                Console.WriteLine(
                    $"Invoice Number : {invoice.InvoiceNumber}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Failed to create purchase.");
                Console.WriteLine($"Reason: {ex.Message}");
            }

            ConsoleInput.Pause();
        }

        private void DisplayAllPurchases()
        {
            Console.Clear();
            Console.WriteLine("========== ALL PURCHASES ==========");

            IReadOnlyList<PurchaseInvoice> invoices =
                _purchaseService.GetAll();

            if (invoices.Count == 0)
            {
                ConsoleInput.ShowMessage("No purchases found.");
                return;
            }

            foreach (PurchaseInvoice invoice in invoices)
            {
                DisplayInvoice(invoice);
            }

            ConsoleInput.Pause();
        }

        private void SearchPurchaseById()
        {
            Console.Clear();
            Console.WriteLine("========== SEARCH PURCHASE ==========");

            try
            {
                int id = ConsoleInput.ReadNonNegativeInteger("Enter Invoice ID: ");

                PurchaseInvoice? invoice =
                    _purchaseService.GetById(id);

                if (invoice is null)
                {
                    ConsoleInput.ShowMessage("Purchase invoice not found.");
                    return;
                }

                DisplayInvoice(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ConsoleInput.Pause();
            }
        }

        private void SearchPurchaseByInvoiceNumber()
        {
            Console.Clear();
            Console.WriteLine(
                "========== SEARCH BY INVOICE NUMBER ==========");

            try
            {
                string invoiceNumber = ConsoleInput.ReadRequiredText(
                    "Enter Invoice Number: ");

                PurchaseInvoice? invoice =
                    _purchaseService.GetByInvoiceNumber(
                        invoiceNumber);

                if (invoice is null)
                {
                    ConsoleInput.ShowMessage("Purchase invoice not found.");
                    return;
                }

                DisplayInvoice(invoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ConsoleInput.Pause();
            }
        }

        private static void DisplaySuppliers(
            IReadOnlyList<Supplier> suppliers)
        {
            Console.WriteLine();
            Console.WriteLine("========== SUPPLIERS ==========");

            foreach (Supplier supplier in suppliers)
            {
                Console.WriteLine(
                    $"ID: {supplier.Id}");
            }
        }

        private static void DisplayMedicines(
            IReadOnlyList<Medicine> medicines)
        {
            Console.WriteLine();
            Console.WriteLine("========== MEDICINES ==========");

            foreach (Medicine medicine in medicines)
            {
                Console.WriteLine(
                    $"ID: {medicine.Id} | " +
                    $"Name: {medicine.Name} | " +
                    $"Purchase Price: {medicine.PurchasePrice:F2}");
            }
        }

        private static void DisplayInvoice(
            PurchaseInvoice invoice)
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Invoice ID     : {invoice.Id}");
            Console.WriteLine(
                $"Invoice Number : {invoice.InvoiceNumber}");
            Console.WriteLine(
                $"Payment Method : {invoice.PaymentMethod}");
            Console.WriteLine("--------------------------------");
        }

        private static PaymentMethod ReadPaymentMethod()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Select Payment Method:");

                string[] names =
                    Enum.GetNames(typeof(PaymentMethod));

                for (int i = 0; i < names.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {names[i]}");
                }

                Console.Write("Choose payment method: ");

                string? input = Console.ReadLine();

                if (int.TryParse(input, out int choice) &&
                    choice >= 1 &&
                    choice <= names.Length)
                {
                    return (PaymentMethod)Enum.Parse(
                        typeof(PaymentMethod),
                        names[choice - 1]);
                }

                Console.WriteLine(
                    "Invalid payment method. Try again.");
            }
        }


        
    }

}