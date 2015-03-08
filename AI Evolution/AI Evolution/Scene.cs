using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AI_Evolution
{
    class Scene
    {
        public bool IsFinished { get { return _battle.IsFinished; } }
        public Actor Hero { get { return _hero; } }
        private Actor _hero;
        private Actor _enemy;
        private Battle _battle;
        
        public Scene(Actor Hero, Actor Enemy)
        {
            _battle = new Battle(Hero, Enemy, Misc.Random.Next(1, 100001));
            _hero = Hero;
            _enemy = Enemy;
        }

        public void Update(GameTime GT, float TurnLength)
        {
            if (_battle.IsFinished)
                return;
            else
                _battle.Update(GT, TurnLength);
        }

        public Tuple<float, Actor> Get_Result()
        {
            return new Tuple<float, Actor>(_battle.FitnessValue, _hero);
        }

    }
}
