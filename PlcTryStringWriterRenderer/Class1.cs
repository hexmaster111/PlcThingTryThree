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
                    EType = DisplayColDEV.Element.ElementType.ContactNormalyOpen,
                    Name = "START BTN",
                    Row = 0,
                    Col = 0
                },
                new DisplayColDEV.Element()
                {
                    EType = DisplayColDEV.Element.ElementType.ContactNormalyOpen,
                    Name = "M0",
                    Row = 1,
                    Col = 0
                },
            },

        };

        var colOne = new DisplayColDEV
        {
            ElementsInCol = new List<DisplayColDEV.Element>{
                new DisplayColDEV.Element(){
                    EType = DisplayColDEV.Element.ElementType.ContactNormalyClosed,
                    Name = "STOP BTN",
                    Row = 0,
                    Col = 1,
                }
            }
        };

        var load = new DisplayColDEV
        {
            ElementsInCol = new List<DisplayColDEV.Element>{
                new DisplayColDEV.Element(){
                    EType = DisplayColDEV.Element.ElementType.Coil,
                    Name = "M0",
                    Row = 0,
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
        public const int DiagramWidth = 80;
        public const int DiagramItemWidth = 10;
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
        public const string NOContact = "---| |----";
        public const string NCContact = "---|/|----";

        public static void AutoRenderTest(IEnumerable<DisplayColDEV> elementCols)
        {
            List<string> lines = new();
            foreach (var col in elementCols)
            {
                for (int i = 0; i < col.ElementsInCol.Count; i++)
                {
                    DisplayColDEV.Element elem = col.ElementsInCol[i];
                    //if(lines[i]) is valid, place the element on that line, else
                    //Add a new line and place the element

                    if (i > lines.Count - 1)
                    {
                        //i is larger then the amount of lines in the lines list, add a new line
                        //and append our element into it
                        lines.Add(elem.ToString());
                        continue;
                    }

                    //We have a line that we can place this element in already, just append it
                    lines[i] += elem.ToString();
                }
            }

            var retString = "";
            foreach (var s in lines) { retString += s + Environment.NewLine; }
            Console.Write(retString);

        }
        public static void TestRenderBranch()
        {
            List<string[]> rows = new();
            //TEST ONE, SIMPLE LATCH
            rows.Add(new string[] { SideBar, Spacer, Name, Name, Spacer, Spacer, Spacer, Name });
            rows.Add(new string[] { SideBar, Wire, NOContact, NCContact, Wire, Wire, Wire, Coil });
            //down bar group
            rows.Add(new string[] { SideBar, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new String[] { SideBar, DownBarSpacer, DownBar, NOContact, DownBar, DownBarSpacer });
            //Spacer
            rows.Add(new String[] { SideBar });
            //Next row Group
            rows.Add(new String[] { SideBar, Spacer, Name, Spacer, Spacer, Spacer, Name });
            rows.Add(new string[] { SideBar, Wire, NOContact, Wire, Wire, "----[NAME5", "67 1234567", " 1234567 ]" });
            //Spacer
            rows.Add(new String[] { SideBar });
            //Row
            rows.Add(new string[] { SideBar, Spacer, Name, Name, Name, Name, Spacer, Spacer, Name });
            rows.Add(new string[] { SideBar, Wire, NOContact, NOContact, NOContact, NOContact, Wire, Wire, Coil });
            rows.Add(new string[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new string[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, NOContact, DownBar, DownBarSpacer });
            rows.Add(new string[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new string[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, NOContact, DownBar, DownBarSpacer });
            rows.Add(new string[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, Name, DownBar, DownBarSpacer });
            rows.Add(new string[] { SideBar, Spacer, Spacer, Spacer, DownBarSpacer, DownBar, NOContact, DownBar, DownBarSpacer });
            //spacer
            rows.Add(new String[] { SideBar });
            //Row
            rows.Add(new string[] { SideBar, Wire, NOContact, NOContact, Wire, Wire, Wire });
            rows.Add(new string[] { SideBar, Spacer, DownBarSpacer, DownBar, Name, Name, DownBar, DownBarSpacer });
            rows.Add(new string[] { SideBar, Spacer, DownBarSpacer, DownBar, NOContact, NOContact, DownBar, DownBarSpacer });



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
            string[] thingsToRender = { NOContact, NCContact, Wire, Coil, LatchCoil, ResetCoil };

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

            public int Row, Col;

            public bool IsLoad;

            // public bool ContinueLeft;
            // public bool ContinueRight;
            public enum ElementType
            {
                ContactNormalyOpen,
                ContactNormalyClosed,
                Coil,
                FunctionBlock
            }

            public override string ToString()
            {
                return EType switch
                {
                    ElementType.Coil => ASSETS.Coil,
                    ElementType.ContactNormalyClosed => ASSETS.NCContact,
                    ElementType.ContactNormalyOpen => ASSETS.NOContact,
                    ElementType.FunctionBlock => "!TODO Function Block",
                    _ => $"{EType} Is Not a valid Element Type"
                };
            }
        }

    }
}