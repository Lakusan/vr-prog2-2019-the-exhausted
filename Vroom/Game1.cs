using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Vroom
{

    public class Game1 : Game
    {
        #region fields 
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Create Player Object
        Player player = new Player(new Vector2(50, 50));
        MapEditor mapEditor;
        MenuComponent menuComponent;
        CharacterSelection characterSelection;
        SettingsMenu settingsMenu;
        GameOver gameOver;
        Credits credits;
        Intro intro;
        Tutorial tutorial;
      

        public static Rectangle screenSize;
        //HUD font
        public static SpriteFont font;
        //Menu Font
        public static SpriteFont menuFont;
        //GameOver Font
        public static SpriteFont gameOverFont;

        public static string GameState = "Intro";
       
        //for Camera - Field of View
        public static Rectangle screen;
        static Camera camera = new Camera();

        //MapReader
        MapReader mapreader = new MapReader();

        //Cursor 
        static Cursor cursor = new Cursor(new Vector2(0, 0));

        //Collision Map
        public static bool[,] collisionMap;

        //threads
        public static Thread[] t = new Thread[3];

        //level 1 stuff
        Texture2D levelOne;
        public static Texture2D hud;
        public static int levelOneMaxScore = 100;

        //Pause
        public static bool isPaused = false;

        //Keyboard
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        //for debuging only
        Texture2D pixel;       
        Texture2D explosion;

        #endregion fields

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth =1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
        }
       
        protected override void Initialize()
        {
            characterSelection = new CharacterSelection();
            Items.Initialize();
            
            //Rectangle Map
            //screenSize = new Rectangle(0, 0, graphics.PreferredBackBufferWidth*4, graphics.PreferredBackBufferHeight*4);
            //screen
            screen = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //makes collisionMap = size of screenSize
            //collisionMap = new bool[screenSize.Width / Pathfinder.gridSize, screenSize.Height / Pathfinder.gridSize];
            mapEditor = new MapEditor();
            menuComponent = new MenuComponent();
            settingsMenu = new SettingsMenu();
            gameOver = new GameOver();
            credits = new Credits();
            intro = new Intro();
            tutorial = new Tutorial();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //characterSelection = new CharacterSelection();
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("HUDFont");
            menuFont = Content.Load<SpriteFont>("menuFont");
            gameOverFont = Content.Load<SpriteFont>("gameOverFont");          
            explosion = Content.Load<Texture2D>("explosion");

            //Load Menu and Gamemodes & Stuff
            mapEditor.LoadContent(Content);
            menuComponent.LoadContent(Content);
            settingsMenu.LoadContent(Content);
            gameOver.LoadContent(Content);
            credits.LoadContent(Content);
            characterSelection.LoadContent(Content);
            intro.LoadContent(Content);
            tutorial.LoadContent(Content);
            
         
            //read ObjectDatabase
            mapreader.ReadMap("ObjectDatabase.xml");

            //load Content of Objects
            foreach (Object o in Items.objectList)
            {
                o.LoadContent(this.Content);
            }
            cursor.LoadContent(Content);
            camera.Update();
            //Level 1 Stuff
            levelOne = Content.Load<Texture2D>("map_bunker");
            hud = Content.Load<Texture2D>("HUD");
            //For Debuging - create single pixel
            // create single pixel for drawing debugging stuff
            pixel = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.DarkRed });
          
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
                  
            switch (GameState)
            {
                case "Tutorial":
                    tutorial.Update(gameTime);
                    break;
                case "Intro":
                    intro.Update(gameTime);
                    break;
                case "Character Selection":
                    characterSelection.Update(gameTime);
                    break;
                case "Game":
                    keyboard = Keyboard.GetState();
                    if (!isPaused)
                    {                      
                        foreach (Object o in Items.objectList)
                        {
                            o.Update();
                        }                        
                    }
                    if (isPaused == true)
                    {
                        if (keyboard.IsKeyDown(Keys.Q)&&prevKeyboard.IsKeyUp(Keys.Q)) { GameState = "GameOver"; }
                        if (keyboard.IsKeyDown(Keys.E)) { isPaused = false; }
                    }
                    if (prevKeyboard.IsKeyUp(Keys.P) && keyboard.IsKeyDown(Keys.P))
                    {
                        isPaused = true;
                    }
                    cursor.Update();
                    keyboard = prevKeyboard;
                    break;
                case "Editor":
                    mapEditor.Update(gameTime);
                    break;
                case "Menu":          
                    menuComponent.Update(gameTime);
                    break;
                case "Settings":
                    settingsMenu.Update(gameTime);
                    break;
                case "Credits":
                    credits.Update(gameTime);
                    break;
                case "GameOver":
                    gameOver.Update(gameTime);                   
                    break;
                case "Quit":
                    Exit();
                        break;
            }
           
            camera.Update();
            cursor.Update();
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
 
            switch (GameState)
            {
                case "Tutorial":
                    tutorial.Draw(spriteBatch);
                    break;
                case "Intro":
                    intro.Draw(spriteBatch);
                    break;

                case "Character Selection":
                    characterSelection.Draw(spriteBatch);
                    break;
                case "Game":
                    //camera Matrix as Parameter for Spritebatch

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform(graphics.GraphicsDevice));
                    spriteBatch.Draw(levelOne,screenSize,Color.White);
                    
                                   
                    foreach (Object o in Items.objectList)
                    {
                        o.Draw(spriteBatch);
                    }
                   if (isPaused==true)
                    {
                        //PAUSE - draw Tutorial Stuff and Back To Menu
                        spriteBatch.DrawString(gameOverFont, "PAUSE", new Vector2(Player.player.position.X,Player.player.position.Y), Color.DarkRed);
                        spriteBatch.DrawString(menuFont, "PRESS Q FOR MENU", new Vector2(Player.player.position.X - 100, Player.player.position.Y + 150), Color.DarkRed);
                        spriteBatch.DrawString(menuFont, "PRESS E FOR RESUME", new Vector2(Player.player.position.X-100, Player.player.position.Y+200), Color.DarkRed);
                    }

                    //Debuging Stuff
                    //screen = MapSize
                    //DrawBorder(screen,5,Color.Blue);
                    //screenSize
                    //DrawBorder(screenSize, 5, Color.White);
                    //Collisions HitBox Player
                    // DrawBorder(Player.player.area, 5, Color.Beige);
                    //Hitbox Object

                    //Hitbox Enemy
                    spriteBatch.End();
                    break;
                case "Editor":
                    mapEditor.Draw(spriteBatch);
                    break;
                case "Menu":
                    menuComponent.Draw(spriteBatch);
                    break;
                case "Settings":
                    settingsMenu.Draw(spriteBatch);
                    break;
                case "Credits":
                    credits.Draw(spriteBatch);
                    break;
                case "GameOver":
                    gameOver.Draw(spriteBatch);
                    break;

            }

            //Cursor in new spritebatch that draws on top of everything
            spriteBatch.Begin();
            HUD.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static Vector2 GetCameraPosition()
        {
            return camera.position;
        }

        public static Vector2 GetCursorPosition()
        {
            return cursor.position;
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        /// <param name="rectangleToDraw"></param>
        /// <param name="thicknessOfBorder"></param>
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        public virtual bool CheckKeyboard(Keys key)
        {

            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }
    }
   }
