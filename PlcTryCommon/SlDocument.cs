namespace PlcTryCommon;

public class SlDocument
{
    public Variable[] Variables;
    public SlStatement[] Statements;

    public static SlDocument Factory(string documentText)
    {
        LatterDiagramParser.ParseText(documentText, out var err, out var diagram);
        
        
        var statements = new List<SlStatement>();

        foreach (var line in diagram.Lines)
        {
            var statement = SlStatement.Factory(line, out var error);
            statements.Add(statement);
        }
        
        return new SlDocument(diagram.Variables.ToArray(), statements.ToArray());
    }
    
    private SlDocument(Variable[] variables, SlStatement[] statements)
    {
        Variables = variables;
        Statements = statements;
    }
}