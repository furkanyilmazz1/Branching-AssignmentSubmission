// Express Shipping Rate Calculator
// Programmer: Thomas Brown
// Date: March 2024
using System;

namespace ShippingExpress
{
    public class PackageEventArgs : EventArgs
    {
        public double Value { get; set; }
        public bool IsValid { get; set; }
    }

    class ShippingProcessor
    {
        private const double MaxLimit = 50;
        public event EventHandler<PackageEventArgs> ValidationEvent;

        private bool ValidateValue(double value, string dimension)
        {
            var args = new PackageEventArgs { Value = value, IsValid = value <= MaxLimit };
            ValidationEvent?.Invoke(this, args);
            return args.IsValid;
        }

        static void Main(string[] args)
        {
            // Show program introduction
            Console.WriteLine("Welcome to Package Express. Please follow the instructions below.");

            var processor = new ShippingProcessor();
            processor.ValidationEvent += (sender, e) =>
            {
                if (!e.IsValid)
                    Console.WriteLine($"Value {e.Value} exceeds maximum limit of {MaxLimit}.");
            };

            try
            {
                // Get package weight
                Console.WriteLine("Please enter the package weight:");
                if (!double.TryParse(Console.ReadLine(), out double weightValue))
                {
                    Console.WriteLine("Invalid weight input.");
                    return;
                }

                // Validate weight
                if (!processor.ValidateValue(weightValue, "weight"))
                {
                    Console.WriteLine("Package too heavy to be shipped via Package Express. Have a good day.");
                    return;
                }

                // Get package dimensions
                Console.WriteLine("Please enter the package width:");
                if (!double.TryParse(Console.ReadLine(), out double dimW))
                {
                    Console.WriteLine("Invalid width input.");
                    return;
                }

                Console.WriteLine("Please enter the package height:");
                if (!double.TryParse(Console.ReadLine(), out double dimH))
                {
                    Console.WriteLine("Invalid height input.");
                    return;
                }

                Console.WriteLine("Please enter the package length:");
                if (!double.TryParse(Console.ReadLine(), out double dimL))
                {
                    Console.WriteLine("Invalid length input.");
                    return;
                }

                // Calculate total dimensions
                double totalDims = dimW + dimH + dimL;

                // Validate size
                if (!processor.ValidateValue(totalDims, "dimensions"))
                {
                    Console.WriteLine("Package too big to be shipped via Package Express.");
                    return;
                }

                // Calculate shipping cost
                // Cost = (width * height * length * weight) / 100
                double finalCost = (dimW * dimH * dimL * weightValue) / 100;

                // Show shipping cost
                Console.WriteLine($"Your estimated total for shipping this package is: ${finalCost:F2}");
                Console.WriteLine("Thank you!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
} 