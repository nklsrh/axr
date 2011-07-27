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
    public class Menu : Microsoft.Xna.Framework.GameComponent
    {
        public Model Model, CarModel;
        public Texture2D Overlay;
        public float Rotation;
        public string Selection;
        public Camera camera;
        public KeyboardState previousKeyState, currentKeyState;
        Vector3 cameraSpeed;

        public Menu(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize(Game1 game)
        {
            // TODO: Add your initialization code here    
            camera = new Camera(game);
            camera.LookAt = new Vector3(0, -400, 0);

            if (game.screen == Game1.Screen.Car)
            {
                Model = game.Content.Load<Model>("Menu//Garage3");
                CarModel = game.Content.Load<Model>("Cars//AXR3");
                Rotation = 0f;

                Selection = "team";
                Overlay = game.Content.Load<Texture2D>("Menu//CarOverlays//" + Selection);

                camera.Position.Y = 700;
                game.bloom.Settings.BaseIntensity = 2f;
            }
            if (game.screen == Game1.Screen.Track)
            {
                Model = game.Content.Load<Model>("Menu//Environment_about_final");
                Rotation = MathHelper.ToRadians(295f);
                camera.Position.Y = 520000;
            }
            if (game.screen == Game1.Screen.Main)
            {
                Model = game.Content.Load<Model>("Menu//TechCentre_final");
                Rotation = MathHelper.ToRadians(295f);
                Selection = "car";
                camera.Position = new Vector3(228,50,325);
            }

            previousKeyState = currentKeyState;            

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(Game1 game)
        {
            currentKeyState = Keyboard.GetState(PlayerIndex.One);

            if (game.screen == Game1.Screen.Car)
            {
                Car(game);
            }
            if (game.screen == Game1.Screen.Track)
            {
                Track(game);
            }
            if (game.screen == Game1.Screen.Main)
            {
                Main(game);
            }
            previousKeyState = currentKeyState;
        }

        public void Car(Game1 game)
        {
            if (currentKeyState.IsKeyUp(Keys.Left) && previousKeyState.IsKeyDown(Keys.Left))
            {
                if (Selection == "engine")
                {
                    Selection = "drivetrain";

                }
                else
                {
                    if (Selection == "team")
                    {
                        Selection = "pilot";

                    }
                    else
                    {
                        if (Selection == "drivetrain")
                        {
                            Selection = "team";
                        }
                        else
                        {
                            if (Selection == "pilot")
                            {
                                Selection = "engine";
                                Rotation = MathHelper.ToRadians(-10);
                            }
                        }
                    }
                }
                Overlay = game.Content.Load<Texture2D>("Menu//CarOverlays//" + Selection);
            }
            if (currentKeyState.IsKeyUp(Keys.Right) && previousKeyState.IsKeyDown(Keys.Right))
            {
                if (Selection == "drivetrain")
                {
                    Selection = "engine";
                    Rotation = MathHelper.ToRadians(120);
                }
                else
                {
                    if (Selection == "pilot")
                    {
                        Selection = "team";

                    }
                    else
                    {
                        if (Selection == "team")
                        {
                            Selection = "drivetrain";
                        }
                        else
                        {
                            if (Selection == "engine")
                            {
                                Selection = "pilot";
                                Rotation = MathHelper.ToRadians(430);                                
                            }
                        }
                    }
                }
                Overlay = game.Content.Load<Texture2D>("Menu//CarOverlays//" + Selection);
            }
            camera.Position.X += (650 - camera.Position.X) / 25f;
            camera.Position.Z += (180 - camera.Position.Z) / 25f;
            camera.Position.Y += (100 - camera.Position.Y) / 25f;
            camera.LookAt.Y += (0 - camera.LookAt.Y) / 25f;
            camera.FieldOfView += (50f - camera.FieldOfView) / 25f;

            if (currentKeyState.IsKeyDown(Keys.Up))
            {
                if (Selection == "drivetrain")
                {
                    camera.Position.X += (70 - camera.Position.X) / 25f;
                    camera.Position.Z += (15 - camera.Position.Z) / 25f;
                    //camera.FieldOfView += (90f - camera.FieldOfView) / 25f;
                }
                if (Selection == "engine")
                {
                    camera.Position.X += (250 - camera.Position.X) / 25f;
                    camera.Position.Z += (170 - camera.Position.Z) / 25f;
                    camera.Position.Y += (-70 - camera.Position.Y) / 25f;
                    //camera.FieldOfView += (90f - camera.FieldOfView) / 25f;
                }
                if (Selection == "pilot")
                {
                    camera.Position.X += (0 - camera.Position.X) / 25f;
                    camera.Position.Z += (0 - camera.Position.Z) / 25f;
                    camera.Position.Y += (-100 - camera.Position.Y) / 25f;
                    //camera.FieldOfView += (90f - camera.FieldOfView) / 25f;
                }
                if (Selection == "team")
                {
                    camera.Position.X += (330 - camera.Position.X) / 25f;
                    camera.Position.Z += (110 - camera.Position.Z) / 25f;
                    camera.Position.Y += (-100 - camera.Position.Y) / 25f;
                    //camera.FieldOfView += (90f - camera.FieldOfView) / 25f;
                }
            }

            if (currentKeyState.IsKeyDown(Keys.Down))
            {
                camera.Position.X += (330 - camera.Position.X) / 25f;
                camera.Position.Z += (110 - camera.Position.Z) / 25f;
                camera.Position.Y += (300 - camera.Position.Y) / 25f;
                //camera.FieldOfView += (110f - camera.FieldOfView) / 25f;
            }

            if (currentKeyState.IsKeyUp(Keys.Escape) && previousKeyState.IsKeyDown(Keys.Escape))
            {
                game.screen = Game1.Screen.Main;
                game.menu.Initialize(game);
            }

            game.bloom.Settings.BaseSaturation += (0.8f - game.bloom.Settings.BaseSaturation) / 25f;
            game.bloom.Settings.BloomIntensity += (2 - game.bloom.Settings.BloomIntensity) / 25f;
            game.bloom.Settings.BlurAmount += (10f - game.bloom.Settings.BlurAmount) / 25f;
            game.bloom.Settings.BaseIntensity += (1f - game.bloom.Settings.BaseIntensity) / 45f;

            if (Selection == "pilot")
            {
                Rotation += (MathHelper.ToRadians(350) - Rotation) / 25f;
                camera.Position.X += (300 - camera.Position.X) / 25f;
                camera.Position.Z += (80 - camera.Position.Z) / 25f;
            }
            if (Selection == "drivetrain")
            {
                Rotation += (MathHelper.ToRadians(120) - Rotation) / 25f;
                camera.Position.X += (300 - camera.Position.X) / 25f;
                camera.Position.Z += (80 - camera.Position.Z) / 25f;
            }
            if (Selection == "team")
            {
                Rotation += (MathHelper.ToRadians(250) - Rotation) / 25f;
            }
            if (Selection == "engine")
            {
                Rotation += (MathHelper.ToRadians(70) - Rotation) / 25f;
            }

            camera.Update(game);
        }
        public void Track(Game1 game)
        {
            camera.Position.Y += (125000 - camera.Position.Y) / 25f;
            if (currentKeyState.IsKeyDown(Keys.Up))
            {
                cameraSpeed.X = 500f;
            }
            if (currentKeyState.IsKeyDown(Keys.Down))
            {
                cameraSpeed.X = -500f;
            }
            if (currentKeyState.IsKeyDown(Keys.Left))
            {
                cameraSpeed.Z = -500f;
            }
            if (currentKeyState.IsKeyDown(Keys.Right))
            {
                cameraSpeed.Z = 500f;
            }
            if (currentKeyState.IsKeyDown(Keys.W))
            {
                camera.Position.Y += (45000 - camera.Position.Y) / 25f;
            }
            if (currentKeyState.IsKeyDown(Keys.S))
            {
                camera.Position.Y += (155000 - camera.Position.Y) / 25f;
            }

            camera.LookAt += cameraSpeed;
            cameraSpeed += (Vector3.Zero - cameraSpeed) / 25f;

            camera.Position.X += ((camera.LookAt.X - 10000f) - camera.Position.X) / 2f;
            camera.Position.Z += (camera.LookAt.Z - camera.Position.Z) / 5f;

            game.bloom.Settings.BaseSaturation += (1 - game.bloom.Settings.BaseSaturation) / 25f;

            if (currentKeyState.IsKeyUp(Keys.Escape) && previousKeyState.IsKeyDown(Keys.Escape))
            {
                game.screen = Game1.Screen.Main;
                game.menu.Initialize(game);
            }

            camera.Update(game);
        }
        public void Main(Game1 game)
        {
            if (currentKeyState.IsKeyUp(Keys.Left) && previousKeyState.IsKeyDown(Keys.Left))
            {
                if (Selection == "exit")
                {
                    Selection = "track";
                }
                else
                {
                    if (Selection == "track")
                    {
                        Selection = "car";
                    }
                    else
                    {
                        if (Selection == "car")
                        {                          
                                Selection = "race";
                        }
                            else
                            {
                                if (Selection == "race")
                                {
                                    Selection = "exit";
                                }
                            }
                    }
                }
            }
        

            if (currentKeyState.IsKeyUp(Keys.Right) && previousKeyState.IsKeyDown(Keys.Right))
            {
                if (Selection == "track")
                {
                    Selection = "exit";
                }
                else
                {
                    if (Selection == "car")
                    {
                        Selection = "track";
                    }
                    else
                    {
                        if (Selection == "race")
                        {
                            Selection = "car";
                        }
                        else
                        {
                            if (Selection == "exit")
                            {
                                Selection = "race";
                            }
                        }
                    }
                }
            }
            game.bloom.Settings.BloomSaturation += (0.6f - game.bloom.Settings.BloomSaturation) / 25f;
            game.bloom.Settings.BaseSaturation += (1f - game.bloom.Settings.BaseSaturation) / 25f;
            game.bloom.Settings.BloomIntensity += (1.5f - game.bloom.Settings.BloomIntensity) / 25f;

            camera.Position.X += (0 - camera.Position.X) / 25f;
            camera.Position.Z += (70 - camera.Position.Z) / 25f;
            camera.Position.Y += (40 - camera.Position.Y) / 25f;
            camera.LookAt.Y += (20 - camera.LookAt.Y) / 25f;
            camera.FieldOfView += (70f - camera.FieldOfView) / 25f;

            if (currentKeyState.IsKeyDown(Keys.Up))
            {
                camera.Position += (camera.LookAt - camera.Position) / 35f;
                camera.FieldOfView += (90f - camera.FieldOfView) / 25f;
            }
            if (currentKeyState.IsKeyDown(Keys.Down))
            {
                camera.FieldOfView += (160f - camera.FieldOfView) / 25f;
            }

            if (Selection == "exit")
            {
                camera.LookAt += (new Vector3(198, 80, 285) - camera.LookAt) / 25f;
            }
            if (Selection == "race")
            {
                camera.LookAt += (new Vector3(-125, 50, 230) - camera.LookAt) / 25f;
            }
            if (Selection == "track")
            {
                camera.LookAt += (new Vector3(100, 30, -10) - camera.LookAt) / 25f;
            }
            if (Selection == "car")
            {
                camera.LookAt += (new Vector3(-190, 50, -70) - camera.LookAt) / 25f;
            }


            if (currentKeyState.IsKeyUp(Keys.A))
            {
                if (Selection == "car" || Selection == "track")
                {
                    camera.LookAt.X += 4;
                }
                else
                {
                    camera.LookAt.X -= 4;
                }
            }
            if (currentKeyState.IsKeyUp(Keys.D))
            {
                if (Selection == "car" || Selection == "track")
                {
                    camera.LookAt.X -= 4;
                }
                else
                {
                    camera.LookAt.X += 4;
                }
            }
            if (currentKeyState.IsKeyUp(Keys.W))
            {
                    camera.LookAt.Y -= 5;
            }
            if (currentKeyState.IsKeyUp(Keys.S))
            {
                    camera.LookAt.Y += 5;
            }

            if (currentKeyState.IsKeyUp(Keys.Enter) && previousKeyState.IsKeyDown(Keys.Enter))
            {
                if (Selection == "exit")
                {
                    game.Exit();
                }
                if (Selection == "race")
                {
                    game.screen = Game1.Screen.Game;
                    game.LoadGame();
                }
                if (Selection == "track")
                {
                    game.screen = Game1.Screen.Track;
                    game.menu.Initialize(game);
                }
                if (Selection == "car")
                {
                    game.screen = Game1.Screen.Car;
                    game.menu.Initialize(game);
                }
            }

            game.Window.Title = camera.LookAt.ToString();

            camera.Update(game);
        }

        public void Draw(Game1 game)
        {
            if (game.screen == Game1.Screen.Car)
            {
                game.GraphicsDevice.Clear(Color.Black);
            }
            if (game.screen == Game1.Screen.Track)
            {
                game.GraphicsDevice.Clear(Color.White);
            }
            if (game.screen == Game1.Screen.Main)
            {
                game.GraphicsDevice.Clear(Color.Blue);
            }
            game.graphics.PreferMultiSampling = true;
            game.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Anisotropic;
            game.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Anisotropic;
            game.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Anisotropic;
            game.GraphicsDevice.SamplerStates[0].MaxAnisotropy = 16;
            game.GraphicsDevice.SamplerStates[0].MipMapLevelOfDetailBias = -2.0f;

            game.GraphicsDevice.RenderState.DepthBufferEnable = true;
            game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            game.GraphicsDevice.RenderState.AlphaTestEnable = false;
            game.GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            game.GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

            game.graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;
             
            DrawModel(Model, camera, Matrix.CreateRotationY(Rotation));

            if (game.screen == Game1.Screen.Car)
            {
                DrawModel(CarModel, camera, Matrix.CreateRotationY(Rotation));
                game.spriteBatch.Begin();
                game.spriteBatch.Draw(Overlay, game.graphics.GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                game.spriteBatch.End();
            }

            game.graphics.GraphicsDevice.RenderState.CullMode = CullMode.CullClockwiseFace;
        }

        public void DrawModel(Model model, Camera camera, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.FogEnabled = true;
                    effect.FogEnd = 400000.0f;
                    effect.FogColor = new Vector3(1f, 1f, 1f);

                    effect.LightingEnabled = true;
                    effect.DiffuseColor = new Vector3(0.6f, 0.6f, 0.6f);
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.SpecularColor = new Vector3(0.02f, 0.02f, 0.02f);
                    effect.TextureEnabled = true;
                    effect.VertexColorEnabled = false;

                    effect.SpecularPower = 0.001f;
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    // Use the matrices provided by the chase camera
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}