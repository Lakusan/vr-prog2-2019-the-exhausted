using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Vroom
{
    class Enemy : Object
    {
        #region fields
        int health;
        const int maxHealth = 100;
        //Following Speed
       
        
        public float spd = 2f;
        private Vector2 dest;
        private bool[,] map;
        private List<Point> path = new List<Point>();
        private int pathIndex = 0;
        private int hitTimer = 0;
        private int hitTime = 60;
        private bool finding = false;
        private Thread t;
        //DAMAGE dealt
        private int dmg;
        public bool exploding;

       
        //static
        public static Enemy enemy;
        #endregion fields


        #region methods
        public Enemy(Vector2 pos) : base(pos)
        {
            position = pos;
            spriteName = "MadMax";
            dest = position;
            health = maxHealth;
            dmg = 15;
            draw = true;
            exploding = false;
        }

        public override void Update()
        {
            
            if (!alive)
            {
                return;
            }
            enemy = this;
            IncrementTimers();
            //if enemy is not in the screen, it wont even chekc for a path
            if(PointDistance(Player.player.position.X,Player.player.position.Y,position.X,position.Y)<Game1.screen.Width/2)
            {
                //MoveToDestination();
                rotation = PointDirection(position.X,position.Y, Player.player.position.X, Player.player.position.Y);
                speed = spd;
            }
            TryToHitPlayer();




            if (health <= 0)
            {
                alive = false;
                exploding = true;
                Player.player.score++;
                Player.player.enemiesKilled++;
                health = maxHealth;
            }

            base.Update();
        }

        private void TryToHitPlayer()
        {
            //if in distance hit player
            if (PointDistance(position.X, position.Y, Player.player.position.X, Player.player.position.Y) < Pathfinder.gridSize)
            {
                if (hitTimer > hitTime)
                {
                    hitTimer = 0;
                    //Damage to Player
                    Player.player.Damage(dmg);
                    Enemy.enemy.alive = false;
                    //Explosion trigger
                    

                }
            }
        }

        public void FindPath()
        {
            //check map collisions and makes grid
            map = Pathfinder.WriteMap(position);
            //create new pathfinder
            Pathfinder finder;
            finder = new Pathfinder(map);
            path = finder.FindPath(position, dest);
        }

        public void SetPath()

        {//if thread is working dont do anything
            if (Game1.t[0] != null)
            {
                if (Game1.t[0].IsAlive == true)
                {
                    return;
                }
            }
            //Set what to search
            dest = Player.player.position;

            //controls amount of enemies on thread
            //controlled amount of threads because of performance issues
            if (Pathfinder.queue < 1 || finding)
            {
                if (!finding)
                {
                    //tries to find path
                    //no () because only one parameter max
                    Game1.t[0] = new Thread(FindPath);
                    Game1.t[0].Start();


                    //t = new Thread(FindPath);
                    //t.Start();
                    finding = true;
                    Pathfinder.queue++;
                }
                //threads (like game.Run() - doesnt slow other threads while working)
                //can be given a method and tires to complete it
                
                if (!Game1.t[0].IsAlive)
                {
                    //thread is finished / !IsAlive
                    Game1.t[0].Abort();
                    Game1.t[0] = null;
                    //path found
                    finding = false;
                    Pathfinder.queue--;
                    pathIndex = 0;
                }
            }
        }

        private void MoveToDestination()
        {
            SetPath();
        
            if (path != null) 
            {
                //if node is smaller than max nodes to Destination -> follow path
                if (pathIndex < path.Count)
                {
                    if (StepToPoint(path[pathIndex]))
                    {
                        pathIndex++;
                    }
                }
                else if (path.Count >= 0)
                {
                    path = null;
                    pathIndex = 0;
                    dest = Player.player.position;
                    SetPath();
                }
            }

        }

        private bool StepToPoint(Point point)
        {
            //if distance between enemy and node in path less than gridsize, stop enemy/return destination
            if (PointDistance(position.X, position.Y, point.X, point.Y) < Pathfinder.gridSize)
            {
                speed = 0;
                return true;
            }
            //face destination
            rotation = PointDirection(position.X, position.Y, point.X, point.Y);
            speed = spd;
            moveTo(speed, rotation);
            return false;
        }
       
        public override void moveTo(float pix, float dir)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(dir));
            float newY = (float)Math.Sin(MathHelper.ToRadians(dir));
            position.X += pix * (float)newX;
            position.Y += pix * (float)newY;
            //If there is nothing in the way - move
            if (!Collision(new Vector2(newX, newY), true))
            {
                base.moveTo(pix, dir);
            }
            else
            {
                alive = false;
                exploding = true;
                
                //no collision -> take enemy Direction and push away from solid
                //Object collisionObject = Collision(new Wall(Vector2.Zero));
                //float tempDir = PointDirection(collisionObject.position.X, collisionObject.position.Y, position.X, position.Y);
                //base.moveTo(pix, tempDir); // Push away from colliding object
                //base.moveTo(pix,dir); // Continue path
            }
        }

        public float PointDistance(float x1, float y1, float x2, float y2)
        {
            float xRect = (x1 - x2) * (x1 - x2);
            float yRect = (y1 - y2) * (y1 - y2);
            double hRect = xRect + yRect;

            float dist = (float)Math.Sqrt(hRect);

            return dist;

        }

        private float PointDirection(float x, float y, float x2, float y2)
        {
            float diffx = x - x2;
            float diffy = y - y2;
            float adj = diffx;
            float opp = diffy;
            float res = MathHelper.ToDegrees((float)Math.Atan2(opp, adj));
            res = (res + 180) % 360;
            if (res < 0)
            {
                res += 360;
            }
            return res;

        }

        public void Damage(int dmg)
        {
            health -= dmg;

        }

        private void IncrementTimers()
        {
            hitTimer++;
        }
        #endregion methods
    }
}
