namespace PlcTryCommon;

public class Node
{
    public Token Token;
    public Node? Left;
    public Node? Right;

    public Node(Token token, Node? left, Node? right)
    {
        Token = token;
        Left = left;
        Right = right;
    }

    public void PrettyPrint(string indent, bool last, StringWriter stringWriter)
    {
        stringWriter.Write(indent);
        if (last)
        {
            stringWriter.Write("\\-");
            indent += "  ";
        }
        else
        {
            stringWriter.Write("|-");
            indent += "| ";
        }

        stringWriter.WriteLine($"{Token.Type} {Token.Value}");

        if (Left != null)
        {
            Left.PrettyPrint(indent, Right == null, stringWriter);
        }

        if (Right != null)
        {
            Right.PrettyPrint(indent, true, stringWriter);
        }
    }
}