using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SeatingsFinder
{
    public class FindSeats
    {
        private CinemaRoom _cinemaRoom;

        public FindSeats(CinemaRoom cinemaRoom)
        {
            _cinemaRoom = cinemaRoom;
        }

        public bool FindInRoom(int nrOfRequestedSeats)
        {
            // First find full block per row
            if (EnoughSeatsFoundInSingleRow(nrOfRequestedSeats))
            {
                return true;
            }
            //find over multiple rows
            if (FindOverMultipleRows(nrOfRequestedSeats))
            {
                return true;
            }

            return false;
        }

        private bool FindOverMultipleRows(int nrOfRequestedSeats)
        {
            int middleRow = (int)Math.Floor((decimal)_cinemaRoom.GetRows() / 2);
            bool seatFound = false;
            int nrSeats = _cinemaRoom.GetSeatsPerRow();
            int nrRows = _cinemaRoom.GetRows();
            int middleSeat = (int)Math.Floor((decimal)nrSeats / 2);
            bool goingUp = true;
            int currentRowIndex = middleRow;
            int nextRowIndex = middleRow + 1;

            do
            {
                // Debug.Print($"looking in row {rowIndex}");
                int currentSeatindex = middleSeat; //middleseat is al gechecked, en was niet vrij
                bool goingRight = true;

                do
                {
                    var firstSeatToReserve = currentSeatindex;

                    var currentRowFreeSpots = _cinemaRoom.SeatsRowCollection[currentRowIndex].FollowSeatsAvailable(firstSeatToReserve);
                    var nextRowFreeSpots = _cinemaRoom.SeatsRowCollection[nextRowIndex].FollowSeatsAvailable(firstSeatToReserve);
                    //Debug.Print($"On index {firstSeatToReserve} Row {currentRowIndex} has {currentRowFreeSpots} spots and Row {nextRowIndex} has {nextRowFreeSpots} spots");

                    if ((currentRowFreeSpots + nextRowFreeSpots) >= nrOfRequestedSeats)
                    {
                        _cinemaRoom.SeatsRowCollection[currentRowIndex]
                            .ReserveSeats(firstSeatToReserve, currentRowFreeSpots);
                        _cinemaRoom.SeatsRowCollection[nextRowIndex].ReserveSeats(firstSeatToReserve,
                            nrOfRequestedSeats - currentRowFreeSpots);
                        seatFound = true;
                    }

                    if (goingRight)
                    {
                        currentSeatindex += 1;
                    }
                    else
                    {
                        currentSeatindex -= 1;
                    }

                    if (currentSeatindex == nrSeats)
                    {
                        goingRight = false;
                        currentSeatindex = middleSeat;
                    }
                } while (seatFound == false && currentSeatindex >= 0);

                if (goingUp)
                {
                    currentRowIndex += 1;
                    nextRowIndex = currentRowIndex + 1;
                }
                else
                {
                    currentRowIndex -= 1;
                    nextRowIndex = currentRowIndex - 1;
                }

                if (nextRowIndex == nrRows)
                {
                    goingUp = false;
                    currentRowIndex = middleRow;
                    nextRowIndex = currentRowIndex - 1;
                }
            } while (seatFound == false && nextRowIndex >= 0);

            return seatFound;
        }

        private bool EnoughSeatsFoundInSingleRow(int nrOfRequestedSeats)
        {
            bool enoughSeatsFoundInSingleRow;

            int middleRow = (int)Math.Floor((decimal)_cinemaRoom.GetRows() / 2);
            bool goingUp = true;
            int rowIndex = middleRow;

            do
            {
                // Debug.Print($"looking in row {rowIndex}");
                enoughSeatsFoundInSingleRow = _cinemaRoom.SeatsRowCollection[rowIndex].FindInRow(nrOfRequestedSeats, true);

                if (goingUp)
                {
                    rowIndex += 1;
                }
                else
                {
                    rowIndex -= 1;
                }

                if (rowIndex == _cinemaRoom.GetRows())
                {
                    goingUp = false;
                    rowIndex = middleRow - 1;
                }
            } while (enoughSeatsFoundInSingleRow == false && rowIndex >= 0);

            return enoughSeatsFoundInSingleRow;
        }
    }
}
