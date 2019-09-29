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
    class Object
    {
        //dynamic Objects - everything that moves
        //loop through all objects, update, draw....

        #region fields
        //Dictionary for sprites
        //keeps int for order, here: key = spriteName
        public static Dictionary<string, Texture2D> spriteDict = new Dictionary<string, Texture2D>();

        public Vector2 position;
        public float rotation = 0.0f;
        public Texture2D spriteIndex;

        //standart sprite for object
        public string spriteName = "block";
        
        public float speed = 0.0f;
        public float scale = 1.0f;
        public bool alive = false;
       
        //for Map Editor
        public bool draw = false;

       public GameTime gameTime;
   


        //Animation
        Rectangle imageArea;
        public Point frame;
        public float imageIndex ;
        protected float imageSpeed;
        public int imageNumber=1;
        public int imageRow=1;
        public int currentImageRow=1;

        //Collision
        //Collision Mask as Rectangle -> HitBox
        public Rectangle area;
        public bool solid = false;

        #endregion fields

        #region properties
        public Object(Vector2 pos)
        {
            position = pos;
            SnapToGrid();
        }

        public Object()
        {
         
        }

        public static void InitSpriteList(ContentManager Content)
        {
            //All SpriteSheets and Images are set manually
            //maybe with xml and string parsing like map objects in the future
            spriteDict.Add("bullet",Content.Load<Texture2D>("bullet"));
            spriteDict.Add("BrideRider",Content.Load<Texture2D>("BrideRider"));
            spriteDict.Add("MadMax",Content.Load<Texture2D>("MadMax"));
          
            spriteDict.Add("wall",Content.Load<Texture2D>("wall"));
            spriteDict.Add("block",Content.Load<Texture2D>("block"));
            spriteDict.Add("BTeam",Content.Load<Texture2D>("BTeam"));
            spriteDict.Add("FatMan",Content.Load<Texture2D>("FatMan"));
            spriteDict.Add("map_bunker",Content.Load<Texture2D>("map_bunker"));
            spriteDict.Add("MadMaxSheet", Content.Load<Texture2D>("MadMaxSheet"));
            spriteDict.Add("test_auto", Content.Load<Texture2D>("test_auto"));
            spriteDict.Add("SpriteSheet", Content.Load<Texture2D>("SpriteSheet"));

        }

        public virtual void Update()
        {
  
            //If Player is dead return
            if (!alive) return;
            //Update area -> HitBox
            UpdateArea();
            moveTo(speed, rotation);
        }

        public virtual void LoadContent(ContentManager content)
        {
            spriteIndex = content.Load<Texture2D>(this.spriteName);
            frame = new Point(spriteIndex.Width/imageNumber, spriteIndex.Height);
            area = new Rectangle(0, 0, spriteIndex.Width/imageNumber, spriteIndex.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

            
            if (!draw || !alive) return;
            Rectangle size;
            Vector2 center = new Vector2(spriteIndex.Width / 2, spriteIndex.Height / 2);
            //Animated Stuff (? and : is like if-condition)
            
            imageIndex += (imageIndex < imageNumber - 1) ? imageSpeed : -imageIndex;
            size = new Rectangle((int)imageIndex * frame.X, 0, frame.X, frame.Y);           
            spriteBatch.Draw(spriteIndex, position, size, Color.White, MathHelper.ToRadians(rotation), center, scale, SpriteEffects.None, 0);
            
        }

        //Collision detection
        //loops through objects
        public bool Collision(Vector2 pos, Object obj)
        {
            //offset area
            //rectangle +=- rectangle would be pointer to rectangle and we don't want that
            Rectangle newArea = new Rectangle(area.X, area.Y, area.Width, area.Height);
            newArea.X += (int)pos.X;
            newArea.Y += (int)pos.Y;

            foreach (Object o in Items.objectList)
            {
                if (o.GetType() == obj.GetType() && o.solid)
                {
                    if (o.area.Intersects(newArea))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //Collision for animation model
        public bool Collision(Vector2 pos, bool checkSolids)
        {

            Rectangle newArea = new Rectangle(area.X, area.Y, area.Width, area.Height);
            newArea.X += (int)pos.X;
            newArea.Y += (int)pos.Y;

            foreach (Object o in Items.objectList)
            {
                if (o.solid || !checkSolids)
                {
                    if (o.area.Intersects(newArea))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //collision Enemy
        public Object Collision(Object obj)
        {
            foreach (Object o in Items.objectList)
            {
                if (o.GetType() == obj.GetType() && o.alive)
                {
                    if (o.area.Intersects(area))
                    {
                        return o;
                    }
                }
            }
            return new Object();
        }

        //Update Area to current Position (Hit Box)
        public void UpdateArea()
        {
            area.X = (int)position.X - (spriteIndex.Width / 2);
            area.Y = (int)position.Y - (spriteIndex.Height / 2);
        }

        public virtual void moveTo(float pix, float dir)
        {
            float newX = (float)Math.Cos(MathHelper.ToRadians(dir));
            float newY = (float)Math.Sin(MathHelper.ToRadians(dir));
            position.X += pix * (float)newX;
            position.Y += pix * (float)newY;
        }

        public virtual void SnapToGrid()
        {
            position = new Vector2(
                (int)Math.Round(position.X / Pathfinder.gridSize)*Pathfinder.gridSize,
                (int)Math.Round(position.Y / Pathfinder.gridSize)*Pathfinder.gridSize);
        }

        
        #endregion properties

    }
}
