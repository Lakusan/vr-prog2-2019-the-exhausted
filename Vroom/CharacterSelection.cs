using System;
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
    class CharacterSelection 
    {
        #region fields
        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        public static CharacterSelection characterSelection;

        MouseState mouse;
        MouseState prevMouse;
        List<string> buttonList = new List<string>();
        public int currentSelectedCar;
        int selected = 0;
        Texture2D background;

        bool set = true;
        
        #endregion fields

        public CharacterSelection()
        {
            characterSelection = this;
            //menubuttons
            buttonList.Add("BTeam");
            buttonList.Add("BrideRider");
            buttonList.Add("FatMan");
            buttonList.Add("Confirm");
            buttonList.Add("Back");
        }
        public virtual void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("CharacterSelection");
          
        }
        public void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            characterSelection = this;
            if (CheckKeyboard(Keys.D) || CheckKeyboard(Keys.Right))
            {
                
            }
            if (CheckKeyboard(Keys.A) || CheckKeyboard(Keys.Left))
            {
                
            }
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
                {//Gestern auf 1 2 3 geändert. keine ahunug was passiert
                    case 0:
                        currentSelectedCar = 2;
                        Player.player.currentSelectedCar = 2;
                        break;
                    case 1:
                        currentSelectedCar = 1;
                        Player.player.currentSelectedCar = 1;
                        break;
                    case 2:                      
                        currentSelectedCar = 3;
                        Player.player.currentSelectedCar = 3;
                        break;
                    case 3:
                        Game1.GameState = "Game";
                        Player.player.position.X = 4000;
                        Player.player.position.Y = 2000;
                        Player.player.score = 0;
                        Player.player.enemiesKilled = 0;
                        Player.player.shotsFired = 0;
                        Game1.isPaused = false;
                        Player.player.imageIndex = 0;
                        break;
                    case 4:
                        Game1.GameState = "Menu";
                        break;
                }
            }
            prevKeyboard = keyboard;
            prevMouse = mouse;
        }

        public  bool CheckKeyboard(Keys key)
        {
            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }
        public  void Draw(SpriteBatch spriteBatch)
        {
            Color color;
            int linePadding = 30;
            spriteBatch.Begin();
            spriteBatch.Draw(background,Game1.screen,Color.White);
            for (int i = 0; i < buttonList.Count; i++)
            {

                //if (i == selected) color = Color.Yellow else color=Color.Black;
                color = (i == selected) ? Color.Yellow : Color.Black;
                //get half the screen, go up to the max height,inc by linespacing (max height of font) + linepadding
                spriteBatch.DrawString(Game1.menuFont, buttonList[i], new Vector2((Game1.screen.Width / 2),// - (Game1.font.MeasureString(buttonList[i]).X / 2),
                    (Game1.screen.Height / 2) - (Game1.font.LineSpacing * buttonList.Count / 2) + ((Game1.font.LineSpacing + linePadding) * i)), color);
                
            }
            spriteBatch.End();
        }
        public void SetCharacter(int  x)
        {
            switch (x)
            {
                case 2://BTeam
                        Player.player.currentSelectedCar = 2;
                        Player.player.maxHealth = 150;
                        Player.player.maxAmmo = 10;
                        Player.player.ammo = 10;
                        Player.player.fireRate = 10;
                        Player.player.bulletSpeed = 7;
                        Player.player.reloadTime = 60 * 3;
                        Player.player.spd = 5;
                        Bullet.dmg = 50;
                        Player.player.health = Player.player.maxHealth;
                   
                        break;
                case 1: //BrideRider                 
                        Player.player.currentSelectedCar = 1;
                        Player.player.maxHealth = 75;
                        Player.player.maxAmmo = 100;
                        Player.player.ammo = 100;
                        Player.player.fireRate = 5;
                        Player.player.bulletSpeed = 15;
                        Player.player.reloadTime = 60;
                        Player.player.spd = 10;
                        Bullet.dmg = 10;
                        Player.player.health = Player.player.maxHealth;
                
                    break;
                case 3: //FatMan          
                        Player.player.currentSelectedCar =3;
                        Player.player.maxHealth = 100;
                        Player.player.maxAmmo = 20;
                        Player.player.ammo = 20;
                        Player.player.fireRate = 1;
                        Player.player.bulletSpeed = 7;
                        Player.player.reloadTime = 60 * 2;
                        Player.player.spd = 8;
                        Bullet.dmg = 25;
                        Player.player.health = Player.player.maxHealth;
                    
                    break;
                default:
                    Player.player.currentSelectedCar = 1;
                    Player.player.maxHealth = 150;
                    Player.player.maxAmmo = 10;
                    Player.player.ammo = 10;
                    Player.player.fireRate = 10;
                    Player.player.bulletSpeed = 2;
                    Player.player.reloadTime = 60 * 3;
                    Player.player.spd = 5;
                    Bullet.dmg = 50;
                    Player.player.health = Player.player.maxHealth;
                   
                    break;

            }
        }   
    }
}
