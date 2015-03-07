using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    class Actor
    {
        public Stats Stats { get { return _stats; } }
        protected Stats _stats;

        public Avatar.Avatar Avatar { get { return _avatar; } }
        protected Avatar.Avatar _avatar;

        public bool IsAlive { get { return _alive; } }
        protected bool _alive = true;
        public float Current_Health { get { return _current_Health; } }
        protected float _current_Health;

        public string Name { get { return _name; } }
        protected string _name;

        public virtual void Draw(SpriteBatch SB)
        {
            _avatar.Draw(SB);
        }

        public virtual void Update(GameTime GT)
        {
            _avatar.Update(ref GT);
        }

        public void Take_Damage(float Amount)
        {
            _current_Health -= Amount;
            if (_current_Health <= 0)
                _alive = false;
        }
    }
}
