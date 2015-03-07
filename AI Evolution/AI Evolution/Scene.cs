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
        Battle _battle;
        public Actor Hero { get { return _hero; } }
        private Actor _hero;
        private Actor _enemy;
        public Scene(ref Actor Hero, Actor Enemy)
        {
            _battle = new Battle(ref Hero, Enemy, Misc.Random.Next(1, 100001));
            _hero = Hero;
            _enemy = Enemy;
        }

        public void Update(GameTime GT)
        {

        }
    }
}
