namespace PlcTryCommon;

/// <summary>
///     The document is a collection of statements and variables.
/// </summary>
public class SlDocument
{
    public Variable[] Variables;
    public SlStatement[] Statements;

    public static SlDocument Factory(string documentText)
    {
        LatterDiagramParser.ParseText(documentText, out var ldParseError, out var diagram);
        if (ldParseError != null) throw new Exception(ldParseError);

        var statements = new List<SlStatement>();

        foreach (var line in diagram.Lines)
        {
            var statement = SlStatement.Factory(line, out var statementparseError);
            if (statementparseError != null) throw new Exception(statementparseError);
            if (statement != null) statements.Add(statement);
            else throw new Exception("Statement was null, and no error was returned.");
        }

        return new SlDocument(diagram.Variables.ToArray(), statements.ToArray());
    }

    private SlDocument(Variable[] variables, SlStatement[] statements)
    {
        Variables = variables;
        Statements = statements;
    }
}