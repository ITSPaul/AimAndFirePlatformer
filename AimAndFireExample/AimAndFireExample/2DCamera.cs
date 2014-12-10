using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AnimatedSprite;

namespace Cameras
{
    public class Camera2D
    {
        #region Fields

        protected float _zoom;
        protected Matrix _transform;
        protected Matrix _inverseTransform;
        protected Vector2 _pos;
        protected float _rotation;
        protected Viewport _viewport;
        protected MouseState _mState;
        protected KeyboardState _keyState;
        protected Int32 _scroll;
        bool _following;
        private Rectangle _cameraExtent;

        public Rectangle CameraExtent
        {
            get { return _cameraExtent; }
            set { _cameraExtent = value; }
        }

        public bool Following
        {
            get { return _following; }
            set { _following = value; }
        }

        #endregion

        #region Properties

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }
        /// <summary>
        /// Camera View Matrix Property
        /// </summary>
        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        /// <summary>
        /// Inverse of the view matrix, can be used to get objects screen coordinates
        /// from its object coordinates
        /// </summary>
        public Matrix InverseTransform
        {
            get { return _inverseTransform; }
        }
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        #endregion

        #region Constructor

        public Camera2D(Viewport viewport)
        {
            _zoom = 1.0f;
            _scroll = 1;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            _viewport = viewport;
            CameraExtent = new Rectangle((int)_pos.X, (int)_pos.Y, _viewport.Width, _viewport.Height);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update the camera view
        /// </summary>
        public void Update()
        {
            //Call Camera Input
            //Input();
            //Clamp zoom value
            _zoom = MathHelper.Clamp(_zoom, 0.0f, 10.0f);
            //Clamp rotation value
            _rotation = ClampAngle(_rotation);
            //Create view matrix
            _transform = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) * 
                            Matrix.CreateRotationZ(_rotation) *
                            Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
                            Matrix.CreateTranslation(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0);
            //Update inverse matrix
            _inverseTransform = Matrix.Invert(_transform);
            CameraExtent = new Rectangle((int)_pos.X, (int)_pos.Y, _viewport.Width, _viewport.Height);
            Point centre = CameraExtent.Center;
        }

        public void revert() { _rotation = 0f; }

        /// <summary>
        /// Example Input Method, rotates using cursor keys and zooms using mouse wheel
        /// </summary>
        protected virtual void Input()
        {
            _mState = Mouse.GetState();
            _keyState = Keyboard.GetState();
            //Check zoom
            if (_mState.ScrollWheelValue > _scroll)
            {
                _zoom += 0.1f;
                _scroll = _mState.ScrollWheelValue;
            }
            else if (_mState.ScrollWheelValue < _scroll)
            {
                _zoom -= 0.1f;
                _scroll = _mState.ScrollWheelValue;
            }
            //Check rotation - no rotation allowed
            //if (_keyState.IsKeyDown(Keys.Left))
            //{
            //    _rotation -= 0.1f;
            //}
            //if (_keyState.IsKeyDown(Keys.Right))
            //{
            //    _rotation += 0.1f;
            //}

            //if (_keyState.IsKeyDown(Keys.R))
            //{
            //    revert();
            //}


            if (!Following)
            {
                //Check Move
                if (_keyState.IsKeyDown(Keys.A))
                {
                    _pos.X += 0.5f;
                }
                if (_keyState.IsKeyDown(Keys.D))
                {
                    _pos.X -= 0.5f;
                }
                if (_keyState.IsKeyDown(Keys.W))
                {
                    _pos.Y += 0.5f;
                }
                if (_keyState.IsKeyDown(Keys.S))
                {
                    _pos.Y -= 0.5f;
                }
            }
        }

        /// <summary>
        /// Clamps a radian value between -pi and pi
        /// </summary>
        /// <param name="radians">angle to be clamped</param>
        /// <returns>clamped angle</returns>
        protected float ClampAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        #endregion
    }
}


