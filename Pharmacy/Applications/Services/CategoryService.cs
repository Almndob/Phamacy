using Pharmacy.Application.Abstraction;
using Pharmacy.Domain.Entities;

namespace Pharmacy.Application.Services
{

    public class CategoryService
    {
        //  private readonly IIdGenerator _idGenerator;
        private readonly List<Category> _categories = new();
        private int _currentId = 0;

        //   public CategoryService(IIdGenerator idGenerator) => _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));

        public void Add(string name)
        {
            name = ValidateName(name);
            if (IsExists(name))
            {

                throw new ArgumentException("This Category is Already Exists.");

            }

            var category = new Category(++_currentId, name);
            _categories.Add(category);

        }

        public Category? GetById(int id)
        {

            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            Category? category = _categories.FirstOrDefault(c => c.Id == id);
            return category;

        }

        public IReadOnlyList<Category> GetAll() => _categories.AsReadOnly();

        public Category? SearchByName(string name)
        {
            name = ValidateName(name);

            var category = _categories.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

            return category;
        }

        //منع تكرار اسم التصنيف
        private bool IsExists(string name) => SearchByName(name) is not null;

        private static string ValidateName(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category Name cannot be Empty.");

            return name.Trim();
        }




    }
}