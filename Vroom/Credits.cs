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
    class Credits
    {
        #region fields
        KeyboardState keyboard;
        KeyboardState prevKeyboard;
        Texture2D background;
        Texture2D names1;
        Texture2D names2;
        Texture2D names3;
        Texture2D names4;
        Texture2D names5;
        Texture2D names6;
        Texture2D names7;
        Texture2D names8;

        Vector2 names1Pos = new Vector2(-2000, Game1.screen.Height / 64);
        Vector2 names2Pos = new Vector2(-2000, Game1.screen.Height / 64);
        Vector2 names3Pos = new Vector2(-2000, Game1.screen.Height / 64);
        Vector2 names4Pos = new Vector2(-2000, Game1.screen.Height / 64);
        Vector2 names5Pos = new Vector2(-2000, Game1.screen.Height / 64);
        Vector2 names6Pos = new Vector2(-2000, Game1.screen.Height / 64);
        Vector2 names7Pos = new Vector2(-2000, Game1.screen.Height / 64);
        Vector2 names8Pos = new Vector2(-2000, Game1.screen.Height / 64);

        public static Credits credits;
        Song creditsMusic;
        public static bool isPlaying = false;

        #endregion fields

        public Credits()
        {
            credits = this;

        
        }
        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("credits");
            names1 = Content.Load<Texture2D>("credits1");
            names2 = Content.Load<Texture2D>("credits2");
            names3 = Content.Load<Texture2D>("credits3");
            names4 = Content.Load<Texture2D>("credits4");
            names5 = Content.Load<Texture2D>("credits5");
            names6 = Content.Load<Texture2D>("credits6");
            names7 = Content.Load<Texture2D>("credits7");
            names8 = Content.Load<Texture2D>("credits8");
            creditsMusic = Content.Load<Song>("04 Cold as Steel");
   
            
        }
        public void Update(GameTime gameTime)
        {
           


            keyboard = Keyboard.GetState();
            
                    names1Pos.X += 5;
                if (names1Pos.X > names1.Width + 20)
                    names2Pos.X += 5;
                if (names2Pos.X > names2.Width + 20)
                    names3Pos.X += 5;
                if (names3Pos.X > names3.Width + 20)
                    names4Pos.X += 5;
                if (names4Pos.X > names4.Width + 20)
                    names5Pos.X += 5;
                if (names5Pos.X > names5.Width + 20)
                    names6Pos.X += 5;
                if (names6Pos.X > names6.Width + 20)
                    names7Pos.X += 5;
                if (names7Pos.X > names7.Width + 20)
                    names8Pos.X += 5;
    
                if(names8Pos.X > names8.Width)
            {
                names1Pos.X = -2000;
                names2Pos.X = -2000;
                names3Pos.X = -2000;
                names4Pos.X = -2000;
                names5Pos.X = -2000;
                names6Pos.X = -2000;
                names7Pos.X = -2000;
                names8Pos.X = -2000;
            }

            if (keyboard.IsKeyDown(Keys.Space) )
            {
                Game1.GameState = "Menu";
                names1Pos.X = -2000;
                names2Pos.X = -2000;
                names3Pos.X = -2000;
                names4Pos.X = -2000;
                names5Pos.X = -2000;
                names6Pos.X = -2000;
                names7Pos.X = -2000;
                names8Pos.X = -2000;
                
            }
           
            prevKeyboard = keyboard;
            if (!isPlaying)
            {
                MediaPlayer.Stop();
                isPlaying = true;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(creditsMusic);
                MenuComponent.menuComponent.SetFalse();
            }


        }

        public bool CheckKeyboard(Keys key)
        {

            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Game1.screen, Color.White);
            spriteBatch.Draw(names1, names1Pos, new Rectangle(0,0,names1.Width,names1.Height), Color.White) ;
            spriteBatch.Draw(names2, names2Pos, new Rectangle(0,0,names2.Width,names2.Height), Color.White) ;
            spriteBatch.Draw(names3, names3Pos, new Rectangle(0,0,names3.Width,names3.Height), Color.White) ;
            spriteBatch.Draw(names4, names4Pos, new Rectangle(0,0,names4.Width,names4.Height), Color.White) ;
            spriteBatch.Draw(names5, names5Pos, new Rectangle(0,0,names5.Width,names5.Height), Color.White) ;
            spriteBatch.Draw(names6, names6Pos, new Rectangle(0,0,names6.Width,names6.Height), Color.White) ;
            spriteBatch.Draw(names7, names7Pos, new Rectangle(0,0,names7.Width,names7.Height), Color.White) ;
            spriteBatch.Draw(names8, names8Pos, new Rectangle(0,0,names8.Width,names8.Height), Color.White);
            spriteBatch.DrawString(Game1.font, "PRESS SPACE", new Vector2(Game1.screen.Width/2-200,Game1.screen.Height - 200), Color.White);             
                         
            spriteBatch.End();
        }
        public void SetFalse()
        {
            isPlaying = false;
        }
    }
}
