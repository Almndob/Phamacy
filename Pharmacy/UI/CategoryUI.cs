using Pharmacy.Application.Services;
using Pharmacy.Domain.Entities;
using Pharmacy.UI.Helpers;

namespace Pharmacy.UI
{
    public sealed class CategoryUI
    {
        private readonly CategoryService _categoryService;

        public CategoryUI(CategoryService categoryService)
        {
            ArgumentNullException.ThrowIfNull(categoryService);
            _categoryService = categoryService;
        }

        public void ShowMenu()
        {
            ConsoleMenu.ShowMenu(
                "1- Category Management",
                "2- Add Category",
                "3- Search Category",
                "4- Back");
        }

        public void RunMenu()
        {
            while (true)
            {
                Console.Clear();

                ShowMenu();

                string choice = Console.ReadLine() ?? string.Empty;

                switch (choice)
                {
                    case "1":
                        AddCategory();
                        break;

                    case "2":
                        SearchCategory();
                        break;

                    case "3":
                        return;

                    default:
                        Console.WriteLine(
                            "Invalid option. Please try again.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void AddCategory()
        {
            string name = ConsoleInput.ReadRequiredText(
                "Enter category name: ");

            _categoryService.Add(name);

            Console.WriteLine("Category added successfully.");
        }

        public void SearchCategory()
        {
            string name = ConsoleInput.ReadRequiredText(
                "Enter category name to search: ");

            Category? category = _categoryService.SearchByName(name);

            if (category is null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            Console.WriteLine($"Category ID: {category.Id}");
            Console.WriteLine($"Category Name: {category.Name}");
        }

        public Category GetCategory()
        {
            IReadOnlyList<Category> categories =
                _categoryService.GetAll();

            if (categories.Count == 0)
            {
                throw new InvalidOperationException(
                    "No categories are registered. Add a category first.");
            }

            Console.WriteLine("======= Select a Category =======");

            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            }

            while (true)
            {
                Console.Write("Enter category number: ");

                string input = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(input, out int choice)
                    && choice >= 1
                    && choice <= categories.Count)
                {
                    return categories[choice - 1];
                }

                Console.WriteLine(
                    "Please enter a valid number from the list.");
            }
        }
    }
}