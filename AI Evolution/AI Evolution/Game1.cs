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
    enum GameState
    {
        Breeding,
        Trialing
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        float _turnLength = 1;
        const int _numberOfScenes = 200000;
        GameState _gameState = GameState.Trialing;

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
            for (int i = 0; i < _numberOfScenes; i++)
            {
                Actor Enemy = new Hero(new Stats(10, 10, 10000, 10, 10, 10, 10));
                _scenes[i] = new Scene(Heroes[i], Enemy);
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch (_gameState)
            {
                case GameState.Breeding:
                    break;
                case GameState.Trialing:
                    for (int z = 0; z < _scenes.Length; z++)
                    {
                        Scene TS = _scenes[z];
                        _tasks[z] = Task.Factory.StartNew(() => TS.Update(gameTime, _turnLength));
                    }
                    Task.WaitAll(_tasks);

                    if (All_Scenes_are_Done())
                    {
                        Get_all_Results();
                        _gameState = GameState.Breeding;
                    }
                    break;
                default:
                    break;
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
            Debug_print_top_5();
            Debug_print_bottom_5();
        }
        private void Debug_print_top_5()
        {
            Actor TH;
            for (int i = 0; i < 5; i++)
            {
                TH = _results[i].Item2;
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("Fitness: " + _results[i].Item1 + "\tRanking: " + (i + 1));
                Console.WriteLine("--------------------------------------------------------------------");
                Debug_Write_Actor_Stats(TH);
                Console.WriteLine("--------------------------------------------------------------------");
            }
        }
        private void Debug_print_bottom_5()
        {
            Actor TH;
            for (int i = _numberOfScenes - 1; i >= _numberOfScenes - 6; i--)
            {
                TH = _results[i].Item2;
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("Fitness: " + _results[i].Item1 + "\tRanking: " + (i + 1) + "\n");
                Console.WriteLine("--------------------------------------------------------------------");
                Debug_Write_Actor_Stats(TH);
                Console.WriteLine("--------------------------------------------------------------------");
            }
        }
        private void Debug_Write_Actor_Stats(Actor TH)
        {
            Console.Write("STR: " + TH.Stats.Strength);
            Console.Write("\tDEX: " + TH.Stats.Dexterity);
            Console.Write("\tPER: " + TH.Stats.Perception);
            Console.Write("\tCON: " + TH.Stats.Constitution);
            Console.Write("\tINT: " + TH.Stats.Intelligence);
            Console.Write("\tFTH: " + TH.Stats.Faith);
            Console.Write("\tWIS: " + TH.Stats.Wisdom + "\n");
            Console.WriteLine("Initiative: " + TH.Stats.Initiative);
            Console.WriteLine("Crit: " + TH.Stats.Critical_Hit_Chance);
            Console.WriteLine("Resilience: " + TH.Stats.Resilience);
            Console.WriteLine("Dodge: " + TH.Stats.Dodge_Chance);
            Console.WriteLine("Health: " + TH.Stats.Health);
            Console.WriteLine("Health Reg: " + TH.Stats.Health_Regen);
            Console.WriteLine("Mana: " + TH.Stats.Mana);
            Console.WriteLine("Mag Attack: " + TH.Stats.Magic_Attack);
            Console.WriteLine("Mag Resist: " + TH.Stats.Magic_Resist);
            Console.WriteLine("Num Attacks: " + TH.Stats.Number_of_Attacks);
            Console.WriteLine("Phys Attack: " + TH.Stats.Physical_Attack);
            Console.WriteLine("Phys Resist: " + TH.Stats.Physical_Resist);
            Console.WriteLine("Size: " + TH.Stats.Size);
            Console.WriteLine("Speed: " + TH.Stats.Speed);
        }
    }
}
