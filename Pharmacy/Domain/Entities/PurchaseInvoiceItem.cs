namespace Pharmacy.Domain.Entities
{


    public class PurchaseInvoiceItem: InvoiceItem
    {


        public PurchaseInvoiceItem(
            int id,
            int quantity,
            decimal unitPrice,
            Medicine medicine): base(id,quantity,unitPrice,medicine)
        {

        }

        public void AddBatch(InvoiceBatch invoiceBatch)
        {
            if (invoiceBatch is not PurchaseInvoiceBatch)
                throw new ArgumentException(
                    "Purchase invoice item requires PurchaseInvoiceBatch.",
                    nameof(invoiceBatch));

            AddInvoiceBatch(invoiceBatch);
        }



    }
}