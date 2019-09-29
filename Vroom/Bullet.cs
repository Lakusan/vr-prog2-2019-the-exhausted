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
    class Bullet : Object
    {
        public static int dmg;
        //:base(pos) make shure all the arguments are the same (inheritance)
        public Bullet(Vector2 pos) : base(pos)
        {
            position = pos;
            spriteName = "bullet";
            dmg = 10;
            
        }

        public override void Update()
        {
            if (!alive) return;
            //collision with wall
            if (Collision(Vector2.Zero, new Wall(new Vector2(0, 0))))
            {
                alive = false;
            }
            //collision with enemy
            Object o = Collision(new Enemy(new Vector2(0, 0)));
            //check if object is enemy
            if (o.GetType() == typeof(Enemy))
            {
                alive = false;
                
                Enemy e = (Enemy)o;
                //Damage per Hit from bullet
                e.Damage(dmg);


            }
            //boundary checks
            if (position.X < 0 || position.Y < 0 || position.X > Game1.screenSize.Width || position.Y > Game1.screenSize.Height)
            {
                alive = false;
            }
            base.Update();
        }
    }
}
