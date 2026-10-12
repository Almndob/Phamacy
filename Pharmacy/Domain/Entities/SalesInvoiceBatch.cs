namespace Pharmacy.Domain.Entities
{
    //يسجل كم وحدة من Batch استخدمت في هذه الفاتورة
    public class SalesInvoiceBatch : InvoiceBatch
    {
        public SalesInvoiceBatch(int id, int quantity, Batch batch)
            : base(id, quantity, batch)
        {
        }
    }
}