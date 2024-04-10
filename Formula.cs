namespace SATSolver;

public class Formula
{
    public int DisjCount { get; set; }
    public int VarCount { get; set; }
    public List<Clause> Clauses;

    public Formula()
    {
        Clauses = new List<Clause>();
    }

    public Formula(Formula f)
    {
        // Clauses = new List<Clause>(f.Clauses);
        Clauses = new List<Clause>();
        foreach (var c in f.Clauses)
        {
            var clause = new Clause();
            foreach(var x in c.Units)
                clause.Units.Add(x);
            Clauses.Add(clause);
        }

        DisjCount = f.DisjCount;
        VarCount = f.VarCount;
    }
}