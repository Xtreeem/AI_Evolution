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
using System.IO;

using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;


namespace AI_Evolution
{
    enum GameState
    {
        Breeding,
        Trialing
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        object _mValue = System.Reflection.Missing.Value;
        Excel.Workbooks _myEBooks;
        Excel.Workbook _myEBook;
        Excel.Application _myEApp;
        Excel.Worksheet _myESheetBestHero,_myESheetWorstHero,_myESheetAverageStat,_myESheetChange;


        List<List<float>> _averageStats = new List<List<float>>();


        int _generation = 0;
        float _turnLength = 0;
        const int _numberOfScenes = 1000;
        const int _statsPerHero = 200;
        const bool _debugFitness = false;
        GameState _gameState = GameState.Trialing;

        Scene[] _scenes = new Scene[_numberOfScenes];
        Task[] _tasks = new Task[_numberOfScenes];
        List<Tuple<float, Actor>> _results;
        List<Tuple<float, Actor>> _debug = new List<Tuple<float, Actor>>();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D[] _dummyTex = new Texture2D[5];

        Actor[] _heroes = new Actor[_numberOfScenes];






        Scene Tscene;
        Hero Thero;
        Hero Tenemy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;



            int temp;
            temp = Misc.DivideByTwo(5, true);
            Console.WriteLine(temp + " : 3");
            temp = Misc.DivideByTwo(5, false);
            Console.WriteLine(temp + " : 2");
            temp = Misc.DivideByTwo(3, true);
            Console.WriteLine(temp + " : 2");
            temp = Misc.DivideByTwo(3, false);
            Console.WriteLine(temp + " : 1");
            temp = Misc.DivideByTwo(2, true);
            Console.WriteLine(temp + " : 1");
            temp = Misc.DivideByTwo(2, false);
            Console.WriteLine(temp + " : 1");

        }


        protected override void Initialize()
        {
            Thero = new Hero(new Stats(13.72998f, 22.42563f, 41.6476f, 39.35927f, 32.03661f, 46.93593f, 6.864989f));
            Tenemy = new Hero(new Stats(10, 10, 10000, 10, 10, 10, 10));
            Tscene = new Scene(Thero, Tenemy);
            Tscene.Update(new GameTime(), 0);
            Set_up_Excel();
            
            base.Initialize();
        }

        private void Set_up_Excel()
        {
            _myEApp = new Excel.Application();
            _myEApp.Visible = true;
            _myEBook = _myEApp.Workbooks.Add(_mValue);
            _myESheetBestHero = (Excel.Worksheet)_myEBook.Worksheets.get_Item(1);
            _myESheetWorstHero = (Excel.Worksheet)_myEBook.Worksheets.Add(_mValue, _mValue, 1, _mValue);
            _myESheetAverageStat = (Excel.Worksheet)_myEBook.Worksheets.Add(_mValue, _mValue, 1, _mValue);
            _myESheetChange = (Excel.Worksheet)_myEBook.Worksheets.Add(_mValue, _mValue, 1, _mValue);

            _myESheetBestHero.Name = "Best Hero";
            _myESheetBestHero.Cells[1, 1] = "Gen";
            _myESheetBestHero.Cells[1, 2] = "Str";
            _myESheetBestHero.Cells[1, 3] = "Con";
            _myESheetBestHero.Cells[1, 4] = "Dex";
            _myESheetBestHero.Cells[1, 5] = "Int";
            _myESheetBestHero.Cells[1, 6] = "Wis";
            _myESheetBestHero.Cells[1,7] = "Fth";
            _myESheetBestHero.Cells[1, 8] = "Per";

            _myESheetWorstHero.Name = "Worst Hero";
            _myESheetWorstHero.Cells[1, 1] = "Gen";
            _myESheetWorstHero.Cells[1, 2] = "Str";
            _myESheetWorstHero.Cells[1, 3] = "Con";
            _myESheetWorstHero.Cells[1, 4] = "Dex";
            _myESheetWorstHero.Cells[1, 5] = "Int";
            _myESheetWorstHero.Cells[1, 6] = "Wis";
            _myESheetWorstHero.Cells[1, 7] = "Fth";
            _myESheetWorstHero.Cells[1, 8] = "Per";

            _myESheetAverageStat.Name = "Average Stats";
            _myESheetAverageStat.Cells[1, 1] = "Gen";
            _myESheetAverageStat.Cells[1, 2] = "Str";
            _myESheetAverageStat.Cells[1, 3] = "Con";
            _myESheetAverageStat.Cells[1, 4] = "Dex";
            _myESheetAverageStat.Cells[1, 5] = "Int";
            _myESheetAverageStat.Cells[1, 6] = "Wis";
            _myESheetAverageStat.Cells[1, 7] = "Fth";
            _myESheetAverageStat.Cells[1, 8] = "Per";

            _myESheetChange.Name = "Stat Changes";
            _myESheetChange.Cells[1, 1] = "Gen";
            _myESheetChange.Cells[1, 2] = "Str";
            _myESheetChange.Cells[1, 3] = "Con";
            _myESheetChange.Cells[1, 4] = "Dex";
            _myESheetChange.Cells[1, 5] = "Int";
            _myESheetChange.Cells[1, 6] = "Wis";
            _myESheetChange.Cells[1, 7] = "Fth";
            _myESheetChange.Cells[1, 8] = "Per";
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

            for (int i = 0; i < _heroes.Length; i++)
            {
                _heroes[i] = new Hero(
                    new StatWeight(
                        Misc.Random.Next(1, 101),
                        Misc.Random.Next(1, 101),
                        Misc.Random.Next(1, 101),
                        Misc.Random.Next(1, 101),
                        Misc.Random.Next(1, 101),
                        Misc.Random.Next(1, 101),
                        Misc.Random.Next(1, 101)),
                        _statsPerHero
                        );
                _debug.Add(new Tuple<float, Actor>(i * 100, _heroes[i]));
            }
            Set_up_Scenes();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch (_gameState)
            {
                case GameState.Breeding:
                    Gather_Average_Stats();
                    if (_generation % 100 == 0)
                        Debug_print_change(100);
                    Write_to_File();
                    _heroes = Breeder.Breed_Actors(_results).ToArray<Actor>();
                    Set_up_Scenes();
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
                _results.Add(_scenes[i].Get_Result(_debugFitness));
            }
            _results = _results.OrderByDescending(x => x.Item1).ToList();
            if (_generation % 100 == 0)
            {
                Debug_print_top_5();
                Debug_print_bottom_5();
                if (_generation == 10)
                    Console.Write("");
            }
        }

        private void Gather_Average_Stats()
        {
            List<float> tempList = new List<float>();
            for (int i = 0; i < 7; i++)
            {
                tempList.Add(0);
            }
            for (int i = 0; i < _numberOfScenes; i++)
            {
                tempList[0] += _heroes[i].Stats.Strength;
                tempList[1] += _heroes[i].Stats.Constitution;
                tempList[2] += _heroes[i].Stats.Dexterity;
                tempList[3] += _heroes[i].Stats.Intelligence;
                tempList[4] += _heroes[i].Stats.Wisdom;
                tempList[5] += _heroes[i].Stats.Faith;
                tempList[6] += _heroes[i].Stats.Perception;
            }
            for (int i = 0; i < tempList.Count; i++)
            {
                tempList[i] /= _heroes.Count();
            }
            _averageStats.Add(tempList);
        }

        private void Set_up_Scenes()
        {
            _generation++;
            if (_heroes.Count() != _numberOfScenes)
                throw (new Exception("SOmethings wrong"));
            for (int i = 0; i < _numberOfScenes; i++)
            {
                Actor Enemy = new Hero(new Stats(10, 10, 10000, 10, 10, 10, 10));
                _scenes[i] = new Scene(_heroes[i], Enemy);
            }
            _gameState = GameState.Trialing;
            if (_generation % 100 == 0)
                Console.WriteLine("Trialing Generation: " + _generation);
        }
        private void Write_to_File()
        {
            if (_myEApp == null)
            {
                Console.Write("") ; //set breakpoint here during execution to see if it IS null
            }

            int lineNumber = _generation +1;
            Actor bH = _heroes[0];
            Actor wH = _heroes[_numberOfScenes - 1];
            #region Best Hero
            _myESheetBestHero.Cells[lineNumber, 1] = _generation;
            _myESheetBestHero.Cells[lineNumber, 2] = bH.Stats.Strength;
            _myESheetBestHero.Cells[lineNumber, 3] = bH.Stats.Constitution;
            _myESheetBestHero.Cells[lineNumber, 4] = bH.Stats.Dexterity;
            _myESheetBestHero.Cells[lineNumber, 5] = bH.Stats.Intelligence;
            _myESheetBestHero.Cells[lineNumber, 6] = bH.Stats.Wisdom;
            _myESheetBestHero.Cells[lineNumber, 7] = bH.Stats.Faith;
            _myESheetBestHero.Cells[lineNumber, 8] = bH.Stats.Perception;
            #endregion

            #region Worst Hero
            _myESheetWorstHero.Cells[lineNumber, 1] = _generation + ":";
            _myESheetWorstHero.Cells[lineNumber, 2] = wH.Stats.Strength;
            _myESheetWorstHero.Cells[lineNumber, 3] = wH.Stats.Constitution;
            _myESheetWorstHero.Cells[lineNumber, 4] = wH.Stats.Dexterity;
            _myESheetWorstHero.Cells[lineNumber, 5] = wH.Stats.Intelligence;
            _myESheetWorstHero.Cells[lineNumber, 6] = wH.Stats.Wisdom;
            _myESheetWorstHero.Cells[lineNumber, 7] = wH.Stats.Faith;
            _myESheetWorstHero.Cells[lineNumber, 8] = wH.Stats.Perception;
            #endregion

            #region Average Stats
            _myESheetAverageStat.Cells[lineNumber, 1] = _generation + ":";
            _myESheetAverageStat.Cells[lineNumber, 2] = _averageStats[_generation - 1][0];
            _myESheetAverageStat.Cells[lineNumber, 3] = _averageStats[_generation - 1][1];
            _myESheetAverageStat.Cells[lineNumber, 4] = _averageStats[_generation - 1][2];
            _myESheetAverageStat.Cells[lineNumber, 5] = _averageStats[_generation - 1][3];
            _myESheetAverageStat.Cells[lineNumber, 6] = _averageStats[_generation - 1][4];
            _myESheetAverageStat.Cells[lineNumber, 7] = _averageStats[_generation - 1][5];
            _myESheetAverageStat.Cells[lineNumber, 8] = _averageStats[_generation - 1][6];
            #endregion

            #region Change in Stats
            if (_generation != 1)
            {
                _myESheetChange.Cells[lineNumber, 1] = _generation + ":";
                _myESheetChange.Cells[lineNumber, 2] = _averageStats[_generation - 1][0] - _averageStats[_generation - 2][0];
                _myESheetChange.Cells[lineNumber, 3] = _averageStats[_generation - 1][1] - _averageStats[_generation - 2][1];
                _myESheetChange.Cells[lineNumber, 4] = _averageStats[_generation - 1][2] - _averageStats[_generation - 2][2];
                _myESheetChange.Cells[lineNumber, 5] = _averageStats[_generation - 1][3] - _averageStats[_generation - 2][3];
                _myESheetChange.Cells[lineNumber, 6] = _averageStats[_generation - 1][4] - _averageStats[_generation - 2][4];
                _myESheetChange.Cells[lineNumber, 7] = _averageStats[_generation - 1][5] - _averageStats[_generation - 2][5];
                _myESheetChange.Cells[lineNumber, 8] = _averageStats[_generation - 1][6] - _averageStats[_generation - 2][6];

            }
            else
            {
                _myESheetChange.Cells[lineNumber, 1] = _generation + ":";
                _myESheetChange.Cells[lineNumber, 2] = 0;
                _myESheetChange.Cells[lineNumber, 3] = 0;
                _myESheetChange.Cells[lineNumber, 4] = 0;
                _myESheetChange.Cells[lineNumber, 5] = 0;
                _myESheetChange.Cells[lineNumber, 6] = 0;
                _myESheetChange.Cells[lineNumber, 7] = 0;
                _myESheetChange.Cells[lineNumber, 8] = 0;
            }
            #endregion
        }
        //private void Write_to_File()
        //{
        //    Actor bH = _heroes[0];
        //    Actor wH = _heroes[_numberOfScenes - 1];
        //    StreamWriter sW = new StreamWriter(@"BestHero.txt", true);

        //    sW.WriteLine("\n" + _generation + ":\t" + bH.Stats.Strength + "\t" + bH.Stats.Constitution + "\t" + bH.Stats.Dexterity + "\t" + bH.Stats.Intelligence + "\t" + bH.Stats.Wisdom + "\t" + bH.Stats.Faith + "\t" + bH.Stats.Perception + "\t");
        //    sW.Close();
        //    sW = new StreamWriter(@"WorstHero.txt", true);
        //    sW.WriteLine("\n"+_generation + ":\t" + wH.Stats.Strength + "\t" + wH.Stats.Constitution + "\t" + wH.Stats.Dexterity + "\t" + wH.Stats.Intelligence + "\t" + wH.Stats.Wisdom + "\t" + wH.Stats.Faith + "\t" + wH.Stats.Perception + "\t");
        //    sW.Close();
        //    sW = new StreamWriter(@"AverageStat.txt", true);
        //    sW.WriteLine("\n" + _generation + "\t" + _averageStats[_generation - 1][0] + "\t" + _averageStats[_generation - 1][1] + "\t" + _averageStats[_generation - 1][2] + "\t" + _averageStats[_generation - 1][3] + "\t" + _averageStats[_generation - 1][4] + "\t" + _averageStats[_generation - 1][5] + "\t" + _averageStats[_generation - 1][6]);
        //    sW.Close();
        //    sW = new StreamWriter(@"Change.txt", true);
        //    if (_generation != 1)
        //    sW.WriteLine(
        //        "\n" + _generation + ":\t" +
        //            (_averageStats[_generation - 1][0] - _averageStats[_generation - 2][0]) + "\t" +
        //            (_averageStats[_generation - 1][1] - _averageStats[_generation - 2][1]) + "\t" +
        //            (_averageStats[_generation - 1][2] - _averageStats[_generation - 2][2]) + "\t" +
        //            (_averageStats[_generation - 1][3] - _averageStats[_generation - 2][3]) + "\t" +
        //            (_averageStats[_generation - 1][4] - _averageStats[_generation - 2][4]) + "\t" +
        //            (_averageStats[_generation - 1][5] - _averageStats[_generation - 2][5]) + "\t" +
        //            (_averageStats[_generation - 1][6] - _averageStats[_generation - 2][6]) + "\t"
        //            );
        //    else
        //        sW.WriteLine("\n" + _generation + ":\t" +0 + "\t" + 0 + "\t" + 0 + "\t" + 0 + "\t" + 0 + "\t" + 0 + "\t" + 0);
        //    sW.Close();
        //}


        private void Debug_print_change(int GenerationsBack)
        {
            if ((_generation - 1) - GenerationsBack < 0)
                return;
            float cSTR = _averageStats[_generation - 1][0] - _averageStats[(_generation - 1) - GenerationsBack][0];
            float cCON = _averageStats[_generation - 1][1] - _averageStats[(_generation - 1) - GenerationsBack][1];
            float cDEX = _averageStats[_generation - 1][2] - _averageStats[(_generation - 1) - GenerationsBack][2];
            float cINT = _averageStats[_generation - 1][3] - _averageStats[(_generation - 1) - GenerationsBack][3];
            float cWIS = _averageStats[_generation - 1][4] - _averageStats[(_generation - 1) - GenerationsBack][4];
            float cFTH = _averageStats[_generation - 1][5] - _averageStats[(_generation - 1) - GenerationsBack][5];
            float cPER = _averageStats[_generation - 1][6] - _averageStats[(_generation - 1) - GenerationsBack][6];

            Console.WriteLine("Change In STR: " + cSTR);
            Console.WriteLine("Change In CON: " + cCON);
            Console.WriteLine("Change In DEX: " + cDEX);
            Console.WriteLine("Change In INT: " + cINT);
            Console.WriteLine("Change In WIS: " + cWIS);
            Console.WriteLine("Change In FTH: " + cFTH);
            Console.WriteLine("Change In PER: " + cPER);
        }

        private void Debug_print_top_5()
        {
            Actor TH;
            for (int i = 0; i < 5; i++)
            {
                TH = _results[i].Item2;
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine("Generation: " + _generation + "\tFitness: " + _results[i].Item1 + "\tRanking: " + (i + 1));
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
                Console.WriteLine("Generation: " + _generation + "\tFitness: " + _results[i].Item1 + "\tRanking: " + (i + 1) + "\n");
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
