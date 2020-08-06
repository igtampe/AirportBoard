using Igtampe.BasicWindows;
using Igtampe.BasicWindows.WindowElements;
using Igtampe.Storyteller.StoryTellerElements;
using Igtampe.Storyteller.Windows.WindowElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igtampe.Storyteller.Windows {
    
    /// <summary>Window to select the actor for a part in the play</summary>
    public class AuditionWindow:Window {

        private readonly SpeakSelector Selector;
        public Actor SelectedActor => Selector.ReturnActor;

        public AuditionWindow(string CharacterName,ConsoleColor CharacterColor,int Pitch,string Description):base("Audition Window",54,8) {
            
            Window.WindowClearColor = ConsoleColor.Black;
            Label MainLabel = new Label(this,"Select a voice for ",ConsoleColor.Gray,ConsoleColor.Black,2,2);
            Label CharLabel = new Label(this,CharacterName,ConsoleColor.Gray,CharacterColor,2 + "Select a voice for ".Length,2);
            Selector = new SpeakSelector(this,CharacterName,CharacterColor,Pitch,2,3,50);
            CloseButton OKButton = new CloseButton(this,"[      O K      ]",ConsoleColor.DarkGray,ConsoleColor.White,ConsoleColor.DarkBlue,17,5);
            Button DescButton = new DescriptionButton(this,Description,35,5);

            AllElements.Add(MainLabel);
            AllElements.Add(CharLabel);
            AllElements.Add(Selector);
            AllElements.Add(OKButton);
            AllElements.Add(DescButton);

            Selector.NextElement = OKButton;
            OKButton.PreviousElement = Selector;
            OKButton.NextElement = DescButton;
            DescButton.PreviousElement = OKButton;

            HighlightedElement = Selector;
            Selector.Highlighted = true;

        }

        /// <summary>Button that will show a DialogBox with a given description when pressed.</summary>
        private class DescriptionButton:Button {
            private string Description;

            public DescriptionButton(Window Parent, string Description, int LeftPos, int TopPos):base(Parent,"[  Description  ]",ConsoleColor.DarkGray,ConsoleColor.White,ConsoleColor.DarkBlue,LeftPos,TopPos) {this.Description = Description;}
            public override KeyPressReturn Action() {
                DialogBox.ShowDialogBox(Icon.IconType.INFORMATION,DialogBox.DialogBoxButtons.OK,Description);
                Parent.Redraw();
                return KeyPressReturn.NOTHING;
            }
        }

    }
}
