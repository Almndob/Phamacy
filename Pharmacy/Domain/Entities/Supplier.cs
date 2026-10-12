namespace Pharmacy.Domain.Entities
{


    public class Supplier
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string? Email { get; private set; }
        public string? Address { get; private set; }
        public string? TaxNumber { get; private set; }
        public string? CommercialRegistrationNumber { get; private set; }



        public Supplier(int id,string name, string phone, string? email, string? address, string? taxNumber, string? commercialRegistrationNumber)
        {
            if(id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            Id = id;
            Name = ValidateRequired(name);
            Phone = ValidatePhone(phone);
            Email = ValidateOptional(email);
            Address = ValidateOptional(address);
            TaxNumber = ValidateOptional(taxNumber);
            CommercialRegistrationNumber = ValidateOptional(commercialRegistrationNumber);
        }

        public void UpdateName(string name) => Name = ValidateRequired(name);

        public void UpdatePhone(string phone) => Phone = ValidatePhone(phone);


        public void UpdateEmail(string? email) => Email = ValidateOptional(email);

        public void UpdateAddress(string? address) => Address = ValidateOptional(address);

        public void UpdateTaxNumber(string? taxNumber) => TaxNumber = ValidateOptional(taxNumber);

        public void UpdateCommercialRegistrationNumber(string? cRN) => CommercialRegistrationNumber = ValidateOptional(cRN);



        // ___________Helper Method __________

        private static string ValidateRequired(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "الحقل لا يمكن أن يكون فارغًا.",
                    nameof(value));
            }
            return value.Trim();
        }
        private static string ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException(
                    "رقم الهاتف لا يمكن أن يكون فارغًا.",
                    nameof(phone));

            phone = phone.Trim();

            if (!phone.All(char.IsDigit))
                throw new ArgumentException(
                    "رقم الهاتف يجب أن يحتوي على أرقام فقط.",
                    nameof(phone));

            return phone;
        }

        private static string? ValidateOptional(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return email.Trim();
        }
    }
}