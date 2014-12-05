using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AnimatedSprite
{
    class ChasingEnemy: Enemy
    {
        float chaseRdaius = 200;
        bool FullOnChase = false;
        
        public ChasingEnemy(Game g, Texture2D texture, Vector2 Position1, int framecount) 
             : base(g,texture,Position1,framecount)
        {
            startPosition = Position1;
            this.Velocity = 2.0f;
        }

        // folow a player if the player comes in the kill zone
        public void follow(Player p)
        {
            if (inChaseZone(p) )
            {
                Vector2 direction = p.position - this.position;
                direction.Normalize();                
                this.position += direction * Velocity;
            }            
        }

        // inChaseZone sees if the player is in the kill zone
        // if so it takes approproate action
            public bool inChaseZone(Player p)
            {
                float distance = Math.Abs(Vector2.Distance(this.position, p.CentrePos));
                if (distance <= chaseRdaius)
                    return true;
                return false;
            }
    }
}
