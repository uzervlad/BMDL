using System.Linq;

namespace BMDL
{
    class Program
    {
        static void Main(string[] args)
        {
            var debug = args.Contains("--debug");
            App.Start(debug);
        }
    }
}
