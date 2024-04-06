namespace SATSolver;

public class Parser
{
    public Formula Parse(string path)
    {
        Formula cnf = new Formula();
        using (var sr = new StreamReader(path))
        {
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line[0] == 'c') continue;
                if (line[0] == 'p')
                {
                    string[] splitted = line.Split();
                    cnf.VarCount = Convert.ToInt32(splitted[2]);
                    cnf.DisjCount = Convert.ToInt32(splitted[3]);
                }
                else
                {
                    var splitted = line.Split().Where(x => !string.IsNullOrEmpty(x));

                    Clause clause = new Clause();
                    foreach (var x in splitted)
                    {
                        if (x == "0") break;
                        clause.Units.Add(Convert.ToInt32(x));
                    }

                    cnf.Clauses.Add(clause);
                }
            }
        }

        return cnf;
    }

    public Parser()
    {
    }
}