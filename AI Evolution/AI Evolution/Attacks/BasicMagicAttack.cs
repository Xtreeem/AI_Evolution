using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    class BasicMagicAttack : Attack
    {
        private const float _MissChance = 10;
        public override AttackType Type
        {
            get { return AttackType.Magical; }
        }

        public override CombatLogEntry Execute(ref Actor Attacker, ref Actor Defender, int Turn)
        {
            float T100 = Misc.Random.Next(1, 101);
            T100 -= Defender.Stats.Dodge_Chance;
            if (T100 < _MissChance)
            {
                return new CombatLogEntry(Turn, AttackType.Magical, Attacker, Defender, 0, false);
            }
            T100 = Misc.Random.Next(1, 101);
            if (T100 < Attacker.Stats.Critical_Hit_Chance - Defender.Stats.Resilience)
            {
                //Dodge as Normal
                //Defender.Take_Damage(MathHelper.Clamp((Attacker.Stats.Magic_Attack * 2) * (1 - (Defender.Stats.Magic_Resist / 100)), 0, 100000));
                //return new CombatLogEntry(Turn, AttackType.Magical, Attacker, Defender, MathHelper.Clamp((Attacker.Stats.Magic_Attack * 2) * (1 - (Defender.Stats.Magic_Resist / 100)), 0, 100000), true);

                //Dodge as Damage Reduction
                Defender.Take_Damage(MathHelper.Clamp(((Attacker.Stats.Magic_Attack * 2) * (1 - Defender.Stats.Magic_Resist / 100)) * (1 - (Defender.Stats.Dodge_Chance / 100)), 0, 100000));
                return new CombatLogEntry(Turn, AttackType.Magical, Attacker, Defender, MathHelper.Clamp(((Attacker.Stats.Magic_Attack * 2) * (1 - Defender.Stats.Magic_Resist / 100)) * (1 - (Defender.Stats.Dodge_Chance / 100)), 0, 100000), true);

            }
            else
            {
                //Dodge as Normal
                //Defender.Take_Damage(MathHelper.Clamp(Attacker.Stats.Magic_Attack * (1 - (Defender.Stats.Magic_Resist / 100)), 0, 100000));
                //return new CombatLogEntry(Turn, AttackType.Magical, Attacker, Defender, MathHelper.Clamp(Attacker.Stats.Magic_Attack * (1 - (Defender.Stats.Magic_Resist / 100)), 0, 100000), true);

                //Dodge as Damage Reduction
                Defender.Take_Damage(MathHelper.Clamp(((Attacker.Stats.Magic_Attack * 1) * (1 - Defender.Stats.Magic_Resist / 100)) * (1 - (Defender.Stats.Dodge_Chance / 100)), 0, 100000));
                return new CombatLogEntry(Turn, AttackType.Magical, Attacker, Defender, MathHelper.Clamp(((Attacker.Stats.Magic_Attack * 1) * (1 - Defender.Stats.Magic_Resist / 100)) * (1 - (Defender.Stats.Dodge_Chance / 100)), 0, 100000), true);

            }
        }
    }
}
