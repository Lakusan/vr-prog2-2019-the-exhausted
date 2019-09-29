using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;





namespace Vroom
{
    class MapReader
    {

        public bool ReadMap(string pathName)
        {//reads xml and adds objects

            try //good for exeptionhandling and makes it easier..shows message if it fails
            {
                StreamReader reader = new StreamReader(pathName);

                string xmlString = reader.ReadToEnd();

                //Set Map width and height
                SetMap(xmlString);

                

                //split all objects 
                string[] xmlObjectArray = SplitObjects(xmlString);
           

             
                //Create Objects from string array
                CreateAllObjects(xmlObjectArray);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void CreateAllObjects(string[] stringArray)
        {
            foreach (string s in stringArray)
            {
                CreateObject(s);
            }
        }

        public void CreateCollisionMapFromBitmap()
        {
            //Set Bitmap blueprint
            Bitmap bmp = new Bitmap(Image.FromFile("Maps/Map1.bmp"));
            //Update ScreenSize
            Game1.screenSize.Width = bmp.Width;
            Game1.screenSize.Height = bmp.Height;
            Game1.collisionMap = new bool[Game1.screenSize.Width / Pathfinder.gridSize, Game1.screenSize.Height / Pathfinder.gridSize];

            //loop through bmp.width & bmp.height
            for(int x=0;x < bmp.Width; x++)
            {
                for (int y =0;y<bmp.Height;y++)
                {
                    //if r+b+g == 0, its black and solid block
                    Game1.collisionMap[x,y]= (bmp.GetPixel(x,y).R + bmp.GetPixel(x,y).G + bmp.GetPixel(x,y).B == 0);
                }
            }
        }

        public void CreateObject(string s)
        {
            //parse string again and get all the attributes

            string _class = GetAttribute(s, "class");
            int x = Convert.ToInt32(GetAttribute(s, "x"));
            int y = Convert.ToInt32(GetAttribute(s, "y"));
     

            switch(_class)
            {
                case "Wall":
                   
                    SetNewObject(new Wall(new Vector2 (x,y)));
                    break;
                case "Player":
                    SetNewObject(new Player(new Vector2(x, y)));
                    break;
                //case "Box":
                //    Items.objectList.Add(new Box(new Vector2(x, y)));
                //    break;
                case "Spawner":
                    SetNewObject(new Spawner(new Vector2(x, y)));
                    break;
                default:
                    break;
            }

        }

        public void SetNewObject(Object obj)
        {//if we want to place a box, we pass an object to this
            foreach(Object o in Items.objectList)
            {
                if(!o.alive)
                {
                    if(o.GetType()==obj.GetType())
                    {
                        //create object
                        o.alive = true;
                        o.position = obj.position;
                        //no break -> create all objects available
                        break;
                    }
                }
            }
        }

        public string GetAttribute(string objString, string attributeString)
        {
            //checks for chars between the ""
            int pos = objString.IndexOf(attributeString+"=\"");

            if (pos != -1)
            {
                int start = pos + (attributeString + "=\"").Length;
                int end = objString.IndexOf("\"", start );
                int length = end  - start;
                //Console.WriteLine(objString.Substring(start,length)); 
                return objString.Substring(start, length);
            }
            else
            {
                return "error";
            }
        }

        public string[] SplitObjects(string xmlString)
        {
            //easier with List because dynamic
            List<string> stringList = new List<string>();

            //check xml and remove uneccessary things
            xmlString = xmlString.Substring(xmlString.IndexOf("<Objects>"), (xmlString.IndexOf("</Objects>") + 1)-xmlString.IndexOf("<Objects>")+1);
            xmlString = xmlString.Replace("\t", "");
            xmlString = xmlString.Replace("\n", "");
            //xmlString = xmlString.Replace(" ", "");
            xmlString = xmlString.Replace("<Objects>", "");
            xmlString = xmlString.Replace("</Objects>", "");

          

            //string keySplit = "<Object".IndexOf("o");
            //If it can't find "o", returns -1"
            string keySplit = "<obj ";
           
            ////first try
            int pos = xmlString.IndexOf(keySplit);        
    
            //Rest of the time
            while (pos != -1)
            {
                
                //keep finding next object(All of the Objects->pos + 1)
                int start = pos+xmlString.IndexOf(keySplit);
                int end = xmlString.IndexOf("/>", pos + 1); ;
                int length = end - start;

                stringList.Add(xmlString.Substring(start, length));

                pos = xmlString.IndexOf(keySplit, pos + 1);
                //Console.WriteLine(pos);
            }

 
            return stringList.ToArray();
        }

        public void SetMap(string xmlString)
        {
            //easier with List because dynamic
           // List<string> stringList = new List<string>();

            //check xml and remove uneccessary things
            xmlString = xmlString.Substring(xmlString.IndexOf("<Map>") + 1, (xmlString.IndexOf("</Map>") + 1)-xmlString.IndexOf("<Map>")+1);
            xmlString = xmlString.Replace("\t", "");
            xmlString = xmlString.Replace("\n", "");           
            xmlString = xmlString.Replace("<Map>", "");
            xmlString = xmlString.Replace("</Map>", "");

        
            
            int width = Convert.ToInt32(GetAttribute(xmlString, "width"));
            int height = Convert.ToInt32(GetAttribute(xmlString, "height"));

        


            //set screensize (mapsize)
            Game1.screenSize.Width = width;
            Game1.screenSize.Height = height;
        }
    }
}
