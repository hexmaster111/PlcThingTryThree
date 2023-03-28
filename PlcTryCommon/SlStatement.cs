namespace PlcTryCommon;

public class SlStatement
{
    public AbstractSyntaxTree SyntaxTree;
    public string[] LoadElement;

    private SlStatement(AbstractSyntaxTree syntaxTree, string[] loadElement)
    {
        SyntaxTree = syntaxTree;
        LoadElement = loadElement;
    }

    public static SlStatement Factory(string statementLine, out string error)
    {
        error = null;
        var parts = statementLine.Split("=");
        if (2 > parts.Length)
        {
            error = "Statement did not have a result element, expected statement = loadName";
            return null;
        }

        var code = parts[0];
        var loadElements = parts[1..];
        var syntaxTree = AbstractSyntaxTree.Parse(code);
        return new SlStatement(syntaxTree, loadElements);
    }
}