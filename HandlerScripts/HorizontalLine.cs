using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Match3Test.HandlerScripts
{
    internal class HorizontalLine
    {
        public List<GridCell> Cells = new List<GridCell>();

        public HorizontalLine(GridCell startCell)
        {
            Cells.Add(startCell);
        }
    }

    internal class VerticalLine
    {
        public List<GridCell> Cells = new List<GridCell>();

        public VerticalLine(GridCell startCell)
        {
            Cells.Add(startCell);
        }
    }
}