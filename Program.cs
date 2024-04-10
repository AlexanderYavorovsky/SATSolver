namespace SATSolver;

class Program()
{
    static void Main(string[] args)
    {
        string path = args[0];

        SAT sat = new SAT();
        
        (bool status, var solution) = sat.Solve(path);
        if (!status)
        {
            Console.WriteLine("s UNSATISFIABLE");
            return;
        }
        Console.WriteLine("s SATISFIABLE");
        Console.Write("v ");
        foreach (var x in solution)   
        {
            Console.Write(x + " ");
        }
        Console.WriteLine("0");
    }
}