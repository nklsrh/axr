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
using BloomPostprocess;
using Microsoft.DirectX.DirectInput;


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
        public Microsoft.Xna.Framework.Input.KeyboardState previousKeyState, currentKeyState;
        public GamePadState prevState, curState;
        public JoystickState JprevState, JcurState;
        Vector3 cameraSpeed;

        public Texture2D OverlaySprite, SubtitleSprite;

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
            Alpha = 0;

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
                try
                {
                    game.Song = game.Content.Load<Song>("Audio//Music//Car");
                    MediaPlayer.Play(game.Song);
                    MediaPlayer.Volume = 0.2f;

                    if (!game.SpeechEngine.Car && game.SpeechEngine.DialogEffectInstance.State != SoundState.Playing)
                    {
                        game.SpeechEngine.PlayDialog(game, "Car");
                    }
                }
                catch { }
            }
            if (game.screen == Game1.Screen.Track)
            {
                Model = game.Content.Load<Model>("Menu//Environment_about_final");
                Rotation = MathHelper.ToRadians(295f);
                Selection = "team";
                camera.Position.Y = 520000;

                try
                {
                    game.Song = game.Content.Load<Song>("Audio//Music//Car");
                    MediaPlayer.Play(game.Song);
                    MediaPlayer.Volume = 0.2f;

                    if (!game.SpeechEngine.Track && game.SpeechEngine.DialogEffectInstance.State != SoundState.Playing)
                    {
                        game.SpeechEngine.PlayDialog(game, "Track");
                    }
                }
                catch { }
            }
            if (game.screen == Game1.Screen.Main)
            {
                Model = game.Content.Load<Model>("Menu//TechCentre_final");
                Rotation = MathHelper.ToRadians(295f);
                Selection = "race";
                camera.Position = new Vector3(228,50,325);
                try
                {
                    game.Song = game.Content.Load<Song>("Audio//Music//Main");
                    MediaPlayer.Play(game.Song);
                    MediaPlayer.Volume = 1f;
                }
                catch { }
               
            }
           
            if (game.screen == Game1.Screen.WelcomeScreen)
            {
                try
                {
                    game.Song = game.Content.Load<Song>("Audio//Music//WelcomeScreen");
                    MediaPlayer.Play(game.Song);
                    MediaPlayer.Volume = 1f;
                }
                catch { }

               
            }
            if (game.screen == Game1.Screen.Garage)
            {
                Model = game.Content.Load<Model>("Menu//Pit_Entrance");
                camera.Position = new Vector3(-50, 0, 800);
                camera.LookAt.Y = -100f;
                try
                {
                    game.Song = game.Content.Load<Song>("Audio//Music//Main");
                    MediaPlayer.Play(game.Song);
                    MediaPlayer.Volume = 1f;
                }
                catch { }

            }
            try
            {
                OverlaySprite = game.Content.Load<Texture2D>("Menu//Overlays//" + game.screen.ToString());
            }
            catch { }
      

            previousKeyState = currentKeyState;
            prevState = curState;
            JprevState = JcurState;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(Game1 game)
        {
            if (Alpha < 255)
            {
                Alpha++;
            }

            currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One);
            curState = GamePad.GetState(PlayerIndex.One);

            if(!game.DevGamePadMode)
            JcurState = game.wheel.joystick.CurrentJoystickState;

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
            if (game.screen == Game1.Screen.WelcomeScreen)
            {
                WelcomeScreen(game);
            }
            if (game.screen == Game1.Screen.Garage)
            {
                Garage(game);
            }

            previousKeyState = currentKeyState;
            prevState = curState;

            if (!game.DevGamePadMode)
            JprevState = JcurState;
        }
        public void Car(Game1 game)
        {
            if (currentKeyState.IsKeyUp(Keys.Left) && previousKeyState.IsKeyDown(Keys.Left) || curState.DPad.Left == ButtonState.Released && prevState.DPad.Left == ButtonState.Pressed)
            {
                game.SelectionSound.Play();
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
                Alpha = 0;
                Overlay = game.Content.Load<Texture2D>("Menu//CarOverlays//" + Selection);
            }
            if (currentKeyState.IsKeyUp(Keys.Right) && previousKeyState.IsKeyDown(Keys.Right) || curState.DPad.Right == ButtonState.Released && prevState.DPad.Right == ButtonState.Pressed)
            {
                game.SelectionSound.Play();
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
                Alpha = 0;
                Overlay = game.Content.Load<Texture2D>("Menu//CarOverlays//" + Selection);
            }
            camera.Position.X += (650 - camera.Position.X) / 25f;
            camera.Position.Z += (180 - camera.Position.Z) / 25f;
            camera.Position.Y += (100 - camera.Position.Y) / 25f;
            camera.LookAt.Y += (0 - camera.LookAt.Y) / 25f;
            camera.FieldOfView += (50f - camera.FieldOfView) / 25f;

            if (currentKeyState.IsKeyDown(Keys.Up) || curState.Triggers.Right > 0)
            {
                if (Selection == "drivetrain")
                {
                    camera.Position.X += (70 - camera.Position.X) / 25f;
                    camera.Position.Z += (15 - camera.Position.Z) / 25f;
                    camera.FieldOfView += (70f - camera.FieldOfView) / 25f;
                }
                if (Selection == "engine")
                {
                    camera.Position.X += (250 - camera.Position.X) / 25f;
                    camera.Position.Z += (170 - camera.Position.Z) / 25f;
                    camera.Position.Y += (-70 - camera.Position.Y) / 25f;
                    camera.FieldOfView += (70f - camera.FieldOfView) / 25f;
                }
                if (Selection == "pilot")
                {
                    camera.Position.X += (0 - camera.Position.X) / 25f;
                    camera.Position.Z += (0 - camera.Position.Z) / 25f;
                    camera.Position.Y += (-100 - camera.Position.Y) / 25f;
                    camera.FieldOfView += (70f - camera.FieldOfView) / 25f;
                }
                if (Selection == "team")
                {
                    camera.Position.X += (330 - camera.Position.X) / 25f;
                    camera.Position.Z += (110 - camera.Position.Z) / 25f;
                    camera.Position.Y += (-100 - camera.Position.Y) / 25f;
                    camera.FieldOfView += (70f - camera.FieldOfView) / 25f;
                }
            }

            if (currentKeyState.IsKeyDown(Keys.Down) || curState.Triggers.Left > 0)
            {
                camera.Position.X += (330 - camera.Position.X) / 25f;
                camera.Position.Z += (110 - camera.Position.Z) / 25f;
                camera.Position.Y += (300 - camera.Position.Y) / 25f;
                camera.FieldOfView += (80f - camera.FieldOfView) / 25f;
            }

            if (currentKeyState.IsKeyUp(Keys.Escape) && previousKeyState.IsKeyDown(Keys.Escape) || curState.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed)
            {
                game.OptionClicked.Play();
                game.screen = Game1.Screen.Main;
                game.menu.Initialize(game);
            }

            game.bloom.Settings.BaseSaturation += (0.8f - game.bloom.Settings.BaseSaturation) / 25f;
            game.bloom.Settings.BloomIntensity += (2 - game.bloom.Settings.BloomIntensity) / 25f;
            game.bloom.Settings.BlurAmount += (10f - game.bloom.Settings.BlurAmount) / 25f;
            game.bloom.Settings.BaseIntensity += (1f - game.bloom.Settings.BaseIntensity) / 45f;

            if (Selection == "pilot")
            {
                Rotation += (MathHelper.ToRadians(350) - Rotation) / 45f;
                camera.Position.X += (300 - camera.Position.X) / 45f;
                camera.Position.Z += (80 - camera.Position.Z) / 45f;
            }
            if (Selection == "drivetrain")
            {
                Rotation += (MathHelper.ToRadians(120) - Rotation) / 45f;
                camera.Position.X += (300 - camera.Position.X) / 45f;
                camera.Position.Z += (80 - camera.Position.Z) / 45f;
            }
            if (Selection == "team")
            {
                Rotation += (MathHelper.ToRadians(250) - Rotation) / 45f;
            }
            if (Selection == "engine")
            {
                Rotation += (MathHelper.ToRadians(70) - Rotation) / 45f;
            }

            camera.Update(game);
        }
        public void Track(Game1 game)
        {
            camera.Position.Y += (125000 - camera.Position.Y) / 25f;
            if (currentKeyState.IsKeyDown(Keys.Up) || curState.ThumbSticks.Left.Y > 0)
            {
                cameraSpeed.X += (500f - cameraSpeed.X)/24f;
            }
            if (!game.DevGamePadMode)
            {
                if (JprevState.GetPointOfView()[0] == 0000)
                { cameraSpeed.X += (500f - cameraSpeed.X)/24f;}
            }

            if (currentKeyState.IsKeyDown(Keys.Down) || curState.ThumbSticks.Left.Y < 0)
            {
                cameraSpeed.X += (-500f - cameraSpeed.X) / 24f; ;
            }
            if (!game.DevGamePadMode)
            {
                if (JprevState.GetPointOfView()[0] == 18000)
                { cameraSpeed.X += (-500f - cameraSpeed.X) / 24f; }
            }

            if (currentKeyState.IsKeyDown(Keys.Left) || curState.ThumbSticks.Left.X < 0)
            {
                cameraSpeed.Z += (-500f - cameraSpeed.Z) / 24f; ;
            }
            if (!game.DevGamePadMode)
            {
                if (JprevState.GetPointOfView()[0] == 27000)
                { cameraSpeed.Z += (-500f - cameraSpeed.Z) / 24f; }
            }

            if (currentKeyState.IsKeyDown(Keys.Right) || curState.ThumbSticks.Left.X > 0)
            {
                cameraSpeed.Z += (500f - cameraSpeed.Z) / 24f;
            }
            if (!game.DevGamePadMode)
            {
                if (JprevState.GetPointOfView()[0] == 9000)
                { cameraSpeed.Z += (500f - cameraSpeed.Z) / 24f; }
            }

            if (currentKeyState.IsKeyDown(Keys.W) || curState.Triggers.Right > 0) 
            {
                camera.Position.Y += (45000 - camera.Position.Y) / 25f;
            }
            if (!game.DevGamePadMode)
            {
                if (JcurState.Y < 700)
                {
                    camera.Position.Y += (45000 - camera.Position.Y) / 25f;
                }
            }
            if (currentKeyState.IsKeyDown(Keys.S) || curState.Triggers.Left > 0)
            {
                camera.Position.Y += (155000 - camera.Position.Y) / 25f;
            }
            if (!game.DevGamePadMode)
            {
                if (JcurState.Rz < 700)
                { camera.Position.Y += (155000 - camera.Position.Y) / 25f; }
            }

            camera.LookAt += cameraSpeed;
            cameraSpeed += (Vector3.Zero - cameraSpeed) / 25f;

            camera.Position.X = camera.LookAt.X - 200.0f;
            camera.Position.Z = camera.LookAt.Z;

            game.bloom.Settings.BaseSaturation += (1 - game.bloom.Settings.BaseSaturation) / 25f;

            if (currentKeyState.IsKeyUp(Keys.Escape) && previousKeyState.IsKeyDown(Keys.Escape) || curState.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed)
            {
                game.OptionClicked.Play();
                game.screen = Game1.Screen.Main;
                game.menu.Initialize(game);
            }
            if (!game.DevGamePadMode) {
                if (game.wheel.ButtonPressed(3))
                {
                    game.OptionClicked.Play();
                    game.screen = Game1.Screen.Main;
                    game.menu.Initialize(game);
                }
            }

            camera.Update(game);
        }
        public void Main(Game1 game)
        {
            if ((currentKeyState.IsKeyUp(Keys.Left) && previousKeyState.IsKeyDown(Keys.Left)) || (curState.DPad.Left == ButtonState.Released && prevState.DPad.Left == ButtonState.Pressed))
            {
                MainMenuGoLeft(game);
            }

            if (!game.DevGamePadMode)
            {
                if ((JprevState.GetPointOfView()[0] > 18000 && JprevState.GetPointOfView()[0] < 36000) && (JcurState.GetPointOfView()[0] == -1))
                {
                    MainMenuGoLeft(game);
                }
            }

            if (currentKeyState.IsKeyUp(Keys.Right) && previousKeyState.IsKeyDown(Keys.Right) || curState.DPad.Right == ButtonState.Released && prevState.DPad.Right == ButtonState.Pressed)
            {
                MainMenuGoRight(game);
            }

            if (!game.DevGamePadMode)
            {
                if ((JprevState.GetPointOfView()[0] > 0000 && JprevState.GetPointOfView()[0] < 18000) && (JcurState.GetPointOfView()[0] == -1))
                {
                    MainMenuGoRight(game);
                }
            }
            game.bloom.Settings = BloomSettings.PresetSettings[2];

            camera.Position.X += (0 - camera.Position.X) / 25f;
            camera.Position.Z += (70 - camera.Position.Z) / 25f;
            camera.Position.Y += (40 - camera.Position.Y) / 25f;
            camera.LookAt.Y += (20 - camera.LookAt.Y) / 25f;
            camera.FieldOfView += (70f - camera.FieldOfView) / 25f;

            if (currentKeyState.IsKeyDown(Keys.Up) || curState.Triggers.Right > 0)
            {
                camera.Position += (camera.LookAt - camera.Position) / 35f;
                camera.FieldOfView += (90f - camera.FieldOfView) / 25f;
            }
            if (!game.DevGamePadMode)
            {
                if (JcurState.Y < 700)
                {
                    camera.Position += (camera.LookAt - camera.Position) / 35f;
                    camera.FieldOfView += (90f - camera.FieldOfView) / 25f;
                }
            }

            if (currentKeyState.IsKeyDown(Keys.Down) || curState.Triggers.Left > 0)
            {
                camera.FieldOfView += (160f - camera.FieldOfView) / 25f;
            }
            if (!game.DevGamePadMode)
            {
                if (JcurState.Rz < 700)
                {
                    camera.FieldOfView += (160f - camera.FieldOfView) / 25f;
                }
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

         
            if (currentKeyState.IsKeyDown(Keys.D) || curState.ThumbSticks.Right.X < 0)
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

            if (!game.DevGamePadMode)
            {
                if (game.wheel.ButtonPressed(5))
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
            }

            if (currentKeyState.IsKeyDown(Keys.A) || curState.ThumbSticks.Right.X > 0)
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

            if (!game.DevGamePadMode)
            {
                if (game.wheel.ButtonPressed(6))
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
            }

            if (currentKeyState.IsKeyDown(Keys.W) || curState.ThumbSticks.Right.Y < 0)
            {
                    camera.LookAt.Y -= 5;
            }

            if (currentKeyState.IsKeyDown(Keys.S) || curState.ThumbSticks.Right.Y > 0)
            {
                    camera.LookAt.Y += 5;
            }

            if (currentKeyState.IsKeyUp(Keys.Enter) && previousKeyState.IsKeyDown(Keys.Enter) || curState.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed)
            {
                game.OptionClicked.Play();
                if (Selection == "exit")
                {
                    game.Exit();
                }
                if (Selection == "race")
                {
                    //game.screen = Game1.Screen.Game;
                    //game.LoadGame();
                    game.screen = Game1.Screen.Garage;
                    game.menu.Initialize(game);
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

            if (!game.DevGamePadMode)
            {
                if (game.wheel.ButtonPressed(1))
                {
                    game.OptionClicked.Play();
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
            }

            camera.Update(game);
        }
        public void WelcomeScreen(Game1 game)
        {
            game.bloom.Settings = BloomSettings.PresetSettings[8];

            if ((currentKeyState.IsKeyUp(Keys.Enter) && previousKeyState.IsKeyDown(Keys.Enter)) || (curState.Buttons.Start == ButtonState.Released && prevState.Buttons.Start == ButtonState.Pressed))
            {
                game.OptionClicked.Play();
                game.screen = Game1.Screen.Main;

                try
                {
                    if (!game.SpeechEngine.Intro)
                    {
                        game.SpeechEngine.PlayDialog(game, "Intro");
                        game.SpeechEngine.Intro = true;
                    }
                }
                catch { }

                this.Initialize(game);
            }
            if (!game.DevGamePadMode)
            {
                if (game.wheel.ButtonPressed(10))
                {
                    game.OptionClicked.Play();
                    game.screen = Game1.Screen.Main;

                    try
                    {
                        if (!game.SpeechEngine.Intro)
                        {
                            game.SpeechEngine.PlayDialog(game, "Intro");
                            game.SpeechEngine.Intro = true;
                        }
                    }
                    catch { }

                    this.Initialize(game);
                }
            }
        }
        public void Garage(Game1 game)
        {
            camera.Position.X += (50 - camera.Position.X) / 55f;
            camera.Position.Z += (750 - camera.Position.Z) / 55f;
            
            camera.Position.Y += (-60 - camera.Position.Y) / 55f;
            camera.LookAt.Y += (10 - camera.LookAt.Y) / 85f;

            if (currentKeyState.IsKeyDown(Keys.W) || curState.Triggers.Right > 0)
            {
                camera.LookAt.Y += 10.0f;
            }
            if (currentKeyState.IsKeyDown(Keys.S) || curState.Triggers.Right > 0)
            {
                camera.LookAt.Y -= 10.0f;
            }
            if (currentKeyState.IsKeyDown(Keys.A) || curState.Triggers.Right > 0)
            {
                camera.LookAt.Z -= 10.0f;
                camera.LookAt.X -= 10.0f;
            }
            if (currentKeyState.IsKeyDown(Keys.D) || curState.Triggers.Right > 0)
            {
                camera.LookAt.Z += 10.0f;
                camera.LookAt.X += 10.0f;
            }
            game.Window.Title = camera.Position.ToString();
            camera.Update(game);
            
            if (currentKeyState.IsKeyUp(Keys.Escape) && previousKeyState.IsKeyDown(Keys.Escape) || curState.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed)
            {
                game.OptionClicked.Play();
                game.screen = Game1.Screen.Main;
                game.menu.Initialize(game);
            }
        }

        void MainMenuGoLeft(Game1 game)
        {
            game.SelectionSound.Play();
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
        void MainMenuGoRight(Game1 game)
        {
            game.SelectionSound.Play();
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

        public int Alpha;

        public void Draw(Game1 game)
        {
            if (game.screen == Game1.Screen.Car)
            {
                game.GraphicsDevice.Clear(Color.Black);
            }
            if (game.screen == Game1.Screen.Track)
            {
                game.GraphicsDevice.Clear(Color.White);
                camera.NearPlaneDistance = 1000.0f;
                camera.FarPlaneDistance = 1000000.0f; //TO AVOID FLICKERING WHEN LONG DISTANCE!
            }
            if (game.screen == Game1.Screen.Main)
            {
                game.GraphicsDevice.Clear(Color.White);
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

            if (game.screen != Game1.Screen.WelcomeScreen)
            { DrawModel(Model, camera, Matrix.CreateRotationY(Rotation)); }

            if (game.screen == Game1.Screen.Car)
            {
                DrawModel(CarModel, camera, Matrix.CreateRotationY(Rotation));
                game.spriteBatch.Begin();
                game.spriteBatch.Draw(Overlay, game.graphics.GraphicsDevice.Viewport.TitleSafeArea, new Color(255, 255, 255, Alpha));
                game.spriteBatch.End();
            }

            try
            {
                game.spriteBatch.Begin();
                game.spriteBatch.Draw(OverlaySprite, game.graphics.GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                game.spriteBatch.End();
            }
            catch { }
                

                try
                {
                    if (game.SpeechEngine.DialogEffectInstance.State == SoundState.Playing)
                    {
                        game.spriteBatch.Begin();
                        game.spriteBatch.Draw(SubtitleSprite, game.graphics.GraphicsDevice.Viewport.TitleSafeArea, Color.White);
                        game.spriteBatch.End();
                    }
                }
                catch { }

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