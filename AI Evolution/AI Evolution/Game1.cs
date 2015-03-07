using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using System.Threading.Tasks;

namespace AI_Evolution
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int _numberOfScenes = 2000;
        Scene[] Scenes = new Scene[_numberOfScenes];
        Task[] Tasks = new Task[_numberOfScenes];

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Avatar.Avatar[] _dummies = new Avatar.Avatar[_numberOfScenes];
        Texture2D[] _dummyTex = new Texture2D[5];


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;



        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _dummyTex[0] = Content.Load<Texture2D>("Head1");
            _dummyTex[1] = Content.Load<Texture2D>("Body1");
            _dummyTex[2] = Content.Load<Texture2D>("Feet1");
            _dummyTex[3] = Content.Load<Texture2D>("Hand1");
            _dummyTex[4] = Content.Load<Texture2D>("Hand1");

            Random _rand = new Random();
            Vector2 _pos;
            float _scale;
            for (int i = 0; i < _numberOfScenes; i++)
            {
                _pos.X = _rand.Next(0, 200);
                _pos.Y = _rand.Next(200, 800);
                _scale = _rand.Next(10, 20);
                _scale /= 100;
                _dummies[i] = new Avatar.Avatar(_pos, _scale, _dummyTex, _rand.Next(1, 1000), 10);
            }

            for (int i = 0; i < _numberOfScenes; i++)
            {
                Scenes[i] = new Scene(0);
            }


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            for (int z = 0; z < _dummies.Length; z++)
            {
                Avatar.Avatar TC = _dummies[z];
                Tasks[z] = Task.Factory.StartNew(() => TC.Update(ref gameTime));
            }
            Task.WaitAll(Tasks);

            //if (InputManager.InputManager.LeftMousePress())
            //    for (int i = 0; i < _dummies.Count; i++)
            //    {
            //        _dummies[i].MoveToVector(new Vector2(InputManager.InputManager.MousePos().X, _dummies[i].Pos.Y));
            //    }


            // TODO: Add your update logic here
            //for (int i = 0; i < _dummies.Count; i++)
            //{
            //    _dummies[i].Update(gameTime);
            //}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 0; i < _dummies.Length; i++)
            {
                _dummies[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
