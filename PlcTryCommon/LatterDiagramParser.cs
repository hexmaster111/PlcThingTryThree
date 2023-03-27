namespace PlcTryCommon;

/*
\          START          STOP          M0   |
\-----------| |-----------|\|------------O---|
\        |   M0   |                          |
\        ---| |---                           |
\                                            |
\                                            |
\ START [I000] VALUE: false Type: bool       |
\  STOP [I001] VALUE: false Type: bool       |
\    M0 [X000] VALUE: false Type: bool       |

(I000 OR X000) AND !I001 = X000
*/
/*
|       START                                |
|-------|  |----------------[ADD N0 N1 N1]---|
|                                            |
|  Start [I000] VALUE: false Type: bool      |
|    N0 [RAM0] VALUE: 0 Type: number         |
|    N1 [RAM1] VALUE: 1 Type: number         |
I000 = [ADD RAM0 RAM1 RAM1]

*/
/*
\          START          STOP          M0   |
\-----------| |-----------|\|------------O---|
\        |   M0   |                          |
\        ---| |---                           |
\       |  REMO  |                           |
\       ---| |---                            |
\                                            |
\ START [I000] VALUE: false Type: bool       |
\  STOP [I001] VALUE: false Type: bool       |
\    M0 [X000] VALUE: false Type: bool       |
\  REMO [I002] VALUE: false Type: bool       |

(I000 OR X000 OR I002) AND !I001 = X000
*/
/*  SELECTED ONE!!
\          START           STOP         M0   |
\-----------| |--------------| |---------O---|
\        |   M0           |                  |
\        ---| |-----------                   |
\       |  REMO    REMS  |                   |
\       ---| |------| |---                   |
\                                            |
[VAR]
BOOL START I000
BOOL STOP  I001
BOOL M0    X000
BOOL REMO  I002
BOOL REMS  I003
[END-VAR]
[SL]
(I000 OR X000 OR (I003 AND I002)) AND !I001 = X000
[END-SL]
*/

public static class LatterDiagramParser
{
    public static void ParseText(string code, out string[] err, out LatterDiagram diagram)
    {
        err = Array.Empty<string>();
        diagram = null;

        var lines = code.Split(Environment.NewLine);
        var variables = new List<Variable>();
        var slLines = new List<string>();
        var inVar = false;
        var inSl = false;

        foreach (var line in lines)
        {
            if (line.StartsWith("[VAR]"))
            {
                inVar = true;
                continue;
            }

            if (line.StartsWith("[END-VAR]"))
            {
                inVar = false;
                continue;
            }

            if (line.StartsWith("[SL]"))
            {
                inSl = true;
                continue;
            }

            if (line.StartsWith("[END-SL]"))
            {
                inSl = false;
                continue;
            }

            if (inVar)
            {
                var parts = line.Split(' ');
                var name = parts[1];
                var type = parts[0];
                var mapId = parts[2];
                variables.Add(new Variable(name, type, mapId));
            }

            if (inSl)
            {
                if (line != string.Empty)
                {
                    slLines.Add(line);
                }
            }
        }

        diagram = new LatterDiagram(variables, slLines);
    }
    
}