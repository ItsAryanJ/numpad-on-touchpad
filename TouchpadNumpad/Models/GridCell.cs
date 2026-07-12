using System;
using System.Collections.Generic;
using System.Text;

namespace TouchpadNumpad.Models;

public class GridCell
{
    public int Row { get; set; }

    public int Column { get; set; }

    public string Label { get; set; } = "";

    public CellAction Action { get; set; } = new();
}
