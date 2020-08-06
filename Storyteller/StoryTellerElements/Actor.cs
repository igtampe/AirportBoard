﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SpeechLib;

namespace Igtampe.Storyteller.StoryTellerElements {

    /// <summary>Holds an actor, that can speak synchronously or asynchronously</summary>
    public class Actor {

        /// <summary>Holds the actual SAPI Object</summary>
        private SpVoice SAPI;

        /// <summary>All possible voices for this actor.</summary>
        public ISpeechObjectTokens AllVoices => SAPI.GetVoices();

        /// <summary>Current voice of this actor</summary>
        public SpObjectToken Voice {
            get { return SAPI.Voice; }
            set { SAPI.Voice = value; }
        }

        /// <summary>Gets the name of this actor's voice</summary>
        public string VoiceName => SAPI.Voice.GetDescription().ToString().Split(' ')[1];

        /// <summary>Color of this actor when they speaks</summary>
        public ConsoleColor Color { get; set; }

        /// <summary>Name of this actor</summary>
        public string Name { get; set; }

        /// <summary>Pitch of this actor's voice</summary>
        public int Pitch { get; set; }

        /// <summary>Volume of this actor's voice</summary>
        public int Volume {
            get { return SAPI.Volume; }
            set { SAPI.Volume = value; }
        } 

        /// <summary>How fast this actor will speak</summary>
        public int Rate {
            get { return SAPI.Rate; }
            set { SAPI.Rate = value; }
        }

        /// <summary>Thread that manages asynch speech.</summary>
        private Thread SpeechThread;

        /// <summary>Indicates whether the actor is talking.</summary>
        public bool IsTalking {get {return SpeechThread?.IsAlive == true;}}

        /// <summary>Creates a default speaker object</summary>
        public Actor():this("Default",ConsoleColor.White) { }

        /// <summary>Creates a speaker with the default voice.</summary>
        /// <param name="Name"></param>
        /// <param name="Color"></param>
        public Actor(string Name,ConsoleColor Color) {
            SAPI = new SpVoice();
            this.Name = Name;
            this.Color = Color;
        }
        
        /// <summary>Creates a speaker with the specified voice</summary>
        /// <param name="Name"></param>
        /// <param name="Color"></param>
        /// <param name="Voice"></param>
        public Actor(string Name,ConsoleColor Color,SpObjectToken Voice) {
            SAPI = new SpVoice {Voice = Voice};
            this.Name = Name;
            this.Color = Color;
        }

        /// <summary>Returns a collection of all available voices for this actor</summary>
        /// <returns></returns>
        public ISpeechObjectTokens ListVoices() { return SAPI.GetVoices(); }

        /// <summary>Says the given text</summary>
        /// <param name="Text"></param>
        public void Say(object Text) {SAPI.Speak("<pitch middle = '" + Pitch + "'/> " + Text);}

        public void SayAsync(string Text) {
            if(IsTalking) { throw new ThreadStateException("Actor is talking! Please wait for him to finish talking before having him talk again!"); }
            SpeechThread = new Thread(Say);
            SpeechThread.Start(Text);
        }

    }
}
