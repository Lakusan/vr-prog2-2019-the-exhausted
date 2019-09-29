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
    class Spawner : Object
    {
        private int spawnTimer;
        //Every 3 Secs spawn 60*3
        private protected int spawnTime = 60;

        public Spawner(Vector2 pos) : base(pos)
        {
            position = pos;
            spriteName = "block";
            draw = false;


        }

        public override void Update()
        {
            if (!alive) return;
            Random rnd = new Random();
            
            IncrementTimers();
            if (spawnTimer > spawnTime)
            {
                spawnTimer = 0;

                foreach (Object o in Items.objectList)
                {
                    if (o.GetType() == typeof(Enemy) && !o.alive)
                    {
                        o.alive = true;
                        o.position = position;
                        o.speed =(float)(rnd.Next(1,6)+ rnd.NextDouble());
                        break;
                    }
                }
            }

        }

        private void IncrementTimers()
        {
            spawnTimer++;
        }
    }
}
