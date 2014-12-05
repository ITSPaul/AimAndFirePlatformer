using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace AnimatedSprite
{
    class rocket : Sprite
    {

            public enum ROCKETSTATE { STILL, FIRING, EXPOLODING };

            public float AngleOfRotation
            { 
                get {return angleOfRotation;}
                set { angleOfRotation = value; }
            }
            protected Game myGame;
            protected float RocketVelocity = 4.0f;
            Vector2 textureCenter;
            Vector2 Target;
            Sprite explosion;
            float ExplosionTimer = 0;
            float ExplosionVisibleLimit = 500;
            Vector2 StartPosition;
            ROCKETSTATE rocketState = ROCKETSTATE.STILL;
            public DIRECTION rocketDirection;
            private float speed = 1f;
        
            public ROCKETSTATE RocketState
            {
                get { return rocketState; }
                set { rocketState = value; }
            }

            public Sprite Explosion
            {
                get { return explosion; }
                set { explosion = value; }
            }

            public rocket(Game g, Texture2D texture, Sprite rocketExplosion, Vector2 userPosition, int framecount) 
                : base(g,texture,userPosition,framecount)
            {
                Target = Vector2.Zero;
                myGame = g;
                textureCenter = new Vector2(texture.Width/2,texture.Height/2);
                angleOfRotation = 0;
                explosion =  rocketExplosion;
                explosion.position -= textureCenter;
                explosion.Visible = false;
                StartPosition = position;
                RocketState = ROCKETSTATE.STILL;
                rocketDirection = DIRECTION.STARTING;
                
            }
            public override void Update(GameTime gametime)
            {
                switch (rocketState)
                {
                    case ROCKETSTATE.STILL:
                        this.Visible = false;
                        explosion.Visible = false;
                        break;
                    case ROCKETSTATE.FIRING:
                        this.Visible = true;
                        // Using Lerp here could use target - pos and normalise for direction and then apply
                        // Velocity
                        
                        position = Vector2.Lerp(position, Target, 0.02f * RocketVelocity);
                        if (Vector2.Distance(position, Target) < 2)
                            rocketState = ROCKETSTATE.EXPOLODING;
                        break;
                    case ROCKETSTATE.EXPOLODING:
                        explosion.position = Target;
                        explosion.Visible = true;
                        break;
                }

                //switch(rocketDirection)
                //{
                //    case DIRECTION.LEFT:
                //        Effect = SpriteEffects.FlipHorizontally;
                //        break;
                //    default:
                //        Effect = SpriteEffects.None;
                //        break;
                //}
                // if the explosion is visible then just play the animation and count the timer
                if (explosion.Visible)
                {
                    explosion.Update(gametime);
                    ExplosionTimer += gametime.ElapsedGameTime.Milliseconds;
                }
                // if the timer goes off the explosion is finished
                if (ExplosionTimer > ExplosionVisibleLimit)
                {
                    explosion.Visible = false;
                    ExplosionTimer = 0;
                    rocketState = ROCKETSTATE.STILL;
                }

                base.Update(gametime);
            }
            public void fire(Vector2 SiteTarget)
            {
                // one rocket at a time
                if (rocketState != ROCKETSTATE.FIRING)
                {
                    // work out the angle of rotation of the image
                    angleOfRotation = TurnToFace(position, SiteTarget, angleOfRotation, 180f);
                    rocketState = ROCKETSTATE.FIRING;
                    // maintain the target overtime
                    Target = SiteTarget;
                    // work out the direction
                    Vector2 direction = new Vector2((float)Math.Cos(angleOfRotation),
                                        (float)Math.Sin(angleOfRotation));
                    direction.Normalize();
                    position += direction * speed;
                }

            }

     protected static float TurnToFace(Vector2 position, Vector2 faceThis,
                float currentAngle, float turnSpeed)
            {
                // The difference in the two points is 
                float x = faceThis.X - position.X;
                float y = faceThis.Y - position.Y;
                // ArcTan calculates the angle of rotation 
                // relative to a point (the gun turret position)
                // in the positive x plane and 
                float desiredAngle = (float)Math.Atan2(y, x);

                float difference = WrapAngle(desiredAngle - currentAngle);

                difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

                return WrapAngle(currentAngle + difference);
            }
            /// <summary>
            /// Returns the angle expressed in radians between -Pi and Pi.
            /// Angle is always positive
            /// </summary>
            private static float WrapAngle(float radians)
            {
                while (radians < -MathHelper.Pi)
                {
                    radians += MathHelper.TwoPi;
                }
                while (radians > MathHelper.Pi)
                {
                    radians -= MathHelper.TwoPi;
                }
                return radians;
            }
            
            public override void Draw(Cameras.Camera2D cam,SpriteBatch spriteBatch)
            {
                
                base.Draw(cam,spriteBatch);
                if (explosion.Visible)
                    explosion.Draw(cam, spriteBatch);
                

            }

    }
}
