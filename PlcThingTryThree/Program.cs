// See https://aka.ms/new-console-template for more information

using PlcTryCommon;

Console.WriteLine("Hello, World!");

const string TestCode =
    @"[VAR]
BOOL START I000
BOOL STOP I001
BOOL M0 X000
BOOL REMO I002
BOOL REMS I003
[END-VAR]
[SL]
(I000 OR X000 OR (I003 AND I002)) AND !I001 = X000
[END-SL]";

LatterDiagramParser.ParseText(TestCode, out var err, out var diagram);
SLTextRenderer.GetAsciiDiagram(diagram.Lines, diagram.Variables, out var error, out var asciiDiagram);
Console.WriteLine(asciiDiagram);
Console.ReadKey();
