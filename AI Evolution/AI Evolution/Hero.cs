using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    class Hero : Actor
    {

        public Hero(Actor P1, Actor P2)
        {
            GenerateStats(P1, P2);
            GeneratePerks(P1, P2);
            _current_Health = _stats.Health;

        }
        public Hero(Stats Stats)
        {
            _stats = Stats;
            _current_Health = _stats.Health;
        }

        public void Take_Damage(float Amount)
        {
            _current_Health -= Amount;
            if (_current_Health <= 0)
                _alive = false;
        }

        private void GenerateStats(Actor P1, Actor P2)
        {
            //Super breed funky town
        }

        private void GeneratePerks(Actor P1, Actor P2)
        {

        }
    }
}
