namespace PlcTryCommon;

public class LatterDiagram
{
    public LatterDiagram(List<Variable> variables, List<string> lines)
    {
        Variables = variables;
        Lines = lines;
    }

    public List<Variable> Variables { get; set; }
    public List<string> Lines { get; set; }
}