using System.Diagnostics;

namespace PlcTryStringWriterRenderer;

public class TextRenderer
{
    public TextRenderer()
    {
        // ASSETS.TestRenderSingleLine();
        // ASSETS.TestRenderBranch();
        var a = new List<DisplayColDEV>();

        var colZero = new DisplayColDEV
        {
            ElementsInCol = new List<DisplayColDEV.Element>
            {
                new DisplayColDEV.Element()
                {
                    EType = DisplayColDEV.Element.ElementType.ContactNormallyOpen,
                    Name = "START BTN",
                    Row = 0,
                    Col = 0,
                    ContinueLine = true
                },
                new DisplayColDEV.Element()
                {
                    EType = DisplayColDEV.Element.ElementType.ContactNormallyOpen,
                    Name = "M0",
                    Row = 1,
                    Col = 0,
                    UpRight = true,
                },
            },
        };

        var colOne = new DisplayColDEV
        {
            ElementsInCol = new List<DisplayColDEV.Element>
            {
                new DisplayColDEV.Element()
                {
                    EType = DisplayColDEV.Element.ElementType.ContactNormallyClosed,
                    Name = "STOP BTN",
                    Row = 0,
                    Col = 1,
                }
            }
        };

        var load = new DisplayColDEV
        {
            ElementsInCol = new List<DisplayColDEV.Element>
            {
                new DisplayColDEV.Element()
                {
                    EType = DisplayColDEV.Element.ElementType.Coil,
                    Name = "M0",
                    Row = 0,
                    Col = 2,
                    IsLoad = true
                }
            }
        };

        a.Add(colZero);
        a.Add(colOne);
        a.Add(load);

        ASSETS.AutoRenderTest(a);
    }

    public string? GetString()
    {
        return "TODO!GetString";
    }

    private static class ASSETS
    {
        public const int DiagramWidth = 70;
        public const int DiagramItemWidth = 10;
        public const int ItemsInDiagram = DiagramWidth / DiagramItemWidth;
        public const string Name = "[  NAME  ]";
        public const string Spacer = "    SP    ";
        public const string FillerSpacer = "    FI    ";
        public const string DownBar = "|";
        public const string SideBar = "|";
        public const string DownBarSpacer = "   DBS   ";
        public const string Wire = "----------";
        public const string Coil = "---(  )---";
        public const string LatchCoil = "---(L)----";
        public const string ResetCoil = "---(R)----";
        public const string NoContact = "---| |----";
        public const string NcContact = "---|/|----";

        public static void AutoRenderTest(IEnumerable<DisplayColDEV> elementCols)
        {
            string ForceToLen(int len, string str) => str.Length > len ? str[..len] : str.PadRight(len);
            string WrapInBrackets(string str) => $"[{str}]";
            bool IsNameLine(int lineIndex) => lineIndex % 2 == 0;
            bool[] GetNewRow() => new bool[DiagramWidth / DiagramItemWidth];
            List<bool[]> rowPlaced = new();
            List<string> lines = new();


            void PlaceOnLine(int lineIndex, int rowIndex, string item, bool doNotMark = false)
            {
                var rowIndexCorrected = rowIndex * DiagramItemWidth;

                if (lines.Count <= lineIndex)
                {
                    var line = new string(' ', DiagramWidth);
                    line = line[..rowIndexCorrected] + item + line[(rowIndexCorrected + item.Length)..];
                    lines.Add(line);
                    rowPlaced.Add(GetNewRow());
                    if (!doNotMark) rowPlaced[lineIndex][rowIndex] = true;
                }
                else
                {
                    var line = lines[lineIndex];
                    line = line[..rowIndexCorrected] + item + line[(rowIndexCorrected + item.Length)..];
                    lines[lineIndex] = line;
                    if (!doNotMark) rowPlaced[lineIndex][rowIndex] = true;
                }
            }

            void PlaceOnNameLine(int lineItemIndex, int rowIndex, string item, bool doNotMark = false)
            {
                var lineIndexCorrected = lineItemIndex * 2; //because we have two lines per item
                //Name should be even
                Debug.Assert(IsNameLine(lineIndexCorrected), "On a name line");
                PlaceOnLine(lineIndexCorrected, rowIndex, item, doNotMark);
            }

            void PlaceOnElementLine(int lineItemIndex, int rowIndex, string item, bool doNotMark = false)
            {
                var lineIndexCorrected = lineItemIndex * 2 + 1; //because we have two lines per item
                //Element line should be odd
                Debug.Assert(!IsNameLine(lineIndexCorrected), "On an element line");
                PlaceOnLine(lineIndexCorrected, rowIndex, item, doNotMark);
            }


            foreach (var col in elementCols)
            {
                foreach (var elem in col.ElementsInCol)
                {
                    var label = WrapInBrackets(ForceToLen(8, elem.Name));
                    var symbol = elem.Symbol;


                    if (!elem.IsLoad)
                    {
                        PlaceOnNameLine(elem.Row, elem.Col, label);
                        PlaceOnElementLine(elem.Row, elem.Col, symbol);
                    }

                    if (elem.IsLoad)
                    {
                        // Loads go on the far right of the diagram, if there is a load there already, we need to move it over
                        // to the right, and place the new load there
                        var loadCol = ItemsInDiagram - 1;
                        while (rowPlaced[elem.Row][loadCol])
                        {
                            loadCol--;
                        }

                        PlaceOnNameLine(elem.Row, loadCol, label);
                        PlaceOnElementLine(elem.Row, loadCol, symbol);
                    }

                    if (elem.ContinueLine)
                    {
                        for (int i = elem.Col; i < DiagramWidth / DiagramItemWidth; i++)
                        {
                            if (rowPlaced[elem.Row][i]) continue;
                            PlaceOnNameLine(elem.Row, i, FillerSpacer, true);
                            PlaceOnElementLine(elem.Row, i, Wire, true);
                        }
                    }

                    if (elem.UpRight)
                    {
                        //Up bar for next element both on name and element line
                        PlaceOnNameLine(elem.Row, elem.Col + 1, DownBar);
                        PlaceOnElementLine(elem.Row, elem.Col + 1, DownBar);
                    }
                }
            }
            // Place the side bar down the left side, by shifting each row to the right by one, and placing a side bar
            // in the new space

            for (var rowIndex = 0; rowIndex < lines.Count; rowIndex++)
            {
                lines[rowIndex] = lines[rowIndex].Insert(0, SideBar);
            }

            var retString = lines.Aggregate("", (current, s) => current + (s + Environment.NewLine));

            Console.Write(retString);
        }

        public static void TestRenderBranch()
        {
            List<string[]> rows = new();
            //TEST ONE, SIMPLE LATCH
            rows.Add(new[] { SideBar, Spacer, Name, Name, Spacer, Spacer, Spacer, Name });
            rows.Add(new[] { SideBar, Wire, NoContact, NcContact, Wire, Wire, Wire, Coil });
            //down bar group
            rows.Add(new[] { SideBar, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new[] { SideBar, DownBarSpacer, DownBar, NoContact, DownBar, DownBarSpacer });
            //Spacer
            rows.Add(new[] { SideBar });
            //Next row Group
            rows.Add(new[] { SideBar, Spacer, Name, Spacer, Spacer, Spacer, Name });
            rows.Add(new[] { SideBar, Wire, NoContact, Wire, Wire, "----[NAME5", "67 1234567", " 1234567 ]" });
            //Spacer
            rows.Add(new[] { SideBar });
            //Row
            rows.Add(new[] { SideBar, Spacer, Name, Name, Name, Name, Spacer, Spacer, Name });
            rows.Add(new[] { SideBar, Wire, NoContact, NoContact, NoContact, NoContact, Wire, Wire, Coil });
            rows.Add(new[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new[]
                { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, NoContact, DownBar, DownBarSpacer });
            rows.Add(new[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new[]
                { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, NoContact, DownBar, DownBarSpacer });
            rows.Add(new[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new[]
                { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, NoContact, DownBar, DownBarSpacer });
            //spacer
            rows.Add(new[] { SideBar });
            //Row
            rows.Add(new[] { SideBar, Wire, NoContact, NoContact, Wire, Wire, Wire });
            rows.Add(new[] { SideBar, Spacer, DownBarSpacer, DownBar, Name, Name, DownBar, DownBarSpacer });
            rows.Add(new[] { SideBar, Spacer, DownBarSpacer, DownBar, NoContact, NoContact, DownBar, DownBarSpacer });


            List<string> resList = new();
            foreach (var row in rows)
            {
                var rowString = string.Empty;
                foreach (var rowElement in row)
                {
                    rowString += rowElement;
                }

                while (rowString.Length < ASSETS.DiagramWidth)
                {
                    rowString += ASSETS.FillerSpacer;
                }

                resList.Add(rowString);
            }

            var res = string.Empty;
            foreach (var line in resList)
            {
                res += line;
                res += DownBar;
                res += Environment.NewLine;
            }


            Console.WriteLine(res);
        }

        public static void TestRenderSingleLine()
        {
            string[] thingsToRender = { NoContact, NcContact, Wire, Coil, LatchCoil, ResetCoil };

            var nameLine = new List<string>();
            var bodyLines = new List<string>();

            foreach (var s in thingsToRender)
            {
                bodyLines.Add(s);
                nameLine.Add(Name);
            }

            var nameString = string.Join(string.Empty, nameLine);
            var bodyString = string.Join(string.Empty, bodyLines);
            var finalString = $"{nameString}{Environment.NewLine}{bodyString}";
            Console.WriteLine("TEST RENDER");
            Console.WriteLine(finalString);
            Console.WriteLine("END TEST");
        }
    }

    public class DisplayColDEV
    {
        public List<Element> ElementsInCol = new();

        public struct Element
        {
            public string Name;
            public ElementType EType;

            public bool UpRight;
            public bool ContinueLine;
            public bool IsLoad;
            public int Row, Col;

            public enum ElementType
            {
                ContactNormallyOpen,
                ContactNormallyClosed,
                Coil,
                FunctionBlock
            }

            public string Symbol => EType switch
            {
                ElementType.ContactNormallyOpen => ASSETS.NoContact,
                ElementType.ContactNormallyClosed => ASSETS.NcContact,
                ElementType.Coil => ASSETS.Coil,
                ElementType.FunctionBlock => "!TODO Function Block",
                _ => $"{EType} Is Not a valid Element Type"
            };
        }
    }
}