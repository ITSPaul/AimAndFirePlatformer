using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
        public enum DIRECTION { LEFT, RIGHT, JUMPING, STARTING }
        public enum PLAYERSTATE {STANDING,JUMPING, FALLING}

        class Player : Sprite
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
            float JumpTime = MAXTIME;
            public Vector2 CentrePos
            {
                get { return position + new Vector2(spriteWidth/ 2, spriteHeight / 2); }
                
            }
            private Rectangle _playerExtent;
            // used to keep the scope in focus

            public Rectangle PlayerExtent
            {
                get
                {
                    return new Rectangle((int)position.X -200,(int)position.Y -200,400,400);
                }
                set { _playerExtent = value; }
            }
            public DIRECTION playerDirection;
            public PLAYERSTATE playerSate;
        
            public Player(Game g, Texture2D texture, Vector2 userPosition, int framecount) : base(g,texture,userPosition,framecount)
            {
                textureStates = new Texture2D[(int)DIRECTION.STARTING + 1];
                textureStates[(int)DIRECTION.STARTING] = texture;
                textureStates[(int)DIRECTION.LEFT] = g.Content.Load<Texture2D>(@"Textures\spr_left_strip4");
                textureStates[(int)DIRECTION.RIGHT] = g.Content.Load<Texture2D>(@"Textures\spr_right_strip4");
                textureStates[(int)DIRECTION.JUMPING] = g.Content.Load<Texture2D>(@"Textures\spr_up_strip4");
                myGame = g;
                Site = new CrossHair(g, g.Content.Load<Texture2D>(@"Textures\Rocketcrosshair"), userPosition, 1);
                playerDirection = DIRECTION.JUMPING;
                playerSate = PLAYERSTATE.FALLING;
            }

            public void loadRocket(rocket r)
            {
                myRocket = r;
                myRocket.Visible = false;
                
            }


        public override void Update(GameTime gameTime)
        {
           
            Viewport gameScreen = myGame.GraphicsDevice.Viewport;
            switch (playerSate)
            {
                case PLAYERSTATE.FALLING:
                //    if (fallingTimer > 0)
                //    {
                        this.position += new Vector2(0, 1) * playerVelocity;
                        if (Keyboard.GetState().IsKeyDown(Keys.D))
                            this.position += new Vector2(1, 0) * playerVelocity;
                        else if (Keyboard.GetState().IsKeyDown(Keys.A))
                            this.position += new Vector2(-1, 0) * playerVelocity;
                        playerSate = PLAYERSTATE.FALLING;
                    //    fallingTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    //}
                    //else
                    //{
                    //    playerSate = PLAYERSTATE.STANDING;
                    //    fallingTimer = MAXTIME;
                    //}
                    break;

                case PLAYERSTATE.JUMPING:
                    if (JumpTime > 0)
                    {
                        this.position += new Vector2(0, 1) * playerVelocity;
                        if (Keyboard.GetState().IsKeyDown(Keys.D))
                            this.position += new Vector2(1, 0) * playerVelocity;
                        else if (Keyboard.GetState().IsKeyDown(Keys.A))
                            this.position += new Vector2(-1, 0) * playerVelocity;
                        playerSate = PLAYERSTATE.FALLING;
                        JumpTime -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                    else
                    {
                        playerSate = PLAYERSTATE.FALLING;
                        JumpTime = MAXTIME;
                    }
                    break;

            }
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
            else if (playerSate != PLAYERSTATE.JUMPING && Keyboard.GetState().IsKeyDown(Keys.W)) // one jump
            {
                this.position += new Vector2(0, -1) * playerVelocity * 5;
                SpriteImage = textureStates[(int)DIRECTION.JUMPING];
                playerDirection=DIRECTION.JUMPING;
                playerSate = PLAYERSTATE.JUMPING;
            }
            else
            {
                SpriteImage = textureStates[(int)DIRECTION.STARTING];
                playerDirection=DIRECTION.STARTING;
                //playerSate=PLAYERSTATE.STANDING;
            }

            
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                angleOfRotation -= 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                angleOfRotation += 0.1f;
            //myRocket.AngleOfRotation = angleOfRotation;
            // check for site change            
            Site.Update(gameTime);
            Site.clamp(PlayerExtent);
            // Whenever the rocket is still and loaded it follows the player posiion
            if (myRocket != null && myRocket.RocketState == rocket.ROCKETSTATE.STILL)
                myRocket.position = this.CentrePos;
            // if a roecket is loaded
            if (myRocket != null)
            {
                // fire the rocket and it looks for the target
            
                if(PlayerRocket.RocketState == rocket.ROCKETSTATE.STILL 
                    &&  Keyboard.GetState().IsKeyDown(Keys.Space))
                    myRocket.fire(Site.position);
            }

            // Make sure the player stays in the bounds see previous lab for details
            //position = Vector2.Clamp(position, Vector2.Zero,
            //                                new Vector2(gameScreen.Width - spriteWidth,
            //                                            gameScreen.Height - spriteHeight));
            //if (position.Y == gameScreen.Height - spriteHeight)
            //    playerSate = PLAYERSTATE.STANDING;
            
            
            if (myRocket != null)
                myRocket.Update(gameTime);
            // Update the players site
            Site.Update(gameTime);
            // call Sprite Update to get it to animated and update the bounding box
            base.Update(gameTime);


        }

        public bool imAbove(Platform p)
        {
            if (this.BoundingBox.Intersects(p.BoundingBox) 
                && this.BoundingBox.Bottom > p.BoundingBox.Top) 
                return true;
            return false;
        }
        
        public bool imBelow(Platform p)
        {
            if (this.BoundingBox.Intersects(p.BoundingBox)
                && this.BoundingBox.Top < p.BoundingBox.Bottom) 
                    return true;
            return false;
        }

        public bool imRightOf(Platform p)
        {
            if (this.BoundingBox.Intersects(p.BoundingBox)
                && this.BoundingBox.Left > p.BoundingBox.Right)
                return true;
            return false;
        }
        public bool imLeftOf(Platform p)
        {
            if (this.BoundingBox.Intersects(p.BoundingBox)
                && this.BoundingBox.Right < p.BoundingBox.Left)
                return true;
            return false;
        }
        public override void Draw(Cameras.Camera2D cam, SpriteBatch spriteBatch)
        {
            base.Draw(cam, spriteBatch);
            Site.Draw(cam,spriteBatch);
            if (myRocket != null && myRocket.RocketState != rocket.ROCKETSTATE.STILL)
                    myRocket.Draw(cam,spriteBatch);
            
        }

    }
}
