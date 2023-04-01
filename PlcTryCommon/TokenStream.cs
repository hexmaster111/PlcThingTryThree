namespace PlcTryCommon;

public class TokenStream
{
    private List<Token> _tokens;
    private int _index;

    public TokenStream(List<Token> tokens)
    {
        _tokens = tokens;
        _index = 0;
    }

    public Token Peek()
    {
        return _tokens[_index];
    }

    public Token Next()
    {
        return _tokens[_index++];
    }

    public bool HasNext()
    {
        return _index < _tokens.Count;
    }

    public void ReWind()
    {
        _index = 0;
    }
}