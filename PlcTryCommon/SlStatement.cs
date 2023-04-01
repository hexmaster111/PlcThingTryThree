using System.Collections;

namespace PlcTryCommon;

/// <summary>
///     The statement is a single line of the document.
/// </summary>
public class SlStatement
{
    public string Code;
    public List<Token> Tokens;
    public string[] LoadElement;
    public StatementItem[] DisplayItems;

    private SlStatement(string[] loadElement, string code, List<Token> tokens, StatementItem[] displayItems)
    {
        LoadElement = loadElement;
        Code = code;
        Tokens = tokens;
        DisplayItems = displayItems;
    }

    public static SlStatement? Factory(string statementLine, out string? error)
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
        var displayItems = Parse(new TokenStream(tokens));
        return new SlStatement(loadElements, code, tokens, displayItems);
    }


    public struct StatementItem
    {
        public Token[] StatementTokes;
        public StatementItem(IEnumerable<Token> statmentTokes)
        {
            if (statmentTokes.Any(x => x.Type == TokenType.OpenParen || x.Type == TokenType.CloseParen))
            {
                throw new Exception("\"(\" or \")\" found in statment item");
            }


            StatementTokes = statmentTokes.ToArray();
        }
    }

    private static StatementItem[] Parse(TokenStream ts)
    {
        var ret = new List<StatementItem>();

        var parenStack = new Stack<List<Token>>();
        var tokensFromThisOpenParen = new List<Token>();

        while (ts.HasNext())
        {
            var token = ts.Next();
            switch (token.Type)
            {
                case TokenType.OpenParen:
                    parenStack.Push(tokensFromThisOpenParen);
                    tokensFromThisOpenParen = new List<Token>();
                    break;
                case TokenType.CloseParen:
                    //We have found the end of this paren, save it 

                    StatementItem tokensWithenParens = new(parenStack.Pop());
                    ret.Add(tokensWithenParens);
                    break;
                case TokenType.Not:
                    tokensFromThisOpenParen.Add(token);
                    break;
                case TokenType.And:
                    tokensFromThisOpenParen.Add(token);
                    break;
                case TokenType.Or:
                    tokensFromThisOpenParen.Add(token);
                    break;
                case TokenType.IoMapId:
                    tokensFromThisOpenParen.Add(token);
                    break;
            }

        }

        return ret.ToArray();
    }

}


public enum DisplayItemType
{
    NormalyOpen,
    NormalyClosed,

    Coil,
    CoilLatchSet,
    CoilLatchReset,
}