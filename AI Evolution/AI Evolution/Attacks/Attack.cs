using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    public abstract class Attack
    {
        abstract public AttackType Type { get; }

        public int Cost { get { return _cost; } }
        protected int _cost = 1;

        abstract public CombatLogEntry Execute(ref Actor Attacker, ref Actor Defender, int Turn, Random Random);

    }
}
