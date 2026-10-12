namespace Pharmacy.Domain.Entities
{
    public class PurchaseInvoiceBatch : InvoiceBatch
    {
        public PurchaseInvoiceBatch(int id,int quantity, Batch batch)
            : base(id,quantity, batch)
        {
        }
    }
}