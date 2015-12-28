using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace CoreyGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // The images to draw
        Texture2D personTexture, ship1Texture, ship2Texture, ship3Texture, ship4Texture, mainWepTexture;
        Texture2D blockTexture, block1Texture, block2Texture, blockLaserTexture, block1LaserTexture, block2LaserTexture;
        Texture2D bearsLogo, explode1,grey,blankTexture, health, cooldownTexture, blastRadius;
        Texture2D alt1, alt2, alt3, alt1Actual, alt2Actual, alt3Actual, alt1shot, alt2shot, alt3shot;
        Texture2D blockDrop, block1Drop, block2Drop,title_screen;
        Texture2D boss, bossLaser1, bossLaser2, bossLaser3, bossLaser4, bossLaser5;
        //Viewport viewport = GraphicsDevice.;
        // The color data for the images; used for per-pixel collision
        Color[] personTextureData, ship1TextureData, ship2TextureData, ship3TextureData, ship4TextureData, mainWepTextureData;
        Color[] blockTextureData, block1TextureData, block2TextureData, cooldownTextureData;
        Color[] blockLaserTextureData, block1LaserTextureData, block2LaserTextureData;
        Color[] alt1TextureData, alt2TextureData, alt3TextureData, alt1ActualTextureData, alt2ActualTextureData, alt3ActualTextureData;
        Color[] alt1shotTextureData, alt2shotTextureData, alt3shotTextureData, blastRadiusTextureData;
        Color[] bossTextureData, bossLaser1TextureData, bossLaser2TextureData, bossLaser3TextureData, bossLaser4TextureData, bossLaser5TextureData; 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font, lfont,controls,title,small_title,narrative,narrative_small;
        // Person
        Vector2 personPosition, ship1Position, ship2Position, ship3Position, ship4Position, alt1Locate, alt2Locate, alt3Locate;
        Vector2 logoPosition, bossPos;
        int PersonMoveSpeed = 5;
        int gameState = 0, oldState=0;
        int score = 0, level = 1, shield = 100;
        bool alreadyShot = false;
        bool personHit = false;
        bool win = false, loss = false, bossNow = false, done1=false;
        bool altWeapon1 = false, altWeapon2 = false, altWeapon3 = false;
        // Blocks
        List<Vector2> blockPositions = new List<Vector2>(), block1Positions = new List<Vector2>(), block2Positions = new List<Vector2>();
        List<Vector2> explodePositions = new List<Vector2>(), blastPos = new List<Vector2>();
        List<Texture2D> blastTex = new List<Texture2D>();
        List<Vector2> laserPos = new List<Vector2>();
        List<Vector2> alt1Pos = new List<Vector2>(), alt2Pos = new List<Vector2>(), alt3Pos = new List<Vector2>();
        List<int> explodeCounters = new List<int>(), blastCounters = new List<int>();
        List<Vector2> enemy1laserPos = new List<Vector2>();
        List<Vector2> enemy2laserPos = new List<Vector2>();
        List<Vector2> enemy3laserPos = new List<Vector2>();
        List<Vector2> boss1LaserPos = new List<Vector2>(), boss2LaserPos = new List<Vector2>(), boss3LaserPos = new List<Vector2>(), boss4LaserPos = new List<Vector2>(), boss5LaserPos = new List<Vector2>(); 
        List<Vector2> alt1Drop = new List<Vector2>();
        List<Vector2> alt2Drop = new List<Vector2>();
        List<Vector2> alt3Drop = new List<Vector2>();
        List<int> block2Speed = new List<int>();
        List<int> block1health = new List<int>(), blockhealth = new List<int>(), block2health = new List<int>();
        float BlockSpawnProbability = 0.005f;
        float Block1SpawnProbability = 0.005f;
        float Block2SpawnProbability = 0.005f;
        float BlockShootProb = .005f;
        float Block1ShootProb = .005f;
        float Block2ShootProb = .005f;
        float block1DropProb = .3f, block2DropProb = .3f, blockDropProb = .3f;
        int BlockFallSpeed = 2, mainWepSpeed = 3, alt1ammo=0, alt2ammo=0, alt3ammo=0;
        int alt1Speed = 2, alt2Speed = 2, alt3Speed = 2, cooldown=0, cooldown1=0, cooldown2=0;
        int missilecool1=0, missilecool=0, missilecool2=0, bossHealth = 500, incre = 0;
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue engineSound = null;
        
        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            personPosition.X = (personTexture.Width);
            personPosition.Y = Window.ClientBounds.Height / 2;
            ship1Position.X = Window.ClientBounds.Width / 6;
            ship1Position.Y = 2 * Window.ClientBounds.Height / 6;
            ship2Position.X = (Window.ClientBounds.Width / 6) * 4;
            ship2Position.Y = 2 * Window.ClientBounds.Height / 6;
            ship3Position.X = Window.ClientBounds.Width / 6;
            ship3Position.Y = (5 * Window.ClientBounds.Height / 6) - 70;
            ship4Position.X = (Window.ClientBounds.Width / 6) * 4;
            ship4Position.Y = (5 * Window.ClientBounds.Height / 6) - 70;
            logoPosition.X = 0;
            logoPosition.Y = 0;
            bossPos.X = 5*(Window.ClientBounds.Width/6);
            bossPos.Y = Window.ClientBounds.Height/2;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        private ScrollingBackground myBack;
        protected override void LoadContent()
        {
            myBack = new ScrollingBackground();
            font = Content.Load<SpriteFont>("Font1");
            small_title = Content.Load<SpriteFont>("small_title");
            controls = Content.Load<SpriteFont>("controls");
            narrative = Content.Load<SpriteFont>("narrative");
            narrative_small = Content.Load<SpriteFont>("narrative_small");
            title = Content.Load<SpriteFont>("title");
            lfont = Content.Load<SpriteFont>("LossFont");
            // Load textures
            title_screen = Content.Load<Texture2D>("title_screen");
            block1Texture = Content.Load<Texture2D>("enemy3");
            blockTexture = Content.Load<Texture2D>("enemy2");
            block2Texture = Content.Load<Texture2D>("enemy1");
            boss = Content.Load<Texture2D>("boss_small");
            blockLaserTexture = Content.Load<Texture2D>("enemy1laser");
            block1LaserTexture = Content.Load<Texture2D>("enemy3laser");
            block2LaserTexture = Content.Load<Texture2D>("enemy2laser");
            personTexture = Content.Load<Texture2D>("Mainship");
            ship1Texture = Content.Load<Texture2D>("Mainship");
            ship2Texture = Content.Load<Texture2D>("2");
            ship3Texture = Content.Load<Texture2D>("1");
            ship4Texture = Content.Load<Texture2D>("main1");
            cooldownTexture = Content.Load<Texture2D>("cooldown");
            mainWepTexture = Content.Load<Texture2D>("mainWep");
            bearsLogo = Content.Load<Texture2D>("night-city");
            myBack.Load(GraphicsDevice, bearsLogo);
            grey = Content.Load<Texture2D>("grey");
            blankTexture = Content.Load<Texture2D>("blank");
            explode1 = Content.Load<Texture2D>("explode1");
            blastRadius = Content.Load<Texture2D>("blast");
            alt1 = Content.Load<Texture2D>("alt1");
            alt2 = Content.Load<Texture2D>("alt2");
            alt3 = Content.Load<Texture2D>("alt3");
            alt1shot = Content.Load<Texture2D>("alt1shot");
            alt2shot = Content.Load<Texture2D>("alt2shot");
            alt3shot = Content.Load<Texture2D>("alt3shot");
            health = Content.Load<Texture2D>("health");
            // Extract collision data
            blockTextureData =
                new Color[blockTexture.Width * blockTexture.Height];
            blockTexture.GetData(blockTextureData);
            block1TextureData =
                new Color[block1Texture.Width * block1Texture.Height];
            block1Texture.GetData(block1TextureData);
            block2TextureData =
                new Color[block2Texture.Width * block2Texture.Height];
            block2Texture.GetData(block2TextureData);
            blockLaserTextureData = new Color[blockLaserTexture.Width * blockLaserTexture.Height];
            blockLaserTexture.GetData(blockLaserTextureData);
            block1LaserTextureData = new Color[block1LaserTexture.Width * block1LaserTexture.Height];
            block1LaserTexture.GetData(block1LaserTextureData);
            block2LaserTextureData = new Color[block2LaserTexture.Width * block2LaserTexture.Height];
            block2LaserTexture.GetData(block2LaserTextureData);
            /*bossLaser1TextureData = new Color[blockLaserTexture.Width * blockLaserTexture.Height];
            bossLaser1TextureData = blockLaserTextureData;
            bossLaser1.GetData(bossLaser1TextureData);
            bossLaser1 = blockLaserTexture;
            bossLaser2TextureData = new Color[block1LaserTexture.Width * block1LaserTexture.Height];
            bossLaser2TextureData = block1LaserTextureData;
            bossLaser2.GetData(bossLaser2TextureData);
            bossLaser2 = block1LaserTexture;
            bossLaser3TextureData = new Color[block2LaserTexture.Width * block2LaserTexture.Height];
            bossLaser3TextureData = block2LaserTextureData;
            bossLaser3.GetData(bossLaser3TextureData);
            bossLaser3 = block2LaserTexture;*/
            cooldownTextureData = new Color[cooldownTexture.Width * cooldownTexture.Height];
            cooldownTexture.GetData(cooldownTextureData);
            blastRadiusTextureData = new Color[blastRadius.Width * blastRadius.Height];
            blastRadius.GetData(blastRadiusTextureData);
            personTextureData =
                new Color[personTexture.Width * personTexture.Height];
            personTexture.GetData(personTextureData);
            bossTextureData = new Color[boss.Width * boss.Height];
            boss.GetData(bossTextureData);
            // Create a new SpriteBatch, which can be used to draw textures.
            ship1TextureData = new Color[ship1Texture.Width * ship1Texture.Height];
            ship1Texture.GetData(ship1TextureData);
            ship2TextureData = new Color[ship2Texture.Width * ship2Texture.Height];
            ship2Texture.GetData(ship2TextureData);
            ship3TextureData = new Color[ship3Texture.Width * ship3Texture.Height];
            ship3Texture.GetData(ship3TextureData);
            ship4TextureData = new Color[ship4Texture.Width * ship4Texture.Height];
            ship4Texture.GetData(ship4TextureData);
            mainWepTextureData = new Color[mainWepTexture.Width * mainWepTexture.Height];
            mainWepTexture.GetData(mainWepTextureData);
            alt1TextureData = new Color[alt1.Width * alt1.Height];
            alt1.GetData(alt1TextureData);
           /* bossLaser4TextureData = alt1TextureData;
            bossLaser4.GetData(bossLaser4TextureData);
            bossLaser4 = alt1;*/
            alt2TextureData = new Color[alt2.Width * alt2.Height];
            alt2.GetData(alt2TextureData);
            alt3TextureData = new Color[alt3.Width * alt3.Height];
            alt3.GetData(alt3TextureData);
            /*bossLaser5TextureData = alt3TextureData;
            bossLaser5.GetData(bossLaser5TextureData);
            bossLaser5 = alt3;*/
            alt1shotTextureData = new Color[alt1shot.Width * alt1shot.Height];
            alt1shot.GetData(alt1shotTextureData);
            alt2shotTextureData = new Color[alt2shot.Width * alt2shot.Height];
            alt2shot.GetData(alt2shotTextureData);
            alt3shotTextureData = new Color[alt3shot.Width * alt3shot.Height];
            alt3shot.GetData(alt3shotTextureData);
            blockDrop = alt1;
            block1Drop = alt2;
            block2Drop = alt3;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            audioEngine = new AudioEngine("Content\\Audio\\MyGameAudio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\Sound Bank.xsb");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private float RotationAngle = 0f;
        private Vector2 rotation_point;
        protected override void Update(GameTime gameTime)
        {
            Boolean soundOff = false, altShot1=false, altShot2=false, altShot3=false;
            if (engineSound == null)
            {
                engineSound = soundBank.GetCue("EinhanderThermosphere");
                engineSound.Play();
            }
            KeyboardState keyboard = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            if (gameState > 3 && gameState != 7)
            {
                if (!loss && !win)
                {
                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    myBack.Update(elapsed * 100);
                    if (score >= 1500 && !done1) 
                    {
                        //bossNow=true;
                        win = true;
                        done1 = true;
                    }
                    if (bossNow==true && score >= 1600 && gameState==4)
                    {
                        win = true;
                        bossNow = false;
                    }
                    // Allows the game to exit
                    if (gamePad.Buttons.Back == ButtonState.Pressed)
                        this.Exit();
                    if (keyboard.IsKeyDown(Keys.Up) || gamePad.DPad.Up == ButtonState.Pressed)
                    {
                        personPosition.Y -= PersonMoveSpeed;
                    }
                    if (keyboard.IsKeyDown(Keys.Down) || gamePad.DPad.Down == ButtonState.Pressed)
                    {
                        personPosition.Y += PersonMoveSpeed;
                    }
                    // Move the player left and right with arrow keys or d-pad
                    if (keyboard.IsKeyDown(Keys.Left) ||
                        gamePad.DPad.Left == ButtonState.Pressed)
                    {
                        personPosition.X -= PersonMoveSpeed;
                    }
                    if (keyboard.IsKeyDown(Keys.Right) ||
                        gamePad.DPad.Right == ButtonState.Pressed)
                    {
                        personPosition.X += PersonMoveSpeed;
                    }
                    if (keyboard.IsKeyDown(Keys.Q))
                    {
                        this.Exit();
                    }
                    if (keyboard.IsKeyDown(Keys.C) || gamePad.IsButtonDown(Buttons.B))
                    {
                        float x;
                        if (altWeapon2)
                        {
                            if (!altShot2)
                            {
                                if (alt2Actual.Equals(alt1shot))
                                {
                                    x = alt1shot.Width;
                                    alt2Pos.Add(new Vector2(personPosition.X, personPosition.Y + 45));
                                    alt2ammo--;
                                }
                                else if (alt2Actual.Equals(alt2shot))
                                {
                                    x = alt2shot.Width;
                                    if (missilecool == 0)
                                    {
                                        alt2Pos.Add(new Vector2(personPosition.X, personPosition.Y + 45));
                                        alt2ammo--;
                                        missilecool = 30;
                                    }
                                }
                                else
                                {
                                    x = alt3shot.Width;
                                    if (cooldown == 0)
                                    {
                                        alt2Pos.Add(new Vector2(personPosition.X, personPosition.Y + 45));
                                        alt2ammo--;
                                        cooldown = 300;
                                    }
                                }
                                if (alt2ammo <= 0)
                                {
                                    altWeapon2 = false;
                                }
                                altShot2 = true;
                            }
                        }
                    }
                    if (keyboard.IsKeyUp(Keys.C))
                    {
                        altShot2 = false;
                    }
                    if (keyboard.IsKeyDown(Keys.E)) { score = 1510; }
                    if (keyboard.IsKeyDown(Keys.Z) || gamePad.IsButtonDown(Buttons.A))
                    {
                        if (!alreadyShot)
                        {
                            soundBank.PlayCue("SHOOTING");
                            float x = (mainWepTexture.Width);
                            laserPos.Add(new Vector2(personPosition.X+x, personPosition.Y + 35));
                            alreadyShot = true;
                        }
                    }
                    if (keyboard.IsKeyUp(Keys.Z))
                    {
                        alreadyShot = false;
                    }
                    if (keyboard.IsKeyDown(Keys.X) || gamePad.IsButtonDown(Buttons.X))
                    {
                        float x;
                        if (altWeapon1)
                        {
                            if (!altShot1)
                            {
                                if (alt1Actual.Equals(alt1shot))
                                {
                                    x = alt1shot.Width;
                                    alt1Pos.Add(new Vector2(personPosition.X, personPosition.Y + 20));
                                    alt1ammo--;
                                }
                                else if (alt1Actual.Equals(alt2shot))
                                {
                                    x = alt2shot.Width;
                                    if (missilecool1 == 0)
                                    {
                                        alt1Pos.Add(new Vector2(personPosition.X, personPosition.Y + 20));
                                        alt1ammo--;
                                        missilecool1 = 30;
                                    }
                                }
                                else
                                {
                                    x = alt3shot.Width;
                                    if (cooldown1 == 0)
                                    {
                                        alt1Pos.Add(new Vector2(personPosition.X, personPosition.Y + 20));
                                        alt1ammo--;
                                        cooldown1 = 300;
                                    }
                                }
                                if (alt1ammo <= 0)
                                {
                                    altWeapon1 = false;
                                }
                                altShot1 = true;
                            }
                        }
                    }
                    if (keyboard.IsKeyUp(Keys.X))
                    {
                        altShot1 = false;
                    }
                    if (keyboard.IsKeyDown(Keys.V) || gamePad.IsButtonDown(Buttons.Y))
                    {
                        float x;
                        if ((altWeapon3) && (personTexture.Equals(ship1Texture) || personTexture.Equals(ship4Texture)))
                        {
                            if (!altShot3)
                            {
                                if (alt3Actual.Equals(alt1shot))
                                {
                                    x = alt1shot.Width;
                                    alt3Pos.Add(new Vector2(personPosition.X, personPosition.Y + 30));
                                    alt3ammo--;
                                }
                                else if (alt3Actual.Equals(alt2shot))
                                {
                                    x = alt2shot.Width;
                                    if (missilecool2 == 0)
                                    {
                                        alt3Pos.Add(new Vector2(personPosition.X, personPosition.Y + 30));
                                        alt3ammo--;
                                        missilecool2 = 30;
                                    }
                                }
                                else
                                {
                                    x = alt3shot.Width;
                                    if (cooldown2 == 0)
                                    {
                                        alt3Pos.Add(new Vector2(personPosition.X, personPosition.Y + 30));
                                        alt3ammo--;
                                        cooldown2 = 300;
                                    }
                                }
                                if (alt3ammo <= 0)
                                {
                                    altWeapon3 = false;
                                }
                                altShot3 = true;
                            }
                        }
                    }
                    if (keyboard.IsKeyUp(Keys.V))
                    {
                        altShot3 = false;
                    }
                    if (keyboard.IsKeyDown(Keys.H))
                    {
                        gameState = 2;
                    }
                    if (keyboard.IsKeyDown(Keys.Escape) || gamePad.IsButtonDown(Buttons.Start))
                    {
                        oldState = gameState;
                        gameState = 7;
                    }
                    if (keyboard.IsKeyDown(Keys.S) && !soundOff)
                    {
                        if (engineSound.IsPlaying)
                        {
                            engineSound.Pause();
                            soundOff = true;
                        }
                    }
                    if (keyboard.IsKeyUp(Keys.S))
                    {
                        soundOff = false;
                    }
                    if (keyboard.IsKeyDown(Keys.R))
                    {
                        if (engineSound.IsPaused)
                        {
                            engineSound.Resume();
                        }
                    }
                    // Prevent the person from moving off of the screen
                    personPosition.X = MathHelper.Clamp(personPosition.X,
                    0, Window.ClientBounds.Width - personTexture.Width);
                    personPosition.Y = MathHelper.Clamp(personPosition.Y, 0, Window.ClientBounds.Height - personTexture.Height);

                    // Spawn new falling blocks
                    if (gameState == 5)
                    {
                        if (incre >= 750)
                        {
                            BlockSpawnProbability *= 2;
                            Block1SpawnProbability *= 2;
                            Block2SpawnProbability *= 2;
                            incre = 0;
                        }
                        incre++;
                        if (random.NextDouble() < BlockSpawnProbability)
                        {
                            float x = (float)random.NextDouble() *
                                (Window.ClientBounds.Width - blockTexture.Width);
                            blockPositions.Add(new Vector2(Window.ClientBounds.Width - blockTexture.Width, x));
                            blockhealth.Add(20);
                        }
                        if (random.NextDouble() < Block1SpawnProbability)
                        {
                            float x = (float)random.NextDouble() *
                                (Window.ClientBounds.Width - block1Texture.Width);
                            block1Positions.Add(new Vector2(Window.ClientBounds.Width - blockTexture.Width, x));
                            block1health.Add(20);
                        }
                        if (random.NextDouble() < Block2SpawnProbability)
                        {
                            float x = (float)random.NextDouble() *
                                (Window.ClientBounds.Width - block2Texture.Width) * 2;
                            if (x < Window.ClientBounds.Width / 2) x = Window.ClientBounds.Width / 2;
                            if (x > Window.ClientBounds.Width) x = Window.ClientBounds.Width - block2Texture.Width;
                            block2Positions.Add(new Vector2(x, Window.ClientBounds.Height));
                            block2Speed.Add(3);
                            block2health.Add(20);
                        }
                    }
                    if (gameState == 4 && score < 1500)
                    {
                        if (random.NextDouble() < BlockSpawnProbability)
                        {
                            float x = (float)random.NextDouble() *
                                (Window.ClientBounds.Width - blockTexture.Width);
                            blockPositions.Add(new Vector2(Window.ClientBounds.Width - blockTexture.Width, x));
                            blockhealth.Add(10);
                        }
                        if (random.NextDouble() < Block1SpawnProbability)
                        {
                            float x = (float)random.NextDouble() *
                                (Window.ClientBounds.Width - block1Texture.Width);
                            block1Positions.Add(new Vector2(Window.ClientBounds.Width - blockTexture.Width, x));
                            block1health.Add(10);
                        }
                        if (random.NextDouble() < Block2SpawnProbability)
                        {
                            float x = (float)random.NextDouble() *
                                (Window.ClientBounds.Width - block2Texture.Width) * 2;
                            if (x < Window.ClientBounds.Width / 2) x = Window.ClientBounds.Width / 2;
                            if (x > Window.ClientBounds.Width) x = Window.ClientBounds.Width - block2Texture.Width;
                            block2Positions.Add(new Vector2(x, Window.ClientBounds.Height));
                            block2Speed.Add(3);
                            block2health.Add(10);
                        }
                    }
                    if (gameState==4 && bossNow)
                    {
                        block1Positions.Clear();
                        blockPositions.Clear();
                        block2Positions.Clear();
                        Rectangle bossRectangle = new Rectangle((int)bossPos.X, (int)bossPos.Y,boss.Width, boss.Height);
                        // elapsed time
                        float elapsed1 = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        //rotation
                        RotationAngle += elapsed1;
                        float circle = MathHelper.Pi * 2;
                        RotationAngle = RotationAngle % circle;
                        rotation_point.X = boss.Width / 2;
                        rotation_point.Y = boss.Height / 2;
                        for (int j = 0; j < laserPos.Count; j++)
                        {
                            laserPos[j] =
                            new Vector2(laserPos[j].X + mainWepSpeed,
                                laserPos[j].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)laserPos[j].X, (int)laserPos[j].Y,
                            mainWepTexture.Width, mainWepTexture.Height);
                            if (IntersectPixels(laserRectangle, mainWepTextureData,
                                    bossRectangle, bossTextureData))
                            {
                                laserPos.RemoveAt(j);
                                bossHealth -= 3;
                                //soundBank.PlayCue("Explosion Sound.wav 43431");
                            }
                        }
                        for (int k = 0; k < alt1Pos.Count; k++)
                        {
                            alt1Pos[k] =
                            new Vector2(alt1Pos[k].X + alt1Speed,
                                alt1Pos[k].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt1Pos[k].X, (int)alt1Pos[k].Y,
                            alt1Actual.Width, alt1Actual.Height);
                            if (IntersectPixels(laserRectangle, alt1ActualTextureData,
                                   bossRectangle, bossTextureData))
                            {
                                if (alt1Actual != alt1shot)
                                {
                                    bossHealth -= 25;
                                    alt1Pos.RemoveAt(k);
                                    //soundBank.PlayCue("Explosion Sound.wav 43431");
                                }
                            }
                        }
                        for (int l = 0; l < alt2Pos.Count; l++)
                        {
                            alt2Pos[l] =
                            new Vector2(alt2Pos[l].X + alt2Speed,
                                alt2Pos[l].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt2Pos[l].X, (int)alt2Pos[l].Y,
                            alt2Actual.Width, alt2Actual.Height);
                            if (IntersectPixels(laserRectangle, alt2ActualTextureData,
                                   bossRectangle, bossTextureData))
                            {
                                if (alt2Actual != alt1shot)
                                {
                                    bossHealth -= 25;
                                    alt2Pos.RemoveAt(l);
                                    //soundBank.PlayCue("Explosion Sound.wav 43431");
                                }
                            }
                        }
                        for (int m = 0; m < alt3Pos.Count; m++)
                        {
                            alt3Pos[m] =
                            new Vector2(alt3Pos[m].X + alt3Speed,
                                alt3Pos[m].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt3Pos[m].X, (int)alt3Pos[m].Y,
                            alt3Actual.Width, alt3Actual.Height);
                            if (IntersectPixels(laserRectangle, alt3ActualTextureData,
                                   bossRectangle, bossTextureData))
                            {
                                if (alt3Actual != alt1shot)
                                {
                                    bossHealth -= 25;
                                    alt3Pos.RemoveAt(m);
                                    //soundBank.PlayCue("Explosion Sound.wav 43431");
                                }
                            }
                        }
                        if (bossHealth <= 0)
                        {
                            Vector2 bossDeath = bossPos;
                            bossDeath.X -= boss.Width/2;
                            bossDeath.Y -= boss.Height/2;
                            blastPos.Add(bossDeath);
                            soundBank.PlayCue("Explosion Sound.wav 43431");
                            score += 100;
                        }
                    }
                    // Get the bounding rectangle of the person
                    Rectangle personRectangle =
                        new Rectangle((int)personPosition.X, (int)personPosition.Y,
                        personTexture.Width, personTexture.Height);
                    personHit = false;
                    // Update each block
                    for (int i = 0; i < blockPositions.Count; i++)
                    {
                        if (random.NextDouble() < BlockShootProb)
                        {
                            enemy1laserPos.Add(new Vector2(blockPositions[i].X, blockPositions[i].Y));
                            soundBank.PlayCue("LASERBUR");
                        }
                        // Animate this block falling
                        blockPositions[i] =
                            new Vector2(blockPositions[i].X - BlockFallSpeed,
                                blockPositions[i].Y);
                        // Get the bounding rectangle of this block
                        Rectangle blockRectangle =
                            new Rectangle((int)blockPositions[i].X, (int)blockPositions[i].Y,
                            blockTexture.Width, blockTexture.Height);
                        // Check collision with person
                        if (IntersectPixels(personRectangle, personTextureData,
                                    blockRectangle, blockTextureData))
                        {
                            explodePositions.Add(blockPositions[i]);
                            explodeCounters.Add(15);
                            soundBank.PlayCue("Explosion Sound.wav 43431");
                            personHit = true;
                            if (personTexture.Equals(ship1Texture))
                            {
                                shield -= 10;
                            }
                            else if (personTexture.Equals(ship3Texture))
                            {
                                shield -= 7;
                            }
                            else
                            {
                                shield -=5;
                            }
                            if (shield <= 0)
                            {
                                if (gameState == 4)
                                {
                                    loss = true;
                                }
                                else { gameState = 11; }
                            }
                            blockPositions.RemoveAt(i);
                        }
                        // Remove this block if it have fallen off the screen
                        if (blockPositions != null && i > 0 && i < blockPositions.Count)
                        {
                            if (blockPositions[i].X <= 0 && blockPositions[i] != null)
                            {
                                blockPositions.RemoveAt(i);

                                // When removing a block, the next block will have the same index
                                // as the current block. Decrement i to prevent skipping a block.
                                i--;
                                if (i < 0) break;
                            }
                        }
                        for (int j = 0; j < laserPos.Count; j++)
                        {
                            laserPos[j] =
                            new Vector2(laserPos[j].X + mainWepSpeed,
                                laserPos[j].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)laserPos[j].X, (int)laserPos[j].Y,
                            mainWepTexture.Width, mainWepTexture.Height);
                            if (IntersectPixels(laserRectangle, mainWepTextureData,
                                    blockRectangle, blockTextureData))
                            {
                                //laserPos.Clear();
                                laserPos.RemoveAt(j);
                                blockhealth[i] = blockhealth[i] - 5;
                                if (blockPositions != null && i >= 0 && i < blockPositions.Count && blockhealth[i] <=0)
                                {
                                    if (blockPositions[i] != null)
                                    {
                                        explodePositions.Add(blockPositions[i]);
                                        explodeCounters.Add(15);
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        if (random.NextDouble() < blockDropProb)
                                        {
                                            alt1Drop.Add(new Vector2(blockPositions[i].X, blockPositions[i].Y));
                                        }
                                        blockPositions.RemoveAt(i);
                                        blockhealth.RemoveAt(i);
                                        score += 5;
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < alt1Pos.Count; k++)
                        {
                            alt1Pos[k] =
                            new Vector2(alt1Pos[k].X + alt1Speed,
                                alt1Pos[k].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt1Pos[k].X, (int)alt1Pos[k].Y,
                            alt1Actual.Width, alt1Actual.Height);
                            if (IntersectPixels(laserRectangle, alt1ActualTextureData,
                                    blockRectangle, blockTextureData))
                            {
                                if (blockPositions != null && i >= 0 && i < blockPositions.Count)
                                {
                                    if (blockPositions[i] != null)
                                    {
                                        if (alt1Actual.Equals(alt2shot))
                                        {
                                            alt1Pos.RemoveAt(k);
                                            blastCounters.Add(50);
                                            blastPos.Add(blockPositions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(blockPositions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        blockPositions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int l = 0; l < alt2Pos.Count; l++)
                        {
                            alt2Pos[l] =
                            new Vector2(alt2Pos[l].X + alt2Speed,
                                alt2Pos[l].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt2Pos[l].X, (int)alt2Pos[l].Y,
                            alt2Actual.Width, alt2Actual.Height);
                            if (IntersectPixels(laserRectangle, alt2ActualTextureData,
                                    blockRectangle, blockTextureData))
                            {
                                if (blockPositions != null && i >= 0 && i < blockPositions.Count)
                                {
                                    if (blockPositions[i] != null)
                                    {
                                        if (alt2Actual.Equals(alt2shot))
                                        {
                                            alt2Pos.RemoveAt(l);
                                            blastCounters.Add(50);
                                            blastPos.Add(blockPositions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(blockPositions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        blockPositions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int m = 0; m < alt3Pos.Count; m++)
                        {
                            alt3Pos[m] =
                            new Vector2(alt3Pos[m].X + alt3Speed,
                                alt3Pos[m].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt3Pos[m].X, (int)alt3Pos[m].Y,
                            alt3Actual.Width, alt3Actual.Height);
                            if (IntersectPixels(laserRectangle, alt3ActualTextureData,
                                    blockRectangle, blockTextureData))
                            {
                                if (blockPositions != null && i >= 0 && i < blockPositions.Count)
                                {
                                    if (blockPositions[i] != null)
                                    {
                                        if (alt3Actual.Equals(alt2shot))
                                        {
                                            alt3Pos.RemoveAt(m);
                                            blastCounters.Add(50);
                                            blastPos.Add(blockPositions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(blockPositions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        blockPositions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int n = 0; n < blastPos.Count; n++)
                        {
                            Rectangle rec = new Rectangle((int)blastPos[n].X, (int)blastPos[n].Y, blastRadius.Width, blastRadius.Height);
                            if (IntersectPixels(rec, blastRadiusTextureData,
                                    blockRectangle, blockTextureData))
                            {
                                if (blockPositions != null && i >= 0 && i < blockPositions.Count)
                                {
                                    if (blockPositions[i] != null)
                                    {
                                        explodeCounters.Add(15);
                                        explodePositions.Add(blockPositions[i]);
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        blockPositions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < block1Positions.Count; i++)
                    {
                        if (random.NextDouble() < Block1ShootProb)
                        {
                            enemy2laserPos.Add(new Vector2(block1Positions[i].X, block1Positions[i].Y));
                            soundBank.PlayCue("LASERBUR");
                        }
                        // Animate this block falling
                        block1Positions[i] =
                             new Vector2(block1Positions[i].X - BlockFallSpeed,
                                 block1Positions[i].Y +
                                 (float)Math.Sin(block1Positions[i].X / 40)
                                 * 5);
                        // Get the bounding rectangle of this block
                        Rectangle block1Rectangle =
                            new Rectangle((int)block1Positions[i].X, (int)block1Positions[i].Y,
                            block1Texture.Width, block1Texture.Height);
                        // Check collision with person
                        if (IntersectPixels(personRectangle, personTextureData,
                                    block1Rectangle, block1TextureData))
                        {
                            explodePositions.Add(block1Positions[i]);
                            explodeCounters.Add(15);
                            soundBank.PlayCue("Explosion Sound.wav 43431");
                            personHit = true;
                            if (personTexture.Equals(ship1Texture))
                            {
                                shield -= 10;
                            }
                            else if (personTexture.Equals(ship3Texture))
                            {
                                shield -= 7;
                            }
                            else
                            {
                                shield -= 5;
                            }
                            if (shield <= 0)
                            {
                                if (gameState == 4)
                                {
                                    loss = true;
                                }
                                else { gameState = 11; }
                            }
                            block1Positions.RemoveAt(i);
                        }
                        // Remove this block if it have fallen off the screen
                        if (block1Positions != null && i >= 0 && i < block1Positions.Count)
                        {
                            if (block1Positions[i].X <= 0 && block1Positions[i] != null)
                            {
                                block1Positions.RemoveAt(i);

                                // When removing a block, the next block will have the same index
                                // as the current block. Decrement i to prevent skipping a block.
                                i--;
                                if (i < 0) break;
                            }
                        }
                        for (int j = 0; j < laserPos.Count; j++)
                        {
                            laserPos[j] =
                            new Vector2(laserPos[j].X + mainWepSpeed,
                                laserPos[j].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)laserPos[j].X, (int)laserPos[j].Y,
                            mainWepTexture.Width, mainWepTexture.Height);
                            if (IntersectPixels(laserRectangle, mainWepTextureData,
                                    block1Rectangle, block1TextureData))
                            {
                                //laserPos.Clear();
                                laserPos.RemoveAt(j);
                                block1health[i] = block1health[i] - 5;
                                if (block1Positions != null && i >= 0 && i < block1Positions.Count && block1health[i] <= 0)
                                {
                                    if (block1Positions[i] != null)
                                    {
                                        explodePositions.Add(block1Positions[i]);
                                        explodeCounters.Add(15);
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        if (random.NextDouble() < block1DropProb)
                                        {
                                            alt2Drop.Add(new Vector2(block1Positions[i].X, block1Positions[i].Y));
                                        }
                                        block1Positions.RemoveAt(i);
                                        block1health.RemoveAt(i);
                                        score += 10;
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < alt1Pos.Count; k++)
                        {
                            alt1Pos[k] =
                            new Vector2(alt1Pos[k].X + alt1Speed,
                                alt1Pos[k].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt1Pos[k].X, (int)alt1Pos[k].Y,
                            alt1Actual.Width, alt1Actual.Height);
                            if (IntersectPixels(laserRectangle, alt1ActualTextureData,
                                    block1Rectangle, block1TextureData))
                            {
                                if (block1Positions != null && i >= 0 && i < block1Positions.Count)
                                {
                                    if (block1Positions[i] != null)
                                    {
                                        if (alt1Actual.Equals(alt2shot))
                                        {
                                            alt1Pos.RemoveAt(k);
                                            blastCounters.Add(50);
                                            blastPos.Add(block1Positions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(block1Positions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block1Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int l = 0; l < alt2Pos.Count; l++)
                        {
                            alt2Pos[l] =
                            new Vector2(alt2Pos[l].X + alt2Speed,
                                alt2Pos[l].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt2Pos[l].X, (int)alt2Pos[l].Y,
                            alt2Actual.Width, alt2Actual.Height);
                            if (IntersectPixels(laserRectangle, alt2ActualTextureData,
                                    block1Rectangle, block1TextureData))
                            {
                                if (block1Positions != null && i >= 0 && i < block1Positions.Count)
                                {
                                    if (block1Positions[i] != null)
                                    {
                                        if (alt2Actual.Equals(alt2shot))
                                        {
                                            alt2Pos.RemoveAt(l);
                                            blastCounters.Add(50);
                                            blastPos.Add(block1Positions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(block1Positions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block1Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int m = 0; m < alt3Pos.Count; m++)
                        {
                            alt3Pos[m] =
                            new Vector2(alt3Pos[m].X + alt3Speed,
                                alt3Pos[m].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt3Pos[m].X, (int)alt3Pos[m].Y,
                            alt3Actual.Width, alt3Actual.Height);
                            if (IntersectPixels(laserRectangle, alt3ActualTextureData,
                                    block1Rectangle, block1TextureData))
                            {
                                if (block1Positions != null && i >= 0 && i < block1Positions.Count)
                                {
                                    if (block1Positions[i] != null)
                                    {
                                        if (alt3Actual.Equals(alt2shot))
                                        {
                                            alt3Pos.RemoveAt(m);
                                            blastCounters.Add(50);
                                            blastPos.Add(block1Positions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(block1Positions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block1Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int n = 0; n < blastPos.Count; n++)
                        {
                            Rectangle rec = new Rectangle((int)blastPos[n].X, (int)blastPos[n].Y, blastRadius.Width, blastRadius.Height);
                            if (IntersectPixels(rec, blastRadiusTextureData,
                                    block1Rectangle, block1TextureData))
                            {
                                if (block1Positions != null && i >= 0 && i < block1Positions.Count)
                                {
                                    if (block1Positions[i] != null)
                                    {
                                        explodeCounters.Add(15);
                                        explodePositions.Add(block1Positions[i]);
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block1Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < block2Positions.Count; i++)
                    {
                        if (random.NextDouble() < Block2ShootProb)
                        {
                            enemy3laserPos.Add(new Vector2(block2Positions[i].X, block2Positions[i].Y));
                            soundBank.PlayCue("LASERBUR");
                        }
                        // Animate this block falling
                        block2Positions[i] =
                            new Vector2(block2Positions[i].X,
                                block2Positions[i].Y + block2Speed[i]);
                        if (block2Positions[i].Y >= Window.ClientBounds.Height)
                        {
                            block2Speed[i] = -3;
                        }
                        if (block2Positions[i].Y <= 0)
                        {
                            block2Speed[i] = 3;
                        }
                        // Get the bounding rectangle of this block
                        Rectangle block2Rectangle =
                            new Rectangle((int)block2Positions[i].X, (int)block2Positions[i].Y,
                            block2Texture.Width, block2Texture.Height);
                        // Check collision with person
                        if (IntersectPixels(personRectangle, personTextureData,
                                    block2Rectangle, block2TextureData))
                        {
                            
                            explodePositions.Add(block2Positions[i]);
                            explodeCounters.Add(15);
                            soundBank.PlayCue("Explosion Sound.wav 43431");
                            personHit = true;
                            if (personTexture.Equals(ship1Texture))
                            {
                                shield -= 10;
                            }
                            else if (personTexture.Equals(ship3Texture))
                            {
                                shield -= 7;
                            }
                            else
                            {
                                shield -= 5;
                            }
                            if (shield <= 0)
                            {
                                if (gameState == 4)
                                {
                                    loss = true;
                                }
                                else { gameState = 11; }
                            }
                            block2Positions.RemoveAt(i);
                        }
                        // Remove this block if it have fallen off the screen
                        if (block2Positions != null && i >=0 && i < block2Positions.Count)
                        {
                            if (block2Positions[i].X <= 0 && block2Positions[i] != null)
                            {
                                block2Positions.RemoveAt(i);
                                block2Speed.RemoveAt(i);
                                // When removing a block, the next block will have the same index
                                // as the current block. Decrement i to prevent skipping a block.
                                i--;
                                if (i < 0) break;
                            }
                        }
                        for (int j = 0; j < laserPos.Count; j++)
                        {
                            laserPos[j] =
                            new Vector2(laserPos[j].X + mainWepSpeed,
                                laserPos[j].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)laserPos[j].X, (int)laserPos[j].Y,
                            mainWepTexture.Width, mainWepTexture.Height);
                            if (IntersectPixels(laserRectangle, mainWepTextureData,
                                    block2Rectangle, block2TextureData))
                            {
                                //laserPos.Clear();
                                laserPos.RemoveAt(j);
                                block2health[i] = block2health[i] - 5;
                                if (block2Positions != null && i >= 0 && i < block2Positions.Count && block2health[i] <= 0)
                                {
                                    if (block2Positions[i] != null)
                                    {
                                        explodePositions.Add(block2Positions[i]);
                                        explodeCounters.Add(15);
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        if (random.NextDouble() < block2DropProb)
                                        {
                                            alt3Drop.Add(new Vector2(block2Positions[i].X, block2Positions[i].Y));
                                        }
                                        block2Positions.RemoveAt(i);
                                        block2health.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < alt1Pos.Count; k++)
                        {
                            alt1Pos[k] =
                            new Vector2(alt1Pos[k].X + alt1Speed,
                                alt1Pos[k].Y);
                            Rectangle laser3Rectangle =
                            new Rectangle((int)alt1Pos[k].X, (int)alt1Pos[k].Y,
                            alt1Actual.Width, alt1Actual.Height);
                            if (IntersectPixels(laser3Rectangle, alt1ActualTextureData,
                                    block2Rectangle, block2TextureData))
                            {
                                if (block2Positions != null && i >= 0 && i < block2Positions.Count)
                                {
                                    if (block2Positions[i] != null)
                                    {
                                        if (alt1Actual.Equals(alt2shot))
                                        {
                                            alt1Pos.RemoveAt(k);
                                            blastCounters.Add(50);
                                            blastPos.Add(block2Positions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(block2Positions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block2Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int l = 0; l < alt2Pos.Count; l++)
                        {
                            alt2Pos[l] =
                            new Vector2(alt2Pos[l].X + alt2Speed,
                                alt2Pos[l].Y);
                            Rectangle laser2Rectangle =
                            new Rectangle((int)alt2Pos[l].X, (int)alt2Pos[l].Y,
                            alt2Actual.Width, alt2Actual.Height);
                            if (IntersectPixels(laser2Rectangle, alt2ActualTextureData,
                                    block2Rectangle, block2TextureData))
                            {
                                if (block2Positions != null && i >= 0 && i < block2Positions.Count)
                                {
                                    if (block2Positions[i] != null)
                                    {
                                        if (alt2Actual.Equals(alt2shot))
                                        {
                                            alt2Pos.RemoveAt(l);
                                            blastCounters.Add(50);
                                            blastPos.Add(block2Positions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(block2Positions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block2Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int m = 0; m < alt3Pos.Count; m++)
                        {
                            alt3Pos[m] =
                            new Vector2(alt3Pos[m].X + alt3Speed,
                                alt3Pos[m].Y);
                            Rectangle laserRectangle =
                            new Rectangle((int)alt3Pos[m].X, (int)alt3Pos[m].Y,
                            alt3Actual.Width, alt3Actual.Height);
                            if (IntersectPixels(laserRectangle, alt3ActualTextureData,
                                    block2Rectangle, block2TextureData))
                            {
                                if (block2Positions != null && i >= 0 && i < block2Positions.Count)
                                {
                                    if (block2Positions[i] != null)
                                    {
                                        if (alt3Actual.Equals(alt2shot))
                                        {
                                            alt3Pos.RemoveAt(m);
                                            blastCounters.Add(50);
                                            blastPos.Add(block2Positions[i]);
                                        }
                                        else
                                        {
                                            explodeCounters.Add(15);
                                            explodePositions.Add(block2Positions[i]);
                                        }
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block2Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                        for (int n = 0; n < blastPos.Count; n++)
                        {
                            Rectangle rec = new Rectangle((int)blastPos[n].X, (int)blastPos[n].Y, blastRadius.Width, blastRadius.Height);
                            if (IntersectPixels(rec, blastRadiusTextureData,
                                    block2Rectangle, block2TextureData))
                            {
                                if (block2Positions != null && i >= 0 && i < block2Positions.Count)
                                {
                                    if (block2Positions[i] != null)
                                    {
                                        explodeCounters.Add(15);
                                        explodePositions.Add(block2Positions[i]);
                                        soundBank.PlayCue("Explosion Sound.wav 43431");
                                        block2Positions.RemoveAt(i);
                                        score += 15;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < enemy1laserPos.Count; i++)
                    {
                        enemy1laserPos[i] = new Vector2(enemy1laserPos[i].X - mainWepSpeed * 2, enemy1laserPos[i].Y);
                        Rectangle blockLaserRectangle = new Rectangle((int)enemy1laserPos[i].X, (int)enemy1laserPos[i].Y,
                            blockLaserTexture.Width, blockLaserTexture.Height);
                        if (IntersectPixels(personRectangle, personTextureData,
                                    blockLaserRectangle, blockLaserTextureData))
                        {
                            personHit = true;
                            if (personTexture.Equals(ship1Texture))
                            {
                                shield -= 10;
                            }
                            else if (personTexture.Equals(ship3Texture))
                            {
                                shield -= 7;
                            }
                            else
                            {
                                shield -= 5;
                            }
                            if (shield <= 0)
                            {
                                if (gameState == 4)
                                {
                                    loss = true;
                                }
                                else { gameState = 11; }
                            }
                            enemy1laserPos.RemoveAt(i);
                        }
                        // Remove this block if it have fallen off the screen
                        if (enemy1laserPos != null && i > 0 && i < enemy1laserPos.Count)
                        {
                            if (enemy1laserPos[i].X <= 0 && enemy1laserPos[i] != null)
                            {
                                enemy1laserPos.RemoveAt(i);

                                // When removing a block, the next block will have the same index
                                // as the current block. Decrement i to prevent skipping a block.
                                i--;
                                if (i < 1) break;
                            }
                        }
                    }
                    for (int i = 0; i < enemy2laserPos.Count; i++)
                    {
                        enemy2laserPos[i] = new Vector2(enemy2laserPos[i].X - mainWepSpeed * 2, enemy2laserPos[i].Y);
                        Rectangle block1LaserRectangle = new Rectangle((int)enemy2laserPos[i].X, (int)enemy2laserPos[i].Y,
                            block1LaserTexture.Width, block1LaserTexture.Height);
                        if (IntersectPixels(personRectangle, personTextureData,
                                    block1LaserRectangle, block1LaserTextureData))
                        {
                            personHit = true;
                            if (personTexture.Equals(ship1Texture))
                            {
                                shield -= 10;
                            }
                            else if (personTexture.Equals(ship3Texture))
                            {
                                shield -= 7;
                            }
                            else
                            {
                                shield -= 5;
                            }
                            if (shield <= 0)
                            {
                                if (gameState == 4)
                                {
                                    loss = true;
                                }
                                else { gameState = 11; }
                            }
                            enemy2laserPos.RemoveAt(i);
                        }
                        // Remove this block if it have fallen off the screen
                        if (enemy2laserPos != null && i > 0 && i < enemy2laserPos.Count)
                        {
                            if (enemy2laserPos[i].X <= 0 && enemy2laserPos[i] != null)
                            {
                                enemy2laserPos.RemoveAt(i);

                                // When removing a block, the next block will have the same index
                                // as the current block. Decrement i to prevent skipping a block.
                                i--;
                                if (i < 1) break;
                            }
                        }
                    }
                    for (int i = 0; i < enemy3laserPos.Count; i++)
                    {
                        enemy3laserPos[i] = new Vector2(enemy3laserPos[i].X - mainWepSpeed * 2, enemy3laserPos[i].Y);
                        Rectangle block2LaserRectangle = new Rectangle((int)enemy3laserPos[i].X, (int)enemy3laserPos[i].Y,
                            block2LaserTexture.Width, block2LaserTexture.Height);
                        if (IntersectPixels(personRectangle, personTextureData,
                                    block2LaserRectangle, block2LaserTextureData))
                        {
                            personHit = true;
                            if (personTexture.Equals(ship1Texture))
                            {
                                shield -= 10;
                            }
                            else if (personTexture.Equals(ship3Texture))
                            {
                                shield -= 7;
                            }
                            else
                            {
                                shield -= 5;
                            }
                            if (shield <= 0)
                            {
                                if (gameState == 4)
                                {
                                    loss = true;
                                }
                                else { gameState = 11; }
                            }
                            enemy3laserPos.RemoveAt(i);
                        }
                        // Remove this block if it have fallen off the screen
                        if (enemy3laserPos != null && i > 0 && i < enemy3laserPos.Count)
                        {
                            if (enemy3laserPos[i].X <= 0 && enemy3laserPos[i] != null)
                            {
                                enemy3laserPos.RemoveAt(i);

                                // When removing a block, the next block will have the same index
                                // as the current block. Decrement i to prevent skipping a block.
                                i--;
                                if (i < 1) break;
                            }
                        }
                    }
                    for (int i=0; i < alt1Drop.Count; i++)
                    {
                        alt1Drop[i] = new Vector2(alt1Drop[i].X, alt1Drop[i].Y + alt1Speed);
                        Rectangle laserRectangle =
                            new Rectangle((int)alt1Drop[i].X, (int)alt1Drop[i].Y,
                            alt1.Width, alt1.Height);
                        if (IntersectPixels(personRectangle, personTextureData,
                                laserRectangle, alt1TextureData))
                        {
                            //laserPos.Clear();
                            if (alt1Drop != null && i >= 0 && i < alt1Drop.Count)
                            {
                                alt1Drop.RemoveAt(i);
                                if (!altWeapon1)
                                {
                                    alt1Actual = alt1shot;
                                    alt1ActualTextureData = alt1shotTextureData;
                                    alt1ammo = 100;
                                    altWeapon1 = true;
                                }
                                else if (!altWeapon2)
                                {
                                    alt2Actual = alt1shot;
                                    alt2ActualTextureData = alt1shotTextureData;
                                    alt2ammo = 150;
                                    altWeapon2 = true;
                                }
                                else if ((!altWeapon3) && (personTexture.Equals(ship1Texture) || personTexture.Equals(ship4Texture)))
                                {
                                    alt3Actual = alt1shot;
                                    alt3ActualTextureData = alt1shotTextureData;
                                    alt3ammo = 150;
                                    altWeapon3 = true;
                                }
                            }
                        }
                        if (alt1Drop != null && i > 0 && i < alt1Drop.Count)
                        {
                            if (alt1Drop[i].Y == Window.ClientBounds.Height )
                            {
                                if (alt1Drop != null && i >= 0 && i < alt1Drop.Count)
                                {
                                    alt1Drop.RemoveAt(i);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < alt2Drop.Count; i++)
                    {
                        alt2Drop[i] = new Vector2(alt2Drop[i].X, alt2Drop[i].Y + alt2Speed);
                        Rectangle laserRectangle =
                            new Rectangle((int)alt2Drop[i].X, (int)alt2Drop[i].Y,
                            alt2.Width, alt2.Height);
                        if (IntersectPixels(personRectangle, personTextureData,
                                laserRectangle, alt1TextureData))
                        {
                            //laserPos.Clear();
                            if (alt2Drop != null && i >= 0 && i < alt2Drop.Count)
                            {
                                alt2Drop.RemoveAt(i);
                                if (!altWeapon1)
                                {
                                    alt1Actual = alt2shot;
                                    alt1ActualTextureData = alt2shotTextureData;
                                    alt1ammo = 15;
                                    altWeapon1 = true;
                                }
                                else if (!altWeapon2)
                                {
                                    alt2Actual = alt2shot;
                                    alt2ActualTextureData = alt2shotTextureData;
                                    alt2ammo = 15;
                                    altWeapon2 = true;
                                }
                                else if ((!altWeapon3) && (personTexture.Equals(ship1Texture) || personTexture.Equals(ship4Texture)))
                                {
                                    alt3Actual = alt2shot;
                                    alt3ActualTextureData = alt2shotTextureData;
                                    alt3ammo = 15;
                                    altWeapon3 = true;
                                }
                            }
                        }
                        if (alt2Drop != null && i >=0 && i < alt2Drop.Count)
                        {
                            if (alt2Drop[i].Y == Window.ClientBounds.Height)
                            {
                                if (alt2Drop != null)
                                {
                                    alt2Drop.RemoveAt(i);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < alt3Drop.Count; i++)
                    {
                        alt3Drop[i] = new Vector2(alt3Drop[i].X, alt3Drop[i].Y + alt3Speed);
                        Rectangle laserRectangle =
                            new Rectangle((int)alt3Drop[i].X, (int)alt3Drop[i].Y,
                            alt3.Width, alt3.Height);
                        if (IntersectPixels(personRectangle, personTextureData,
                                laserRectangle, alt3TextureData))
                        {
                            //laserPos.Clear();
                            if (alt3Drop != null && i >= 0 && i < alt3Drop.Count)
                            {
                                alt3Drop.RemoveAt(i);
                                if (!altWeapon1)
                                {
                                    alt1Actual = alt3shot;
                                    alt1ActualTextureData = alt3shotTextureData;
                                    alt1ammo = 5;
                                    altWeapon1 = true;
                                }
                                else if (!altWeapon2)
                                {
                                    alt2Actual = alt3shot;
                                    alt2ActualTextureData = alt3shotTextureData;
                                    alt2ammo = 5;
                                    altWeapon2 = true;
                                }
                                else if ((!altWeapon3) && (personTexture.Equals(ship1Texture) || personTexture.Equals(ship4Texture)))
                                {
                                    alt3Actual = alt3shot;
                                    alt3ActualTextureData = alt3shotTextureData;
                                    alt3ammo = 5;
                                    altWeapon3 = true;
                                }
                            }
                        }
                        if (alt3Drop != null && i >= 0 && i < alt3Drop.Count)
                        {
                            if (alt3Drop[i].Y == Window.ClientBounds.Height)
                            {
                                if (alt3Drop != null && i > 0 && i < alt3Drop.Count)
                                {
                                    alt3Drop.RemoveAt(i);
                                }
                            }
                        }
                    }
                    base.Update(gameTime);
                    if (cooldown != 0)
                    {
                        cooldown--;
                    }
                    if (cooldown1 != 0)
                    {
                        cooldown1--;
                    }
                    if (cooldown2 != 0)
                    {
                        cooldown2--;
                    }
                    if (missilecool != 0)
                    {
                        missilecool--;
                    }
                    if (missilecool1 != 0)
                    {
                        missilecool1--;
                    }
                    if (missilecool2 != 0)
                    {
                        missilecool2--;
                    }
                    audioEngine.Update();
                }
                else
                {
                    if (keyboard.IsKeyDown(Keys.Q))
                    {
                        this.Exit();
                    }
                    else if (keyboard.IsKeyDown(Keys.Escape))
                    {
                        gameState = 7;
                    }
                }
            }
            else if (gameState == 0 || gameState == 7 || gameState==2 || gameState==3 || gameState == 11)
            {
                if (keyboard.IsKeyDown(Keys.Space) || gamePad.IsButtonDown(Buttons.Start) && gameState != 11)
                {
                    if (gameState == 7) { gameState = oldState; }
                    else gameState++;
                }
                if (keyboard.IsKeyDown(Keys.Q))
                {
                    this.Exit();
                }
            }
            else if (gameState == 10)
            {
                if (keyboard.IsKeyDown(Keys.C))
                {
                    gameState = 5;
                    win = false;
                    Block1SpawnProbability *= 2;
                    BlockSpawnProbability *= 2;
                    Block2SpawnProbability *= 2;
                    level = 2;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;
            spriteBatch.Begin();
            KeyboardState keyboard = Keyboard.GetState();
            if (gameState > 3 && gameState != 7 && gameState !=10)
            {
                if (personHit)
                {
                    if (loss)
                    {
                        device.Clear(Color.Black);
                    }
                    else if (win)
                    {
                        device.Clear(Color.White);
                   //     spriteBatch.Draw(bearsLogo, logoPosition, Color.White);
                    }
                    else
                    {
                        device.Clear(Color.YellowGreen);

                    }
                }
                else
                {
                    if (win)
                    {
                        device.Clear(Color.White);
                 //       spriteBatch.Draw(bearsLogo, logoPosition, Color.White);
                    }
                    else
                    {
                        device.Clear(Color.DarkGreen);
                        spriteBatch.Draw(bearsLogo, logoPosition, Color.White);
                    }
                }
                // Draw person
                spriteBatch.Draw(personTexture, personPosition, Color.White);
                if (altWeapon1)
                {
                    alt1Locate.X = personPosition.X;
                    alt1Locate.Y = personPosition.Y + 20;
                    if (alt1Actual.Equals(alt1shot))
                    {
                        spriteBatch.Draw(alt1, alt1Locate, Color.White);
                    }
                    else if (alt1Actual.Equals(alt2shot))
                    {
                        spriteBatch.Draw(alt2, alt1Locate, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(alt3, alt1Locate, Color.White);
                    }
                    if (alt1Actual.Equals(alt1shot))
                    {
                        Rectangle rec = new Rectangle(2, 105, alt1.Width, alt1.Height);
                        spriteBatch.Draw(alt1, rec, Color.White);
                        spriteBatch.DrawString(lfont, "(Press X): " + alt1ammo, new Vector2(2 + alt1.Width, 105), Color.White); 
                    }
                    if (alt1Actual.Equals(alt2shot))
                    {
                        Rectangle rec = new Rectangle(2, 105, alt2.Width, alt2.Height);
                        spriteBatch.Draw(alt2, rec, Color.White);
                        if (missilecool1 > 0)
                        {
                            Rectangle rec1 = new Rectangle(2 + rec.Width, 105, cooldownTexture.Width, cooldownTexture.Height);
                            spriteBatch.Draw(cooldownTexture, rec1, Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(lfont, "(Press X): " + alt1ammo, new Vector2(2 + alt2.Width, 105), Color.White);
                        }
                    }
                    if (alt1Actual.Equals(alt3shot))
                    {
                        Rectangle rec = new Rectangle(2, 105, alt3.Width, alt3.Height);
                        spriteBatch.Draw(alt3, rec, Color.White);
                        if (cooldown1 > 0)
                        {
                            Rectangle rec1 = new Rectangle(2+rec.Width, 105, cooldownTexture.Width, cooldownTexture.Height);
                            spriteBatch.Draw(cooldownTexture, rec1, Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(lfont, "(Press X): " + alt1ammo, new Vector2(2 + alt3.Width, 105), Color.White);
                        }
                    }
                }
                if (altWeapon2)
                {
                    alt2Locate.X = personPosition.X;
                    alt2Locate.Y = personPosition.Y + 45;
                    if (alt2Actual.Equals(alt1shot))
                    {
                        spriteBatch.Draw(alt1, alt2Locate, Color.White);
                    }
                    else if (alt2Actual.Equals(alt2shot))
                    {
                        spriteBatch.Draw(alt2, alt2Locate, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(alt3, alt2Locate, Color.White);
                    }
                    if (alt2Actual.Equals(alt1shot))
                    {
                        Rectangle rec = new Rectangle(2, 125, alt1.Width, alt1.Height);
                        spriteBatch.Draw(alt1, rec, Color.White);
                        spriteBatch.DrawString(lfont, "(Press C): " + alt2ammo, new Vector2(2 + alt1.Width, 125), Color.White);
                    }
                    if (alt2Actual.Equals(alt2shot))
                    {
                        Rectangle rec = new Rectangle(2, 125, alt2.Width, alt2.Height);
                        spriteBatch.Draw(alt2, rec, Color.White);
                        if (missilecool > 0)
                        {
                            Rectangle rec1 = new Rectangle(2 + rec.Width, 125, cooldownTexture.Width, cooldownTexture.Height);
                            spriteBatch.Draw(cooldownTexture, rec1, Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(lfont, "(Press C): " + alt2ammo, new Vector2(2 + alt2.Width, 125), Color.White);
                        }
                    }
                    if (alt2Actual.Equals(alt3shot))
                    {
                        Rectangle rec = new Rectangle(2, 125, alt3.Width, alt3.Height);
                        spriteBatch.Draw(alt3, rec, Color.White);
                        if (cooldown > 0)
                        {
                            Rectangle rec1 = new Rectangle(2+rec.Width, 125, cooldownTexture.Width, cooldownTexture.Height);
                            spriteBatch.Draw(cooldownTexture, rec1, Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(lfont, "(Press C): " + alt2ammo, new Vector2(2 + alt3.Width, 125), Color.White);
                        }
                    }
                }
                if (altWeapon3)
                {
                    alt3Locate.X = personPosition.X;
                    alt3Locate.Y = personPosition.Y + 30;
                    if (alt3Actual.Equals(alt1shot))
                    {
                        spriteBatch.Draw(alt1, alt3Locate, Color.White);
                    }
                    else if (alt3Actual.Equals(alt2shot))
                    {
                        spriteBatch.Draw(alt2, alt3Locate, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(alt3, alt3Locate, Color.White);
                    }
                    if (alt3Actual.Equals(alt1shot))
                    {
                        Rectangle rec = new Rectangle(2, 145, alt1.Width, alt1.Height);
                        spriteBatch.Draw(alt1, rec, Color.White);
                        spriteBatch.DrawString(lfont, "(Press V): " + alt3ammo, new Vector2(2+alt1.Width, 145), Color.White);
                    }
                    if (alt3Actual.Equals(alt2shot))
                    {
                        Rectangle rec = new Rectangle(2, 145, alt2.Width, alt2.Height);
                        spriteBatch.Draw(alt2, rec, Color.White);
                        if (missilecool2 > 0)
                        {
                            Rectangle rec1 = new Rectangle(2 + rec.Width, 145, cooldownTexture.Width, cooldownTexture.Height);
                            spriteBatch.Draw(cooldownTexture, rec1, Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(lfont, "(Press V): " + alt3ammo, new Vector2(2 + alt2.Width, 145), Color.White);
                        }
                    }
                    if (alt3Actual.Equals(alt3shot))
                    {
                        Rectangle rec = new Rectangle(2, 145, alt3.Width, alt3.Height);
                        spriteBatch.Draw(alt3, rec, Color.White);
                        if (cooldown2 > 0)
                        {
                            Rectangle rec1 = new Rectangle(2+rec.Width, 145, cooldownTexture.Width, cooldownTexture.Height);
                            spriteBatch.Draw(cooldownTexture, rec1, Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(lfont, "(Press V): " + alt3ammo, new Vector2(2 + alt3.Width, 145), Color.White);
                        }
                    }
                }
                
                // Draw blocks
                foreach (Vector2 blockPosition in blockPositions)
                {
                    spriteBatch.Draw(blockTexture, blockPosition, Color.White);
                }
                foreach (Vector2 block1Position in block1Positions)
                {
                    spriteBatch.Draw(block1Texture, block1Position, Color.White);
                }
                foreach (Vector2 block2Position in block2Positions)
                {
                    spriteBatch.Draw(block2Texture, block2Position, Color.White);
                }
                foreach (Vector2 explodePosition in explodePositions)
                {
                    spriteBatch.Draw(explode1, explodePosition, Color.White);
                }
                foreach (Vector2 explodePosition in blastPos)
                {
                    spriteBatch.Draw(blastRadius, explodePosition, Color.White);
                }
                for (int i = 0; i < explodeCounters.Count; i++)
                {
                    explodeCounters[i]--;
                }
                for (int i = 0; i < blastCounters.Count; i++)
                {
                    blastCounters[i]--;
                }
                for (int i = 0; i < explodeCounters.Count; i++)
                {
                    if (explodeCounters[i] <= 0)
                    {
                        explodeCounters.Remove(explodeCounters[i]);
                        explodePositions.RemoveAt(i);
                    }
                }
                for (int i = 0; i < blastCounters.Count; i++)
                {
                    if (blastCounters[i] <= 0)
                    {
                        blastCounters.Remove(blastCounters[i]);
                        blastPos.RemoveAt(i);
                    }
                }
                foreach (Vector2 laserPosition in laserPos)
                {
                    spriteBatch.Draw(mainWepTexture, laserPosition, Color.White);
                }
                foreach (Vector2 alt1pickup in alt1Drop)
                {
                    spriteBatch.DrawString(lfont, "Plasma Cannon", new Vector2(alt1pickup.X, alt1pickup.Y - 20), Color.White);
                    spriteBatch.Draw(alt1, alt1pickup, Color.White);
                }
                foreach (Vector2 alt2pickup in alt2Drop)
                {
                    spriteBatch.DrawString(lfont, "Power Missile", new Vector2(alt2pickup.X, alt2pickup.Y - 20), Color.White);
                    spriteBatch.Draw(alt2, alt2pickup, Color.White);
                }
                foreach (Vector2 alt3pickup in alt3Drop)
                {
                    spriteBatch.DrawString(lfont, "BFG", new Vector2(alt3pickup.X, alt3pickup.Y - 20), Color.White);
                    spriteBatch.Draw(alt3, alt3pickup, Color.White);
                }
                foreach (Vector2 alt1shoot in alt1Pos)
                {
                    if (alt1Actual.Equals(alt1shot))
                    {
                        spriteBatch.Draw(alt1shot, alt1shoot, Color.White);
                    }
                    else if (alt1Actual.Equals(alt2shot))
                    {
                        spriteBatch.Draw(alt2shot, alt1shoot, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(alt3shot, alt1shoot, Color.White);
                    }
                }
                foreach (Vector2 alt2shoot in alt2Pos)
                {
                    if (alt2Actual.Equals(alt1shot))
                    {
                        spriteBatch.Draw(alt1shot, alt2shoot, Color.White);
                    }
                    else if (alt2Actual.Equals(alt2shot))
                    {
                        spriteBatch.Draw(alt2shot, alt2shoot, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(alt3shot, alt2shoot, Color.White);
                    }
                }
                foreach (Vector2 alt3shoot in alt3Pos)
                {
                    if (alt3Actual.Equals(alt1shot))
                    {
                        spriteBatch.Draw(alt1shot, alt3shoot, Color.White);
                    }
                    else if (alt3Actual.Equals(alt2shot))
                    {
                        spriteBatch.Draw(alt2shot, alt3shoot, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(alt3shot, alt3shoot, Color.White);
                    }
                }
                foreach (Vector2 blockLaserPosition in enemy1laserPos)
                {
                    spriteBatch.Draw(blockLaserTexture, blockLaserPosition, Color.White);
                }
                foreach (Vector2 block1LaserPosition in enemy2laserPos)
                {
                    spriteBatch.Draw(block1LaserTexture, block1LaserPosition, Color.White);
                }
                foreach (Vector2 block2LaserPosition in enemy3laserPos)
                {
                    spriteBatch.Draw(block2LaserTexture, block2LaserPosition, Color.White);
                }
                if (bossNow)
                {
                    Rectangle bossy = new Rectangle((int)bossPos.X, (int)bossPos.Y, boss.Width, boss.Height);
                    if (bossHealth > 0)
                    {
                        spriteBatch.Draw(boss, bossy, null, Color.White, RotationAngle, rotation_point, SpriteEffects.None, 0);
                    }
                    //draw the boss!
                }
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(5, 5), Color.White);
                spriteBatch.DrawString(font, "Level: " + level, new Vector2(5, 35), Color.White);
                spriteBatch.Draw(health, new Rectangle(5, 65, shield*2, 20), Color.White);
                spriteBatch.DrawString(font, "SHIELDS", new Vector2(5, 65), Color.White);
                //spriteBatch.DrawString(font, "" + BlockSpawnProbability + " " + Block1SpawnProbability + " " + Block2SpawnProbability, new Vector2(5, 95), Color.White);
                spriteBatch.DrawString(font, "Press Q to Exit, H for Help", new Vector2(Window.ClientBounds.Width / 3, 5), Color.White);
                if (loss)
                {
                    spriteBatch.DrawString(narrative, "You Have Lost!", new Vector2(Window.ClientBounds.Width / 3  + 40, Window.ClientBounds.Height / 2 - 100), Color.Red);
                    spriteBatch.DrawString(narrative, "Press Q to exit.", new Vector2(Window.ClientBounds.Width / 3 + 40,  (Window.ClientBounds.Height / 2) ), Color.Red);
                    if (keyboard.IsKeyDown(Keys.Q)) this.Exit();
                }
                if (win)
                {
                    spriteBatch.DrawString(lfont, "You Have Won! You have saved Selene!", new Vector2(Window.ClientBounds.Width / 4, Window.ClientBounds.Height / 3), Color.SteelBlue);
                    spriteBatch.DrawString(lfont, "Press Space To Continue.", new Vector2(Window.ClientBounds.Width / 4, 2 * (Window.ClientBounds.Height / 3)), Color.SteelBlue);
                    if (keyboard.IsKeyDown(Keys.Q)) this.Exit();
                    if (keyboard.IsKeyDown(Keys.Space)) gameState = 10;
                }
            }
            else if (gameState == 0)
            {

                device.Clear(Color.Black);
                spriteBatch.Draw(title_screen, logoPosition, Color.White);
                spriteBatch.DrawString(title, "Zweihander", new Vector2(230, 2* Window.ClientBounds.Height / 3 ), Color.Red);
                spriteBatch.DrawString(small_title, "Press Space to Continue.", new Vector2(230 + 70, (2* Window.ClientBounds.Height / 3 + 90) ), Color.Red);
                
            }
            else if (gameState == 1)
            {
                device.Clear(Color.Black);
                String narrative1 = "The year is 2058 and all seemed well on the moon base of Selene.";

                String narrative2 = "A sudden massive push of alien forces upon the city has sent the";
                String narrative3 = "people into uproar. Unable to quickly mobilize defenses the city";
                String narrative4 = "has already lost thousands on the outskirts. As the enemy closes ";
                String narrative5 = "in on the city's central hub the military gave birth to but one";
                String narrative6 = "ship, their one hope - the Zweihander. Besides its main gun its";
                String narrative7 = "only chance of survival is its prototype \"Manipulator\" arm, which";
                String narrative8 = "can adapt itself to use enemies' weapons as its own. You are the";
                String narrative9 = "pilot of this scavenger kamakize ship, the last hope for Selene.";
                String narrative10 = "Good luck pilot!";
                spriteBatch.DrawString(narrative, narrative1, new Vector2(15, 25), Color.White);
                spriteBatch.DrawString(narrative, narrative2, new Vector2(15, 60), Color.White);
                spriteBatch.DrawString(narrative, narrative3, new Vector2(15, 95), Color.White);
                spriteBatch.DrawString(narrative, narrative4, new Vector2(15, 130), Color.White);
                spriteBatch.DrawString(narrative, narrative5, new Vector2(15, 165), Color.White);
                spriteBatch.DrawString(narrative, narrative6, new Vector2(15, 200), Color.White);
                spriteBatch.DrawString(narrative, narrative7, new Vector2(15, 235), Color.White);
                spriteBatch.DrawString(narrative, narrative8, new Vector2(15, 270), Color.White);
                spriteBatch.DrawString(narrative, narrative9, new Vector2(15, 305), Color.White);
                spriteBatch.DrawString(narrative, narrative10, new Vector2(300, 355), Color.White);
                spriteBatch.DrawString(small_title, "Press Enter to Continue.", new Vector2(230 + 70, (2 * Window.ClientBounds.Height / 3 + 90)), Color.Red);
                KeyboardState kbd = Keyboard.GetState();
                GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

                if (kbd.IsKeyDown(Keys.Enter) || gamePad.IsButtonDown(Buttons.Start))
                {
                    gameState++;
                }
            }
            else if (gameState == 2)
            {
                device.Clear(Color.Black);
                spriteBatch.DrawString(narrative, "Please choose your ship", new Vector2(260, 50), Color.White);
                Rectangle rec1 = new Rectangle((int)ship1Position.X, (int)ship1Position.Y,
                       ship1Texture.Width, ship1Texture.Height);
                Rectangle rec2 = new Rectangle((int)ship2Position.X, (int)ship2Position.Y,
                        ship2Texture.Width, ship2Texture.Height);
                Rectangle rec3 = new Rectangle((int)ship3Position.X, (int)ship3Position.Y,
                         ship3Texture.Width, ship3Texture.Height);
                Rectangle rec4 = new Rectangle((int)ship4Position.X, (int)ship4Position.Y,
                        ship4Texture.Width, ship4Texture.Height);

                spriteBatch.Draw(ship1Texture, rec1, Color.LightCyan);
                spriteBatch.Draw(ship2Texture, rec2, Color.White);

                spriteBatch.Draw(ship3Texture, rec3, Color.LightCyan);
                spriteBatch.Draw(ship4Texture, rec4, Color.White);
                spriteBatch.DrawString(narrative_small, "3 Weapon slots, Low Grade Shields, Normal Speed", new Vector2(30, 140), Color.White);
                spriteBatch.DrawString(narrative_small, "Press A to select", new Vector2(100, 170), Color.White);

                spriteBatch.DrawString(narrative_small, "2 Weapon slots, High Grade Shields, Normal Speed", new Vector2(420, 140), Color.White);
                spriteBatch.DrawString(narrative_small, "Press B to select", new Vector2(500, 170), Color.White);

                spriteBatch.DrawString(narrative_small, "2 Weapon slots, Medium Grade Shields, Fast Speed", new Vector2(30, 340), Color.White);
                spriteBatch.DrawString(narrative_small, "Press C to select", new Vector2(100, 400), Color.White);

                spriteBatch.DrawString(narrative_small, "3 Weapon slots (Fully Loaded), High Grade Shields,", new Vector2(420, 340), Color.White);
                spriteBatch.DrawString(narrative_small, "Slow Speed", new Vector2(525, 370), Color.White);
                spriteBatch.DrawString(narrative_small, "Press D to select", new Vector2(500, 400), Color.White);
                if (keyboard.IsKeyDown(Keys.A))
                {
                    personTexture = ship1Texture;
                    personTextureData = ship1TextureData;
                    gameState++;
                }
                else if (keyboard.IsKeyDown(Keys.B))
                {
                    personTexture = ship2Texture;
                    personTextureData = ship2TextureData;
                    gameState++;
                }
                else if (keyboard.IsKeyDown(Keys.C))
                {
                    personTexture = ship3Texture;
                    personTextureData = ship3TextureData;
                    PersonMoveSpeed = 7;
                    gameState++;
                }
                else if (keyboard.IsKeyDown(Keys.D))
                {
                    personTexture = ship4Texture;
                    personTextureData = ship4TextureData;
                    PersonMoveSpeed = 2;
                    altWeapon1 = true;
                    alt1Actual = alt1shot;
                    alt1ActualTextureData = alt1shotTextureData;
                    alt1ammo = 100;
                    alt2Actual = alt2shot;
                    alt2ActualTextureData = alt2shotTextureData;
                    alt2ammo = 15;
                    alt3Actual = alt3shot;
                    alt3ActualTextureData = alt3shotTextureData;
                    alt3ammo = 5;
                    altWeapon2 = true;
                    altWeapon3 = true;
                    gameState++;
                }
            }
            else if (gameState == 3)
            {
                int w = Window.ClientBounds.Width;
                int h = Window.ClientBounds.Height;
                Color maintext = Color.Red;
                spriteBatch.Draw(bearsLogo, logoPosition, Color.White);
                spriteBatch.Draw(blankTexture,
                                 new Rectangle(25, 25, w - 50, h - 50),
                                 new Color(0, 0, 0, (byte)(180)));

                // spriteBatch.End();
                //spriteBatch.Begin(SpriteBlendMode.Additive);
                //spriteBatch.Draw(bearsLogo, logoPosition, Color.White);
                //spriteBatch.Draw(grey, new Rectangle(50, 50, 600, 600),Color.White);
                //spriteBatch.End();
                //spriteBatch.Begin();
                // device.Clear(Color.MidnightBlue);
                spriteBatch.DrawString(controls, "Controls", new Vector2(w / 2 - 40, 20), maintext);
                // spriteBatch.DrawString(controls, "Keyboard", new Vector2(w/4 + 80, 55), maintext);
                spriteBatch.DrawString(controls, "Move:", new Vector2(w / 9, 100), maintext);
                spriteBatch.DrawString(controls, "Arrow Keys", new Vector2(w / 4, 100), maintext);
                //                spriteBatch.DrawString(controls, "D-Pad", new Vector2(3*w/4 -90, 100), maintext);

                spriteBatch.DrawString(controls, "Shoot:", new Vector2(6 * w / 9, 100), maintext);
                spriteBatch.DrawString(controls, "Z", new Vector2(3 * w / 4 + 80, 100), maintext);


                spriteBatch.DrawString(controls, "Destroy enemies before you run out of shields.", new Vector2(w / 9, 180), maintext);
                spriteBatch.DrawString(controls, "Be sure to pick up falling weapons when enemies die.", new Vector2(w / 9, 220), maintext);
                spriteBatch.DrawString(controls, "When you collect weapons, use the following keys -", new Vector2(w / 9, 260), maintext);
                spriteBatch.DrawString(controls, "Weapon Slot #2:", new Vector2(w / 9, 300), maintext);
                spriteBatch.DrawString(controls, "X", new Vector2(w / 4 + 122, 300), maintext);

                spriteBatch.DrawString(controls, "Weapon Slot #3:", new Vector2(5 * w / 9, 300), maintext);
                spriteBatch.DrawString(controls, "C", new Vector2(6 * w / 9 + 145, 300), maintext);


                spriteBatch.DrawString(controls, "Weapon Slot #4:", new Vector2(3 * w / 9, 340), maintext);
                spriteBatch.DrawString(controls, "V", new Vector2(4 * w / 9 + 145, 340), maintext);

                spriteBatch.DrawString(controls, "Pause:", new Vector2(w / 9, 420), maintext);
                spriteBatch.DrawString(controls, "Escape", new Vector2(w / 4, 420), maintext);


                spriteBatch.DrawString(controls, "Quit:", new Vector2(6 * w / 9, 420), maintext);
                spriteBatch.DrawString(controls, "Q", new Vector2(3 * w / 4 + 70, 420), maintext);
                //              spriteBatch.DrawString(controls, "A Button", new Vector2(3*w/4 -90, 140), maintext);

                //       spriteBatch.DrawString(controls, "Pause:", new Vector2(w/9, 180), maintext);
                //      spriteBatch.DrawString(controls, "Escape Key", new Vector2(w/4 +80, 180), maintext);
                //            spriteBatch.DrawString(controls, "Start Button", new Vector2(3*w/4 -90, 180), maintext);

                //                spriteBatch.DrawString(controls, "Weapon #2:", new Vector2(w/9, 220), maintext);
                //              spriteBatch.DrawString(controls, "X", new Vector2(w/4 + 100, 220), maintext);
                //          spriteBatch.DrawString(controls, "X Button", new Vector2(3*w/4 -90, 220), maintext);

                //            spriteBatch.DrawString(controls, "Weapon #3:", new Vector2(w/9, 260), maintext);
                //          spriteBatch.DrawString(controls, "C", new Vector2(w/4 + 100, 260), maintext);
                //        spriteBatch.DrawString(controls, "B Button", new Vector2(3*w/4-90, 260), maintext);

                //        spriteBatch.DrawString(controls, "Weapon #4:", new Vector2(w/9, 300), maintext);
                //       spriteBatch.DrawString(controls, "V", new Vector2(w/4 + 100, 300), maintext);
                //                spriteBatch.DrawString(controls, "Y Button", new Vector2(3*w/4 -90, 300), maintext);

                //spriteBatch.DrawString(controls, "Press Q at any time to exit, Space to continue.", new Vector2(w/9 -20, 350), maintext);
                //spriteBatch.DrawString(controls, "Destroy enemies before they destroy you!", new Vector2(w/9 - 20, 400), maintext);
                //spriteBatch.DrawString(controls, "Be sure to pick up falling weapons for added bonus!", new Vector2(w/9 -20, 450), maintext);
                spriteBatch.DrawString(controls, "1500 points wins!", new Vector2(3 * w / 9 + 20, 470), maintext);
                spriteBatch.DrawString(small_title, "Press Space to continue", new Vector2(3 * w / 9 + 20, 560), maintext);

                spriteBatch.End();
                spriteBatch.Begin();

            }
            else if (gameState == 7)
            {
                device.Clear(Color.Orange);
                spriteBatch.DrawString(font, "Game is Paused.", new Vector2(5, Window.ClientBounds.Height / 3), Color.White);
                spriteBatch.DrawString(font, "Press Space Key to Continue...", new Vector2(5, Window.ClientBounds.Height / 2), Color.White);
            }
            else if (gameState == 10)
            {
                device.Clear(Color.Green);
                spriteBatch.DrawString(narrative, "Level #2: Endurance Round", new Vector2(Window.ClientBounds.Width/3, Window.ClientBounds.Height/3), Color.Red);
                spriteBatch.DrawString(narrative, "See how long you can last!", new Vector2(Window.ClientBounds.Width / 3, Window.ClientBounds.Height / 2), Color.Red);
                spriteBatch.DrawString(font, "Press C to Continue...", new Vector2(Window.ClientBounds.Width/3, 2*(Window.ClientBounds.Height / 3)), Color.Red);
                if (keyboard.IsKeyDown(Keys.C))
                {
                    gameState = 5;
                    win = false;
                    blastPos.Clear();
                    level = 2;
                    Block1SpawnProbability *= 2;
                    BlockSpawnProbability *= 2;
                    Block2SpawnProbability *= 2;
                }
            }
            else if (gameState == 11)
            {
                device.Clear(Color.OrangeRed);
                spriteBatch.DrawString(font, "Game Over!", new Vector2(5, Window.ClientBounds.Height / 3), Color.White);
                spriteBatch.DrawString(font, "Your final score is: " + score, new Vector2(5, Window.ClientBounds.Height / 2), Color.White);
                spriteBatch.DrawString(font, "Press Q to quit.", new Vector2(5, 2 * (Window.ClientBounds.Height / 3)), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                            Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);
            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }
            // No intersection found
            return false;
        }
    }
}
