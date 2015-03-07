using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Avatar
{
    public class Avatar
    {

        public Vector2 Pos { get { return _pos; } }
        private Vector2 _pos;
        private Vector2[] _bodyPartsPos = new Vector2[5];
        private Vector2[] _bodyPartsOrigin = new Vector2[5];
        private Vector2[] _bodyPartsDistanceFromAnchor = new Vector2[5];
        public float Rotation { get { return _rotation; } }
        private float _rotation;

        private float _scale;
        private Random _random;

        private Texture2D _headTex;
        private Texture2D _bodyTex;
        private Texture2D _feetTex;
        private Texture2D _handRTex;
        private Texture2D _handLTex;

        public Vector2 Target { get { return _target; } }
        private Vector2 _target = Vector2.Zero;
        private Vector2 _velocity = Vector2.Zero;
        private float _speed;




        private bool _rotatingLeft = true;
        private float _rotationSpeed = 0.01f;
        private bool _bouncingUp = true;
        private float _bounceHeightMax = 10f;
        private float _bounceHeightStep = 2f;
        private float _bounceHeight;
        private bool _movingLeft = true;

        public Avatar(Vector2 Position, float Scale, Texture2D[] Textures, int RandomSeed, float Speed)
        {
            _pos = Position;
            _speed = Speed;
            _scale = Scale;
            _random = new Random(RandomSeed);
            _rotation = _random.Next(-45, 46);
            _rotation = MathHelper.ToRadians(_rotation);
            _rotation += (float)Math.PI;
            _bounceHeight = _random.Next(0, 11);
            _headTex = Textures[0];
            _bodyTex = Textures[1];
            _feetTex = Textures[2];
            _handRTex = Textures[3];
            _handLTex = Textures[4];

            //_rotation = (float)Math.PI;
            InitialBodyPartOrgCalc();
            InitialBodyPartPosCalc();
        }

        public void Update(ref GameTime GT)
        {
            //if (_target != _pos)
            //{
            //    MoveToTarget(GT);

            //}
            TestWalk();
            CalculatePartsPosition();
        }

        public void Draw(SpriteBatch SB)
        {
            SB.Draw(_feetTex, _bodyPartsPos[0], null, Color.White, _rotation - (float)(Math.PI), _bodyPartsOrigin[0], _scale, SpriteEffects.None, 1f);
            SB.Draw(_bodyTex, _bodyPartsPos[1], null, Color.White, _rotation - (float)(Math.PI), _bodyPartsOrigin[1], _scale, SpriteEffects.None, 1f);
            SB.Draw(_headTex, _bodyPartsPos[2], null, Color.White, _rotation - (float)(Math.PI), _bodyPartsOrigin[2], _scale, SpriteEffects.None, 1f);
            SB.Draw(_handRTex, _bodyPartsPos[3], null, Color.White, _rotation - (float)(Math.PI), _bodyPartsOrigin[3], _scale, SpriteEffects.None, 1f);
            SB.Draw(_handLTex, _bodyPartsPos[4], null, Color.White, _rotation - (float)(Math.PI), _bodyPartsOrigin[4], _scale, SpriteEffects.None, 1f);
        }

        public void MoveToVector(Vector2 NewTarget)
        {
            _target = NewTarget;
            _velocity = _target - _pos;
            Vector2.Normalize(ref _velocity, out _velocity);
        }

        private void MoveToTarget(GameTime GT)
        {
            if (Math.Abs(_pos.X - _target.X) <= (_velocity.X * _speed) * GT.ElapsedGameTime.TotalSeconds)
                _pos.X = _target.X;
            if (Math.Abs(_pos.Y - _target.Y) <= (_velocity.Y * _speed) * GT.ElapsedGameTime.TotalSeconds)
                _pos.Y = _target.Y;

            if (_pos == _target)
                return;

            if (_pos.X != _target.X)
                _pos.X += (_velocity.X * _speed) * (float)GT.ElapsedGameTime.TotalSeconds;
            if (_pos.Y != _target.Y)
                _pos.Y += (_velocity.Y * _speed) * (float)GT.ElapsedGameTime.TotalSeconds;






            if (_rotation > (Math.PI / 25) + Math.PI)
                _rotatingLeft = false;
            else if (_rotation < -(Math.PI / 25) + Math.PI)
                _rotatingLeft = true;
            if (_rotatingLeft)
                _rotation += _rotationSpeed;
            else
                _rotation -= _rotationSpeed;

            if (_bounceHeight > _bounceHeightMax)
                _bouncingUp = false;
            else if (_bounceHeight <= 0)
                _bouncingUp = true;
            if (_bouncingUp)
                _bounceHeight += _bounceHeightStep;
            else
                _bounceHeight -= _bounceHeightStep;

        }


        private void TestWalk()
        {
            {
                if (_rotation > (Math.PI / 25) + Math.PI)
                    _rotatingLeft = false;
                else if (_rotation < -(Math.PI / 25) + Math.PI)
                    _rotatingLeft = true;
                if (_rotatingLeft)
                    _rotation += _rotationSpeed;
                else
                    _rotation -= _rotationSpeed;

                if (_bounceHeight > _bounceHeightMax)
                    _bouncingUp = false;
                else if (_bounceHeight <= 0)
                    _bouncingUp = true;
                if (_bouncingUp)
                    _bounceHeight += _bounceHeightStep;
                else
                    _bounceHeight -= _bounceHeightStep;

                if (_pos.X > 1080)
                    _movingLeft = false;
                else if (_pos.X < 0)
                    _movingLeft = true;
                if (_movingLeft)
                    _pos.X++;
                else
                    _pos.X--;
            }
        }

        private void InitialBodyPartPosCalc()
        {
            float overlap = 10 * _scale;
            Vector2 headOffSet = new Vector2(0, _headTex.Height * _scale);
            Vector2 bodyOffSet = new Vector2(0, (_bodyTex.Height * _scale) - overlap);
            Vector2 feetOffSet = new Vector2(0, _feetTex.Height * _scale);
            Vector2 handsOffSet = new Vector2((_bodyTex.Width * _scale) / 2, -((_bodyTex.Height * _scale) / 2f));

            _bodyPartsPos[0] = _pos - new Vector2(feetOffSet.X, feetOffSet.Y);
            _bodyPartsPos[1] = _bodyPartsPos[0] - new Vector2(bodyOffSet.X - feetOffSet.X, bodyOffSet.Y - overlap);
            _bodyPartsPos[2] = _bodyPartsPos[1] - new Vector2(headOffSet.X - bodyOffSet.X, headOffSet.Y - overlap);
            _bodyPartsPos[3] = _bodyPartsPos[1] - new Vector2(handsOffSet.X, handsOffSet.Y);
            _bodyPartsPos[4] = _bodyPartsPos[1] - new Vector2(-handsOffSet.X, handsOffSet.Y);

            _bodyPartsPos[0].Y += (_bodyPartsOrigin[0].Y * _scale);
            _bodyPartsPos[1].Y += (_bodyPartsOrigin[1].Y * _scale);
            _bodyPartsPos[2].Y += (_bodyPartsOrigin[2].Y * _scale);
            _bodyPartsPos[3].Y += (_bodyPartsOrigin[3].Y * _scale);
            _bodyPartsPos[4].Y += (_bodyPartsOrigin[4].Y * _scale);

            for (int i = 0; i < _bodyPartsDistanceFromAnchor.Length; i++)
            {
                _bodyPartsDistanceFromAnchor[i].Y = Vector2.Distance(_pos, _bodyPartsPos[i]);
                _bodyPartsDistanceFromAnchor[i].X = _pos.X - _bodyPartsPos[i].X;
            }
        }

        private void InitialBodyPartOrgCalc()
        {
            _bodyPartsOrigin[0] = new Vector2(_feetTex.Width / 2, _feetTex.Height / 2);
            _bodyPartsOrigin[1] = new Vector2(_bodyTex.Width / 2, _bodyTex.Height / 2);
            _bodyPartsOrigin[2] = new Vector2(_headTex.Width / 2, _headTex.Height / 2);
            _bodyPartsOrigin[3] = new Vector2(_handRTex.Width / 2, _handRTex.Height / 2);
            _bodyPartsOrigin[4] = new Vector2(_handLTex.Width / 2, _handLTex.Height / 2);
        }
        private void CalculatePartsPosition()
        {
            for (int i = 0; i < _bodyPartsPos.Length; i++)
            {
                _bodyPartsPos[i].X = _bodyPartsDistanceFromAnchor[i].X + (float)(_pos.X + (_bodyPartsDistanceFromAnchor[i].Y * Math.Sin(-_rotation)));
                _bodyPartsPos[i].Y = (float)(_pos.Y + ((_bodyPartsDistanceFromAnchor[i].Y + _bounceHeight) * Math.Cos(-_rotation)));
            }
        }
    }
}
