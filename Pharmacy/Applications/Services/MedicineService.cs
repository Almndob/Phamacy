
using Pharmacy.Domain.Entities;
using Pharmacy.Domain.Enums;

namespace Pharmacy.Application.Services
{

    public class MedicineService
    {
        private readonly List<Medicine> _medicines = new();


        private int _currentId = 0;


        public void Add(string name, string company, string barcode,
                     string activeIngredient, string strength,
                        decimal purchasePrice, decimal salePrice,
                        int reorderLevel, DosageForm dosageForm, Category category)
        {
            if (IsExistsBarcode(barcode))
                throw new ArgumentException("Barcode is aleardy exists.");

            if (IsExistsName(name))
                throw new ArgumentException("Name is already exists.");
            var medicine = new Medicine(++_currentId, name, company, barcode, activeIngredient, strength, purchasePrice, salePrice, reorderLevel, dosageForm, category);

            _medicines.Add(medicine);

        }


        public Medicine? GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }
            return _medicines.FirstOrDefault(m => m.Id == id);
        }

        public List<Medicine> SearchMedicineByName(string name)
        {
            ValidateName(name);
            return _medicines.Where(
                m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }
        /*
        private bool HasMedicineWithBarcode(string barcode)
        {
            var bar = ValidateBarcode(barcode);
            return _medicines.Any(m => m.Barcode == bar);
        }*/

        public Medicine? FindMedicineByBarcode(string barcode)
        {
            var bar = ValidateBarcode(barcode);

            return _medicines.FirstOrDefault(m => m.Barcode == bar);
        }


        public IReadOnlyList<Medicine> GetAll() => _medicines.AsReadOnly();
        // use it for remove a medicine
            public Medicine? FindMedicineByExactName(string name)
            {
                ValidateName(name);

                return _medicines.FirstOrDefault(
                        m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                            );
            }


        public void RemoveMedicine(int id)
        {
            Medicine? medicine = GetById(id);

            if (medicine is null)
            {
                throw new InvalidOperationException(
                        $"There is no medicine with this ID: {id}.");
            }
            _medicines.Remove(medicine);
        }

        // ______helper method _________


        private bool IsExistsBarcode(string barcode)
        {
            string validatedBarcode = ValidateBarcode(barcode);

            return _medicines.Any(m =>
                m.Barcode.Equals(
                    validatedBarcode,
                    StringComparison.OrdinalIgnoreCase));
        }

        private bool IsExistsName(string name)
        {
            ValidateName(name);

            string validatedName = name.Trim();

            return _medicines.Any(m =>
                m.Name.Equals(
                    validatedName,
                    StringComparison.OrdinalIgnoreCase));
        }



        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty or null", nameof(name));
            }
        }

        private static string ValidateBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                throw new ArgumentException("Barcode cannot be empty or null", nameof(barcode));
            }

            return barcode.Trim();
        }


    }
}