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
using Microsoft.DirectX.DirectInput;
using BloomPostprocess;

namespace Racing
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Car : Microsoft.Xna.Framework.GameComponent
    {
        public Car(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }
        public Model Model;
        public Vector3 PreviousPosition, PreviousDirection, Position, Direction, Right, Up, Velocity;
        public float Speed, Power, Grip, Angle, TopSpeed, WheelTurn, Turn;
        public Matrix world;
        public TimeSpan CurrentLap, LastLap, FastestLap;
        public int LapCount;
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        /// 
        public Device WheelDevice;

        public override void Initialize()
        {
            // TODO: Add your initialization code here
            Power = 0;
            Speed = 0.05f;
            TopSpeed = 180;
            Grip = 1;
            WheelTurn = 0;
            LapCount = 0;
            PreviousPosition = Position;
            PreviousDirection = Direction;
            Direction = new Vector3(-0.32759f, 0, -0.9448f);
            Up = Vector3.Up;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Game1 game)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            PreviousPosition = Position;
            PreviousDirection = Direction;

            InputAndPhysics(game);

            Matrix rotationMatrix = Matrix.CreateRotationY(Angle);
            // Rotate orientation vectors
            Direction = Vector3.TransformNormal(Direction, rotationMatrix);
            Position += Speed * Direction;

            world = Matrix.Identity;
            world.Forward = Direction;
            world.Up = Vector3.Up;
            world.Right = Vector3.Cross(Up, Direction);
            world.Translation = Position;

            Collisions(game);

            CurrentLap += gameTime.ElapsedGameTime;
            
            base.Update(gameTime);
        }

        void InputAndPhysics(Game1 game)
        {
            game.u1 = game.users.GetUser(0);
            if (Speed < TopSpeed)
            {
                Power = GamePad.GetState(PlayerIndex.One).Triggers.Right;
                if (GamePad.GetState(PlayerIndex.One).Triggers.Right == 0 && Speed > 0) { Speed -= 0.25f; }

                if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up)) { Power++; }

                if (!game.DevGamePadMode)
                {
                    Power = (game.u1.GetLeftStick().Y + 1) / 2.0f;
                }

                Grip = 1 - ((Speed) / TopSpeed);
                Grip = MathHelper.Clamp(Grip, 0.1f, 0.2f);
                Speed += Power;
            }
            else
            {
                Speed = TopSpeed - 1;
            }

            if (Speed > 1)
            {
                Speed -= GamePad.GetState(PlayerIndex.One).Triggers.Left * 2.5f;
                WheelTurn = -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X * 0.7f;

                if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down)) { Speed -= 2.5f; }
                if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left)) { WheelTurn += (1f - WheelTurn) / 2f; }
                if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right)) { WheelTurn += (-1f - WheelTurn) / 2f; }

                if (!game.DevGamePadMode)
                {
                    Speed -= ((game.u1.GetRightStick().Y + 1) / 2.0f) * 2.5f;
                    WheelTurn = -game.u1.GetLeftStick().X;
                }

                Turn += WheelTurn * Grip;
                Angle = MathHelper.ToRadians(Turn);
                Turn += (0 - Turn) / 15f;
            }
        }

        Vector2 MarkerPosition;

        public void Collisions(Game1 game)
        {
            MarkerPosition = new Vector2((int)((Position.X + 102400) / 100), (int)((Position.Z + 102400) / 100));
            Color[] bgColorArr00 = new Color[1];

            game.HeightMap.GetData<Color>(0, new Rectangle((int)MarkerPosition.X, (int)MarkerPosition.Y, 1, 1), bgColorArr00, 0, 1);

            Color bgColor = bgColorArr00[0];
            if (bgColor.R < 200)
            {
                Speed = -Speed * 0.25f;
                game.bloom.Settings = BloomSettings.PresetSettings[7];

                game.chaseCamera.ChaseDirection = PreviousDirection;
                Position = PreviousPosition;
            }
        }

       public void Draw(Model model, ChaseCamera chaseCamera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.FogEnabled = true;
                    effect.FogEnd = 50000.0f;
                    effect.FogColor = new Vector3(0.01f, .011f, .011f);

                    effect.LightingEnabled = true;
                    effect.DiffuseColor = new Vector3(0.7f, 0.7f, 0.7f);
                    effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.SpecularColor = new Vector3(0.02f, 0.02f, 0.02f);
                    effect.TextureEnabled = true;
                    effect.DirectionalLight0.Enabled = true;

                    effect.SpecularPower = 0.001f;

                    effect.PreferPerPixelLighting = true;
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    // Use the matrices provided by the chase camera
                    effect.View = chaseCamera.View * Matrix.CreateRotationZ(Angle * 16f);
                    effect.Projection = chaseCamera.Projection;
                }
                mesh.Draw();
            }
        }

    }
}