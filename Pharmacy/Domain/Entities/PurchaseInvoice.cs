using Pharmacy.Domain.Enums;

namespace Pharmacy.Domain.Entities
{

    public class PurchaseInvoice : Invoice
    {
        public Supplier Supplier { get; private set; }

        private readonly List<PurchaseInvoiceItem> _purchaseInvoiceItems = new();
        public IReadOnlyList<PurchaseInvoiceItem> PurchaseInvoiceItems => _purchaseInvoiceItems.AsReadOnly();

        public PurchaseInvoice(int id, string invoiceNumber, PaymentMethod paymentMethod, Supplier supplier)
        : base(id, invoiceNumber, paymentMethod)
        {
            ArgumentNullException.ThrowIfNull(supplier);
            Supplier = supplier;
        }

        public override decimal TotalPrice => _purchaseInvoiceItems.Sum(item => item.TotalPrice);

        public void AddItem(PurchaseInvoiceItem purchaseInvoiceItem)
        {
            ArgumentNullException.ThrowIfNull(purchaseInvoiceItem);
            _purchaseInvoiceItems.Add(purchaseInvoiceItem);
        }

    }
}