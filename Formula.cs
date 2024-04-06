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
        Clauses = new List<Clause>(f.Clauses);
        DisjCount = f.DisjCount;
        VarCount = f.VarCount;
    }
}