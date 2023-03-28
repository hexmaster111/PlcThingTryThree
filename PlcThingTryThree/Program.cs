// See https://aka.ms/new-console-template for more information

using PlcTryCommon;

Console.WriteLine("Hello, World!");

const string Latching =
    @"[VAR]
BOOL START I000
BOOL STOP I001
BOOL M0 X000
BOOL REMO I002
BOOL REMS I003
[END-VAR]
[SL]
( ( #START | #M0 ) & ! #STOP ) = M0
[END-SL]";

const string OrThree =
    @"[VAR]
BOOL START I000
BOOL STOP I001
BOOL M0 X000
BOOL REMO I002
BOOL REMS I003
[END-VAR]
[SL]
( #START | #STOP | #M0) = M0
[END-SL]";

const string AndThree =
    @"[VAR]
BOOL START I000
BOOL STOP I001
BOOL M0 X000
BOOL REMO I002
BOOL REMS I003
[END-VAR]
[SL]
( #START & #STOP & #M0 ) = M0
[END-SL]";


const string OrAndThree =
    @"[VAR]
BOOL START I000
BOOL STOP I001
BOOL M0 X000
BOOL REMO I002
BOOL REMS I003
[END-VAR]
[SL]
( ( #FIRST & #SECOND & #THIRD ) & ( #FOURTH | #FIFTH | #SIXTH ) ) = #M0
[END-SL]";


var doc = SlDocument.Factory(OrAndThree);


Console.ReadKey();