namespace Pharmacy.Domain.Entities
{


    public abstract class InvoiceItem
    {
        public int Id { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public Medicine Medicine { get; private set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        private readonly List<InvoiceBatch> _invoiceBatches = new();

        public IReadOnlyList<InvoiceBatch> InvoiceBatches => _invoiceBatches;

        protected InvoiceItem(
             int id,
             int quantity,
             decimal unitPrice,
             Medicine medicine)
        {
            ArgumentNullException.ThrowIfNull(medicine);
            if (id <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "المعرف يجب أن يكون أكبر من الصفر.");

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "ادخل كمية موجبة أكبر من الصفر.");

            if (unitPrice <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(unitPrice),
                    "السعر يجب أن يكون موجبًا وأكبر من الصفر.");
            Id = id;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Medicine = medicine;
        }

       // public abstract void AddBatch(InvoiceBatch invoiceBatch);

        protected void AddInvoiceBatch(InvoiceBatch invoiceBatch)
        {
            ArgumentNullException.ThrowIfNull(invoiceBatch);

            int allocatedQuantity =
                _invoiceBatches.Sum(x => x.Quantity);

            if (allocatedQuantity + invoiceBatch.Quantity > Quantity)
                throw new InvalidOperationException(
                    "لا يمكن أن تتجاوز كميات الدفعات كمية الصنف في الفاتورة.");

            _invoiceBatches.Add(invoiceBatch);
        }
    }
}