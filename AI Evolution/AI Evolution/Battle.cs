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
        private float _heroHealthAtTurnStart, _enemyHealthAtTurnStart;
        private float _turnTimer;
        private Random _rand;

        private const int _baseHitChance = 10;

        public Battle(ref Actor Hero, Actor Enemy, int RandomSeed)
        {
            _hero = Hero;
            _enemy = Enemy;
            _rand = new Random(RandomSeed);
        }

        public void Update(GameTime GT, float TurnLength)
        {
            if (!_finished)
            {
                if (TurnLength == 0)
                {
                    do
                    {
                        Start_Combat_Turn();
                    } while (_finished != true);
                }
                else
                {
                    _turnTimer += GT.ElapsedGameTime.Milliseconds;
                    if (_turnTimer > TurnLength)
                    {
                        _turnTimer = 0;
                        Start_Combat_Turn();
                    }
                }
            }
        }

        private void Start_Combat_Turn()
        {
            _heroHealthAtTurnStart = _hero.Current_Health;
            _enemyHealthAtTurnStart = _enemy.Current_Health;
            Actor inititiveWinner;
            Actor inititiveLoser;
            Roll_for_Initiative(out inititiveWinner, out inititiveLoser);
            if (Physical_Phase(ref inititiveWinner, ref inititiveLoser))
            { Combat_Over(); return; }
            Roll_for_Initiative(out inititiveWinner, out inititiveLoser);
            if (Magical_Phase(ref inititiveWinner, ref inititiveLoser))
            { Combat_Over(); return; }
            Recovery_Phase();
        }

        private void Combat_Over()
        {
            _finished = true;
            _fitnessValue = Math.Abs(_enemy.Current_Health - _enemy.Stats.Health);
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

        private bool Physical_Phase(ref Actor A1, ref Actor A2)
        {
            int remainingAttacksA1 = A1.Stats.Number_of_Attacks;
            int remainingAttacksA2 = A2.Stats.Number_of_Attacks;

            do
            {
                if (remainingAttacksA1 != 0)
                {
                    remainingAttacksA1--;
                    Physical_Attack(ref A1, ref A2);
                    if (Death_Check(ref A2))
                        return true;
                }
                if (remainingAttacksA2 != 0)
                {
                    remainingAttacksA2--;
                    Physical_Attack(ref A2, ref A1);
                    if (Death_Check(ref A1))
                        return true;
                }
            } while (remainingAttacksA1 != 0 && remainingAttacksA2 != 0);
            return false;
        }

        private void Physical_Attack(ref Actor Attacker, ref Actor Defender)
        {
            float T100 = _rand.Next(1, 101);
            T100 -= Defender.Stats.Dodge_Chance;
            if (T100 < _baseHitChance)
                return;
            T100 = _rand.Next(1, 101);
            if (T100 < Attacker.Stats.Critical_Hit_Chance - Defender.Stats.Resilience)
                Defender.Take_Damage(MathHelper.Clamp((Attacker.Stats.Physical_Attack * 2) * (1 - (Defender.Stats.Physical_Resist / 100)), 0, 100000));
            else
                Defender.Take_Damage(MathHelper.Clamp(Attacker.Stats.Physical_Attack * (1 - (Defender.Stats.Physical_Resist / 100)), 0, 100000));
        }

        private bool Magical_Phase(ref Actor A1, ref Actor A2)
        {
            int remainingAttacksA1 = A1.Stats.Mana;
            int remainingAttacksA2 = A2.Stats.Mana;

            do
            {
                if (remainingAttacksA1 != 0)
                {
                    remainingAttacksA1--;
                    Magical_Attack(ref A1, ref A2);
                    if (Death_Check(ref A2))
                        return true;
                }
                if (remainingAttacksA2 != 0)
                {
                    remainingAttacksA2--;
                    Magical_Attack(ref A2, ref A1);
                    if (Death_Check(ref A1))
                        return true;
                }
            } while (remainingAttacksA1 != 0 && remainingAttacksA2 != 0);
            return false;
        }

        private void Magical_Attack(ref Actor Attacker, ref Actor Defender)
        {
            float T100 = _rand.Next(1, 101);
            T100 -= Defender.Stats.Dodge_Chance;
            if (T100 < _baseHitChance)
                return;
            T100 = _rand.Next(1, 101);
            if (T100 < Attacker.Stats.Critical_Hit_Chance - Defender.Stats.Resilience)
                Defender.Take_Damage(MathHelper.Clamp((Attacker.Stats.Magic_Attack * 2) * (1 - (Defender.Stats.Magic_Resist / 100)), 0, 100000));
            else
                Defender.Take_Damage(MathHelper.Clamp(Attacker.Stats.Magic_Attack * (1 - (Defender.Stats.Magic_Resist / 100)), 0, 100000));
        }

        private void Recovery_Phase()
        {
            float DamageTaken;
            DamageTaken = _heroHealthAtTurnStart - _hero.Current_Health;
            _hero.Heal(DamageTaken * (_hero.Stats.Health_Regen / 100));

            DamageTaken = _enemyHealthAtTurnStart - _enemy.Current_Health;
            _enemy.Heal(DamageTaken * (_enemy.Stats.Health_Regen / 100));
        }

        private bool Death_Check(ref Actor A)
        {
            if (A.Current_Health <= 0)
                return true;
            return false;
        }





    }
}
