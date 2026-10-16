using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Enums;
using Pharmacy.UI.Helpers;

namespace Pharmacy.UI
{
    public sealed class SaleUI
    {
        private readonly SaleService _saleService;
        private readonly MedicineService _medicineService;
        private readonly BatchService _batchService;

        public SaleUI(
            SaleService saleService,
            MedicineService medicineService,
            BatchService batchService)
        {
            _saleService = saleService
                ?? throw new ArgumentNullException(nameof(saleService));

            _medicineService = medicineService
                ?? throw new ArgumentNullException(nameof(medicineService));

            _batchService = batchService
                ?? throw new ArgumentNullException(nameof(batchService));
        }

        public void ShowMenu()
        {
            ConsoleMenu.ShowMenu(
                "       SALE MANAGEMENT",
                "1. Create Sale",
                "2. Display All Sales",
                "3. Search Sale By ID",
                "4. Search Sale By Invoice Number",
                "0. Back");
        }

        public void RunMenu()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                ShowMenu();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateSale();
                        break;

                    case "2":
                        DisplayAllSales();
                        break;

                    case "3":
                        SearchSaleById();
                        break;

                    case "4":
                        SearchSaleByInvoiceNumber();
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

        private void CreateSale()
        {
            Console.Clear();
            Console.WriteLine("========== CREATE SALE ==========");

            try
            {
                IReadOnlyList<Medicine> medicines =
                    _medicineService.GetAll();

                if (medicines.Count == 0)
                {
                    ConsoleInput.ShowMessage(
                        "No medicines found. Add a medicine first.");
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

                IReadOnlyList<Batch> batches =
                    _batchService.GetBatchByMedicine(medicine);

                if (batches.Count == 0)
                {
                    ConsoleInput.ShowMessage(
                        "No batches found for this medicine.");
                    return;
                }

                DisplayBatches(batches);

                string batchNumber = ConsoleInput.ReadRequiredText(
                    "Enter Batch Number: ");

                Batch? batch = _batchService.FindBatchByBatchNumber(
                    medicine,
                    batchNumber);

                if (batch is null)
                {
                    ConsoleInput.ShowMessage("Batch not found.");
                    return;
                }

                if (batch.IsExpired())
                {
                    ConsoleInput.ShowMessage(
                        "This batch is expired and cannot be sold.");
                    return;
                }

                if (batch.Quantity <= 0)
                {
                    ConsoleInput.ShowMessage(
                        "This batch is out of stock.");
                    return;
                }

                int quantity = ConsoleInput.ReadNonNegativeInteger(
                    "Enter Quantity: ");

                if (quantity <= 0)
                {
                    ConsoleInput.ShowMessage(
                        "Quantity must be greater than zero.");
                    return;
                }

                if (quantity > batch.Quantity)
                {
                    ConsoleInput.ShowMessage(
                        $"Insufficient stock. Available quantity: {batch.Quantity}");
                    return;
                }

                decimal unitPrice = medicine.SalePrice;

                PaymentMethod paymentMethod = ReadPaymentMethod();

                decimal total = quantity * unitPrice;

                Console.WriteLine();
                Console.WriteLine("========== SALE SUMMARY ==========");
                Console.WriteLine($"Medicine      : {medicine.Name}");
                Console.WriteLine($"Batch Number  : {batch.BatchNumber}");
                Console.WriteLine($"Quantity      : {quantity}");
                Console.WriteLine($"Available     : {batch.Quantity}");
                Console.WriteLine($"Unit Price    : {unitPrice:F2}");
                Console.WriteLine($"Total         : {total:F2}");
                Console.WriteLine($"Payment Method: {paymentMethod}");

                Console.WriteLine();
                Console.Write("Confirm sale? (Y/N): ");

                string? confirmation = Console.ReadLine();

                if (!string.Equals(
                    confirmation?.Trim(),
                    "Y",
                    StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleInput.ShowMessage("Sale cancelled.");
                    return;
                }

                SalesInvoice invoice = _saleService.CreateSale(
                    paymentMethod,
                    medicine.Name,
                    batchNumber,
                    quantity,
                    unitPrice);

                Console.WriteLine();
                Console.WriteLine("Sale created successfully.");
                Console.WriteLine($"Invoice ID     : {invoice.Id}");
                Console.WriteLine($"Invoice Number : {invoice.InvoiceNumber}");
                Console.WriteLine($"Total          : {total:F2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Failed to create sale.");
                Console.WriteLine($"Reason: {ex.Message}");
            }

            ConsoleInput.Pause();
        }

        private void DisplayAllSales()
        {
            Console.Clear();
            Console.WriteLine("========== ALL SALES ==========");

            IReadOnlyList<SalesInvoice> invoices =
                _saleService.GetAll();

            if (invoices.Count == 0)
            {
                ConsoleInput.ShowMessage("No sales found.");
                return;
            }

            foreach (SalesInvoice invoice in invoices)
            {
                DisplayInvoice(invoice);
            }

            ConsoleInput.Pause();
        }

        private void SearchSaleById()
        {
            Console.Clear();
            Console.WriteLine("========== SEARCH SALE BY ID ==========");

            try
            {
                int id = ConsoleInput.ReadNonNegativeInteger(
                    "Enter Invoice ID: ");

                SalesInvoice? invoice = _saleService.GetById(id);

                if (invoice is null)
                {
                    ConsoleInput.ShowMessage(
                        "Sales invoice not found.");
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

        private void SearchSaleByInvoiceNumber()
        {
            Console.Clear();
            Console.WriteLine(
                "========== SEARCH SALE BY INVOICE NUMBER ==========");

            try
            {
                string invoiceNumber = ConsoleInput.ReadRequiredText(
                    "Enter Invoice Number: ");

                SalesInvoice? invoice =
                    _saleService.GetByInvoiceNumber(invoiceNumber);

                if (invoice is null)
                {
                    ConsoleInput.ShowMessage(
                        "Sales invoice not found.");
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
                    $"Sale Price: {medicine.SalePrice:F2}");
            }
        }

        private static void DisplayBatches(
            IReadOnlyList<Batch> batches)
        {
            Console.WriteLine();
            Console.WriteLine("========== BATCHES ==========");

            foreach (Batch batch in batches)
            {
                Console.WriteLine(
                    $"Batch Number: {batch.BatchNumber} | " +
                    $"Quantity: {batch.Quantity} | " +
                    $"Expiry Date: {batch.ExpiryDate:yyyy-MM-dd} | " +
                    $"Expired: {batch.IsExpired()}");
            }
        }

        private static void DisplayInvoice(SalesInvoice invoice)
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Invoice ID     : {invoice.Id}");
            Console.WriteLine($"Invoice Number : {invoice.InvoiceNumber}");
            Console.WriteLine($"Payment Method : {invoice.PaymentMethod}");
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