namespace Pharmacy.Domain.Entities
{


    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        public Category(int id, string name)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            Id = id;
            Name = ValidatedName(name);
        }


        //__________Helper Method__________

        private string ValidatedName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("الاسم لايمكن ان يكون فارغا او null");
            }
            return name.Trim();
        }

    }
}