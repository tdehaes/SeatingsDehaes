using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SeatingsFinder
{
    public class CinemaRoom
    {
        public SeatsRow[] SeatsRowCollection;
        private int Rows;
        private int Columns;

        public CinemaRoom(int rows, int columns)
        {
            SeatsRowCollection = new SeatsRow[rows];
            for (int rowNr = 0; rowNr < rows; rowNr++)
            {
                SeatsRowCollection[rowNr] = new SeatsRow(rowNr, columns);
            }
            Rows = rows;
            Columns = columns;
        }

        public  int GetSeatsPerRow()
        {
            return Columns;
        }

        public  int GetRows()
        {
            return Rows;
        }
    }
}
