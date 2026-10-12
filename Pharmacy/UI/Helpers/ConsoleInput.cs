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

                Console.WriteLine("لا يمكن ترك القيمة فارغة.");
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

                Console.WriteLine("أدخل رقمًا صحيحًا غير سالب.");
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

                Console.WriteLine("أدخل رقمًا عشريًا غير سالب.");
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

                Console.WriteLine("أدخل تاريخًا صحيحًا.");
            }
        }
    }
}