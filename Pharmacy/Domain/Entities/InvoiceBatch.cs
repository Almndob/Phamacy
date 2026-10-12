using System;

namespace Pharmacy.Domain.Entities
{

    public abstract class InvoiceBatch
    {
        public int Id { get; private set; }
        public int Quantity { get; private set; }
        public Batch Batch { get; private set; }

        protected InvoiceBatch(int id, int quantity, Batch batch)
        {
            ArgumentNullException.ThrowIfNull(batch);
            if (id <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "يجب أن يكون المعرّف أكبر من الصفر.");
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "ادخل كمية موجبه اكبر من الصفر");
            Id = id;
            Quantity = quantity;
            Batch = batch;
        }
    }
}