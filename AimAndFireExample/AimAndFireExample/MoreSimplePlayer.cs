using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
        //public enum DIRECTION { LEFT, RIGHT, JUMPING, STARTING }
        //public enum PLAYERSTATE {STANDING,FALLING}

        class SimplePlayer : Sprite
        {
            
            protected Texture2D[] textureStates;
            protected Game myGame;
            protected float playerVelocity = 6.0f;
            private rocket myRocket;
            public rocket PlayerRocket
            
            {
                get { return myRocket; }
                set { myRocket = value; }
            }
            protected CrossHair Site;
            const int MAXTIME = 4000;
            float fallingTimer = MAXTIME;
            public Vector2 CentrePos
            {
                get { return position + new Vector2(spriteWidth/ 2, spriteHeight / 2); }
                
            }

            public DIRECTION playerDirection;
        
            public SimplePlayer(Game g, Texture2D texture, Vector2 userPosition, int framecount) : base(g,texture,userPosition,framecount)
            {
                textureStates = new Texture2D[(int)DIRECTION.STARTING + 1];

                textureStates[(int)DIRECTION.STARTING] = texture;
                textureStates[(int)DIRECTION.LEFT] = g.Content.Load<Texture2D>(@"Textures\RunLeft");
                textureStates[(int)DIRECTION.RIGHT] = g.Content.Load<Texture2D>(@"Textures\RunRight");
                textureStates[(int)DIRECTION.JUMPING] = g.Content.Load<Texture2D>(@"Textures\Idle");
                myGame = g;
                Site = new CrossHair(g, g.Content.Load<Texture2D>(@"Textures\Rocketcrosshair"), userPosition, 1);
                playerDirection = DIRECTION.STARTING;
            }

            public void loadRocket(rocket r)
            {
                myRocket = r;
                myRocket.Visible = false;
                
            }


        public override void Update(GameTime gameTime)
        {
           
            Viewport gameScreen = myGame.GraphicsDevice.Viewport;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.position += new Vector2(1, 0) * playerVelocity;
                SpriteImage = textureStates[(int)DIRECTION.RIGHT];
                playerDirection = DIRECTION.RIGHT;
                myRocket.rocketDirection = DIRECTION.RIGHT;
                
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.position += new Vector2(-1, 0) * playerVelocity;
                SpriteImage = textureStates[(int)DIRECTION.LEFT];
                playerDirection=DIRECTION.LEFT;
                myRocket.rocketDirection = DIRECTION.LEFT;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W)) // one jump
            {
                this.position += new Vector2(0, -1) * playerVelocity * 20;
                SpriteImage = textureStates[(int)DIRECTION.JUMPING];
                playerDirection=DIRECTION.JUMPING;
            }
            else
            {
                SpriteImage = textureStates[(int)DIRECTION.STARTING];
                playerDirection=DIRECTION.STARTING;
                //playerSate=PLAYERSTATE.STANDING;
            }

            
            //// check for site change
            //if (Keyboard.GetState().IsKeyDown(Keys.Z))
            //    angleOfRotation -= 0.1f;
            //if (Keyboard.GetState().IsKeyDown(Keys.X))
            //    angleOfRotation += 0.1f;
            
            Site.Update(gameTime);
            // Whenever the rocket is still and loaded it follows the player posiion
            if (myRocket != null && myRocket.RocketState == rocket.ROCKETSTATE.STILL)
                myRocket.position = this.CentrePos;
            // if a roecket is loaded
            if (myRocket != null)
            {
                // fire the rocket and it looks for the target
                if(Keyboard.GetState().IsKeyDown(Keys.Space))
                    myRocket.fire(Site.position);
            }

            // Make sure the player stays in the bounds see previous lab for details
            position = Vector2.Clamp(position, Vector2.Zero,
                                            new Vector2(gameScreen.Width - spriteWidth,
                                                        gameScreen.Height - spriteHeight));
            
            
            if (myRocket != null)
                myRocket.Update(gameTime);
            // Update the players site
            Site.Update(gameTime);
            // call Sprite Update to get it to animated 
            base.Update(gameTime);
        }
            
        public override void Draw(Cameras.Camera2D cam, SpriteBatch spriteBatch)
        {
            base.Draw(cam,spriteBatch);
            Site.Draw(cam,spriteBatch);
            if (myRocket != null && myRocket.RocketState != rocket.ROCKETSTATE.STILL)
                    myRocket.Draw(cam,spriteBatch);
            
        }

    }
}
