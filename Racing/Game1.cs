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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Model TrackModel, EnvironModel, Skybox;
        Texture2D Marker;
        Vector2 MarkerPosition;
        
        public BloomComponent bloom;

        public Car car;
        public Texture2D HeightMap;
        Camera camera;
        public ChaseCamera chaseCamera;

        BoundingSphere StartFinishLine, Turn1, TunnelEntrance, OrangeZone, BlueZone, TunnelExit, Chicane;

        public User[] localUsers;
        public IUserInterface users;
        public User u1;

        bool isLapComplete = false;
        public TimeSpan SessionTime;
        
        FrameRateCounter FPSCounter;

        public SpriteFont sfont;

        public bool DevGamePadMode = true;
        public bool GodMode = false;

        public Vector2 DisplayScale;

        public HUD HUD;

        public Rival Rival;

        public enum Screen
        {
            Main,
            Car,
            Track,
            Game
        }

        public Screen screen = Screen.Main;
        public Menu menu;
        public Wheel wheel;

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            GraphicsSettings(false, 1280, 720);

            graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
            DisplayScale = new Vector2(graphics.PreferredBackBufferWidth / 1280, graphics.PreferredBackBufferHeight / 720);

            FPSCounter = new FrameRateCounter(this);
            Components.Add(FPSCounter);

            new UserControl(this);
            BloomPostprocessGame();

            menu = new Menu(this);
        }

        public void GraphicsSettings(bool IsFullScreen, int width, int height)
        {
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        public void LoadGame()
        {
            this.Initialize();
        }

        protected override void Initialize()
        {
            if (screen == Screen.Game)
            {
                camera = new Camera(this);
                camera.Position = new Vector3(3000, 15000, 3000);
                camera.LookAt = Vector3.Zero;

                car = new Car(this);
                car.Position.X = 30740;
                car.Position.Z = 22084;
                car.Position.Y = 1000f;
                car.Direction = new Vector3(-0.32759f, 0, -0.9448f);
                car.Initialize();

                chaseCamera = new ChaseCamera();
                chaseCamera.Up = Vector3.Up;
                chaseCamera.ChasePosition = car.Position;
                chaseCamera.Reset();

                SetupBoundingBoxes();

                car.FastestLap = new TimeSpan(99, 99, 99);
                car.LastLap = new TimeSpan(0, 0, 0);
                car.CurrentLap = new TimeSpan(0, 0, 0);
                car.LapCount = -1;
                isLapComplete = false;

                SessionTime = new TimeSpan(0, 3, 0);

                HUD = new HUD(this);
                HUD.Initialize(this);

                Rival = new Rival(this);
                Rival.random = new Random();
                Rival.Initialize();                           
            }

            if (screen != Screen.Game)
            {
                menu.Initialize(this);
            }

            if (!DevGamePadMode)
            {
                wheel = new Wheel(this);
                wheel.Initialize(this);
            }

            base.Initialize();
        }
    
        
         public void BloomPostprocessGame()
        {
            bloom = new BloomComponent(this);
            //graphics.ToggleFullScreen();
            //Components.Add(bloom);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            users = (IUserInterface)Services.GetService(typeof(IUserInterface));
            FPSCounter.spriteFont = Content.Load<SpriteFont>("sfont");

            if (screen == Screen.Game)
            {
                car.Model = Content.Load<Model>("Cars//AXR3");
                TrackModel = Content.Load<Model>("Track//Track_High_Dynamic");
                EnvironModel = Content.Load<Model>("Track//Environment_Awesome_Textured");
                
                HeightMap = Content.Load<Texture2D>("Zone_HeightMap");
                
                Marker = Content.Load<Texture2D>("OrangeGrunge");
                Skybox = Content.Load<Model>("Skybox");
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (screen == Screen.Game)
            {
                if (users.FindUserOne())
                {
                    u1 = users.GetUser(0);
                }
                if (!DevGamePadMode) { wheel.Update(this, gameTime); }

                if (!GodMode)
                {
                    car.Update(gameTime, this);


                    if (StartFinishLine.Contains(car.Position) == ContainmentType.Contains && !isLapComplete && car.LapCount >= 0)
                    {
                        isLapComplete = true;
                        car.LastLap = car.CurrentLap;
                        if (car.LastLap < car.FastestLap && car.CurrentLap > TimeSpan.Zero)
                        {
                            car.FastestLap = car.LastLap;
                        }
                        car.CurrentLap = TimeSpan.Zero;
                        car.LapCount++;
                    }
                    else
                    {
                        if (StartFinishLine.Contains(car.Position) == ContainmentType.Contains && !isLapComplete && car.LapCount < 0)
                        {
                            car.LapCount = 0;
                            isLapComplete = true;
                            car.CurrentLap = TimeSpan.Zero;
                        }
                    }
                    if (Turn1.Contains(car.Position) == ContainmentType.Contains && isLapComplete)
                    {
                        isLapComplete = false;
                    }
                  

                    if (DevGamePadMode)
                    {
                        chaseCamera.ChasePosition = car.Position;
                        chaseCamera.ChaseDirection = car.Direction;
                        chaseCamera.desiredPositionOffset.Y = 300 * (car.Speed / car.TopSpeed);
                        chaseCamera.desiredPositionOffset.Y = MathHelper.Clamp(chaseCamera.desiredPositionOffset.Y, 200, 300);
                        chaseCamera.desiredPositionOffset.Z = 10 * (car.Speed / car.TopSpeed);
                        chaseCamera.desiredPositionOffset.Z = MathHelper.Clamp(chaseCamera.desiredPositionOffset.Z, 350, 451);
                        chaseCamera.FieldOfView = MathHelper.ToRadians(45 + car.Speed / 3.5f);
                        chaseCamera.Update(gameTime);
                    }
                    else
                    {
                        chaseCamera.ChasePosition = car.Position;
                        chaseCamera.ChaseDirection = car.Direction;
                        chaseCamera.desiredPositionOffset.X = 0;
                        chaseCamera.desiredPositionOffset.Y = 0;
                        chaseCamera.desiredPositionOffset.Z = 0;
                        chaseCamera.Update(gameTime);
                    }

                    Rival.Update(gameTime);

                    SessionTime -= gameTime.ElapsedGameTime;
                }

                if (GodMode)
                {
                    if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                        car.Position.X += 1000;

                    if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                        car.Position.X -= 1000;

                    if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
                        car.Position.Z += 1000;

                    if (Microsoft.Xna.Framework.Input.Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
                        car.Position.Z -= 1000;

                    car.Position.Y = 1000;
                    chaseCamera.ChasePosition = car.Position;
                    chaseCamera.ChaseDirection = car.Direction;
                    chaseCamera.desiredPositionOffset = new Vector3(0, 40000, 0);
                    chaseCamera.DesiredPositionOffset = new Vector3(0, 40000, 0);
                    chaseCamera.Update(gameTime);
                    car.Update(gameTime);

                    Window.Title = car.Position.ToString();
                }
            }
            if (screen != Screen.Game)
            {
                menu.Update(this);
            }
            base.Update(gameTime);
        }

        public void DynamicBloomEffects(GameTime gameTime)
        {
            //DEFAULTS
            bloom.Settings = BloomSettings.PresetSettings[0];

            if (OrangeZone.Contains(car.Position) == ContainmentType.Contains || Turn1.Contains(car.Position) == ContainmentType.Contains) //Orange ZONE Dark
            {
                SmoothBlendingBloom(1);
            }
            if (BlueZone.Contains(car.Position) == ContainmentType.Contains) //BLUE ZONE BRIGHTER
            {
                SmoothBlendingBloom(2);
            }
            if (TunnelEntrance.Contains(car.Position) == ContainmentType.Contains) //TUNNEL GOES DARK
            {
                SmoothBlendingBloom(3);
            }
            if (TunnelExit.Contains(car.Position) == ContainmentType.Contains) //Orange ZONE Dark
            {
                SmoothBlendingBloom(4);
            }     
            if (StartFinishLine.Contains(car.Position) == ContainmentType.Contains) //STARTFINISH BRIGHT FLASH
            {
                SmoothBlendingBloom(5);
            }
            if (Chicane.Contains(car.Position) == ContainmentType.Contains) //Orange ZONE Dark
            {
                SmoothBlendingBloom(6);
            }
        }

        void SmoothBlendingBloom(int index)
        {
            bloom.Settings.BaseIntensity += (BloomSettings.PresetSettings[index].BaseIntensity - bloom.Settings.BaseIntensity)/5f;
            bloom.Settings.BaseSaturation += (BloomSettings.PresetSettings[index].BaseSaturation - bloom.Settings.BaseSaturation) / 5f;
            bloom.Settings.BloomIntensity += (BloomSettings.PresetSettings[index].BloomIntensity - bloom.Settings.BloomIntensity) / 5f;
            bloom.Settings.BloomSaturation += (BloomSettings.PresetSettings[index].BloomSaturation - bloom.Settings.BloomSaturation) / 5f;
            bloom.Settings.BloomThreshold += (BloomSettings.PresetSettings[index].BloomThreshold - bloom.Settings.BloomThreshold) / 5f;
            bloom.Settings.BlurAmount += (BloomSettings.PresetSettings[index].BlurAmount - bloom.Settings.BlurAmount) / 5f;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (screen == Screen.Game)
            {
                GraphicsDevice.Clear(new Color(210, 210, 210));

                GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.Anisotropic;
                GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.Anisotropic;
                GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.Anisotropic;
                GraphicsDevice.SamplerStates[0].MaxAnisotropy = 16;
                GraphicsDevice.SamplerStates[0].MipMapLevelOfDetailBias = -2.0f;

                GraphicsDevice.RenderState.DepthBufferEnable = true;
                GraphicsDevice.RenderState.AlphaBlendEnable = false;
                GraphicsDevice.RenderState.AlphaTestEnable = false;
                GraphicsDevice.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
                GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

                DrawModel(TrackModel, Matrix.Identity);
                DrawModel(EnvironModel, Matrix.Identity);
                DrawModel(Skybox, Matrix.Identity);

                graphics.GraphicsDevice.RenderState.CullMode = CullMode.None;

                if (DevGamePadMode)
                    car.Draw(car.Model, chaseCamera);

                HUD.Draw(this, spriteBatch);

                DynamicBloomEffects(gameTime);
                //DrawDebugShapes(gameTime);
            }
            if (screen != Screen.Game)
            {
                graphics.GraphicsDevice.Clear(Color.White);
                menu.Draw(this);
            }                

                base.Draw(gameTime);
            
        }

        private void DrawModel(Model model, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    if (!GodMode && model != Skybox)
                    {
                        effect.FogEnabled = true;
                        effect.FogEnd = 150000.0f;
                        effect.FogColor = new Vector3(1f, 1f, 1f);

                        effect.LightingEnabled = true;
                        effect.DiffuseColor = new Vector3(0.6f, 0.6f, 0.6f);
                        effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
                        effect.SpecularColor = new Vector3(0.02f, 0.02f, 0.02f);
                        effect.TextureEnabled = true;
                        effect.DirectionalLight0.Enabled = true;

                        effect.SpecularPower = 0.001f;
                    }
                    effect.PreferPerPixelLighting = true;
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    // Use the matrices provided by the chase camera
                    effect.View = chaseCamera.View * Matrix.CreateRotationZ(car.Angle * 2f);

                    effect.Projection = chaseCamera.Projection;
                }
                mesh.Draw();
            }
        }

        void SetupBoundingBoxes()
        {
            TunnelEntrance = new BoundingSphere(new Vector3(62400, 1300, 8100), 4584);
            StartFinishLine = new BoundingSphere(new Vector3(22740, car.Position.Y, 2000), 4584);
            Turn1 = new BoundingSphere(new Vector3(15000, 4000, -15000), 5000);
            OrangeZone = new BoundingSphere(new Vector3(-15000, 4000, 20000), 45000);
            BlueZone = new BoundingSphere(new Vector3(75000, 4000, 14000), 45000);
            TunnelExit = new BoundingSphere(new Vector3(50000, 4000, -2500), 4600);
            Chicane = new BoundingSphere(new Vector3(35000, 4000, 38000), 8000);

            DebugShapeRenderer.Initialize(GraphicsDevice);
        }

        void DrawDebugShapes(GameTime gameTime)
        {
                DebugShapeRenderer.AddBoundingSphere(StartFinishLine, Color.Green);
                DebugShapeRenderer.AddBoundingSphere(Turn1, Color.Red);
                DebugShapeRenderer.AddBoundingSphere(TunnelEntrance, Color.Black);
                DebugShapeRenderer.AddBoundingSphere(BlueZone, Color.Blue);
                DebugShapeRenderer.AddBoundingSphere(OrangeZone, Color.Orange);
                DebugShapeRenderer.AddBoundingSphere(TunnelExit, Color.White);
                DebugShapeRenderer.AddBoundingSphere(Chicane, Color.Purple);
                DebugShapeRenderer.Draw(gameTime, chaseCamera.View, chaseCamera.Projection);
        }
    }
}
