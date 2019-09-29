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
    class Wall : Object
    {
        public Wall(Vector2 pos) : base(pos)
        {
            solid = true;
            position = pos;
            spriteName = "wall";
            draw = false;
            
        }
    }
}
