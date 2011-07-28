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
    public class Speech : Microsoft.Xna.Framework.GameComponent
    {
        public Speech(Game game)
            : base(game)
        {
        }
        public SoundEffect DialogEffect;
        public bool Intro, Car, Track, GameIntro, CheckeredFlag, NotQuickEnuf, TimeBeaten, TestEndFail, TestEndPass, TalkToManagement = false;  
        public SoundEffectInstance DialogEffectInstance;

        public override void Initialize()
        {
            base.Initialize();
        }

        public void PlayDialog(Game1 game, string DialogName)
        {
            DialogEffect = game.Content.Load<SoundEffect>("Audio//Speech//" + DialogName);
            DialogEffectInstance =  DialogEffect.CreateInstance();
            DialogEffectInstance.Pan = 0f;
            DialogEffectInstance.Play();
            try
            {
                game.menu.SubtitleSprite = game.Content.Load<Texture2D>("Menu//Overlays//Subtitle" + DialogName);
            }
            catch { }
        }

        public void ResetAllDialogs()
        {
            Intro = false;
            Car = false;
            Track = false;
            GameIntro = false;
            CheckeredFlag  = false;
            NotQuickEnuf = false;
            TimeBeaten = false;
            TestEndFail = false;
            TestEndPass = false;
            TalkToManagement = false;
        }

        public void ResetSpecificDialog(bool Dialog)
        {
            Dialog = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}