using Pharmacy.UI;
using Pharmacy.UI.Helpers;

namespace Pharmacy.UI.Menus
{
    public sealed class MainMenu
    {
        private readonly CategoryUI _categoryUI;
        private readonly MedicineUI _medicineUI;
        private readonly SupplierUI _supplierUI;
        private readonly PurchaseUI _purchaseUI;
        private readonly SaleUI _saleUI;

        public MainMenu(
            CategoryUI categoryUI,
            MedicineUI medicineUI,
            SupplierUI supplierUI,
            PurchaseUI purchaseUI,
            SaleUI saleUI)
        {
            _categoryUI = categoryUI
                ?? throw new ArgumentNullException(nameof(categoryUI));

            _medicineUI = medicineUI
                ?? throw new ArgumentNullException(nameof(medicineUI));

            _supplierUI = supplierUI
                ?? throw new ArgumentNullException(nameof(supplierUI));

            _purchaseUI = purchaseUI
                ?? throw new ArgumentNullException(nameof(purchaseUI));

            _saleUI = saleUI
                ?? throw new ArgumentNullException(nameof(saleUI));
        }

        public void ShowMenu()
        {
            ConsoleMenu.ShowMenu(
                "       PHARMACY MANAGEMENT SYSTEM",
                "1. Category Management",
                "2. Medicine Management",
                "3. Supplier Management",
                "4. Purchase Management",
                "5. Sale Management",
                "0. Exit");
        }

        public void Run()
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
                        _categoryUI.RunMenu();
                        break;

                    case "2":
                        _medicineUI.RunMenu();
                        break;

                    case "3":
                        _supplierUI.RunMenu();
                        break;

                    case "4":
                        _purchaseUI.RunMenu();
                        break;

                    case "5":
                        _saleUI.RunMenu();
                        break;

                    case "0":
                        isRunning = false;
                        break;

                    default:
                        ConsoleInput.ShowMessage("Invalid option.");
                        break;
                }
            }

            Console.WriteLine("Thank you for using Pharmacy Management System.");
        }
    }
}