namespace SATSolver;

public class Clause
{
    public List<int> Units;

    public Clause()
    {
        Units = new List<int>();
    }

    public Clause(int x)
    {
        Units = new List<int>();
        Units.Add(x);
    }
}