using System.Text;

namespace PlcTryCommon;

public static class SLTextRenderer
{
    private record DisplayTokens(Token Token, int Row, int Column);
}