
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Services
{
    public class SupplierService
    {
      //  private readonly IIdGenerator _idGenerator;
        private readonly List<Supplier> _suppliers = new();
        private int _currentId =0;
        /*
        public SupplierService(IIdGenerator idGenerator)
        {
            _idGenerator = idGenerator
                ?? throw new ArgumentNullException(nameof(idGenerator));
        }
        */
        // =========================
        // Add
        // =========================

        public void Add(
            string name,
            string phone,
            string? email = null,
            string? address = null,
            string? taxNumber = null,
            string? commercialReg = null)
        {
            name = ValidateRequired(name, "اسم المورد");
            phone = ValidatePhone(phone);

            if (_suppliers.Any(s =>
                string.Equals(
                    s.Name,
                    name,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "يوجد مورد بهذا الاسم مسبقًا.");
            }

            if (_suppliers.Any(s =>
                string.Equals(
                    s.Phone,
                    phone,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "يوجد مورد بهذا الرقم مسبقًا.");
            }

            var supplier = new Supplier(
                ++_currentId,
                name,
                phone,
                email,
                address,
                taxNumber,
                commercialReg);

            _suppliers.Add(supplier);
        }

        // =========================
        // Update
        // =========================

        public void Update(
            int id,
            string? name = null,
            string? phone = null,
            string? email = null,
            string? address = null,
            string? taxNumber = null,
            string? commercialReg = null)
        {
            var supplier = GetById(id);

            if (supplier is null)
                throw new KeyNotFoundException(
                    $"لا يوجد مورد بالرقم {id}.");

            if (name is not null)
            {
                name = ValidateRequired(name, "اسم المورد");

                if (_suppliers.Any(s =>
                    s.Id != id &&
                    string.Equals(
                        s.Name,
                        name,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException(
                        "يوجد مورد آخر بهذا الاسم مسبقًا.");
                }

                supplier.UpdateName(name);
            }

            if (phone is not null)
            {
                phone = ValidatePhone(phone);

                if (_suppliers.Any(s =>
                    s.Id != id &&
                    string.Equals(
                        s.Phone,
                        phone,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException(
                        "يوجد مورد آخر بهذا الرقم مسبقًا.");
                }

                supplier.UpdatePhone(phone);
            }

            if (email is not null)
                supplier.UpdateEmail(email);

            if (address is not null)
                supplier.UpdateAddress(address);

            if (taxNumber is not null)
                supplier.UpdateTaxNumber(taxNumber);

            if (commercialReg is not null)
                supplier.UpdateCommercialRegistrationNumber(
                    commercialReg);
        }

        // =========================
        // Delete
        // =========================

        public bool Delete(int id)
        {
            var supplier = GetById(id);

            if (supplier is null)
                return false;

            _suppliers.Remove(supplier);

            return true;
        }

        // =========================
        // Get By Id
        // =========================

        public Supplier? GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Id must be greater than zero.");

            return _suppliers.FirstOrDefault(
                s => s.Id == id);
        }

        // =========================
        // Get All
        // =========================

        public IReadOnlyList<Supplier> GetAll()
        {
            return _suppliers.AsReadOnly();
        }

        // =========================
        // Search By Name
        // =========================

        public IReadOnlyList<Supplier> SearchByName(string name)
        {
            name = ValidateRequired(name, "اسم المورد");

            return _suppliers
                .Where(s =>
                    s.Name.Contains(
                        name,
                        StringComparison.OrdinalIgnoreCase))
                .ToList()
                .AsReadOnly();
        }
        public Supplier? Search(string name)
        {
            name = ValidateRequired(name, "اسم المورد").Trim();

            return _suppliers
                .FirstOrDefault(s =>
                    s.Name.Contains(
                        name,
                        StringComparison.OrdinalIgnoreCase));
        }

        // =========================
        // Search By Phone
        // =========================

        public IReadOnlyList<Supplier> SearchByPhone(string phone)
        {
            phone = ValidatePhone(phone);

            return _suppliers
                .Where(s => s.Phone.Contains(phone))
                .ToList()
                .AsReadOnly();
        }

        // =========================
        // Helper Methods
        // =========================

        private static string ValidateRequired(
            string value,
            string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    $"{fieldName} لا يمكن أن يكون فارغًا.",
                    nameof(value));
            }

            return value.Trim();
        }

        private static string ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException(
                    "رقم الهاتف لا يمكن أن يكون فارغًا.",
                    nameof(phone));
            }

            phone = phone.Trim();

            // يسمح بعلامة + في البداية ثم أرقام فقط
            var digits = phone.StartsWith('+')
                ? phone[1..]
                : phone;

            if (digits.Length == 0 ||
                !digits.All(char.IsDigit))
            {
                throw new ArgumentException(
                    "رقم الهاتف غير صحيح.",
                    nameof(phone));
            }

            return phone;
        }
    }
}