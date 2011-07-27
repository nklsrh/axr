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
    public class FrameRateCounter : DrawableGameComponent
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        public SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FrameRateCounter(Game game)
            : base(game)
        {
            content = new ContentManager(game.Services);
        }


        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);

            } 
        }


        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
                content.Unload();
        }


        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("FPS: {0}", frameRate);

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.White, 0f, Vector2.Zero, GraphicsDevice.Viewport.Width / 1280f, SpriteEffects.None, 0);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.Black, 0f, Vector2.Zero, GraphicsDevice.Viewport.Width / 1280f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}