//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VRGameMobile
{
	public partial class PlayerController
	{
		
		[Header("Player")]
		public Alteruna.Avatar Avatar;
		
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;

		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;

		[Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;

		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		public AudioClip LandingAudioClip;
		public AudioClip[] FootstepAudioClips;
		[Range(0, 1)] public float FootstepAudioVolume = 0.5f;

		[Space(10)] [Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;

		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)] [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;

		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")] [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;

		[Tooltip("Useful for rough ground")] public float GroundedOffset = -0.14f;

		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;

		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

        [Header("Teleport Points")]
        public TeleportPoint[] teleportPoints;
        private int currentTeleportIndex = 0;


        private CharacterController _controller;
        public float FixedRotationAngle = 45f;
        //public InputActionReference teleportNextAction; // Action để teleport tới mốc tiếp
        //public InputActionReference teleportPreviousAction; // Action để teleport tới mốc trước
        //public InputActionReference rotateLeftAction; // Action để xoay trái
        //public InputActionReference rotateRightAction;

        // player
        private float _speed;
		private float _animationBlend;
		private float _targetRotation;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
        public float teleportReachedDistance = 0.5f;


        private int currentWaypointIndex = 0;
        [Header("Waypoint System")]
        [Tooltip("Array of teleport points acting as waypoints")]
        //public TeleportPoint[] teleportPoints;
        //[Tooltip("Whether the player is moving along waypoints")]
        public bool isMovingWaypoints = false;
        [Tooltip("Whether to loop through waypoints when reaching the end")]
        public bool isLoopWaypoints = true;
        [Tooltip("Distance threshold to consider a waypoint reached")]
        public float waypointReachedDistance = 0.5f;
        private float _nextFootstepTime;
        private const float FootstepInterval = 0.4f;
        private void GroundedCheck()
		{
			// set sphere position, with offset
			var position = transform.position;
			Vector3 spherePosition = new Vector3(position.x, position.y - GroundedOffset, position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
				QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
				// Debug.Log($"[ANIM] Grounded: {Grounded}");
            }
		}
        //private void Move()
        //{
        //    float horizontal = _horizontal;
        //    float vertical = _vertical;

        //    bool input = horizontal != 0f || vertical != 0f;

        //    // set target speed based on move speed, sprint speed and if sprint is pressed
        //    float targetSpeed = 0;

        //    // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        //    float inputMagnitude = 1f;

        //    // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        //    // if there is no input, set the target speed to 0
        //    if (input)
        //    {
        //        targetSpeed = _sprint ? SprintSpeed : MoveSpeed;
        //        //inputMagnitude = new Vector2(horizontal, vertical).magnitude;
        //    }

        //    // a reference to the players current horizontal velocity
        //    float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        //    float speedOffset = 0.1f;

        //    // accelerate or decelerate to target speed
        //    if (currentHorizontalSpeed < targetSpeed - speedOffset ||
        //        currentHorizontalSpeed > targetSpeed + speedOffset)
        //    {
        //        // creates curved result rather than a linear one giving a more organic speed change
        //        // note T in Lerp is clamped, so we don't need to clamp our speed
        //        _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
        //            Time.deltaTime * SpeedChangeRate);

        //        // round speed to 3 decimal places
        //        _speed = Mathf.Round(_speed * 1000f) / 1000f;
        //    }
        //    else
        //    {
        //        _speed = targetSpeed;
        //    }

        //    _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        //    if (_animationBlend < 0.01f) _animationBlend = 0f;

        //    // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        //    // if there is a move input rotate player when the player is moving
        //    if (input)
        //    {
        //        // normalise input direction
        //        Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;

        //        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _cameraTarget.eulerAngles.y;

        //        if (_firstPerson)
        //        {
        //            _bodyRotate = Mathf.SmoothDampAngle(_bodyRotate, _cameraTarget.eulerAngles.y, ref _rotationVelocity,
        //                RotationSmoothTime);

        //            transform.rotation = Quaternion.Euler(0.0f, _bodyRotate, 0.0f);

        //            // play animation based on which way the character is moving relative to the characters forward
        //            inputMagnitude *= vertical;
        //            inputMagnitude += horizontal * (1 - Mathf.Abs(inputMagnitude));
        //        }
        //        else
        //        {
        //            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
        //                RotationSmoothTime);

        //            // rotate to face input direction relative to camera position
        //            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        //        }
        //    }


        //    Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        //    // move the player
        //    _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
        //                     new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        //    // update animator if using character
        //    if (_hasAnimator)
        //    {
        //        _animator.SetFloat(_animIDSpeed, _animationBlend);
        //        _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        //    }
        //}

        
        private void LogicMove()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {

                MoveToNextWaypoint(); 

            }
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                MoveToPreviousWaypoint();
            }
            if (isMovingWaypoints)
            {
                MoveToWaypoint();
            }
            else
            {
                Move();
            }
        }

        private void Move()
        {
            float horizontal = _horizontal;
            float vertical = _vertical;
            bool input = horizontal != 0f || vertical != 0f;
            float targetSpeed = input ? (_sprint ? SprintSpeed : MoveSpeed) : 0f;
            float inputMagnitude = input ? new Vector2(horizontal, vertical).magnitude : 1f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            if (input)
            {
                Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _cameraTarget.eulerAngles.y;

                if (_firstPerson)
                {
                    _bodyRotate = Mathf.SmoothDampAngle(_bodyRotate, _cameraTarget.eulerAngles.y, ref _rotationVelocity, RotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0.0f, _bodyRotate, 0.0f);
                    inputMagnitude *= vertical;
                    inputMagnitude += horizontal * (1 - Mathf.Abs(inputMagnitude));
                }
                else
                {
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        public void StartMovingWaypoints()
        {
            if (teleportPoints == null || teleportPoints.Length == 0) return;
            currentWaypointIndex = 0;
            // Xoay nhân vật theo rotation của điểm đầu tiên trước khi di chuyển
            if (teleportPoints.Length > 0)
            {
                _controller.enabled = false;
                transform.rotation = teleportPoints[currentWaypointIndex].rotation;
                _controller.enabled = true;
            }
            isMovingWaypoints = true;
        }

        public void MoveToNextWaypoint()
        {
            if (teleportPoints == null || teleportPoints.Length == 0)
            {
                return;
            }
            currentWaypointIndex++;

            if (currentWaypointIndex < 0 || currentWaypointIndex >= teleportPoints.Length)
            {
                currentWaypointIndex = 0; 
            }
            int nextIdx = currentWaypointIndex + 1;
            if (nextIdx < teleportPoints.Length)
            {
                teleportPoints[nextIdx].SetLight(true);
                teleportPoints[currentWaypointIndex].SetLight(false);
            }
            

            _controller.enabled = false;
            transform.rotation = teleportPoints[currentWaypointIndex].rotation;
            _controller.enabled = true;
            isMovingWaypoints = true;
        }

        public void MoveToPreviousWaypoint()
        {
            if (teleportPoints == null || teleportPoints.Length == 0)
            {
                Debug.LogWarning("[PlayerController] No teleport points assigned");
                return;
            }

            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                if (isLoopWaypoints)
                {
                    currentWaypointIndex = teleportPoints.Length - 1; 
                }
                else
                {
                    currentWaypointIndex = 0; 
                    isMovingWaypoints = false;
                    // Debug.Log($"[PlayerController] Reached start of waypoints, stopping");
                    return;
                }
            }

            _controller.enabled = false;
            transform.rotation = teleportPoints[currentWaypointIndex].rotation;
            _controller.enabled = true;
            // Debug.Log($"[PlayerController] Rotated to current waypoint {currentWaypointIndex} rotation: {teleportPoints[currentWaypointIndex].rotation.eulerAngles}");

            isMovingWaypoints = true;
            // Debug.Log($"[PlayerController] Moving to previous waypoint: {currentWaypointIndex}");
        }

        public void StopMovingWaypoints()
        {
            isMovingWaypoints = false;
            // Debug.Log($"[PlayerController] Stopped moving waypoints");
        }

        public void ContinueMovingWaypoints()
        {
            if (teleportPoints == null || teleportPoints.Length == 0)
            {
                Debug.LogWarning("[PlayerController] No teleport points assigned");
                return;
            }

            currentWaypointIndex++;
            if (currentWaypointIndex >= teleportPoints.Length)
            {
                if (isLoopWaypoints)
                {
                    currentWaypointIndex = 0;
                }
                else
                {
                    currentWaypointIndex = teleportPoints.Length - 1;
                    isMovingWaypoints = false;
                    // Debug.Log($"[PlayerController] Reached end of waypoints, stopping");
                    return;
                }
            }

            _controller.enabled = false;
            transform.rotation = teleportPoints[currentWaypointIndex].rotation;
            _controller.enabled = true;
            // Debug.Log($"[PlayerController] Rotated to current waypoint {currentWaypointIndex} rotation: {teleportPoints[currentWaypointIndex].rotation.eulerAngles}");

            isMovingWaypoints = true;
            // Debug.Log($"[PlayerController] Continuing to waypoint: {currentWaypointIndex}");
        }


        private void MoveToWaypoint()
        {
            if (teleportPoints == null || teleportPoints.Length == 0)
            {
                isMovingWaypoints = false;
                return;
            }

            if (currentWaypointIndex >= teleportPoints.Length || currentWaypointIndex < 0)
            {
                isMovingWaypoints = false;
                return;
            }

            Vector3 targetPosition = teleportPoints[currentWaypointIndex].position;
            Vector3 targetPositionHorizontal = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            Vector3 direction = (targetPositionHorizontal - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, targetPositionHorizontal);

            if (distance <= waypointReachedDistance)
            {
                isMovingWaypoints = false;
                _controller.enabled = false;
                transform.position = targetPosition;
                transform.rotation = teleportPoints[currentWaypointIndex].rotation;
                _controller.enabled = true;
                _verticalVelocity = 0f; 
                GroundedCheck(); 
                // Debug.Log($"[PlayerController] Reached waypoint: {currentWaypointIndex}, stopping movement");
                return;
            }

            float targetSpeed = MoveSpeed;
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            _targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            Vector3 moveDirection = direction * (_speed * Time.deltaTime);
            _controller.Move(moveDirection + new Vector3(0.0f, Mathf.Clamp(_verticalVelocity, -5f, 5f), 0.0f) * Time.deltaTime);

            if (transform.position.y < -10f)
            {
                isMovingWaypoints = false;
                transform.position = new Vector3(0f, 0f, 0f);
            }

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, 1f);
            }

            //// Thêm hiệu ứng tiếng bước chân
            //if (Grounded && Time.time >= _nextFootstepTime && _speed > 0.1f)
            //{
            //    int index = Random.Range(0, FootstepAudioClips.Length);
            //    if (FootstepAudioClips.Length > 0 && FootstepAudioClips[index] != null)
            //    {
            //        AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            //        _nextFootstepTime = Time.time + FootstepInterval / (_speed / MoveSpeed);
            //    }
            //}
        }


        private void JumpAndGravity()
		{
			if (Grounded)
			{
				_fallTimeoutDelta = FallTimeout;

				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				if (_jump && _jumpTimeoutDelta <= 0.0f)
				{
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				_jumpTimeoutDelta = JumpTimeout;

				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

			}

			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}
	}
}