namespace Pharmacy.UI.Helpers
{
    public static class ConsoleInput
    {
        public static string ReadRequiredText(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine(
                      "Enter a valid positive decimal number.");
            }          
        }

        public static int ReadNonNegativeInteger(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(input, out int value) && value >= 0)
                    return value;

                //  Console.WriteLine("أدخل رقمًا صحيحًا غير سالب.");
                Console.WriteLine(
                      "Enter a valid positive number.");
            }
        }

        public static decimal ReadDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? string.Empty;

                if (decimal.TryParse(input, out decimal value) && value >= 0)
                    return value;

                   Console.WriteLine(
                      "Enter a valid positive decimal number.");
            }
        }

        public static DateTime ReadDate(string message)
        {
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? string.Empty;

                if (DateTime.TryParse(input, out DateTime value))
                    return value;

                Console.WriteLine(
                    "Invalid date. Use yyyy-MM-dd, e.g. 2027-12-31.");
            }
        }
        
        public static void ShowMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Pause();
        }

        public static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}