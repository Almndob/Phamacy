using Pharmacy.Domain.Enums;

namespace Pharmacy.Domain.Entities
{

    public abstract class Invoice
    {
        public int Id { get; private set; }
        public string InvoiceNumber { get; private set; } = string.Empty;
        public DateTime InvoiceDate { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }

        public abstract decimal TotalPrice { get; }
        protected Invoice(int id, string invoiceNumber,  PaymentMethod paymentMethod)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (string.IsNullOrWhiteSpace(invoiceNumber))
                throw new ArgumentException(
                    "رقم الفاتورة لا يمكن أن يكون فارغًا.",
                    nameof(invoiceNumber));
            Id = id;
            InvoiceNumber = invoiceNumber.Trim();
            InvoiceDate = DateTime.Now;
            PaymentMethod = paymentMethod;
        }


    }
}