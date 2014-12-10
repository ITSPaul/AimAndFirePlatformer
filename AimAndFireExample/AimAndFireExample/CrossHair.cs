using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
    class CrossHair : Sprite
    {
        private Game myGame;
        private float CrossHairVelocity = 5.0f;
        private MouseState previousMouseSate;
        private Vector2 previousPosition;

        public CrossHair(Game g, Texture2D texture, Vector2 userPosition, int framecount) : base(g,texture,userPosition,framecount)
            {
                previousMouseSate = Mouse.GetState();
                myGame = g;
                previousPosition = userPosition;
                
            }

        public override void Update(GameTime gametime)
        {
            // not usable as mouse can go out of window
            MouseState ms = Mouse.GetState();
            previousPosition = position;
            if (ms.X != previousMouseSate.X && ms.Y != previousMouseSate.Y)
                this.position = new Vector2(ms.X, ms.Y);

            Viewport gameScreen = myGame.GraphicsDevice.Viewport;
            //if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //    this.position += new Vector2(1, 0) * CrossHairVelocity;
            //if (Keyboard.GetState().IsKeyDown(Keys.Left))
            //    this.position += new Vector2(-1, 0) * CrossHairVelocity;
            //if (Keyboard.GetState().IsKeyDown(Keys.Up))
            //    this.position += new Vector2(0, -1) * CrossHairVelocity;
            //if (Keyboard.GetState().IsKeyDown(Keys.Down))
            //    this.position += new Vector2(0, 1) * CrossHairVelocity;
            
             //Make sure the Cross Hair stays in the bounds see previous lab for details
            position = Vector2.Clamp(position, Vector2.Zero,
                                            new Vector2(gameScreen.Width - spriteWidth,
                                                        gameScreen.Height - spriteHeight));
            
            base.Update(gametime);
        }

        public void clamp(Rectangle r)
        {
            if (!r.Contains(new Point((int)position.X, (int)position.Y)))
                position = previousPosition;
        }

        public override void Draw(Cameras.Camera2D cam, SpriteBatch spriteBatch)
        {
            base.Draw(cam, spriteBatch);
        }
    }
}
