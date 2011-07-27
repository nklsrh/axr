using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Racing
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Rival : Microsoft.Xna.Framework.GameComponent
    {
        public Rival(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public TimeSpan LastLap, FastestLap, CurrentLap;
        public int Skill;
        public Random random = new Random();
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            Skill = random.Next(1,4);
            GenerateLaptime();

            base.Initialize();
        }

        public void GenerateLaptime()
        {
            LastLap = new TimeSpan(0, 0, 0, random.Next(22 + ((4 - Skill) / 2), 30 - ((4 - Skill) / 2)), random.Next(0, 999));
            if (LastLap < FastestLap  && FastestLap > TimeSpan.Zero)
            {
                FastestLap = LastLap;
            }
            if (FastestLap == TimeSpan.Zero)
            {
                FastestLap = LastLap;
            }
            CurrentLap = LastLap;
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}