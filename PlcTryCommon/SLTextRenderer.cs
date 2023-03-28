using System.Text;

namespace PlcTryCommon;

public static class SLTextRenderer
{
    private record DisplayTokens(Token Token, int Row, int Column);


    public static string GetAsciiDiagram(SlDocument document)
    {
        var sb = new StringBuilder();

        foreach (var statement in document.Statements)
        {
            int row = 0;
            int column = 0;
            List<DisplayTokens> displayTokens = new();
            TraverseTree(statement.SyntaxTree.Root, ref row, ref column, displayTokens);
        }

        return sb.ToString();
    }

    private static void TraverseTree(Node root, ref int row, ref int column, List<DisplayTokens> displayTokens)
    {
        //Go left into the tree
        if (root.Left != null)
        {
            TraverseTree(root.Left, ref row, ref column, displayTokens);
        }

        //We are now all the way left, so we can handle the current node
        switch (root.Token.Type)
        {
            case TokenType.OpenParen:
                break;
            case TokenType.CloseParen:
                break;
            case TokenType.Not:
                break;
            case TokenType.And:
                break;
            case TokenType.Or:
                break;
            case TokenType.IoMapId:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}