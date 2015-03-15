using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    public enum AttackType
    {
        Magical,
        Physical,
        Heal
    }

    public struct CombatLogEntry
    {
        int Turn;
        AttackType Type;
        Actor Attacker;
        Actor Defender;
        float Damage;
        bool Hit;

        public CombatLogEntry(int Turn, AttackType Type, Actor Attacker, Actor Defender, float Damage, bool Hit)
        {
            this.Turn = Turn;
            this.Type = Type;
            this.Attacker = Attacker;
            this.Defender = Defender;
            this.Damage = Damage;
            this.Hit = Hit;
        }
    }

    class CombatLog
    {
        List<CombatLogEntry> Log = new List<CombatLogEntry>();
        public CombatLog()
        {

        }

        public void AddEntry(int Turn, AttackType Type, Actor Attacker, Actor Defender, float Damage, bool Hit)
        {
            if (Hit)
                Log.Add(new CombatLogEntry(Turn, Type, Attacker, Defender, Damage, Hit));
            else
                Log.Add(new CombatLogEntry(Turn, Type, Attacker, Defender, 0f, Hit));
        }

        public void AddEntry(CombatLogEntry Entry)
        {
            Log.Add(Entry);
        }
    }
}
