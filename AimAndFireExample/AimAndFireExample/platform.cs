using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimatedSprite
{
    class Platform: Sprite
    {
        

        public Platform(Game g, Texture2D texture, Vector2 pos, int fc ) : base(g,texture,pos,fc)
        {
        }

        public bool onTopofMe(Sprite p )
        {
            if (this.BoundingBox.Intersects(p.BoundingBox) && p.BoundingBox.Bottom > this.BoundingBox.Top)
                return true;
            return false;
        }
    }
}
