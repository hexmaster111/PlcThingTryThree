using System.Text;

namespace PlcTryCommon;

public class Token
{
    public TokenType Type;
    public string Value;

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
    
    public static List<Token> Tokenize(string statement)
    {
        var sr = new StringReader(statement);
        var tokens = new List<Token>();
        while (sr.Peek() != -1)
        {
            var c = (char)sr.Read();
            switch (c)
            {
                case '(':
                    tokens.Add(new Token(TokenType.OpenParen, "("));
                    break;
                case ')':
                    tokens.Add(new Token(TokenType.CloseParen, ")"));
                    break;
                case '!':
                    tokens.Add(new Token(TokenType.Not, "!"));
                    break;
                case '&':
                    tokens.Add(new Token(TokenType.And, "&"));
                    break;
                case '|':
                    tokens.Add(new Token(TokenType.Or, "|"));
                    break;
                case '=':
                    tokens.Add(new Token(TokenType.IoMapId, "="));
                    break;
                //Remove whitespace
                case ' ':
                    break;

                case '#':
                case 'M':
                case 'I':
                case 'X':
                {
                    var sb = new StringBuilder();
                    sb.Append(c);
                    while (sr.Peek() != -1)
                    {
                        var next = (char)sr.Peek();
                        if ( next != ' ' && next != '(' && next != ')' && next != '!' && next != '&' && next != '|' && next != '=')
                        {
                            sb.Append((char)sr.Read());
                        }
                        else
                        {
                            break;
                        }
                    }

                    tokens.Add(new Token(TokenType.IoMapId, sb.ToString()));
                    break;
                }
            }
        }

        return tokens;
    }
    
}