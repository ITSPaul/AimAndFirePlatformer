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
using TileMapEx;

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
        Game _game;
        private Platform[] platform1;
        List<Platform> allBlocks = new List<Platform>();
        int tileWidth = 64;
        int tileHeight = 64;
        public enum TileType { Dirt, Grass, Ground, Mud, Road, Rock, Wood };
        TileManager _tManager;
        
        List<Texture2D> tileTextures = new List<Texture2D>();
        int[,] tileMap = new int[,]  
    {
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,},

    };


        public chaseAndFireEngine(Game game)
        {
            _game = game;
            Vector2 centreScreen = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
            player1 = new Player(game, game.Content.Load<Texture2D>(@"Textures\spr_stand_strip4"), centreScreen, 4);
            chaser = new ChasingEnemy(game, game.Content.Load<Texture2D>(@"Textures\PlayerDot"), new Vector2(200, 200), 2);
            Sprite explosion = new Sprite(game, game.Content.Load<Texture2D>(@"Textures\explodestrip"), Vector2.Zero, 8);
            rocket r = new rocket(game, game.Content.Load<Texture2D>(@"Textures\Rocket"), explosion, Vector2.Zero, 1);
            explosionSound = game.Content.Load<SoundEffect>(@"Audio\explode1");
            explosionPlayer = explosionSound.CreateInstance();
            setupTiles();


            Platform p = new Platform(game,game.Content.Load<Texture2D>(@"Textures\Floor"),Vector2.Zero,1);
            floor = new Platform[50];
            platform1 = new Platform[10];
            
            Vector2 platformPos = new Vector2(0,
                game.GraphicsDevice.Viewport.Height * 0.5f + player1.spriteHeight + 1 );

            for (int i = 0; i < floor.Count(); i++)
            {
                floor[i] = new Platform(game,game.Content.Load<Texture2D>(@"Textures\Floor"),platformPos,1);
                platformPos.X += floor[i].spriteWidth;
                allBlocks.Add(floor[i]);
            }

            platformPos = new Vector2(600, 200);
            for (int i = 0; i < platform1.Count(); i++)
            {
                platform1[i] = new Platform(game, game.Content.Load<Texture2D>(@"Textures\Floor"), platformPos, 1);
                platformPos.X += platform1[i].spriteWidth;
                allBlocks.Add(platform1[i]);
            }

            player1.loadRocket(r);
            player1.position = new Vector2(_game.GraphicsDevice.Viewport.Width / 2,
                                            _game.GraphicsDevice.Viewport.Height / 2);
            cam = new Cameras.Camera2D(game.GraphicsDevice.Viewport);
            cam.Following = true;
            player1.playerSate = PLAYERSTATE.FALLING;
            //cam.Pos = player1.position;
            //-new Vector2(_game.GraphicsDevice.Viewport.Width / 2,
            //                                _game.GraphicsDevice.Viewport.Height / 2);
            
        }

        private void setupTiles()
        {
            _tManager = new TileManager();
            Texture2D dirt = _game.Content.Load<Texture2D>("Tiles/se_free_dirt_texture");
            tileTextures.Add(dirt);

            Texture2D grass = _game.Content.Load<Texture2D>("Tiles/se_free_grass_texture");
            tileTextures.Add(grass);

            Texture2D ground = _game.Content.Load<Texture2D>("Tiles/se_free_ground_texture");
            tileTextures.Add(ground);

            string[] backTileNames = { "free", "grass", "ground" };
            string[] impassibleTiles = { };

            _tManager.addLayer("background", backTileNames, tileMap);
            _tManager.ActiveLayer = _tManager.getLayer("background");
        }

        public void Update(GameTime gameTime)
        {

            foreach (Platform block in floor)
                block.Update(gameTime);
            foreach (Platform block in platform1)
                block.Update(gameTime);

            Vector2 oldPlayerPosition =  player1.position;
            player1.Update(gameTime);
                        
            foreach (Platform block in floor)
            {
                if (block.onTopofMe(player1)){
                    player1.playerSate = PLAYERSTATE.STANDING;
                    break;
                }
            }

            foreach (Platform block in platform1)
            {
                if (block.onTopofMe(player1)){
                    player1.playerSate = PLAYERSTATE.STANDING;
                    break;
                }

            }

            // need to clip against camera extents
            foreach (var block in allBlocks)
            {
                if (block.onTopofMe(player1))
                {
                    player1.playerSate = PLAYERSTATE.STANDING;
                    break;
                }
                else player1.playerSate = PLAYERSTATE.FALLING;
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
            if (player1.position != oldPlayerPosition)
            {
                if (player1.position.X > _game.GraphicsDevice.Viewport.Width * 0.5f
                    && player1.position.X < (tileMap.GetLength(1) * tileWidth) - _game.GraphicsDevice.Viewport.Width * 0.5f
                    && player1.position.Y > 0
                    && player1.position.Y < (tileMap.GetLength(0) * tileHeight) - _game.GraphicsDevice.Viewport.Height * 0.5f)
                {
                    cam.Pos = player1.position;
                    cam.Update();
                }
                else
                {
                    player1.position = oldPlayerPosition;
                    cam.Pos = player1.position;
                    cam.Update();
                }
                //Vector2 difference = player1.position - oldPlayerPosition;
                //difference.Normalize();
                //cam.Pos = 
                //    player1.position - new Vector2(_game.GraphicsDevice.Viewport.Width ,
                  //                          _game.GraphicsDevice.Viewport.Height);
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the background texture
            TileLayer background = _tManager.getLayer("background");
            
         spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cam.Transform);
            for (int x = 0; x < background.MapWidth; x++)
                for (int y = 0; y < background.MapHeight; y++)
                    {
                        int textureIndex = background.Tiles[y, x].Id;
                        Texture2D texture = tileTextures[textureIndex];
                            spriteBatch.Draw(texture,
                                new Rectangle(x * tileWidth,
                              y * tileHeight,
                              tileWidth,
                              tileHeight),
                                Color.White);
                    }
            spriteBatch.End();
            player1.Draw(cam, spriteBatch);
                // TODO: Add your drawing code here
                if (!(chaser.EnemyState == Enemy.ENEMYSTATE.DEAD))
                    chaser.Draw(cam, spriteBatch);
                foreach (Platform block in floor)
                    block.Draw(cam, spriteBatch);
                foreach (Platform block in platform1)
                    block.Draw(cam, spriteBatch);

                    }

                


    }
}
