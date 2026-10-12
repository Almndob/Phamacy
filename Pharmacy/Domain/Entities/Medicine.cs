using Pharmacy.Domain.Enums;
namespace Pharmacy.Domain.Entities
{



    public class Medicine : Product
    {
        public string ActiveIngredient { get; private set; } = string.Empty;
        public string Strength { get; private set; } = string.Empty;
        public decimal PurchasePrice { get; private set; }
        public decimal SalePrice { get; private set; }
        public int ReorderLevel { get; private set; }
        public DosageForm DosageForm { get; private set; }
        public Category Category { get; private set; }

        public Medicine(int id, string name, string company, string barcode,
                        string activeIngredient, string strength,
                        decimal purchasePrice, decimal salePrice, int reorderLevel,
                        DosageForm dosageForm, Category category) : base(id, name, company, barcode)
        {
            ArgumentNullException.ThrowIfNull(category);
            ActiveIngredient = ValidateRequired(activeIngredient, nameof(activeIngredient));
            Strength = ValidateRequired(strength, nameof(strength));
            ValidatePrices(purchasePrice, salePrice);
            if (reorderLevel < 0)
                throw new ArgumentOutOfRangeException(nameof(reorderLevel), "مستوى إعادة الطلب لا يمكن أن يكون سالبًا.");

            PurchasePrice = purchasePrice;
            SalePrice = salePrice;
            ReorderLevel = reorderLevel;
            DosageForm = dosageForm;
            Category = category;
        }

        /// <summary>تحديث سعر البيع مع الحفاظ على القواعد: > 0 و >= سعر الشراء.</summary>
        public void UpdateSalePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "سعر البيع يجب أن يكون أكبر من صفر.");
            if (newPrice < PurchasePrice)
                throw new InvalidOperationException("سعر البيع لا يمكن أن يكون أقل من سعر الشراء. استخدم UpdatePrices لتحديث الاثنين معًا.");

            SalePrice = newPrice;
        }

        /// <summary>تحديث سعر الشراء. إن تجاوز سعر البيع الحالي يُرفض — استخدم UpdatePrices.</summary>
        public void UpdatePurchasePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), "سعر الشراء يجب أن يكون أكبر من صفر.");
            if (newPrice > SalePrice)
                throw new InvalidOperationException("سعر الشراء الجديد يتجاوز سعر البيع الحالي. استخدم UpdatePrices لتحديث الاثنين معًا.");

            PurchasePrice = newPrice;
        }

        /// <summary>تحديث السعرين معًا — الطريق الآمن الوحيد لتغيير العلاقة بينهما.</summary>
        public void UpdatePrices(decimal newPurchasePrice, decimal newSalePrice)
        {
            ValidatePrices(newPurchasePrice, newSalePrice);
            PurchasePrice = newPurchasePrice;
            SalePrice = newSalePrice;
        }

        public void UpdateReorderLevel(int newLevel)
        {
            if (newLevel < 0)
                throw new ArgumentOutOfRangeException(nameof(newLevel), "مستوى إعادة الطلب لا يمكن أن يكون سالبًا.");

            ReorderLevel = newLevel;
        }
        public void UpdateCategory(Category category)
        {
            ArgumentNullException.ThrowIfNull(category);

            Category = category;
        }


        /// <summary>قرار مستندي: المخزون الحالي عند أو تحت مستوى إعادة الطلب.</summary>
      //  public bool IsLowStock() => TotalStock <= ReorderLevel;


        private static void ValidatePrices(decimal purchasePrice, decimal salePrice)
        {
            if (purchasePrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(purchasePrice), "سعر الشراء يجب أن يكون أكبر من صفر.");
            if (salePrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(salePrice), "سعر البيع يجب أن يكون أكبر من صفر.");
            if (salePrice < purchasePrice)
                throw new InvalidOperationException("سعر البيع لا يمكن أن يكون أقل من سعر الشراء.");
        }
    }
}