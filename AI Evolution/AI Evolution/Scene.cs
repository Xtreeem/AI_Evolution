using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI_Evolution
{
    class Scene
    {
        public bool IsTurnDone { get { return _turnDone; } }
        private bool _turnDone = false;

        public int Counter { get { return _counter; } }
        private int _counter;
        public Scene(int Counter)
        {
            _counter = Counter;
        }

        public void Update()
        {
            _counter++;
            _turnDone = true;
        }

        public void StartTurn()
        {
            _turnDone = false;
            Update();
        }


    }
}
