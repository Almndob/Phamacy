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

        // عرض قائمة إدارة الموردين
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

        // إضافة مورد
        public void AddSupplier()
        {
            Console.WriteLine("========== إضافة مورد جديد ==========");

            string name = ConsoleInput.ReadRequiredText(
                "اسم المورد: ");

            string phone = ConsoleInput.ReadRequiredText(
                "رقم الهاتف: ");

            Console.Write("البريد الإلكتروني (اختياري): ");
            string? email = Console.ReadLine();

            Console.Write("العنوان (اختياري): ");
            string? address = Console.ReadLine();

            Console.Write("الرقم الضريبي (اختياري): ");
            string? taxNumber = Console.ReadLine();

            Console.Write("رقم السجل التجاري (اختياري): ");
            string? commercialReg = Console.ReadLine();

            _supplierService.Add(
                name,
                phone,
                email,
                address,
                taxNumber,
                commercialReg);

            Console.WriteLine();
            Console.WriteLine("تمت إضافة المورد بنجاح.");
        }

        // البحث عن مورد بالاسم أو رقم الهاتف
        public void SearchSupplier()
        {
            Console.WriteLine("========== البحث عن مورد ==========");
            Console.WriteLine("1. البحث بالاسم");
            Console.WriteLine("2. البحث برقم الهاتف");

            Console.Write("اختر طريقة البحث: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    {
                        string name = ConsoleInput.ReadRequiredText(
                            "أدخل اسم المورد: ");

                        IReadOnlyList<Supplier> suppliers =
                            _supplierService.SearchByName(name);

                        DisplaySuppliers(suppliers);
                        break;
                    }

                case "2":
                    {
                        string phone = ConsoleInput.ReadRequiredText(
                            "أدخل رقم الهاتف: ");

                        IReadOnlyList<Supplier> suppliers =
                            _supplierService.SearchByPhone(phone);

                        DisplaySuppliers(suppliers);
                        break;
                    }

                default:
                    Console.WriteLine("اختيار غير صحيح.");
                    break;
            }
        }

        // تعديل بيانات مورد
        public void UpdateSupplier()
        {
            IReadOnlyList<Supplier> suppliers =
                _supplierService.GetAll();

            if (suppliers.Count == 0)
            {
                Console.WriteLine("لا يوجد موردون لتعديل بياناتهم.");
                return;
            }

            DisplaySuppliers(suppliers);

            int id = ConsoleInput.ReadNonNegativeInteger(
                "أدخل معرف المورد المراد تعديله: ");

            Supplier? supplier = _supplierService.GetById(id);

            if (supplier is null)
            {
                Console.WriteLine("لم يتم العثور على المورد.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("اترك الحقل فارغًا للاحتفاظ بالقيمة الحالية.");

            Console.Write($"اسم المورد [{supplier.Name}]: ");
            string? name = Console.ReadLine();

            Console.Write($"رقم الهاتف [{supplier.Phone}]: ");
            string? phone = Console.ReadLine();

            Console.Write($"البريد الإلكتروني [{supplier.Email ?? "غير محدد"}]: ");
            string? email = Console.ReadLine();

            Console.Write($"العنوان [{supplier.Address ?? "غير محدد"}]: ");
            string? address = Console.ReadLine();

            Console.Write($"الرقم الضريبي [{supplier.TaxNumber ?? "غير محدد"}]: ");
            string? taxNumber = Console.ReadLine();

            Console.Write(
                $"السجل التجاري [{supplier.CommercialRegistrationNumber ?? "غير محدد"}]: ");
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
            Console.WriteLine("تم تحديث بيانات المورد بنجاح.");
        }

        // حذف مورد
        public void DeleteSupplier()
        {
            IReadOnlyList<Supplier> suppliers =
                _supplierService.GetAll();

            if (suppliers.Count == 0)
            {
                Console.WriteLine("لا يوجد موردون للحذف.");
                return;
            }

            DisplaySuppliers(suppliers);

            int id = ConsoleInput.ReadNonNegativeInteger(
                "أدخل معرف المورد المراد حذفه: ");

            Supplier? supplier = _supplierService.GetById(id);

            if (supplier is null)
            {
                Console.WriteLine("لم يتم العثور على المورد.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"المورد: {supplier.Name}");
            Console.Write("هل أنت متأكد من الحذف؟ (y/n): ");

            string confirmation = Console.ReadLine() ?? string.Empty;

            if (!confirmation.Equals(
                    "y",
                    StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("تم إلغاء عملية الحذف.");
                return;
            }

            bool deleted = _supplierService.Delete(id);

            Console.WriteLine(
                deleted
                    ? "تم حذف المورد بنجاح."
                    : "تعذر حذف المورد.");
        }

        // عرض جميع الموردين
        public void DisplayAll()
        {
            IReadOnlyList<Supplier> suppliers =
                _supplierService.GetAll();

            Console.WriteLine("========== جميع الموردين ==========");

            DisplaySuppliers(suppliers);
        }

        // عرض قائمة من الموردين
        private void DisplaySuppliers(
            IReadOnlyList<Supplier> suppliers)
        {
            if (suppliers.Count == 0)
            {
                Console.WriteLine("لا توجد نتائج للعرض.");
                return;
            }

            foreach (Supplier supplier in suppliers)
            {
                Console.WriteLine("----------------------------------------");
                DisplaySupplierDetails(supplier);
            }

            Console.WriteLine("----------------------------------------");
        }

        // عرض تفاصيل مورد واحد
        private void DisplaySupplierDetails(Supplier supplier)
        {
            Console.WriteLine($"المعرف             : {supplier.Id}");
            Console.WriteLine($"اسم المورد         : {supplier.Name}");
            Console.WriteLine($"رقم الهاتف         : {supplier.Phone}");
            Console.WriteLine($"البريد الإلكتروني  : {supplier.Email ?? "غير محدد"}");
            Console.WriteLine($"العنوان            : {supplier.Address ?? "غير محدد"}");
            Console.WriteLine($"الرقم الضريبي      : {supplier.TaxNumber ?? "غير محدد"}");
            Console.WriteLine(
                $"السجل التجاري      : {supplier.CommercialRegistrationNumber ?? "غير محدد"}");
        }

        // تشغيل قائمة الموردين
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
                                "اختيار غير صحيح، حاول مرة أخرى.");
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"خطأ في البيانات: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"تعذر تنفيذ العملية: {ex.Message}");
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine($"لم يتم العثور على العنصر: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("اضغط أي مفتاح للمتابعة...");
                Console.ReadKey();
            }
        }
    }

}