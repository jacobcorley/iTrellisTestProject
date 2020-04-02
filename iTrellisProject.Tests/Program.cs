using System.Threading.Tasks;

namespace iTrellisProject.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var tests = new Tests();
            await tests.run();
        }
    }
}