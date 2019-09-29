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
    class MapEditor
    {
        #region fields  
        KeyboardState keyboard;
        KeyboardState prevKeyboard;
        MouseState mouse;
        MouseState prevMouse;
        int selected = 0;
        bool isPressed = false;


        //List for all valid Objects (Map Editor)
        List<Object> objectList = new List<Object>();


        Cursor cursor = new Cursor(Vector2.Zero);

        Object selectedObject;

        #endregion fields
        public MapEditor()
        {

        }

        public void LoadContent(ContentManager Content)
        {
            cursor.LoadContent(Content);
            cursor.alive = true;
        }

        public void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            //Scroll to select valid object
            selected += (mouse.ScrollWheelValue > prevMouse.ScrollWheelValue) ? 1 : (mouse.ScrollWheelValue == prevMouse.ScrollWheelValue) ? 0 :-1;
            selected = (selected > Items.objDB.Count -1) ? Items.objDB.Count -1 : ( selected < 0) ? 0:selected;
            if (keyboard.IsKeyDown(Keys.Escape))
                Game1.GameState = "Menu";
            //Controls
            if(mouse.LeftButton==ButtonState.Pressed)
            {
                if(isPressed)
                {
                    //drag selected object with mouse
                    selectedObject.position = new Vector2(mouse.X, mouse.Y);
                    selectedObject.SnapToGrid();
                }
                else
                {
                    isPressed = true;
                    selectedObject = CreateNewObject(selected);
                }
            }
            if(mouse.LeftButton==ButtonState.Released)
            {
                isPressed = false;
                selectedObject = null;
            }
            if(keyboard.IsKeyDown(Keys.Space)) { Game1.GameState = "Menu"; }

            prevMouse = mouse;
            prevKeyboard = keyboard;
            cursor.Update();
        }
        public Object CreateNewObject(int s)
        {
            foreach(Object o in Items.objectList)
            {
                //check i f itemtypes match
                if (!o.draw && o.GetType() == Items.objDB[s].GetType())
                {
                    o.draw = true;
                    o.position = new Vector2(mouse.X, mouse.Y);
                    //Items.objectList.Add(o);
                    objectList.Add(o);
                    return o;
                }
            }
            return new Object();
        }

        public bool CheckMouse()
        {
            //checks leftklicked and havent bevore
            //place object every frame
            return (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            HUD.Draw(spriteBatch);
            spriteBatch.Begin();
            //Draws all  objects
            foreach (Object o in Items.objectList)
            {
                o.Draw(spriteBatch);
            }
            cursor.Draw(spriteBatch);
            spriteBatch.End();
        }

        
    }
}
