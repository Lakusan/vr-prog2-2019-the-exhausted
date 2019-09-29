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
    class HUD
    {
        public static void Draw(SpriteBatch spriteBatch)
        {
            //HUD
            if (Game1.GameState == "Game")
            {
                spriteBatch.Draw(Game1.hud, new Rectangle(0,(Game1.screen.Height-Game1.hud.Height), Game1.hud.Width, Game1.hud.Height), Color.White);
                //Health
                spriteBatch.DrawString(Game1.font, "Health " , new Vector2(150,(Game1.screen.Height-(Game1.hud.Height/2-20))), Color.White);
                spriteBatch.DrawString(Game1.font, Player.player.health + " / " + Player.player.maxHealth, new Vector2(150,(Game1.screen.Height-(Game1.hud.Height/2-40))), Color.White);
                //Ammo 
                spriteBatch.DrawString(Game1.font, "Ammo ", new Vector2(360,870), Color.White);
                spriteBatch.DrawString(Game1.font, ""+Player.player.maxAmmo, new Vector2(450,880), Color.White);
                spriteBatch.DrawString(Game1.font, ""+Player.player.ammo, new Vector2(450,910), Color.White);
                //reloading indicator
                if (Player.player.reloading)
                {
                    spriteBatch.DrawString(Game1.font, "-RELOADING-", new Vector2(Game1.hud.Width-375,Game1.screen.Height -Game1.hud.Height/2+50), Color.Red);
                }
                //score
                spriteBatch.DrawString(Game1.font, "Score: " + Player.player.score, new Vector2(Game1.hud.Width - 150, Game1.screen.Height - Game1.hud.Height / 2 + 70), Color.Red);
  
            }

        }
    }
}
