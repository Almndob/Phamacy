namespace Pharmacy.Domain.Entities
{
    public class SalesInvoiceItem : InvoiceItem
    {


        public SalesInvoiceItem(
            int id,
            int quantity,
            decimal unitPrice,
            Medicine medicine)
            : base(id, quantity, unitPrice, medicine) { }

        public void AddBatch(InvoiceBatch invoiceBatch)
        {
            if (invoiceBatch is not SalesInvoiceBatch)
                throw new ArgumentException(
                    "Sales invoice item requires SalesInvoiceBatch.",
                    nameof(invoiceBatch));

            AddInvoiceBatch(invoiceBatch);
        }




    }
}