namespace Pharmacy.Domain.Entities;

public abstract class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Company { get; private set; } = string.Empty;
    public string Barcode { get; private set; } = string.Empty;

    public Product(int id,
                    string name,
                    string company,
                    string barcode)
    {

        if(id <= 0){
            throw new ArgumentException("Id must be greater than zero.");
        }
        Id = id;
        Name = ValidateRequired(name, nameof(name));
        Company = ValidateRequired(company, nameof(company));
        Barcode = ValidateBarcode(barcode);

    }
    public void UpdateName(string newName)
    {
        Name = ValidateRequired(newName, nameof(newName));
    }

    public void UpdateCompany(string newCompany)
    {
        Company = ValidateRequired(newCompany, nameof(newCompany));
    }

    protected static string ValidateRequired(string value, string paraName)
    {

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("This input must be required and not be empty.", paraName);
        }
        var trimmed = value.Trim();
        return trimmed;
    }
    private static string ValidateBarcode(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
        {
            throw new ArgumentException("Barcode is required.");
        }
        var trimmed = barcode.Trim();
        if (trimmed.Length is not (8 or 12 or 13))
        {
            throw new ArgumentException(
                "Barcode must contain 8, 12, or 13 digits.");
        }
        if (!trimmed.All(char.IsDigit))
        {
            throw new ArgumentException("Barcode must be numbers.");
        }
        return trimmed;
    }
}