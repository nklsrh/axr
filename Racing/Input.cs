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
    public class Input : Microsoft.Xna.Framework.GameComponent
    {
        public Input(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here



            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Car car, User u1)
        {
            try
            {
                car.WheelTurn = u1.GetLeftStick().X;
                car.Power += u1.GetRightStick().Y;
                //angleChange += u1.GetLeftStick().X / 40f;
                //cameraAngle.X += u1.GetLeftStick().X / 70f;
                //if (speed < 120)
                //{
                //    speed += u1.GetLeftStick().Y + 1;
                //}
                //else
                //{
                //    speed = 120;
                //}
                //if (u1.GetRightStick().Y > -1 && speed > 35)
                //{
                //    speed -= u1.GetRightStick().Y * 2;
                //}
            }
            catch { }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
            {
                //angle -= MathHelper.ToRadians(1);
            }
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
            {
                //angle += MathHelper.ToRadians(1);
            }
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
            {
                //speed += 1;
            }
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
            {
                //speed -= 1;
            }
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
            {
                //angleChange -= MathHelper.ToRadians(1);
                //cameraAngle.X += ((-0.4f) - cameraAngle.X) / 10f;
            }
            if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
            {
                //angleChange += MathHelper.ToRadians(1);
                //cameraAngle.X += (0.4f - cameraAngle.X) / 10f;
            }
            // TODO: Add your update code her
            base.Update(gameTime);
        }
    }
}