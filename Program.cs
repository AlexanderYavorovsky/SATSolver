namespace SATSolver;

class Program()
{
    static void Main(string[] args)
    {
        string path = args[0];

        SAT sat = new SAT();
        Parser parser = new Parser();
        sat.CNF = parser.Parse(path);
        
        (bool status, var solution) = sat.DPLL(new List<int>());
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