namespace Pharmacy.Domain.Enums
{


    /// <summary>
    /// أشكال الجرعات الدوائية — enum يمنع الأخطاء الإملائية ويسهّل الفلترة.
    /// </summary>
    public enum DosageForm
    {
        Tablet,        // أقراص
        Capsule,       // كبسولات
        Syrup,         // شراب
        Injection,     // حقن
        Cream,         // كريم
        Ointment,      // مرهم
        Drops,         // قطّارات
        Inhaler,       // استنشاق
        Sachet,        // أكياس (ساشيه)
        Suppository,   // تحميلة
        Spray,         // بخاخ
        Gel,           // جل
        Solution,      // محلول
        Patch          // لصقة
    }

}