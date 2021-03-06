﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    class Hero : Actor
    {

        public Hero(Actor P1, Actor P2)
        {
            GenerateStats_Breed(P1, P2);
            GeneratePerks(P1, P2);
            _current_Health = _stats.Health;

        }
        public Hero(Stats Stats)
        {
            _stats = Stats;
            _current_Health = _stats.Health;
        }

        public Hero(StatWeight Weights, float TotalStats)
        {
            float statsPerPercent = TotalStats / 100;
            _stats = new Stats(
                Weights.STR * statsPerPercent,
                Weights.DEX * statsPerPercent,
                Weights.CON * statsPerPercent,
                Weights.INT * statsPerPercent,
                Weights.WIS * statsPerPercent,
                Weights.FTH * statsPerPercent,
                Weights.PER * statsPerPercent);

            float T =
                Stats.Strength +
            Stats.Constitution +
            Stats.Dexterity +
            Stats.Intelligence +
            Stats.Wisdom +
            Stats.Faith +
            Stats.Perception;
            //if (T != 200)
                //Console.WriteLine("");
            _current_Health = _stats.Health;

        }



        private void GenerateStats_Breed(Actor P1, Actor P2)
        {
            //Super breed funky town



        }

        private void GeneratePerks(Actor P1, Actor P2)
        {

        }
    }
}
