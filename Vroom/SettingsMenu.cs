﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Vroom
{
    class SettingsMenu
    {
        #region fields
        KeyboardState keyboard;
        KeyboardState prevKeyboard;
        
        


        MouseState mouse;
        MouseState prevMouse;
        Texture2D background;
        List<string> buttonList = new List<string>();

        int selected = 0;

        #endregion fields

        public SettingsMenu()
        {
            //menubuttons

            buttonList.Add("Comming Soon");
            buttonList.Add("Back");
        }
        public virtual void LoadContent(ContentManager Content)
        {
           background = Content.Load<Texture2D>("Menue");
           
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
            if (CheckKeyboard(Keys.Enter) || CheckKeyboard(Keys.Space))
            {
                switch (selected)
                {
                    case 0:
                        
                        break;
                    case 1:
                        Game1.GameState = "Menu";
                        
                        break;
                    default:
                        break;

                  
                }
            }
            prevKeyboard = keyboard;
            prevMouse = mouse;
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
            spriteBatch.Draw(background, new Vector2(0, 0), Game1.screen, Color.White);
            for (int i = 0; i < buttonList.Count; i++)
            {

                //if (i == selected) color = Color.Yellow else color=Color.Black;
                color = (i == selected) ? Color.Yellow : Color.Black;
                //get half the screen, go up to the max height,inc by linespacing (max height of font) + linepadding
                spriteBatch.DrawString(Game1.menuFont, buttonList[i], new Vector2((Game1.screen.Width / 2) - (Game1.font.MeasureString(buttonList[i]).X / 2),
                    (Game1.screen.Height / 2) - (Game1.font.LineSpacing * buttonList.Count / 2) + ((Game1.font.LineSpacing + linePadding) * i)), color);
               
            }
            spriteBatch.End();
        }

    }
}

