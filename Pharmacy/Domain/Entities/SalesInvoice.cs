using Pharmacy.Domain.Enums;

namespace Pharmacy.Domain.Entities
{
    //حاوية لعملية البيع كاملة.
    public class SalesInvoice : Invoice
    {

        private readonly List<SalesInvoiceItem> _salesInvoiceItems = new();
        public IReadOnlyList<SalesInvoiceItem> Items => _salesInvoiceItems;

        // public decimal TotalPrice => _salesInvoiceItems.sum(item=> item.TotalPrice);
        public SalesInvoice(int id, string invoiceNumber, PaymentMethod paymentMethod)
         : base(id, invoiceNumber, paymentMethod) { }

        public override decimal TotalPrice =>_salesInvoiceItems.Sum(item => item.TotalPrice);
        public void AddItem(SalesInvoiceItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            _salesInvoiceItems.Add(item);
        }

    }
}