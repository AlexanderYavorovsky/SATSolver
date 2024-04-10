namespace SATSolver;

public class SAT
{
    public Formula CNF;
    public List<int> Solution;
    
    public (bool, IEnumerable<int>) DPLL(Formula CNF, List<int> solution)
    {
        PureLiterals(CNF, solution);
        UnitPropagation(CNF, solution);
        
        //check
        if (CNF.Clauses.Count == 0)
        {
            var solutionSet = solution.ToHashSet();
            addVarsToSolution(solutionSet);
            return (true, solutionSet.OrderBy(x => Math.Abs(x)));
        }
        
        foreach (var clause in CNF.Clauses)
            if (clause.Units.Count == 0)
                return (false, solution);
        
        //choose lit
        var literal = CNF.Clauses[0].Units[0];
        
        //recursion
        // dpll(cnf & literal)
        // dpll(cnf & ~literal)

        var posClause = new Clause(literal);
        var posCNF = new Formula(CNF);
        posCNF.Clauses.Add(posClause);
        (bool posRes, var posSolution) = DPLL(posCNF, solution);

        if (posRes)
        {
            var posSolutionSet = posSolution.ToHashSet();
            addVarsToSolution(posSolutionSet);
            return (posRes, posSolutionSet.OrderBy(x => Math.Abs(x)));
        }
        
        var negClause = new Clause(-literal);
        var negCNF = new Formula(CNF);
        negCNF.Clauses.Add(negClause);
        
        return DPLL(negCNF, solution);
    }
    
    public (bool, IEnumerable<int>) DPLL(List<int> solution)
    {
        return DPLL(CNF, solution);
    }

    public void PureLiterals(Formula CNF, List<int> solution)
    {
        var literals = new HashSet<int>();
        var pure = new List<int>();
        
        foreach (var clause in CNF.Clauses)
            foreach (var x in clause.Units)
                literals.Add(x);
        
        foreach (var x in literals)
            if (!literals.Contains(-x))
                pure.Add(x);

        foreach (var clause in CNF.Clauses.ToList())
        {
            foreach (var x in clause.Units)
            {
                if (pure.Contains(x))
                    CNF.Clauses.Remove(clause);
            }
        }
        
        solution.AddRange(pure);
    }

    public void UnitPropagation(Formula CNF, List<int> solution)
    {
        var units = new List<int>();
        
        foreach (var clause in CNF.Clauses)
            if (clause.Units.Count == 1)
                units.Add(clause.Units[0]);

        // foreach (var x in units)
        // {
        //     foreach (var clause in CNF.Clauses.ToList())
        //     {
        //         if (clause.Units.Contains(x))
        //             CNF.Clauses.Remove(clause);
        //         
        //     }
        // }
        foreach (var clause in CNF.Clauses.ToList())
        {
            foreach (var x in units)
            {
                if (clause.Units.Contains(x))
                    CNF.Clauses.Remove(clause);
                clause.Units.RemoveAll(u => u == -x);
            }
        }
        
        solution.AddRange(units);
        
        Console.WriteLine("after");
        Console.Write("Solution: ");
        var tt = solution.ToHashSet().OrderBy(u => Math.Abs(u));
        foreach (var x in tt)
            Console.Write(x + " ");
        Console.WriteLine("0");
        
        // Console.WriteLine("CNF: ");
        // foreach (var c in CNF.Clauses)
        // {
        //     Console.Write("Clause:");
        //     foreach (var x in c.Units)
        //         Console.Write(x + " ");
        //     Console.WriteLine("0");
        // }
    }
    
    public void Print()
    {
        Console.WriteLine("CNF: ");
        foreach (var clause in CNF.Clauses)
        {
            Console.Write("Clause: ");
            foreach (var x in clause.Units)
                Console.Write(x + " ");
            Console.WriteLine();
        }

        Console.WriteLine("Solution: ");
        foreach (var x in Solution)
            Console.Write(x + " ");
    }
    
    private void addVarsToSolution(HashSet<int> solutionSet)
    {
        if (solutionSet.Count < CNF.VarCount)
            for (int i = 1; i <= CNF.VarCount; i++)
                if (!solutionSet.Contains(i) && !solutionSet.Contains(-i))
                    solutionSet.Add(i);
    }
    
    public SAT()
    {
        CNF = new Formula();
        Solution = new List<int>();
    }
}