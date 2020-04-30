using System;
using SeatingsFinder;

namespace SeatingsDehaes
{
    class Program
    {
        static void Main(string[] args)
        {
            CinemaRoom kinezaal = new CinemaRoom(4, 10);
            FindSeats seatsFinder = new FindSeats(kinezaal);
            
            PrintHeader();
            PrintZaal(kinezaal);

            var input = "";
            do
            {
                Console.WriteLine("How many tickets would you like to purchase?");
                input = Console.ReadLine();
                bool inputValid = Int32.TryParse(input, out int nrSeats);
                if (inputValid && nrSeats > 0)
                {
                    var succes = seatsFinder.FindInRoom(nrSeats);
                    if (succes)
                    {
                        PrintZaal(kinezaal);
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find a seat");
                    }
                }
                else
                {
                    if (nrSeats != 0)
                    {
                        Console.WriteLine("That was not a valid number, input 0 to exit or a valid number");
                    }
                }
            } while (input != "0");
            Console.WriteLine("Thanks for your purchases");
            Console.ReadLine();
        }

        static void PrintZaal(CinemaRoom PrintedRoom)
        {
            int printRow;
            
            for (int row = PrintedRoom.GetRows() - 1; row > -1; row--)
            {
                printRow = row + 1;
                Console.Write(printRow + "  ");
                for (int seat = 0; seat < PrintedRoom.GetSeatsPerRow(); seat++)
                {
                    if (PrintedRoom.SeatsRowCollection[row].RowSeats[seat].Available)
                    {
                        Console.Write(" _");
                    }
                    else
                    {
                        Console.Write(" *");
                    }
                }
                Console.WriteLine();
            }
        }

        static void PrintHeader()
        {
            Console.WriteLine("_ = empty seat");
            Console.WriteLine("* = reserved seat");
        }
    }
}
