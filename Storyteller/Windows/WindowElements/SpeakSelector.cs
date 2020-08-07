using Igtampe.BasicWindows;
using Igtampe.BasicWindows.WindowElements;
using Igtampe.Storyteller.StoryTellerElements;
using SpeechLib;
using System;
using System.Collections.Generic;

namespace Igtampe.Storyteller.Windows.WindowElements {

    /// <summary>Modified LeftRightSelect that when Enter is pressed, will preview the voice.</summary>
    public class SpeakSelector:WindowElement {

        public Actor ReturnActor { get; private set; }
        public LeftRightSelect VoiceSelector;

        public SpeakSelector(Window Parent, string ActorName, ConsoleColor ActorColor, int Pitch, int LeftPos, int TopPos, int Length):base(Parent) {
            ReturnActor = new Actor(ActorName,ActorColor) {Pitch = Pitch}; //ha pitch=pitch fantastic.
            List<string> AllVoices = new List<string>();
            
            foreach(ISpeechObjectToken Voice in ReturnActor.AllVoices) {AllVoices.Add(Voice.GetDescription());}
            VoiceSelector = new LeftRightSelect(Parent,AllVoices,Length,LeftPos,TopPos,ConsoleColor.DarkGray,ConsoleColor.DarkBlue,ConsoleColor.White);
        }

        public override KeyPressReturn OnKeyPress(ConsoleKeyInfo Key) {
            if(Key.Key == ConsoleKey.Enter) {ReturnActor.SayAsync("Hi, I'm " + ReturnActor.VoiceName); return KeyPressReturn.NOTHING; }
            KeyPressReturn RET = VoiceSelector.OnKeyPress(Key);

            ReturnActor.Voice = ReturnActor.AllVoices.Item(VoiceSelector.SelectedItemIndex);

            return RET;
        }

        public override void DrawElement() {
            VoiceSelector.Highlighted = Highlighted;
            VoiceSelector.DrawElement(); 
        }
    }
}
