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
            // Assign from self
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>() as CapsuleCollider2D;

            // Assign from children
            _grabBox = GetComponentInChildren<GrabBox>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.tag == "CarryPos") _carryPos = child;
            }

            if (!_gravityEnabledDefault) _gravityEnabled = false;            
        }
        private void FixedUpdate()
        {
            Act();
            UpdateHeldPlayerPos();
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

                _canJump = true;
            }
            // If not falling faster than max falling speed
            else if (_motionVectorCont.y > -_gravityStrengthMax)
            {
                // ...and is actually falling, not rising
                if (_motionVectorCont.y < 0)
                {
                    _motionVectorCont.y -= _gravityStrengthCurrent * _fallMultiplier;
                }
                // ...otherwise must be rising
                else if (_motionVectorCont.y > 0 && !_inputJumpHeld)
                {
                    _motionVectorCont.y -= _gravityStrengthCurrent * _lowJumpMultiplier;
                }
                else
                {
                    _motionVectorCont.y -= _gravityStrengthCurrent;
                }

                _canJump = false;
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

        #region INPUT & MOTION VARIABLES
        // Calling these trigger methods switches the related bool to true.
        // This bool is reset each frame, thus making it function as a trigger.
        public void TriggerInputThrow() => _inputThrow = true;
        public void TriggerInputJump() => _inputJump = true;        
        public void TriggerInputJumpHeld() => _inputJumpHeld = true;        
        public void TriggerInputHorizontal(float input) => _inputHor = input;
        public void TriggerInputVertical(float input) => _inputVert = input;

        private bool _canJump = true;
        private bool _holdingPlayer = false;
        private Vector3 _heldPlayerPos = new Vector2(-1, 1);

        private bool _inputThrow = false;
        private bool _inputJump = false;
        private bool _inputJumpHeld = false;
        private float _inputHor = 0;
        private float _inputVert = 0;

        public int Sign
        {
            get { return _sign; }
            set { if (value != 0)  _sign = Mathf.Clamp(value, -1, 1); }
        }
        private int _sign = 1;

        private PlayerState _state = PlayerState.Normal;
        [SerializeField]
        private LayerMask GroundMask;
        #endregion
        #region INPUT & MOTION METHODS
        private void Act()
        {
            FaceSign();

            switch (_state)
            {
                case PlayerState.Normal:
                    // Check inputs
                    DoMotionStanding();

                    if (_holdingPlayer) DoThrow();
                    else DoGrab();

                    // Apply motion
                    UpdatePosition();
                    break;

                case PlayerState.Climbing:
                    break;

                case PlayerState.Assisting:
                    break;

                case PlayerState.Held:
                    break;

                default:
                    break;
            }

            ResetInputVars();
        }
        private void DoMotionStanding()
        {
            _motionVectorInput.x = _inputHor;
            Sign = _inputHor < 0 ? (int)Math.Floor(_inputHor) : (int)Math.Ceiling(_inputHor);
            if (_canJump && _inputJump)
            {
                _motionVectorCont.y = _jumpStrength;
            }
        }
        private void ResetInputVars()
        {
            _inputThrow = false;
            _inputJump = false;
            _inputJumpHeld = false;
            _inputHor = 0;
            _inputVert = 0;
        }
        private void FaceSign()
        {
            transform.localScale =
                new Vector3(
                    Sign,
                    transform.localScale.y,
                    transform.localScale.z
                    );
        }
        #endregion

        #region MECHANIC VARIABLES
        private GrabBox _grabBox;
        private Transform _carryPos;
        private PlayerController _heldPlayer;
        #endregion
        #region MECHANIC METHODS
        public void Hold()
        {
            _state = PlayerState.Held;
        }
        public void Launch(Vector2 dir)
        {
            _state = PlayerState.Normal;
            Debug.Log("vWHOOSH");
        }
        private void DoGrab()
        {
            if (_inputThrow)
            {
                var otherPlayer = _grabBox.GrabPlayer();
                Debug.Log(otherPlayer != null ? otherPlayer.name : "nothing");

                if (otherPlayer == null) return;

                otherPlayer.Hold();
                _heldPlayer = otherPlayer;
                _holdingPlayer = true;
            }
        }
        private void DoThrow()
        {
            if (_inputThrow)
            {
                if (_heldPlayer == null) return;

                _heldPlayer.Launch(Vector2.one);
                _holdingPlayer = false;
                _heldPlayer = null;
            }
        }
        private void UpdateHeldPlayerPos()
        {
            if (_heldPlayer == null) return;

            _heldPlayer.transform.position = _carryPos.position;
        }
        #endregion

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
        
    }
    internal enum PlayerState
    {
        Normal,
        Launched,
        Climbing,
        Assisting,
        Held
    }
}
