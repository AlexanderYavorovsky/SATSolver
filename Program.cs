using System.Diagnostics;

namespace SATSolver;

class Program
{
    static void Main(string[] args)
    {
        string path = args[0];

        // warming up
        for (int i = 0; i < 40; i++)
        {
            SAT s_sat = new SAT();
            var s_solution = s_sat.Solve(path);
        }

        var sw = new Stopwatch();
        for (int i = 0; i < 80; i++)
        {
            SAT warmSat = new SAT();

            sw.Restart();
            var warmSolution = warmSat.Solve(path);
            sw.Stop();

            var time = sw.ElapsedMilliseconds;
            Console.WriteLine(time);
        }

    }
}
