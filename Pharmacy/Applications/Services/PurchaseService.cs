
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Enums;

namespace Pharmacy.Application.Services
{
    public class PurchaseService
    {
        private int _currentId = 0;
        private int _currentInvoiceId = 0;
        private int _currentItemId = 0;
        private int _currentInvoiceBatchId = 0;

        private readonly SupplierService _supplierService;
        private readonly MedicineService _medicineService;
        private readonly BatchService _batchService;

        private readonly List<PurchaseInvoice> _purchaseInvoices = new();

        public PurchaseService(

            SupplierService supplierService,
            MedicineService medicineService,
            BatchService batchService)
        {
            ++_currentId;
            _supplierService = supplierService
                ?? throw new ArgumentNullException(nameof(supplierService));

            _medicineService = medicineService
                ?? throw new ArgumentNullException(nameof(medicineService));

            _batchService = batchService
                ?? throw new ArgumentNullException(nameof(batchService));
        }



        public PurchaseInvoice CreatePurchase(
            string invoiceNumber,
            PaymentMethod paymentMethod,
            int supplierId,
            int medicineId,
            string batchNumber,
            int quantity,
            DateTime productionDate,
            DateTime expiryDate,
            decimal purchasePrice)
        {
            // 1. التحقق من رقم الفاتورة
            if (string.IsNullOrWhiteSpace(invoiceNumber))
            {
                throw new ArgumentException(
                    "Invoice number cannot be empty.",
                    nameof(invoiceNumber));
            }

            invoiceNumber = invoiceNumber.Trim();

            // 2. منع تكرار رقم الفاتورة
            bool invoiceExists = _purchaseInvoices.Any(invoice =>
                string.Equals(
                    invoice.InvoiceNumber,
                    invoiceNumber,
                    StringComparison.OrdinalIgnoreCase));

            if (invoiceExists)
            {
                throw new InvalidOperationException(
                    $"Invoice number '{invoiceNumber}' already exists.");
            }

            // 3. التحقق من معرف المورد
            if (supplierId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(supplierId),
                    "Supplier ID must be greater than zero.");
            }

            // 4. التحقق من معرف الدواء
            if (medicineId <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(medicineId),
                    "Medicine ID must be greater than zero.");
            }

            // 5. الحصول على المورد المسجل
            Supplier? supplier = _supplierService.GetById(supplierId);

            if (supplier is null)
            {
                throw new KeyNotFoundException(
                    $"Supplier with ID {supplierId} was not found.");
            }

            // 6. الحصول على الدواء المسجل
            Medicine? medicine = _medicineService.GetById(medicineId);

            if (medicine is null)
            {
                throw new KeyNotFoundException(
                    $"Medicine with ID {medicineId} was not found.");
            }



            // 8. إنشاء فاتورة الشراء
            var invoice = new PurchaseInvoice(
                ++_currentInvoiceId,
                invoiceNumber,
                paymentMethod,
                supplier);

            // 9. إنشاء الدفعة وتسجيلها في BatchService
            Batch batch = _batchService.AddBatch(
                batchNumber,
                medicine,
                supplier,
                quantity,
                productionDate,
                expiryDate,
                purchasePrice);

            // 10. إنشاء بند الشراء
            var item = new PurchaseInvoiceItem(
                ++_currentItemId,
                quantity,
                purchasePrice,
                medicine);

            // 11. إنشاء الربط بين بند الشراء والدفعة
            var invoiceBatch = new PurchaseInvoiceBatch(
                ++_currentInvoiceBatchId,
                quantity,
                batch);

            item.AddBatch(invoiceBatch);

            // 12. إضافة البند إلى الفاتورة
            invoice.AddItem(item);

            // 13. تسجيل الفاتورة
            _purchaseInvoices.Add(invoice);

            return invoice;
        }

        public PurchaseInvoice? GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Invoice ID must be greater than zero.");
            }

            return _purchaseInvoices.FirstOrDefault(
                invoice => invoice.Id == id);
        }

        public PurchaseInvoice? GetByInvoiceNumber(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber))
            {
                throw new ArgumentException(
                    "Invoice number cannot be empty.",
                    nameof(invoiceNumber));
            }

            invoiceNumber = invoiceNumber.Trim();

            return _purchaseInvoices.FirstOrDefault(invoice =>
                string.Equals(
                    invoice.InvoiceNumber,
                    invoiceNumber,
                    StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<PurchaseInvoice> GetAll()
        {
            return _purchaseInvoices.AsReadOnly();
        }
    }
}