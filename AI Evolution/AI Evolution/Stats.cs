﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AI_Evolution
{
    class Stats
    {
        #region Base Stats
        public float Strength { get { return _str; } }
        private float _str;

        public float Dexterity { get { return _dex; } }
        private float _dex;

        public float Constitution { get { return _con; } }
        private float _con;

        public float Intelligence { get { return _int; } }
        private float _int;

        public float Wisdom { get { return _wis; } }
        private float _wis;

        public float Faith { get { return _fth; } }
        private float _fth;

        public float Perception { get { return _per; } }
        private float _per;
        #endregion

        #region Sub Stats
        public float Critical_Hit_Chance { get { return _chc; } }
        private float _chc;

        public float Dodge_Chance { get { return _dodge; } }
        private float _dodge;

        public float Speed { get { return _speed; } }
        private float _speed;

        public float Initiative { get { return _init; } }
        private float _init;

        public int Number_of_Attacks { get { return (int)_nAtk; } }
        private float _nAtk;

        /// <summary>
        /// Anti-Crit
        /// </summary>
        public float Resilience { get { return _res; } }
        private float _res;

        public float Size { get { return _size; } }
        private float _size;

        public float Physical_Attack { get { return _pAtk; } }
        private float _pAtk;

        public float Physical_Resist { get { return _pDef; } }
        private float _pDef;

        public float Health_Regen { get { return _hps; } }
        private float _hps;

        public float Health { get { return _hp; } }
        private float _hp;

        public float Magic_Attack { get { return _mAtk; } }
        private float _mAtk;

        public float Magic_Resist { get { return _mDef; } }
        private float _mDef;

        public float Mana { get { return _mp; } }
        private float _mp;

        #endregion

        public Stats(float STR, float DEX, float CON, float INT, float WIS, float FTH, float PER)
        {
            _str = STR;
            _dex = DEX;
            _con = CON;
            _int = INT;
            _wis = WIS;
            _fth = FTH;
            _per = PER;
            CalculateSubStats();
        }

        public void Mutate()
        {
            //Mutate logic 
            CalculateSubStats();
        }

        private void CalculateSubStats()
        {
            Calc_CHC();
            Calc_Dodge();
            Calc_Size();
            Calc_Speed();
            Calc_Health();
            Calc_HpReg();
            Calc_Mana();
            Calc_MAtk();
            Calc_MDef();
            Calc_Number_of_Attacks();
            Calc_PAtk();
            Calc_PDef();
            Calc_Res();
        }
        #region Sub Stat Functions
        private void Calc_CHC()
        {
            _chc = MathHelper.Clamp(5f + (Dexterity + Intelligence) / 200f, 5f, 50f);
        }

        private void Calc_Dodge()
        {
            _dodge = MathHelper.Clamp((Perception * 0.5f + Dexterity * 1.5f) / 200, 0f, 50f);
        }

        private void Calc_Res()
        {
            _res = MathHelper.Clamp((Wisdom + Dexterity) / 200, 0f, 50f);
        }

        private void Calc_Size()
        {
            _size = MathHelper.Clamp((Strength + Constitution) / 200, 0f, 50f);
        }

        private void Calc_Speed()
        {
            _speed = MathHelper.Clamp((Dexterity - Size) / 100, 5f, 50f);
        }

        private void Calc_Number_of_Attacks()
        {
            _nAtk = MathHelper.Clamp((_speed) / 10f, 5f, 50f);
        }

        private void Calc_PAtk()
        {
            _pAtk = MathHelper.Clamp((Strength + Dexterity) / 200, 0f, 50f);
        }
        private void Calc_PDef()
        {
            _pDef = MathHelper.Clamp((Constitution + Size) / 200, 0f, 50f);
        }
        private void Calc_HpReg()
        {
            _hps = MathHelper.Clamp(Constitution / 200, 5f, 50f);
        }
        private void Calc_Health()
        {
            _hp = MathHelper.Clamp((Constitution + Size) / 200, 5f, 50f);
        }

        private void Calc_MAtk()
        {
            _mAtk = MathHelper.Clamp((Intelligence + Faith) / 200, 5f, 50f);
        }

        private void Calc_MDef()
        {
            _mDef = MathHelper.Clamp((Faith + Wisdom) / 200, 5f, 50f);
        }

        private void Calc_Mana()
        {
            _mp = MathHelper.Clamp((Intelligence + Faith) / 200f, 5f, 50f);
        }
        #endregion












    }
}
