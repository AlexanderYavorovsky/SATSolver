namespace SATSolver;

public class SAT
{
    public List<List<int>> CNF;
    public int VarCount { get; private set; }
    public int ClauseCount { get; private set; }

    public IEnumerable<int>? Solve(string path)
    {
        Parse(path);
        return DPLL(CNF, new List<int>());
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

    private IEnumerable<int>? DPLL(List<List<int>> CNF, List<int> solution)
    {
        var cnfcopy = this.copyCNF(CNF);
        var solutioncopy = new List<int>(solution);
        
        UnitPropagation(cnfcopy, solutioncopy);
        PureLiterals(cnfcopy, solutioncopy);

        //check
        if (cnfcopy.Count == 0)
        {
            var solutionSet = solutioncopy.ToHashSet();
            addVarsToSolution(VarCount, solutionSet);
            return solutionSet.OrderBy(x => Math.Abs(x));
        }

        foreach (var clause in cnfcopy)
            if (clause.Count == 0)
                return null;

        //choose lit
        var literal = cnfcopy[0][0];

        //recursion
        // dpll(cnf & literal)
        // dpll(cnf & ~literal)

        var posClause = new List<int> { literal };
        var posCNF = copyCNF(cnfcopy);
        posCNF.Add(posClause);
        
        var posSolution = DPLL(posCNF, solutioncopy);

        if (posSolution != null)
        {
            var posSolutionSet = posSolution.ToHashSet();
            addVarsToSolution(VarCount, posSolutionSet);
            return  posSolutionSet.OrderBy(x => Math.Abs(x));
        }

        var negClause = new List<int> { -literal };
        var negCNF = copyCNF(cnfcopy);
        negCNF.Add(negClause);
        
        return DPLL(negCNF, solutioncopy);
    }

    private void PureLiterals(List<List<int>> CNF, List<int> solution)
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

    private void UnitPropagation(List<List<int>> CNF, List<int> solution)
    {
        var units = new List<int>();
        
        foreach (var clause in CNF)
            if (clause.Count == 1)
                units.Add(clause[0]);

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
    }
    
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