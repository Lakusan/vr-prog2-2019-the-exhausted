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
    class Camera
    {
        public Vector2 position;
        private float rotation;
        private float zoom;
        public int maxZoom=0;
        public int minZoom=4;
        private Matrix mTransform;
        public static float zoomAmount = 0.1f;

        public Camera()
        {
            position = new Vector2(Game1.screenSize.Width / 2, Game1.screenSize.Height / 2);
            rotation = 0.0f;
            zoom = 1.0f;

            float wRatio = (float)Game1.screenSize.Width / (float)Game1.screen.Width;
            float hRatio = (float)Game1.screenSize.Height / (float)Game1.screen.Height;
            bool lowestRatio = (wRatio < hRatio) ? true : false;

            float rLength = (lowestRatio) ? Game1.screenSize.Width : Game1.screenSize.Height;
            float sLength = (lowestRatio) ? Game1.screen.Width : Game1.screen.Height;

            float tempZoom = 1.0f;
            int c = 0;
            while (sLength * tempZoom < rLength)
            {
                tempZoom *= 1.0f + (zoomAmount * 2);
                c++;
            }

            maxZoom = c;
            if (maxZoom > 9) { maxZoom = 9; }
        }

        public void Update()
        {
            //follow player
            position = Player.player.position;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 1.0f - (zoomAmount * maxZoom)) zoom = 1.0f - (zoomAmount*maxZoom);
                if (zoom > 1.0f + (zoomAmount * minZoom)) zoom = 1.0f + (zoomAmount * minZoom);
            }
        }

        public void Move(Vector2 amount)
        {
            position += amount;
        }

        public Matrix Transform(GraphicsDevice graphics)
        {
            float ViewPortWidth = graphics.Viewport.Width;
            float ViewPortHeight = graphics.Viewport.Height;

            mTransform = Matrix.CreateTranslation(new Vector3(-position.X,-position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(zoom) *
                Matrix.CreateTranslation(new Vector3(ViewPortWidth * 0.5f, ViewPortHeight * 0.5f, 0));
                

            return mTransform;
        }
        //translate pos relative from map to screen
        public static Vector2 GlobalToLocal(Vector2 pos)
        {
            pos -= (Game1.GetCameraPosition() - new Vector2(Game1.screen.Width / 2, Game1.screen.Height / 2));
            return pos;
        }
        //Translate position relative from map to camera Viewport

        public static Vector2 LocalToGlobal(Vector2  pos)
        {
            pos += (Game1.GetCameraPosition() - new Vector2(Game1.screen.Width / 2, Game1.screen.Height / 2));
            return pos;          
        }
    }
}
