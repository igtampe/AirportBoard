using AirportBoard;
using Igtampe.BasicRender;
using Igtampe.Storyteller.StoryTellerElements;
using System;

namespace Igtampe.Storyteller {
    class Program {
        public static void Main(string[] args) {

            //Set color
            RenderUtils.Color(ConsoleColor.Black,ConsoleColor.Green);

            //Set window size, clear, and set the title
            Console.SetWindowSize(80,25);
            Console.SetBufferSize(80,25);
            Console.Clear();
            Console.Title = "StoryBoard";

            AirportBoard.AirportBoard Board = new AirportBoard.AirportBoard();

            Board.AllParsers.Add(new CoreParser(ref Board));
            Board.AllParsers.Add(new Stage());

            try {Board.Execute(Environment.GetCommandLineArgs());} 
            catch(Exception ex) {GuruMeditationError.LoadErrorPage(ex,Board.CurrentPage,Board.CurrentLine);}

        }

    }
}
