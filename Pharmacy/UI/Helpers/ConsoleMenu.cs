
namespace Pharmacy.UI.Helpers
{
    public static class ConsoleMenu
    {
        public static void ShowMenu(string title, params string[] options)
        {
            Console.WriteLine("================================");
            Console.WriteLine($"=== {title} ===");
            Console.WriteLine("================================");
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            Console.Write("Choose an option: ");
        }
    }
}