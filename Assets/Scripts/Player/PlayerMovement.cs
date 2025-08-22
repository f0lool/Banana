using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats _stats;
    [SerializeField] private PlayerAnimator _playerAnimator;
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize;
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;

    private PlayerInputSystem _playerInput;

    #region COMPONENTS
    public Rigidbody2D _rigidbody2D { get; private set; }
    #endregion

    #region STATE PARAMETERS
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsSliding { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    public float LastOnGroundTime { get; private set; }

    private bool _isJumpCut;
    private bool _isJumpFalling;
    #endregion

    #region INPUT PARAMETERS
    private Vector2 _moveInput;

    public float LastPressedJumpTime { get; private set; }
    #endregion

    public void Init(PlayerStats stats, PlayerInputSystem inputSystem)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _stats = stats;
        SetGravityScale(_stats.gravityScale);
        IsFacingRight = true;
        _stats.IsInputEnabled = true;
        _playerInput = inputSystem;
        _playerInput.Player.Enable();


        _playerInput.Player.Jump.performed += ctx => OnJumpInput();
    }

    private void Update()
    {
        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
        _moveInput.x = _playerInput.Player.Movement.ReadValue<Vector2>().x;
        _moveInput.y = _playerInput.Player.Movement.ReadValue<Vector2>().y;

        #endregion

        #region COLLISION CHECKS
        if (!IsJumping)
        {
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
            {
                if (LastOnGroundTime < -0.1f)
                {
                    _playerAnimator.SetBool("IsJumping", false);
                }

                LastOnGroundTime = _stats.coyoteTime; 
            }

            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
                    || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)))
                LastOnWallRightTime = _stats.coyoteTime;

            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
                || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)))
                LastOnWallLeftTime = _stats.coyoteTime;

            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        #endregion

        #region JUMP CHECKS
        if (IsJumping && _rigidbody2D.linearVelocity.y < 0)
        {
            IsJumping = false;

            _isJumpFalling = true;
        }

        if (LastOnGroundTime > 0 && !IsJumping)
        {
            _isJumpCut = false;

            _isJumpFalling = false;
        }


        if (CanJump() && LastPressedJumpTime > 0 && _stats.IsInputEnabled)
        {
            _playerAnimator.SetBool("IsJumping", true);
            IsJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            Jump();
        }
        #endregion

        #region GRAVITY
        //Higher gravity if we've released the jump input or are falling
        if (IsSliding)
        {
            SetGravityScale(0);
        }
        else if (_rigidbody2D.linearVelocity.y < 0 && _moveInput.y < 0)
        {
            //Much higher gravity if holding down
            SetGravityScale(_stats.gravityScale * _stats.fastFallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, Mathf.Max(_rigidbody2D.linearVelocity.y, -_stats.maxFastFallSpeed));
        }
        else if (_isJumpCut)
        {
            //Higher gravity if jump button released
            SetGravityScale(_stats.gravityScale * _stats.jumpCutGravityMult);
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, Mathf.Max(_rigidbody2D.linearVelocity.y, -_stats.maxFallSpeed));
        }
        else if ((IsJumping || _isJumpFalling) && Mathf.Abs(_rigidbody2D.linearVelocity.y) < _stats.jumpHangTimeThreshold)
        {
            SetGravityScale(_stats.gravityScale * _stats.jumpHangGravityMult);
        }
        else if (_rigidbody2D.linearVelocity.y < 0)
        {
            //Higher gravity if falling
            SetGravityScale(_stats.gravityScale * _stats.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, Mathf.Max(_rigidbody2D.linearVelocity.y, -_stats.maxFallSpeed));
        }
        else
        {
            //Default gravity if standing on a platform or moving upwards
            SetGravityScale(_stats.gravityScale);
        }
        #endregion

        #region SLIDE CHECKS
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
        #endregion

        _playerAnimator.SetFloat("xVelocity", Mathf.Abs(_rigidbody2D.linearVelocity.x));
        _playerAnimator.SetFloat("yVelocity", _rigidbody2D.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        if(_stats.IsInputEnabled)
            Run(1);

        if (IsSliding)
            Slide();
    }

    #region INPUT CALLBACKS
    public void OnJumpInput()
    {
        LastPressedJumpTime = _stats.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut())
            _isJumpCut = true;
    }
    #endregion

    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        float targetSpeed = _moveInput.x * _stats.runMaxSpeed;
        targetSpeed = Mathf.Lerp(_rigidbody2D.linearVelocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _stats.runAccelAmount : _stats.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _stats.runAccelAmount * _stats.accelInAir : _stats.runDeccelAmount * _stats.deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        if ((IsJumping || _isJumpFalling) && Mathf.Abs(_rigidbody2D.linearVelocity.y) < _stats.jumpHangTimeThreshold)
        {
            accelRate *= _stats.jumpHangAccelerationMult;
            targetSpeed *= _stats.jumpHangMaxSpeedMult;
        }
        #endregion

        #region Conserve Momentum
        if (_stats.doConserveMomentum && Mathf.Abs(_rigidbody2D.linearVelocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_rigidbody2D.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            accelRate = 0;
        }
        #endregion

        float speedDif = targetSpeed - _rigidbody2D.linearVelocity.x;

        float movement = speedDif * accelRate;
        _rigidbody2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;

        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region JUMP METHODS
    private void Jump()
    {
        _playerAnimator.SetTrigger("IsJump");
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        float force = _stats.jumpForce;
        if (_rigidbody2D.linearVelocity.y < 0)
            force -= _rigidbody2D.linearVelocity.y;

        _rigidbody2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
    #endregion

    #region CHECK METHODS

    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }

    private bool CanJumpCut()
    {
        return IsJumping && _rigidbody2D.linearVelocity.y > 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && LastOnGroundTime <= 0)
            return true;
        else
            return false;
    }
    #endregion

    private void OnDestroy()
    {
        _playerInput.Player.Jump.performed -= ctx => OnJumpInput();

        _playerInput.Player.Disable();
    }

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
    }
    #endregion

    #region OTHER MOVEMENT METHODS
    private void Slide()
    {
        //We remove the remaining upwards Impulse to prevent upwards sliding
        if (_rigidbody2D.linearVelocity.y > 0)
        {
            _rigidbody2D.AddForce(-_rigidbody2D.linearVelocity.y * Vector2.up, ForceMode2D.Impulse);
        }

        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        float speedDif = _stats.slideSpeed - _rigidbody2D.linearVelocity.y;
        float movement = speedDif * _stats.slideAccel;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        _rigidbody2D.AddForce(movement * Vector2.up);
    }
    #endregion

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        _rigidbody2D.gravityScale = scale;
    }

    private void Sleep(float duration)
    {
        StartCoroutine(nameof(PerformSleep), duration);
    }

    private IEnumerator PerformSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }
    #endregion
}
