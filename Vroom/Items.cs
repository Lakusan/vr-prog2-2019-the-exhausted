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
    class Items
    {
        
        //public static List<Object> objectList = new List<Object>();
        //placeable objects(Map Editor)
        public static List<Object> objDB = new List<Object>();
        public static List<Object> objectList = new List<Object>();

        //Initializes all the Objects needed
        public static void Initialize()
        {
            objDB.Add(new Object(Vector2.Zero));
            objDB.Add(new Wall(Vector2.Zero));
            objDB.Add(new Player(Vector2.Zero));
            objDB.Add(new Enemy(Vector2.Zero));
            objDB.Add(new Spawner(Vector2.Zero));
            objDB.Add(new Cursor(Vector2.Zero));



            //Map Editor load Stuff
            objectList.Add(new Player(Vector2.Zero));
            for (int i = 0; i < 300; i++)
            {
               

                //place here all available objects for Map Editor
                objectList.Add(new Object(Vector2.Zero));
                objectList.Add(new Wall(Vector2.Zero));
                objectList.Add(new Bullet(new Vector2(0,0)));
                objectList.Add(new Enemy(Vector2.Zero));
                objectList.Add(new Spawner(Vector2.Zero));
                objectList.Add(new Animated(Vector2.Zero));
                objectList.Add(new Cursor(Vector2.Zero));





            }
            ////Player !!! Player Position must not be (0,0) at al cost, because of gameCrash because of pathfinder logic (grid[-1,-1] doesn't exist inside the screen)
            //objectList.Add(new Player(new Vector2(800, 800)));
            ////Wall
            //objectList.Add(new Wall(new Vector2(300, 300)));
            ////Place Spawning Points for Enemy
            //objectList.Add(new Spawner(new Vector2(400, 300)));
            ////TestAnimation
            //objectList.Add(new Animated(new Vector2(600, 600)));


            //Enemy (always after Player!!) and Postition not x or y not 0 or outside the screen because of pathfinder
            //give amount of enemy
            //enemy pos must not be 0,0 ...reason: needs to "see" player
            for (int i = 0; i < 16; i++)
            {
                Enemy e = new Enemy(new Vector2(200, 200));
                e.alive = false;
                objectList.Add(e);
            }

            
        }


        //Reset all Objects in the Objects list
        public static void Reset()
        {

            // if player gets killed - set all objects to !alive
            foreach (Object o in objectList)
            {
                o.alive = false;
            }
        }

    }
}
