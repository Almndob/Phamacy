using Pharmacy.Application.Services;
using Pharmacy.Data;
using Pharmacy.UI;
using Pharmacy.UI.Menus;

namespace Pharmacy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Create Services
            var categoryService = new CategoryService();

            var medicineService = new MedicineService();

            var supplierService = new SupplierService();
            
            var pharmacySeeder = new PharmacySeeder(supplierService,categoryService,medicineService);
            pharmacySeeder.Seed();

            var batchService = new BatchService();

            var purchaseService = new PurchaseService(
                supplierService,
                medicineService,
                batchService);

            var saleService = new SaleService(
                medicineService,
                batchService);

            // 2. Create UI Objects
            var categoryUI = new CategoryUI(categoryService);

            var medicineUI = new MedicineUI(medicineService,categoryUI);

            var supplierUI = new SupplierUI(supplierService);

            var purchaseUI = new PurchaseUI(
                purchaseService,
                supplierService,
                medicineService);

            var saleUI = new SaleUI(
                saleService,
                medicineService,
                batchService);

            // 3. Create Main Menu
            var mainMenu = new MainMenu(
                categoryUI,
                medicineUI,
                supplierUI,
                purchaseUI,
                saleUI);

            // 4. Start Application
            mainMenu.Run();
        }
    }
}