using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Enums;

namespace Pharmacy.Application.Services
{
    public sealed class SaleService
    {
        private int _currentId = 0;
        private int _currentInvoiceId = 0;
        private int _currentItemId = 0;
        private int _currentInvoiceBatchId = 0;

        private readonly MedicineService _medicineService;
        private readonly BatchService _batchService;

        private readonly List<SalesInvoice> _saleInvoices = new();

        public SaleService(
            MedicineService medicineService,
            BatchService batchService)
        {
            ++_currentId;
            _medicineService = medicineService
                ?? throw new ArgumentNullException(nameof(medicineService));

            _batchService = batchService
                ?? throw new ArgumentNullException(nameof(batchService));
        }

        public SalesInvoice CreateSale(
            PaymentMethod payment,
            string medicineName,
            string batchNumber,
            int quantity,
            decimal unitPrice)
        {
            // 1. التحقق من اسم الدواء
            if (string.IsNullOrWhiteSpace(medicineName))
            {
                throw new ArgumentException(
                    "Medicine name cannot be empty.",
                    nameof(medicineName));
            }

            medicineName = medicineName.Trim();

            // 2. التحقق من رقم الدفعة
            if (string.IsNullOrWhiteSpace(batchNumber))
            {
                throw new ArgumentException(
                    "Batch number cannot be empty.",
                    nameof(batchNumber));
            }

            batchNumber = batchNumber.Trim();

            // 3. التحقق من الكمية
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Quantity must be greater than zero.");
            }

            // 4. التحقق من سعر البيع
            if (unitPrice <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(unitPrice),
                    "Unit price must be greater than zero.");
            }

            // 5. البحث عن الدواء
            Medicine? medicine =
                _medicineService.FindMedicineByExactName(medicineName);

            if (medicine is null)
            {
                throw new InvalidOperationException(
                    $"Medicine '{medicineName}' was not found.");
            }

            // 6. البحث عن دفعة الدواء
            Batch? batch =
                _batchService.FindBatchByBatchNumber(medicine, batchNumber);

            if (batch is null)
            {
                throw new InvalidOperationException(
                    $"Batch '{batchNumber}' was not found for medicine '{medicineName}'.");
            }

            // 7. التحقق من صلاحية الدفعة
            if (batch.IsExpired())
            {
                throw new InvalidOperationException(
                    "Cannot sell an expired batch.");
            }

            // 8. التحقق من توفر الكمية
            if (quantity > batch.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock. Available quantity: {batch.Quantity}.");
            }

            // 9. التحقق من سعر البيع المعتمد للدواء
            if (unitPrice != medicine.SalePrice)
            {
                throw new InvalidOperationException(
                    $"The registered sale price is {medicine.SalePrice}.");
            }

            // 10. إنشاء رقم الفاتورة
            string invoiceNumber = GenerateInvoiceNumber();

            // 11. إنشاء فاتورة البيع
            var invoice = new SalesInvoice(
                ++_currentInvoiceId,
                invoiceNumber,
                payment);

            // 12. إنشاء بند البيع
            var saleItem = new SalesInvoiceItem(
                ++_currentItemId,
                quantity,
                unitPrice,
                medicine);

            // 13. إنشاء ربط بين بند البيع والدفعة
            var invoiceBatch = new SalesInvoiceBatch(
                ++_currentInvoiceBatchId,
                quantity,
                batch);

            // 14. ربط الدفعة ببند البيع
            saleItem.AddBatch(invoiceBatch);

            // 15. إضافة بند البيع إلى الفاتورة
            invoice.AddItem(saleItem);

            // 16. خصم الكمية المباعة من المخزون
            batch.ReduceStock(quantity);

            // 17. حفظ الفاتورة في قائمة فواتير البيع
            _saleInvoices.Add(invoice);

            return invoice;
        }

        public SalesInvoice? GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Invoice ID must be greater than zero.");
            }

            return _saleInvoices.FirstOrDefault(s=>s.Id == id);
        }



        private string GenerateInvoiceNumber()
        {
            // مثال: SAL-2026-0001
            string prefix = $"SAL-{DateTime.Now.Year}-";

            int maxSequence = _saleInvoices
                .Where(invoice =>
                    invoice.InvoiceNumber.StartsWith(
                        prefix,
                        StringComparison.OrdinalIgnoreCase))
                .Select(invoice =>
                    invoice.InvoiceNumber.Substring(prefix.Length))
                .Where(number => int.TryParse(number, out _))
                .Select(int.Parse)
                .DefaultIfEmpty(0)
                .Max();

            int nextSequence = maxSequence + 1;

            return $"{prefix}{nextSequence:D4}";
        }

        public SalesInvoice? GetByInvoiceNumber(string invoiceNumber)
        {
            if (string.IsNullOrWhiteSpace(invoiceNumber))
            {
                throw new ArgumentException(
                    "Invoice number cannot be empty.",
                    nameof(invoiceNumber));
            }

            invoiceNumber = invoiceNumber.Trim();

            return _saleInvoices.FirstOrDefault(invoice =>
                string.Equals(
                    invoice.InvoiceNumber,
                    invoiceNumber,
                    StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<SalesInvoice> GetAll()
        {
            return _saleInvoices.AsReadOnly();
        }

    }
}