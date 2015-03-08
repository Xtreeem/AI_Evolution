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
        float _turnLength = 100;
        const int _numberOfScenes = 20;

        Scene[] _scenes = new Scene[_numberOfScenes];
        Task[] _tasks = new Task[_numberOfScenes];
        List<Tuple<float, Actor>> _results;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D[] _dummyTex = new Texture2D[5];

        Actor[] Heroes = new Actor[_numberOfScenes];
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

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Random _rand = new Random();
        
            _dummyTex[0] = Content.Load<Texture2D>("Head1");
            _dummyTex[1] = Content.Load<Texture2D>("Body1");
            _dummyTex[2] = Content.Load<Texture2D>("Feet1");
            _dummyTex[3] = Content.Load<Texture2D>("Hand1");
            _dummyTex[4] = Content.Load<Texture2D>("Hand1");

            for (int i = 0; i < Heroes.Length; i++)
            {
                Heroes[i] = new Hero(new Stats(Misc.Random.Next(20, 61), Misc.Random.Next(20, 61), Misc.Random.Next(20, 61), Misc.Random.Next(20, 61), Misc.Random.Next(20, 61), Misc.Random.Next(20, 61), Misc.Random.Next(20, 61)));
            }
            Actor Enemy = new Hero(new Stats(10, 10, 100, 10, 10, 10, 10));
            for (int i = 0; i < _numberOfScenes; i++)
            {
                _scenes[i] = new Scene(ref Heroes[i], Enemy);
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            for (int z = 0; z < _scenes.Length; z++)
            {
                Scene TS = _scenes[z];
                _tasks[z] = Task.Factory.StartNew(() => TS.Update(gameTime, _turnLength));
            }
            Task.WaitAll(_tasks);

            if (All_Scenes_are_Done())
            {
                Get_all_Results();

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
        /// <summary>
        /// Checks to see if all the scenes have finished 
        /// and are ready to return their results
        /// </summary>
        /// <returns></returns>
        private bool All_Scenes_are_Done()
        {
            if (Array.Exists(_scenes, x => x.IsFinished == false))
                return false;
            else
                return true;
        }
        /// <summary>
        /// Loops all the scenes and collects their results 
        /// Results are stored in a class wide list (_results) 
        /// Results are stored as Tuple<float, Actor> 
        /// Results are sorted from highest fitness to lowest
        /// </summary>
        private void Get_all_Results()
        {
            _results = new List<Tuple<float, Actor>>(_numberOfScenes);
            for (int i = 0; i < _numberOfScenes; i++)
            {
                _results.Add(_scenes[i].Get_Result());
            }
            _results = _results.OrderByDescending(x => x.Item1).ToList();
        }
    }
}
