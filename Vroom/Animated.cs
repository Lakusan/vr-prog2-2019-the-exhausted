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

    class Animated:Object
    {
        public Animated(Vector2 pos):base(pos)
        {
            position = pos;
            spriteName = "explosion";
            imageNumber = 1;
            imageSpeed = 1f;
            solid = false;
            draw = true;
        }
    }
}
