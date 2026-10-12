using Pharmacy.Application.Services;
using Pharmacy.Domain.Enums;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Data
{
    public sealed class PharmacySeeder
    {
        private readonly SupplierService _supplierService;
        private readonly CategoryService _categoryService;
        private readonly MedicineService _medicineService;

        public PharmacySeeder(
            SupplierService supplierService,
            CategoryService categoryService,
            MedicineService medicineService)
        {
            _supplierService = supplierService
                ?? throw new ArgumentNullException(nameof(supplierService));

            _categoryService = categoryService
                ?? throw new ArgumentNullException(nameof(categoryService));

            _medicineService = medicineService
                ?? throw new ArgumentNullException(nameof(medicineService));
        }

        public void Seed()
        {
            // 1. إضافة الموردين
            _supplierService.Add(
                "Al-Shifa Pharmaceutical Company",
                "777123456");

            _supplierService.Add(
                "Al-Hayat Medical Supplies",
                "777654321");

            // 2. إضافة التصنيفات
            _categoryService.Add("Painkillers");
            _categoryService.Add("Antibiotics");
            Category? painKillers = _categoryService.SearchByName("Painkillers");
            Category? antibiotics = _categoryService.SearchByName("Antibiotics");
            // 3. إضافة الأدوية
            _medicineService.Add(
                "Panadol",
                "GSK",
                "12345678",
                "Paracetamol",
                "500 mg",
                100m,
                150m,
                10,
                DosageForm.Tablet,
                painKillers);

            _medicineService.Add(
                "Amoxicillin",
                "Pharmaceutical Company",
                "23456789",
                "Amoxicillin",
                "500 mg",
                200m,
                250m,
                5,
                DosageForm.Capsule,
                antibiotics);
        }
    }
}