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
    class Grid
    {
        public int G = 0;
        public int H = 0;
        public int F = 0;
        public Point parent = new Point(0, 0);
        public bool walkable = false;
        public bool closed = false;

        public void Reset()
        {
            G = 0;
            H = 0;
            F = 0;
            parent = new Point(0, 0);
            closed = false;

        }
    }
}
