namespace TrialCsharp
{
    public class BasicInput
    {
        public BasicInput(){
        Print.Text("=== C# .NET 8 Input Demo ===");

        Calculator calculator = new Calculator();

        Print.Text("Insert first number: ");
        double number1 = ReadNumber();

        Print.Text("Insert second number: ");
        double number2 = ReadNumber();

        Print.Text($"Add result: {calculator.Add(number1, number2)}");
        Print.Text($"Multiply result: {calculator.Multiply(number1, number2)}");
        Print.Text($"Subtract result: {calculator.Subtract(number1, number2)}");
        Print.Text($"Divide result: {calculator.Divide(number1, number2)}");
        }

        private static double ReadNumber()
        {
            while (true)
            {
                string? input = Console.ReadLine();
                if (double.TryParse(input, out double result))
                {
                    return result;
                }
                else
                {
                    Print.Text("Invalid input. Please enter a valid number:");
                }
            }
        }
    }
}