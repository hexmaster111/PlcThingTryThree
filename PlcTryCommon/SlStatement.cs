namespace PlcTryCommon;

public class SlStatement
{
    public string Code;
    public List<Token> Tokens;
    public AbstractSyntaxTree SyntaxTree;
    public Lexer Lexer;
    public string[] LoadElement;

    private SlStatement(AbstractSyntaxTree syntaxTree, Lexer lexer, string[] loadElement, string code)
    {
        SyntaxTree = syntaxTree;
        LoadElement = loadElement;
        Lexer = lexer;
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
        var tokens = Token.Tokenize(code);
        var syntaxTree = AbstractSyntaxTree.Parse(tokens);
        var lexer = Lexer.Lex(syntaxTree);
        return new SlStatement(syntaxTree, lexer, loadElements, code);
    }
}