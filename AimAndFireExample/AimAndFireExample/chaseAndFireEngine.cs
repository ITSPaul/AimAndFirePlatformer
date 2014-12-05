using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatedSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AimAndFireExample
{
    class chaseAndFireEngine
    {
        Player player1;
        ChasingEnemy chaser;
        SoundEffect explosionSound;
        SoundEffectInstance explosionPlayer;
        Platform[] floor;
        Cameras.Camera2D cam;

        public chaseAndFireEngine(Game game)
        {
            Vector2 centreScreen = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
            player1 = new Player(game, game.Content.Load<Texture2D>(@"Textures\Idle"), centreScreen, 10);
            chaser = new ChasingEnemy(game, game.Content.Load<Texture2D>(@"Textures\PlayerDot"), new Vector2(200, 200), 2);
            Sprite explosion = new Sprite(game, game.Content.Load<Texture2D>(@"Textures\explodestrip"), Vector2.Zero, 8);
            rocket r = new rocket(game, game.Content.Load<Texture2D>(@"Textures\Rocket"), explosion, Vector2.Zero, 1);
            explosionSound = game.Content.Load<SoundEffect>(@"Audio\explode1");
            explosionPlayer = explosionSound.CreateInstance();
            Platform p = new Platform(game,game.Content.Load<Texture2D>(@"Textures\Floor"),Vector2.Zero,1);
            floor = new Platform[(game.GraphicsDevice.Viewport.Width / p.spriteWidth) + 1];
            
            Vector2 platformPos = new Vector2(0,game.GraphicsDevice.Viewport.Height - p.spriteHeight);

            for (int i = 0; i < floor.Count(); i++)
            {
                floor[i] = new Platform(game,game.Content.Load<Texture2D>(@"Textures\Floor"),platformPos,1);
                platformPos.X += floor[i].spriteWidth;
            }

            player1.loadRocket(r);
            cam = new Cameras.Camera2D(game.GraphicsDevice.Viewport);
        }

        public void Update(GameTime gameTime)
        {

            foreach (Platform block in floor)
                block.Update(gameTime);

            player1.Update(gameTime);
            
            foreach (Platform block in floor)
            {
                if (block.onTopofMe(player1))
                    player1.playerSate = PLAYERSTATE.STANDING;
            }    
            
            // chase enemy can only follow the player if they are alive
            if (chaser.EnemyState == Enemy.ENEMYSTATE.ALIVE)
                chaser.follow(player1);
            
            chaser.Update(gameTime);
            // is the rocket is exploding and the player rocket is at the target over the chaser
            if (!(chaser.EnemyState == Enemy.ENEMYSTATE.DEAD) &&
                player1.PlayerRocket.RocketState == rocket.ROCKETSTATE.EXPOLODING
                && player1.PlayerRocket.collisionDetect(chaser) )
                chaser.die();

            if (chaser.EnemyState == Enemy.ENEMYSTATE.DYING
                && !(explosionPlayer.State == SoundState.Playing))
                    explosionPlayer.Play();

            if (chaser.EnemyState == Enemy.ENEMYSTATE.DEAD)
                    explosionPlayer.Stop();

            cam.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player1.Draw(cam, spriteBatch);
            // TODO: Add your drawing code here
            if (!(chaser.EnemyState == Enemy.ENEMYSTATE.DEAD))
                chaser.Draw(cam,spriteBatch);
            foreach (Platform block in floor)
                block.Draw(cam,spriteBatch);

        }

    }
}
