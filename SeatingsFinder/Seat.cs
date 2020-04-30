using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SeatingsFinder
{
    public class Seat
    {
        public int RowNr { get; set; }
        public int ColumnNr { get; set; }
        public bool Available { get; set; }

        public Seat(int rownr, int columnnr)
        {
            RowNr = rownr;
            ColumnNr = columnnr;
            Available = true;
        }
    }
}
