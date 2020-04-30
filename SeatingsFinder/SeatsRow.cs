using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace SeatingsFinder
{
    public class SeatsRow
    {
        public Seat[] RowSeats;
        private int nrSeats;

        public SeatsRow(int rownr, int nrSeatsInRow)
        {
            RowSeats = new Seat[nrSeatsInRow];
            nrSeats = nrSeatsInRow;

            for (int column = 0; column < nrSeatsInRow; column++)
            {
                RowSeats[column] = new Seat(rownr, column);
            }
        }

        public bool ReserveSeats(int begin, int nrSeats)
        {
            bool reservationSuccess = true;

            for (int i = begin; i < begin + nrSeats; i++)
            {
                if (SeatAvailable(i))
                {
                    RowSeats[i].Available = false;
                }
                else
                {
                    reservationSuccess = false;
                }
            }

            return reservationSuccess;
        }

        public bool SeatAvailable(int index)
        {
            if (index >= nrSeats || index < 0)
            {
                return false;
            }

            return RowSeats[index].Available;
        }

        public  bool FindInRow(int nrOfRequestedSeats, bool reserve)
        {
            bool seatFound = false;
            int middleSeat = (int)Math.Floor((decimal)RowSeats.Length / 2);
            int firstSeatToReserve;

            if (RowSeats[middleSeat].Available)
            {
                // Rij is nog volledig leeg als de middelste stoel vrij is. 
                // Dus we gaan de bestelde plaatsen proberen te centreren.
                int halveOfRequestedSeats = (int)Math.Floor((decimal)nrOfRequestedSeats / 2);
                firstSeatToReserve = middleSeat - halveOfRequestedSeats;
                if (FollowSeatsAvailable(firstSeatToReserve) >= nrOfRequestedSeats)
                {
                    seatFound = true;
                }
            }
            else
            {
                //opvullen vanaf het midden
                int currentSeatindex = middleSeat + 1; //middleseat is al gechecked, en was niet vrij
                bool goingRight = true;

                do
                {
                    firstSeatToReserve = currentSeatindex;
                    if (FollowSeatsAvailable(firstSeatToReserve) >= nrOfRequestedSeats)
                    {
                        seatFound = true;
                    }
                    else
                    {
                        seatFound = false;
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
                        currentSeatindex = middleSeat - 1;
                    }
                } while (seatFound == false && currentSeatindex >= 0);
            }

            if (seatFound && reserve)
            {
                ReserveSeats(firstSeatToReserve, nrOfRequestedSeats);
            }
            return seatFound;
        }

        public int FollowSeatsAvailable(int index)
        {
            var count = 0;

            if (SeatAvailable(index))
            {
                var movingIndex = index;
                do
                {
                    count += 1;
                    movingIndex += 1;
                } while (SeatAvailable(movingIndex));
            }
            //Debug.Print(count.ToString());
            return count;
        }
    }
}
