namespace Pharmacy.Domain.Entities
{

    public class Batch
    {
        public int Id { get; private set; }
        public string BatchNumber { get; private set; } = string.Empty;
        //Supplier supplier;
        public Medicine Medicine { get; private set; }
        public Supplier Supplier { get; private set; }

        public int Quantity { get; private set; }
        public DateTime ProductionDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public decimal PurchasePrice { get; private set; }

        public Batch(int id, string batchNumber, Medicine medicine, Supplier supplier, int quantity, DateTime productionDate, DateTime expiryDate, decimal purchasePrice)
        {
            ArgumentNullException.ThrowIfNull(medicine);
            ArgumentNullException.ThrowIfNull(supplier);
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "الكمية يجب أن تكون أكبر من الصفر");
            if (productionDate > DateTime.Today)
                throw new ArgumentException("تاريخ الإنتاج لا يمكن أن يكون في المستقبل.");
            if (expiryDate <= DateTime.Today)
                throw new ArgumentException("تاريخ الانتهاء يجب أن يكون بعد اليوم.");
            if (productionDate >= expiryDate)
                throw new ArgumentException("تاريخ الإنتاج يجب أن يسبق تاريخ الانتهاء.");
            if (purchasePrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(purchasePrice), "السعر يجب أن يكون أكبر من الصفر");
            Id = id;
            BatchNumber = ValidateBatchNumber(batchNumber);
            Medicine = medicine;
            Supplier = supplier;
            Quantity = quantity;
            ProductionDate = productionDate;
            ExpiryDate = expiryDate;
            PurchasePrice = purchasePrice;
        }

        public void AddStock(int quantity)
        {
            if (IsInvalidQuantity(quantity))
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "الكمية يجب ان تكون اكبر من الصفر");
            }
            this.Quantity += quantity;
        }

        public void ReduceStock(int quantity)
        {
            if (IsInvalidQuantity(quantity))
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "الكمية يجب ان تكون اكبر من الصفر");
            }
            if (quantity > Quantity)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "الكمية غير كافيه ");
            }
            Quantity -= quantity;
        }

        public bool IsExpired() => ExpiryDate <= DateTime.Today;
        private static bool IsInvalidQuantity(int quantity) => quantity <= 0;

        private static string ValidateBatchNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("لايجب ان يكون فارغ");
            }
            var trimmed = value.Trim();
            return trimmed;
        }
    }

}