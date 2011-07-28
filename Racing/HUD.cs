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
    public class HUD : Microsoft.Xna.Framework.GameComponent
    {
        public HUD(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        public Texture2D OverlayImage, speedoImage;
        public SpriteFont sfont, HUDfont;
        Rectangle SpeedoMeterRect;
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Game1 game)
        {
            // TODO: Add your initialization code here
            OverlayImage = game.Content.Load<Texture2D>("GlassHUD");
            sfont = game.Content.Load<SpriteFont>("sfont");
            HUDfont = game.Content.Load<SpriteFont>("Fonts//HUDfont");
            speedoImage = game.Content.Load<Texture2D>("OrangeGrunge");

            SpeedoMeterRect.Height = (int)(52 * game.DisplayScale.Y);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            

            base.Update(gameTime);
        }

        public void Draw(Game1 game, SpriteBatch sb)
        {
            SpeedoMeterRect.Location = new Point((int)(1004 * game.DisplayScale.X), (int)(629 * game.DisplayScale.Y));
            SpeedoMeterRect.Width = (int)((game.car.Speed / game.car.TopSpeed) *  250 * game.DisplayScale.X);
            
            sb.Begin();

            sb.Draw(speedoImage, SpeedoMeterRect, Color.White);

            sb.Draw(OverlayImage, game.graphics.GraphicsDevice.Viewport.TitleSafeArea, Color.White);

            sb.DrawString(HUDfont, (game.Rival.FastestLap.Seconds.ToString("00") + "." + game.Rival.FastestLap.Milliseconds.ToString("000")), new Vector2(242, 100) * game.DisplayScale, Color.White, 0f, new Vector2(100, 50), game.DisplayScale, SpriteEffects.None, 0);

            if (game.SessionTime > TimeSpan.Zero)
            {
                sb.DrawString(HUDfont, (game.SessionTime.Minutes.ToString("0") + "." + game.SessionTime.Seconds.ToString("00")), new Vector2(745, 100) * game.DisplayScale, Color.Black, 0f, new Vector2(100, 50), game.DisplayScale, SpriteEffects.None, 0);
            }
            else
            {
                sb.DrawString(HUDfont, "0.00", new Vector2(745, 100) * game.DisplayScale, Color.Red, 0f, new Vector2(100, 50), game.DisplayScale, SpriteEffects.None, 0);
            }

            if (game.car.LapCount >= 0)
            {
                sb.DrawString(HUDfont, game.car.LapCount.ToString(), new Vector2(1270, 100) * game.DisplayScale, Color.Black, 0f, new Vector2(100, 50), game.DisplayScale, SpriteEffects.None, 0);
                sb.DrawString(HUDfont, (game.car.CurrentLap.Seconds.ToString("00") + "." + game.car.CurrentLap.Milliseconds.ToString("000")), new Vector2(243, 690) * game.DisplayScale, Color.Black, 0f, new Vector2(100, 50), game.DisplayScale, SpriteEffects.None, 0);
                sb.DrawString(HUDfont, (game.car.FastestLap.Seconds.ToString("00") + "." + game.car.FastestLap.Milliseconds.ToString("000")), new Vector2(242, 175) * game.DisplayScale, Color.Black, 0f, new Vector2(100, 50), game.DisplayScale, SpriteEffects.None, 0);
            }
            
            sb.End();
        }
    }
}