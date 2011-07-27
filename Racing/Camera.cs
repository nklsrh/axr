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
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Camera(Game game)
            : base(game)
        {  }

        public Vector3 Position;
        public Vector3 LookAt;
        public float NearPlaneDistance = 1.0f;
        public float FarPlaneDistance = 1000000.0f;
        public float AspectRatio = 18/9;
        public float FieldOfView = 45f;
        public Matrix View;
        public Matrix Projection;
        public Vector3 Up;

        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(Game1 game)
        {
                View = Matrix.CreateLookAt(Position, LookAt, Vector3.Up);
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FieldOfView),
                AspectRatio, NearPlaneDistance, FarPlaneDistance);
        }
    }
}