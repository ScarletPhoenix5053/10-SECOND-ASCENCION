using System;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>() as CapsuleCollider2D;

            if (!_gravityEnabledDefault) _gravityEnabled = false;
            
        }
        private void Update()
        {
        }
        private void FixedUpdate()
        {
            Act();
        }




        #region MOVEMENT VARIABLES
        private bool _gravityEnabledDefault = true;
        private bool _gravityEnabled = true;
        private bool _impulseLastFrame = false;

        private const float _groundStickDistance = 0.1f;
        private const float _groundingBuffer = 0.05f;
        private const float _zeroThreshold = 0.05f;

        public float _drag = 0.5f;
        public float _fallMultiplier = 1.5f;
        public float _lowJumpMultiplier = 1.5f;
        public float _jumpStrength = 10f;
        public float _gravityStrengthMax = 20f;
        public float _gravityStrengthCurrent = 0.2f;
        public float _speed = 10f;

        private Vector2 _motionVectorInput;
        private Vector2 _motionVectorCont;
        private Vector2 _motionVectorCombined;

        private CapsuleCollider2D _col;
        private Rigidbody2D _rb;
        #endregion
        #region INPUT VARIABLES
        public void TriggerInputThrow() => _inputThrow = true;
        public void TriggerInputJump() => _inputJump = true;        
        public void TriggerInputJumpHeld() => _inputJumpHeld = true;        
        public void TriggerInputHorizontal(float input) => _inputHor = input;
        public void TriggerInputVertical(float input) => _inputVert = input;

        private bool _inputThrow = false;
        private bool _inputJump = false;
        private bool _inputJumpHeld = false;
        private float _inputHor = 0;
        private float _inputVert = 0;

        private bool _isThrowing = false;
        private PlayerState _state = PlayerState.Standing;
        #endregion

        public LayerMask GroundMask;
        /// <summary>
        /// True if touching the ground.
        /// </summary>
        public bool IsGrounded()
        {
            return
                // Cast ray downward on left side
                Physics2D.Raycast(
                    new Vector2(
                        _col.bounds.center.x - _col.bounds.extents.x,
                        _col.bounds.center.y - _col.bounds.extents.y
                        ),
                    Vector2.down,
                    _groundingBuffer,
                    GroundMask)
            ||
                // Cast ray downward on right side
                Physics2D.Raycast(
                    new Vector2(
                        _col.bounds.center.x + _col.bounds.extents.x,
                        _col.bounds.center.y - _col.bounds.extents.y
                        ),
                    Vector2.down,
                    _groundingBuffer,
                    GroundMask);

            // Returns true if either ray hits the ground
        }

        #region MOVEMENT METHODS
        /// <summary>
        /// Applies an accelerating downward force whenver the player is airborne.
        /// </summary>
        private void ApplyGravity()
        {
            if (!_gravityEnabled) return;

            if (IsGrounded())
            {
                if (_motionVectorCont.y < 0f) _motionVectorCont.y = -0.5f;
            }
            // If not falling faster than max falling speed
            else if (_motionVectorCont.y > -_gravityStrengthMax)
            {
                // ...and is actually falling, not rising
                if (_motionVectorCont.y < 0)
                {
                    Debug.Log("Fall");
                    _motionVectorCont.y -= _gravityStrengthCurrent * _fallMultiplier;
                }
                // ...otherwise must be rising
                else if (_motionVectorCont.y > 0 && !_inputJumpHeld)
                {
                    Debug.Log("Rise");
                    _motionVectorCont.y -= _gravityStrengthCurrent * _lowJumpMultiplier;
                }
                else
                {
                    Debug.Log("Held");
                    _motionVectorCont.y -= _gravityStrengthCurrent;
                }
            }

        }
        /// <summary>
        /// Applies drag effects to the player.
        /// </summary>
        protected virtual void ApplyDrag()
        {
            if (_motionVectorCont.x <= -_zeroThreshold || _motionVectorCont.x >= _zeroThreshold)
            {
                _motionVectorCont.x -= Math.Sign(_motionVectorCont.x) * _drag;
            }
            else
            {
                _motionVectorCont.x = 0;
            }
        }

        /// <summary>
        /// Moves the object based on a calculated motion vector.
        /// </summary>
        private void CommitPosUpdate()
        {
            _rb.velocity = _motionVectorCombined;
        }

        /// <summary>
        /// Reset variables relating to the impulse method, allowing movement to resume
        /// normal operation.
        /// </summary>
        private void ResetImpulse()
        {
            _impulseLastFrame = false;
        }
        /// <summary>
        /// Reset motion input vector for next update cycle.
        /// </summary>
        protected void ResetInputVector()
        {
            _motionVectorInput = Vector2.zero;
        }

        /// <summary>
        /// Method that sticks player to ground if they are close by, allowing smooth ramp descending.
        /// Does not operate if there was an impulse last frame.
        /// </summary>
        private void StickToGround()
        {
            if (!_gravityEnabled) return;
            if (_impulseLastFrame) return;

            // Check if close enough to stick to ground
            var stickyRay =
                Physics2D.Raycast(
                    new Vector2(
                        _col.bounds.center.x,
                        _col.bounds.center.y - _col.bounds.extents.y
                        ),
                    Vector2.down,
                    _groundStickDistance,
                    GroundMask);

            // Stick player to ground if close enough
            if ((bool)stickyRay)
            {
                transform.position =
                    new Vector3(
                        transform.position.x,
                        transform.position.y - stickyRay.distance,
                        transform.position.z
                        );
            }
        }
        /// <summary>
        /// Combine movement vector with continuous motion applied by gravity
        /// or impulese to create a fintal motion vector.
        /// </summary>
        private void SetCombinedMotionVector()
        {
            _motionVectorCombined =
                _motionVectorInput * _speed + _motionVectorCont;
        }

        /// <summary>
        /// Calculate the player's new position and move it.
        /// </summary>
        private void UpdatePosition()
        {
            StickToGround();
            ApplyGravity();

            SetCombinedMotionVector();
            CommitPosUpdate();

            ApplyDrag();
            ResetInputVector();
            ResetImpulse();
        }
        #endregion
        #region INPUT METHODS
        private void Act()
        {
            switch (_state)
            {
                case PlayerState.Standing:
                    // Adjust motion vector
                    _motionVectorInput.x = _inputHor;
                    if (_inputJump)
                    {
                        _motionVectorCont.y = _jumpStrength;
                    }

                    // Apply motion
                    UpdatePosition();
                    break;

                case PlayerState.Climbing:
                    break;

                case PlayerState.PullingTeammateUp:
                    break;

                case PlayerState.BeingPulledUp:
                    break;

                default:
                    break;
            }

            ResetInputVars();
        }
        private void ResetInputVars()
        {
            _inputThrow = false;
            _inputJump = false;
            _inputJumpHeld = false;
            _inputHor = 0;
            _inputVert = 0;
        }
        #endregion
    }
    internal enum PlayerState
    {
        Standing,
        Climbing,
        PullingTeammateUp,
        BeingPulledUp
    }
}
