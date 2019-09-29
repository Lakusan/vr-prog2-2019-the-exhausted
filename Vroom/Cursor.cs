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
    class Cursor : Object
    {
        MouseState mouse;

        public Cursor(Vector2 pos) : base(pos)
        {
            position = pos;
            spriteName = "cursor";
            draw = true;
        }

        public override void Update()
        {
            mouse = Mouse.GetState();
            position = new Vector2(mouse.X, mouse.Y);

            base.Update();
        }
    }
}
