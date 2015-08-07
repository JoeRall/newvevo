
namespace NewVevo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"C:\Users\JoeRall\Downloads\videoDB_1mil_tab.txt";
            DBImporter.ReadFile(file);
        }
    }
}
