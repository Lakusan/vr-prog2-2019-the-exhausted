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
    class Tutorial
    {
        #region fields
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        Tutorial tutorial;
        Texture2D tutorialBackground;

        #endregion fields

        public Tutorial()
        {
            tutorial = this;
        }
        public virtual void LoadContent(ContentManager Content)
        {
            tutorialBackground = Content.Load<Texture2D>("Tutorial");
        }
        public virtual void Update(GameTime gameTime)
        {

            keyboard = Keyboard.GetState();

            if (CheckKeyboard(Keys.Space))
            {
                Game1.GameState = "Menu";
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
            spriteBatch.Draw(tutorialBackground, new Vector2(0, 0), Game1.screenSize, Color.White);

            spriteBatch.DrawString(Game1.menuFont, "PRESS SPACE TO PROCEED", new Vector2(Game1.screen.Width / 3 + 50, Game1.screen.Height - 200), Color.DarkRed);

            spriteBatch.End();
        }

    }
}
