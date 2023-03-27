using System.Text;

namespace PlcTryCommon;

public static class SLTextRenderer
{
    public static void GetAsciiDiagram(
        List<string> statementLists,
        List<Variable> variables,
        out string error,
        out string diagram
    )
    {
        error = null;
        diagram = null;

        var rungs = new List<string>();

        var idName = new Dictionary<string, Variable>();
        foreach (var variable in variables)
        {
            idName.Add(variable.MapId, variable);
        }

        foreach (var list in statementLists)
        {
            GetSingleLatterRung(list, out var err, out var rung, idName);
            if (err != null)
            {
                error = err;
                return;
            }

            rungs.Add(rung);
        }
    }


    private static void GetSingleLatterRung
    (
        string statement,
        out string? error,
        out string rung,
        Dictionary<string, Variable> idName)
    {
        const int maxWidth = 80;
        const string contactNo = "---] [---";
        const string contactNc = "---]/[---";
        const string coil = "---O---";
        const string wire = "-------";
        const string boundingBar = "|";

        var sb = new StringBuilder();
        sb.Append(boundingBar);

        //split the statment into parts on space, and, or , = ! and ()
        

        throw new NotImplementedException();
    }
}

/*
(I000 OR X000 OR (I003 AND I002)) AND !I001 = X000
\   START           STOP         M0   |
\---| |--------------| |---------O----|
\    M0           |                   |
\---| |-----------                    |
\   REMO    REMS  |                   |
\---| |------| |---                   |
\                                     |
*/