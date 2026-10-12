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
            string name = ConsoleInput.ReadRequiredText("اسم الدواء: ");
            string company = ConsoleInput.ReadRequiredText("الشركة المصنعة: ");
            string barcode = ConsoleInput.ReadRequiredText("الباركود: ");
            string activeIngredient = ConsoleInput.ReadRequiredText("المادة الفعالة: ");
            string strength = ConsoleInput.ReadRequiredText("التركيز: ");

            decimal purchasePrice = ConsoleInput.ReadDecimal("سعر الشراء: ");
            decimal salePrice = ConsoleInput.ReadDecimal("سعر البيع: ");

            int reorderLevel = ConsoleInput.ReadNonNegativeInteger(
                "مستوى إعادة الطلب: ");

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

            Console.WriteLine("تمت إضافة الدواء بنجاح.");
        }

        public void SearchMedicine()
        {
            string name = ConsoleInput.ReadRequiredText(
                "البحث عن الدواء، أدخل الاسم: ");

            List<Medicine> medicines =
                _medicineService.SearchMedicineByName(name);

            if (medicines.Count == 0)
            {
                Console.WriteLine("لم يتم العثور على أي دواء بهذا الاسم.");
                return;
            }

            DisplayMedicine(medicines);
        }

        public void DisplayMedicine(List<Medicine> medicines)
        {
            if (medicines.Count == 0)
            {
                Console.WriteLine("لا توجد أدوية للعرض.");
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
            Console.WriteLine($"المعرف            : {medicine.Id}");
            Console.WriteLine($"الاسم             : {medicine.Name}");
            Console.WriteLine($"الشركة المصنعة    : {medicine.Company}");
            Console.WriteLine($"الباركود          : {medicine.Barcode}");
            Console.WriteLine($"المادة الفعالة     : {medicine.ActiveIngredient}");
            Console.WriteLine($"التركيز            : {medicine.Strength}");
            Console.WriteLine($"سعر الشراء         : {medicine.PurchasePrice:C}");
            Console.WriteLine($"سعر البيع          : {medicine.SalePrice:C}");
            Console.WriteLine($"مستوى إعادة الطلب  : {medicine.ReorderLevel}");
            Console.WriteLine($"الشكل الدوائي      : {medicine.DosageForm}");
            Console.WriteLine($"الفئة              : {medicine.Category?.Name}");
        }

        public void DisplayAll()
        {
            IReadOnlyList<Medicine> medicines = _medicineService.GetAll();

            Console.WriteLine("========== عرض جميع الأدوية ==========");

            DisplayMedicine(medicines.ToList());
        }

        public void RemoveMedicine()
        {
            IReadOnlyList<Medicine> medicines = _medicineService.GetAll();

            if (medicines.Count == 0)
            {
                Console.WriteLine("لا توجد أدوية للحذف.");
                return;
            }

            DisplayMedicine(medicines.ToList());

            int inputId = ConsoleInput.ReadNonNegativeInteger(
                "أدخل معرف الدواء المراد حذفه: ");

            Medicine? medicine = _medicineService.GetById(inputId);

            if (medicine is null)
            {
                Console.WriteLine($"لا يوجد دواء بهذا المعرف: {inputId}");
                return;
            }

            _medicineService.RemoveMedicine(medicine.Id);

            Console.WriteLine("تم حذف الدواء بنجاح.");
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
                        Console.WriteLine("اختيار غير صحيح، حاول مرة أخرى.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("اضغط أي مفتاح للمتابعة...");
                Console.ReadKey();
            }

        }
    }

}