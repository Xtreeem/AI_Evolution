using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    class Battle
    {
        public bool IsFinished { get { return _finished; } }
        private bool _finished;

        public float FitnessValue { get { return _fitnessValue; } }
        private float _fitnessValue;

        private Actor _hero, _enemy;
        private float _turnTimer;
        private Random _rand;

        private const int _baseHitChance = 50;

        public Battle(ref Actor Hero, Actor Enemy, int RandomSeed)
        {
            _hero = Hero;
            _enemy = Enemy;
            _rand = new Random(RandomSeed);
        }

        public void Update(GameTime GT, float TurnLength)
        {
            if (TurnLength == 0)
            {
                do
                {
                    StartCombatTurn();
                } while (_finished != true);
            }
            else
            {
                _turnTimer += GT.ElapsedGameTime.Milliseconds;
                if (_turnTimer > TurnLength)
                {
                    _turnTimer = 0;
                    StartCombatTurn();
                }
            }
        }

        private void StartCombatTurn()
        {
            Actor inititiveWinner;
            Actor inititiveLoser;
            Roll_for_Initiative(out inititiveWinner, out inititiveLoser);

            Physical_Phase(ref inititiveWinner, ref inititiveLoser);
            Magical_Phase();
        }

        private void Roll_for_Initiative(out Actor Winner, out Actor Loser)
        {
            float heroInitiativeRoll = _rand.Next(1, 1);
            float enemyInitiativeRoll = _rand.Next(1, 1);
            heroInitiativeRoll += _hero.Stats.Initiative;
            enemyInitiativeRoll += _enemy.Stats.Initiative;

            if (heroInitiativeRoll > enemyInitiativeRoll)
            {
                Winner = _hero;
                Loser = _enemy;
                return;
            }
            else
            {
                Winner = _enemy;
                Loser = _hero;
                return;
            }
        }

        private void Physical_Phase(ref Actor A1, ref Actor A2)
        {
            int remainingAttacksA1 = A1.Stats.Number_of_Attacks;
            int remainingAttacksA2 = A2.Stats.Number_of_Attacks;

            do
            {
                if (remainingAttacksA1 != 0)
                {
                    remainingAttacksA1--;
                    Physical_Attack(ref A1, ref A2);
                    if (DeathCheck(ref A2))
                        return;
                }
                if (remainingAttacksA2 != 0)
                {
                    remainingAttacksA2--;
                    Physical_Attack(ref A2, ref A1);
                    if (DeathCheck(ref A1))
                        return;
                }
            } while (remainingAttacksA1 != 0 && remainingAttacksA2 != 0);

        }

        private void Physical_Attack(ref Actor Attacker, ref Actor Defender)
        {
            float T100 = _rand.Next(1, 101);
            T100 -= Defender.Stats.Dodge_Chance;
            if (T100 < _baseHitChance)
                return;
            T100 = _rand.Next(1, 101);
            if (T100 < Attacker.Stats.Critical_Hit_Chance)
                Defender.Take_Damage(Attacker.Stats.Physical_Attack * 2);
            else
                Defender.Take_Damage(Attacker.Stats.Physical_Attack);
        }

        private void Magical_Phase()
        {

        }

        private bool DeathCheck(ref Actor A)
        {
            if (A.Current_Health <= 0)
                return true;
            return false;
        }





    }
}
