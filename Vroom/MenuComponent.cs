using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Vroom
{
    class MenuComponent
    {
        #region fields
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        Song menuMusic;
        public static bool isPlaying = false;
        public static MenuComponent menuComponent;
        
        MouseState mouse;
        MouseState prevMouse;
        Texture2D background;
        
        private protected SoundEffect buttonFX;
        List<string> buttonList = new List<string>();

        int selected = 0;
        

        #endregion fields

        public MenuComponent()
        {
            //menubuttons
            buttonList.Add("New Game");
            buttonList.Add("Map Editor");
            buttonList.Add("Settings");
            buttonList.Add("Credits");
            buttonList.Add("Quit");
            menuComponent = this;
        }
        public virtual void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("Menue");
            menuMusic = Content.Load<Song>("01 The Misadventure Begins");
           
           

            
            //buttonFX = Content.Load<SoundEffect>("");

        }
        public virtual void Update(GameTime gameTime)
        {

            
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            
            if (CheckKeyboard(Keys.W) || CheckKeyboard(Keys.Up))
            {
                if (selected > 0) selected--;
            }
            if (CheckKeyboard(Keys.S) || CheckKeyboard(Keys.Down))
            {
                if (selected < buttonList.Count - 1) selected++;
            }
            if(CheckKeyboard(Keys.Enter))
            {
                switch(selected)
                {
                    case 0:
                        Game1.GameState = "Character Selection";
                        break;
                    case 1:
                        Game1.GameState = "Editor";
                        break;
                    case 2:
                        Game1.GameState = "Settings";
                        break;
                    case 3:
                        Game1.GameState = "Credits";
                        break;
                    case 4:
                        Game1.GameState = "Quit";
                        break;
                }
            }
            if (!isPlaying)
            {
                MediaPlayer.Stop();
                isPlaying = true;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(menuMusic);
                Credits.credits.SetFalse();
            }
            prevKeyboard = keyboard;
            prevMouse = mouse;
        }
        public virtual bool CheckMouse()
        {
            //checks leftklicked and havent bevore
            //place object every frame
            return (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released);
        }
        public virtual bool CheckKeyboard(Keys key)
        {

            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            //
            int linePadding = 30;
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0,0),Game1.screen,Color.White);
            for (int i =0;i<buttonList.Count;i++)
            {
                
                //if (i == selected) color = Color.Yellow else color=Color.Black;
                color = (i==selected) ? Color.Yellow : Color.Black;
                //get half the screen, go up to the max height,inc by linespacing (max height of font) + linepadding
                spriteBatch.DrawString(Game1.menuFont, buttonList[i],new Vector2((Game1.screen.Width/2),//-(Game1.font.MeasureString(buttonList[i]).X/2)
                    (Game1.screen.Height/2)-(Game1.font.LineSpacing*buttonList.Count/2)+((Game1.font.LineSpacing+linePadding)*i)),color);
            }
            spriteBatch.End();
        }

        public void SetFalse()
        {
            isPlaying = false;
        }

    }
}
