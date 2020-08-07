using AirportBoard;
using Igtampe.BasicGraphics;
using Igtampe.BasicRender;
using Igtampe.Storyteller.Windows;
using System;
using System.Collections.Generic;

namespace Igtampe.Storyteller.StoryTellerElements {
    public class Stage:IABParser {

        private readonly Dictionary<String,Actor> Cast;
        private readonly Actor DefaultActor;

        /// <summary>Sets the stage</summary>
        public Stage(){
            Cast = new Dictionary<string,Actor>();
            DefaultActor = new Actor();
            Cast.Add(DefaultActor.Name,DefaultActor);
        }

        public bool Parse(string Line) {
            string UpperLine = Line.ToUpper();
            if(string.IsNullOrWhiteSpace(UpperLine)) { return false; }

            string[] CommandSplit = Line.Split(' ');

            switch(UpperLine.Split(' ')[0]) {
                case "ADDACTOR":
                    //AddActor Mark F 0 Mark is a guy and he's cool.
                    AuditionWindow ActorWindow = new AuditionWindow(CommandSplit[1], GraphicUtils.ColorCharToConsoleColor(CommandSplit[2][0]),int.Parse(CommandSplit[3]),string.Join(" ",CommandSplit,4,CommandSplit.Length-4));
                    ActorWindow.Execute();
                    Cast.Add(ActorWindow.SelectedActor.Name,ActorWindow.SelectedActor);
                    return true;
                case "HEADER":
                    //Header Mark
                    RenderUtils.Echo(".");
                    RenderUtils.Color(Cast[CommandSplit[1]].Color);
                    String HeaderText = "_[" + CommandSplit[1] + "]_";
                    RenderUtils.Echo(HeaderText);
                    for(int i = 0; i < (Console.WindowWidth-HeaderText.Length); i++) {RenderUtils.Echo("_");}
                    return true;
                case "SPEAK":
                    //Speak Mark Hello I am a person
                    Speak(Cast[CommandSplit[1]],string.Join(" ",CommandSplit,2,CommandSplit.Length - 2));
                    return true;
                case "RATE":
                    //Rate Mark 10
                    Cast[CommandSplit[1]].Rate = int.Parse(CommandSplit[2]);
                    return true;
                case "VOLUME":
                    //Vol Mark 10
                    Cast[CommandSplit[1]].Volume = int.Parse(CommandSplit[2]);
                    return true;
                case "PITCH":
                    //pitch mark 10
                    Cast[CommandSplit[1]].Pitch = int.Parse(CommandSplit[2]);
                    return true;
                case "SETPOS":
                    //Setpos 0 0
                    RenderUtils.SetPos(int.Parse(CommandSplit[1]),int.Parse(CommandSplit[2]));
                    return true;
                default:
                    return false;
            }

        }

        private void Speak(Actor Star,string Speech) {

            //split the sentence.
            String[] SentenceArray = Speech.Split('.','!','?');
            int TextPosition=0;

            RenderUtils.Color(Star.Color);

            foreach(String Sentence in SentenceArray) {
                TextPosition += Sentence.Length;

                string Terminator;
                try {Terminator = Speech[TextPosition] + "";} catch(Exception) { Terminator = "."; }
                

                if(!String.IsNullOrEmpty(Sentence)) {

                    double Delay = 5;
                    //250 is max
                    //2 is min
                    if(Star.Rate > 0) { Delay -= Star.Rate / 3.0; } 
                    else { Delay += (-15 * Star.Rate) ; }

                    Star.SayAsync(Sentence + Terminator);
                    RenderUtils.Type(Sentence + Terminator,Convert.ToInt32(Delay));

                }

                TextPosition++;

                //wait for the start to finish talking.
                while(Star.IsTalking) { RenderUtils.Sleep(5); };

            }

            RenderUtils.Echo("\n\n");

        }

    }
}
