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
                        Console.WriteLine("اختيار غير صحيح، حاول مرة أخرى.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("اضغط أي مفتاح للمتابعة...");
                Console.ReadKey();
            }
        }

        public void AddCategory()
        {
            string name = ConsoleInput.ReadRequiredText("أدخل اسم الفئة: ");

            _categoryService.Add(name);

            Console.WriteLine("تمت إضافة الفئة بنجاح.");
        }

        public void SearchCategory()
        {
            string name = ConsoleInput.ReadRequiredText(
                "أدخل اسم الفئة للبحث: ");


            Category? category = _categoryService.SearchByName(name);

            if (category is null)
            {
                Console.WriteLine("لم يتم العثور على الفئة.");
                return;
            }

            Console.WriteLine($"معرف الفئة: {category.Id}");
            Console.WriteLine($"اسم الفئة: {category.Name}");
        }

        public Category GetCategory()
        {
            IReadOnlyList<Category> categories = _categoryService.GetAll();

            if (categories.Count == 0)
            {
                throw new InvalidOperationException(
                    "لا توجد فئات مسجلة. أضف فئة أولًا.");
            }

            Console.WriteLine("======= اختر الفئة =======");

            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");
            }

            while (true)
            {
                Console.Write("اختر رقم الفئة: ");

                string input = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(input, out int choice)
                    && choice >= 1
                    && choice <= categories.Count)
                {
                    return categories[choice - 1];
                }

                Console.WriteLine("أدخل رقمًا صحيحًا من القائمة.");
            }
        }
    }

}