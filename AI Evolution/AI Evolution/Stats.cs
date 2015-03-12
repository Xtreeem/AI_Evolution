﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AI_Evolution
{
    public class Stats
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

        public int Mana { get { return (int)_mp; } }
        private float _mp;

        #endregion

        const int _softCapStep = 5;
        const float _softCapMagnitude = 0.02f;
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
            SoftCap(42.1489f);
        }

        private void CalculateSubStats()
        {
            Calc_CHC();
            Calc_Size();
            Calc_Speed();
            Calc_Dodge();
            Calc_Health();
            Calc_HpReg();
            Calc_Mana();
            Calc_MAtk();
            Calc_MDef();
            Calc_Number_of_Attacks();
            Calc_PAtk();
            Calc_PDef();
            Calc_Res();
            Calc_Init();
        }
        #region Sub Stat Functions
        private void Calc_CHC()
        {
            _chc = MathHelper.Clamp(((SoftCap(Dexterity) + SoftCap(Intelligence)) * 0.25f + Perception * 0.5f) / 2f, 5f, 50f);
        }

        private void Calc_Dodge()
        {
            _dodge = MathHelper.Clamp(((SoftCap(Perception) + SoftCap(Dexterity)) - Size * 0.25f) / 2f, 0f, 50f);
        }

        private void Calc_Res()
        {
            _res = MathHelper.Clamp((SoftCap(Wisdom) * 1.5f + SoftCap(Dexterity) * 0.5f) / 4f, 0f, 50f);
        }

        private void Calc_Size()
        {
            _size = MathHelper.Clamp((SoftCap(Strength) * 0.75f + SoftCap(Constitution) * 0.25f), 0f, 50f);
        }

        private void Calc_Speed()
        {
            _speed = MathHelper.Clamp((SoftCap(Dexterity) - Size), 5f, 50f);
        }

        private void Calc_Number_of_Attacks()
        {
            _nAtk = MathHelper.Clamp((Speed) / 10f, 1f, 50f);
        }

        private void Calc_PAtk()
        {
            _pAtk = MathHelper.Clamp((SoftCap(Strength) * 2f + SoftCap(Dexterity) * 0.5f) / 2f, 0f, 5000f);
        }
        private void Calc_PDef()
        {
            _pDef = MathHelper.Clamp((Speed + Size) / 2f, 0f, 50f);
        }
        private void Calc_HpReg()
        {
            _hps = MathHelper.Clamp(SoftCap(Constitution) / 4f, 5f, 50f);
        }
        private void Calc_Health()
        {
            _hp = MathHelper.Clamp((SoftCap(Constitution) + Size) * 2f, 5f, 5000f);
        }

        private void Calc_MAtk()
        {
            _mAtk = MathHelper.Clamp((SoftCap(Intelligence) * 1.5f + SoftCap(Faith) * 0.5f) / 2f, 5f, 50f);
        }

        private void Calc_MDef()
        {
            _mDef = MathHelper.Clamp((SoftCap(Faith) * 0.75f + SoftCap(Wisdom) * 1.25f) / 2f, 5f, 50f);
        }

        private void Calc_Mana()
        {
            _mp = MathHelper.Clamp((SoftCap(Wisdom) * 1.5f) / 10f, 1f, 5f);
        }
        private void Calc_Init()
        {
            _init = MathHelper.Clamp((Speed) / 1.5f, 1f, 50f);
        }
        #endregion

        //private float SoftCap(float Input)
        //{
        //    float result = 0f;
        //    int temp = (int)(Input * 100000);
        //    for (int i = 0; i < Input; i++)
        //    {
        //        if (result < _softCapStep && result > _softCapStep * 2) { result += 1; }
        //        else if (result < _softCapStep * 2 && result > _softCapStep * 3) { result += 1 * 0.9f; }
        //        else if (result < _softCapStep * 3 && result > _softCapStep * 4) { result += 1 * 0.8f; }
        //        else if (result < _softCapStep * 4 && result > _softCapStep * 5) { result += 1 * 0.7f; }
        //        else if (result < _softCapStep * 5 && result > _softCapStep * 6) { result += 1 * 0.6f; }
        //        else if (result < _softCapStep * 6 && result > _softCapStep * 7) { result += 1 * 0.5f; }
        //        else if (result < _softCapStep * 7 && result > _softCapStep * 8) { result += 1 * 0.4f; }
        //        else if (result < _softCapStep * 8 && result > _softCapStep * 9) { result += 1 * 0.3f; }
        //        else if (result < _softCapStep * 9 && result > _softCapStep * 10) { result += 1 * 0.2f; }
        //        else { result += 1 * 0.1f; }
        //    }
        //    result /= 100000;
        //    return result;
        //}

        private float SoftCap(float Input)
        {
            float Value = 1;
            float Result = 0;
            float Remainder = Input;
            do
            {
                if (Remainder - _softCapStep > 0)
                {
                    Result += _softCapStep * Value;
                    Remainder -= _softCapStep;
                    Value = MathHelper.Clamp(Value - (_softCapStep * _softCapMagnitude), 0, 1);
                }
                else
                {
                    Result += Remainder * Value;
                    Remainder = 0;
                }


            } while (Remainder > 0);



            return Result;
        }










    }
}
