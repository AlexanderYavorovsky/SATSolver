namespace SATSolver;

public class SAT
{
    public List<List<int>> CNF;
    public int VarCount { get; private set; }

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

    private IEnumerable<int> returnSolution(IEnumerable<int> solution)
    {
        var solutionSet = solution.ToHashSet();
        addVarsToSolution(VarCount, solutionSet);
        return solutionSet.OrderBy(x => Math.Abs(x));
    }

    private IEnumerable<int>? DPLL(List<List<int>> CNF, List<int> solution)
    {
        var cnfCopy = copyCNF(CNF);
        var solutionCopy = new List<int>(solution);
        
        UnitPropagation(cnfCopy, solutionCopy);
        PureLiterals(cnfCopy, solutionCopy);

        if (cnfCopy.Count == 0)
            return returnSolution(solutionCopy);

        foreach (var clause in cnfCopy)
            if (clause.Count == 0)
                return null;

        var literal = cnfCopy[0][0];

        var posClause = new List<int> { literal };
        var posCNF = copyCNF(cnfCopy);
        posCNF.Add(posClause);
        
        var posSolution = DPLL(posCNF, solutionCopy);
        if (posSolution != null)
            return returnSolution(posSolution);

        var negClause = new List<int> { -literal };
        cnfCopy.Add(negClause);

        return DPLL(cnfCopy, solutionCopy);
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