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
    class Intro
    {
        #region fields
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        Intro intro;
        Texture2D introBackground;

        #endregion fields

        public Intro()
        {
            intro = this;
        }
        public virtual void LoadContent(ContentManager Content)
        {
            introBackground = Content.Load<Texture2D>("Intro");
        }
        public virtual void Update(GameTime gameTime)
        { 

            keyboard = Keyboard.GetState();
           
            if (CheckKeyboard(Keys.Escape))
            {
                Game1.GameState = "Tutorial";
            }
          
            prevKeyboard = keyboard;
        }
      
        public virtual bool CheckKeyboard(Keys key)
        {

            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
          
            spriteBatch.Begin();
            spriteBatch.Draw(introBackground, new Vector2(0, 0), Game1.screenSize, Color.White);
            
                spriteBatch.DrawString(Game1.menuFont, "PRESS ESC TO PROCEED", new Vector2(Game1.screen.Width/3+50,Game1.screen.Height-200), Color.DarkRed);
            
            spriteBatch.End();
        }

    }
}
