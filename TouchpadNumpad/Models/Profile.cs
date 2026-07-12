using System;
using System.Collections.Generic;
using System.Text;

namespace TouchpadNumpad.Models;
public class Profile
{
    public string Name { get; set; } = "";

    public int Rows { get; set; }

    public int Columns { get; set; }

    public List<GridCell> Cells { get; set; } = new();
}