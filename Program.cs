using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("1. Send message");
        Console.WriteLine("2. Receive message");
        Console.Write("Select option: ");
        var option = Console.ReadLine();

        if (option == "1")
        {
            await Send.RunAsync();
        }
        else if (option == "2")
        {
            await Receive.RunAsync();
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
}
