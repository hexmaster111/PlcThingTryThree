namespace PlcTryCommon;

public class AbstractSyntaxTree
{
    public Node Root;

    private AbstractSyntaxTree(Node root)
    {
        Root = root;
    }

    public static AbstractSyntaxTree Parse(List<Token> tokens)
    {
        var root = ParseTokens(tokens);
        return new AbstractSyntaxTree(root);
    }

    /// <summary>
    ///     Takes in the list of tokens and returns the root node of the tree
    /// </summary>
    /// <param name="tokens">Parsed tokens from the logical line</param>
    /// <returns>Root node</returns>
    private static Node ParseTokens(List<Token> tokens)
    {
        // Start with the lowest precedence operator
        return ParseOr(tokens);
    }

    private static Node ParseOr(List<Token> tokens)
    {
        // Parse the left operand
        var left = ParseAnd(tokens);

        // If the next token is an OR operator, parse the right operand and create a new OR node
        while (tokens.Count > 0 && tokens[0].Type == TokenType.Or)
        {
            tokens.RemoveAt(0); // Consume the OR operator
            var right = ParseAnd(tokens);
            left = new Node(new Token(TokenType.Or, "|"), left, right);
        }

        return left;
    }

    private static Node ParseAnd(List<Token> tokens)
    {
        // Parse the left operand
        var left = ParseNot(tokens);

        // If the next token is an AND operator, parse the right operand and create a new AND node
        while (tokens.Count > 0 && tokens[0].Type == TokenType.And)
        {
            tokens.RemoveAt(0); // Consume the AND operator
            var right = ParseNot(tokens);
            left = new Node(new Token(TokenType.And, "&"), left, right);
        }

        return left;
    }

    private static Node ParseNot(List<Token> tokens)
    {
        // If the next token is a NOT operator, parse the operand and create a new NOT node
        if (tokens.Count > 0 && tokens[0].Type == TokenType.Not)
        {
            tokens.RemoveAt(0); // Consume the NOT operator
            var child = ParseNot(tokens);
            return new Node(new Token(TokenType.Not, "!"), child, null);
        }

        // Otherwise, parse the next level of precedence
        return ParsePrimary(tokens);
    }

    private static Node ParsePrimary(List<Token> tokens)
    {
        // If the next token is an opening parenthesis, parse the subexpression and consume the closing parenthesis
        if (tokens.Count > 0 && tokens[0].Type == TokenType.OpenParen)
        {
            tokens.RemoveAt(0); // Consume the opening parenthesis
            Node subexpr = ParseOr(tokens);
            if (tokens.Count > 0 && tokens[0].Type == TokenType.CloseParen)
            {
                tokens.RemoveAt(0); // Consume the closing parenthesis
            }
            else
            {
                throw new ArgumentException("Missing closing parenthesis");
            }

            return subexpr;
        }

        // Otherwise, parse the operand (an I/O map ID or an error)
        if (tokens.Count > 0 && tokens[0].Type == TokenType.IoMapId)
        {
            Token token = tokens[0];
            tokens.RemoveAt(0); // Consume the operand
            return new Node(token, null, null);
        }
        else
        {
            throw new ArgumentException("Invalid token");
        }
    }
}