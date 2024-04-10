namespace SATSolver;

public class SAT
{
    public List<List<int>> CNF;
    public int VarCount { get; private set; }
    public int ClauseCount { get; private set; }

    public (bool, IEnumerable<int>) Solve(string path)
    {
        Parse(path);
        return DPLL(this.CNF, new List<int>(), true);
    }
    
    private void Parse(string path)
    {
        using (var sr = new StreamReader(path))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line[0] == 'c') continue;
                if (line[0] == 'p')
                {
                    string[] splitted = line.Split();
                    VarCount = Convert.ToInt32(splitted[2]);
                    ClauseCount = Convert.ToInt32(splitted[3]);
                }
                else
                {
                    var splitted = line.Split().Where(x => !string.IsNullOrEmpty(x));

                    List<int> clause = new List<int>();
                    foreach (var x in splitted)
                    {
                        if (x == "0") break;
                        clause.Add(Convert.ToInt32(x));
                    }

                    CNF.Add(clause);
                }
            }
        }
    }

    private List<List<int>> copyCNF(List<List<int>> CNF)
    {
        var copyCNF = new List<List<int>>();
        foreach (var clause in CNF)
        {
            List<int> copy = new List<int>(clause);
            copyCNF.Add(copy);
        }

        return copyCNF;
    }

    public (bool, IEnumerable<int>) DPLL(List<List<int>> CNF, List<int> solution, bool isPosBranch)
    {
        // Console.WriteLine("======");
        // Console.WriteLine(isPosBranch ? "PosBranch:" : "NegBranch:");

        var cnfcopy = this.copyCNF(CNF);
        var solutioncopy = new List<int>(solution);
        
        UnitPropagation(cnfcopy, solutioncopy);
        PureLiterals(cnfcopy, solutioncopy);

        //check
        if (cnfcopy.Count == 0)
        {
            var solutionSet = solutioncopy.ToHashSet();
            addVarsToSolution(VarCount, solutionSet);
            return (true, solutionSet.OrderBy(x => Math.Abs(x)));
        }

        foreach (var clause in cnfcopy)
            if (clause.Count == 0)
                return (false, solutioncopy);

        //choose lit
        var literal = cnfcopy[0][0];

        //recursion
        // dpll(cnf & literal)
        // dpll(cnf & ~literal)

        var posClause = new List<int> { literal };
        // var posCNF = new List<List<int>>();
        // foreach (var clause in cnfcopy)
        // {
        //     List<int> copy = new List<int>(clause);
        //     posCNF.Add(copy);
        // }
        var posCNF = copyCNF(cnfcopy);
        posCNF.Add(posClause);
        
        (bool posRes, var posSolution) = DPLL(posCNF, solutioncopy, true);

        if (posRes)
        {
            var posSolutionSet = posSolution.ToHashSet();
            addVarsToSolution(VarCount, posSolutionSet);
            return (posRes, posSolutionSet.OrderBy(x => Math.Abs(x)));
        }

        var negClause = new List<int> { -literal };
        // var negCNF = new List<List<int>>();
        // foreach (var clause in cnfcopy)
        // {
        //     List<int> copy = new List<int>(clause);
        //     negCNF.Add(copy);
        // }
        var negCNF = copyCNF(cnfcopy);
        negCNF.Add(negClause);
        
        return DPLL(negCNF, solutioncopy, false);
    }

    public void PureLiterals(List<List<int>> CNF, List<int> solution)
    {
        var literals = new HashSet<int>();
        var pure = new List<int>();
        
        foreach (var clause in CNF)
            foreach (var x in clause)
                literals.Add(x);
        
        foreach (var x in literals)
            if (!literals.Contains(-x))
                pure.Add(x);

        foreach (var clause in CNF.ToList())
        {
            foreach (var x in clause)
            {
                if (pure.Contains(x))
                    CNF.Remove(clause);
            }
        }
        
        solution.AddRange(pure);
    }

    public void UnitPropagation(List<List<int>> CNF, List<int> solution)
    {
        var units = new List<int>();
        
        foreach (var clause in CNF)
            if (clause.Count == 1)
                units.Add(clause[0]);

        // foreach (var x in units)
        // {
        //     foreach (var clause in CNF.Clauses.ToList())
        //     {
        //         if (clause.Units.Contains(x))
        //             CNF.Clauses.Remove(clause);
        //         
        //     }
        // }
        foreach (var clause in CNF.ToList())
        {
            foreach (var x in units)
            {
                if (clause.Contains(x))
                    CNF.Remove(clause);
                clause.RemoveAll(u => u == -x);
            }
        }
        
        solution.AddRange(units);
        
        // Console.WriteLine("after");
        // Console.Write("Solution: ");
        // var tt = solution.ToHashSet().OrderBy(u => Math.Abs(u));
        // foreach (var x in tt)
        //     Console.Write(x + " ");
        // Console.WriteLine("0");
        //
        // Console.WriteLine("CNF: ");
        // foreach (var c in CNF.Clauses)
        // {
        //     Console.Write("Clause:");
        //     foreach (var x in c.Units)
        //         Console.Write(x + " ");
        //     Console.WriteLine("0");
        // }
    }
    
    // public void Print()
    // {
    //     Console.WriteLine("CNF: ");
    //     foreach (var clause in CNF)
    //     {
    //         Console.Write("Clause: ");
    //         foreach (var x in clause)
    //             Console.Write(x + " ");
    //         Console.WriteLine();
    //     }
    //
    //     Console.WriteLine("Solution: ");
    //     foreach (var x in Solution)
    //         Console.Write(x + " ");
    // }
    
    private void addVarsToSolution(int varCount, HashSet<int> solutionSet)
    {
        if (solutionSet.Count < varCount)
            for (int i = 1; i <= varCount; i++)
                if (!solutionSet.Contains(i) && !solutionSet.Contains(-i))
                    solutionSet.Add(i);
    }
    
    public SAT()
    {
        CNF = new List<List<int>>();
    }
}