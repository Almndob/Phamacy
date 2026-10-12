using System;

namespace Pharmacy.Application.Abstraction
{
    // تستخدم لكي يتم توليد قيمة id
    //  لجميل الكلاسات تلقائي
    public interface IIdGenerator
    {
        int GetNextId();
    }
}