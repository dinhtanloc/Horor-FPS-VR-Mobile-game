using System;
using UnityEngine;
using Alteruna;
using UnityEngine.InputSystem;

namespace VRGameMobile
{
	
	// Class split into multiple files for clarity

	[RequireComponent(typeof(CharacterController), typeof(InputSynchronizable), typeof(Health))]
	public partial class PlayerController : Synchronizable
	{
        private void Awake()
        {
			_controller = GetComponent<CharacterController>();
            isMovingWaypoints = false;
        }

        private void Start()
		{
            currentWaypointIndex = -1;
            InitializeNetworking();
			InitializeGun();
			InitialiseAnimations();
			InitializeInput();
			//AutoMoveToNextTeleportPoint();
            //Debug.Log($"[START] HasAnimator = {_hasAnimator}");

        }

        private new void OnEnable()
		{
			base.OnEnable();

			ResetAmmo();
			Commit();

			if (_isOwner && _possesed)
			{
				CinemachineVirtualCameraInstance.Instance.Follow(_cameraTarget);
				CinemachineVirtualCameraInstance.Instance.gameObject.SetActive(true);
			}
		}

		// called when the local player is possessed by a client
		// Called from the PlayerController.Networking
		private void OnPossession()
		{
			InitializeCamera();
			InitializeHealth();
		}

		private void OnDisable()
		{
			Commit();
			Sync();
		}

		private void Update()
		{
            
            JumpAndGravity();
			GroundedCheck();
            //AutoMove();
            LogicMove();
			
        }

		private void LateUpdate()
		{
            bool lockInput = LockCameraPosition || (MenuInstance.Instance != null && MenuInstance.Instance.IsMenuOpen);
            CameraRotation(lockInput);
			GunAction(lockInput);

			Sync();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Grounded ? 
				new Color(0.0f, 1.0f, 0.0f, 0.35f) : 
				new Color(1.0f, 0.0f, 0.0f, 0.35f);

			Vector3 pos = transform.position;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(
				new Vector3(pos.x, pos.y - GroundedOffset, pos.z),
				GroundedRadius);

            if (GunAimTarget != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(GunAimTarget.position, 0.2f);
            }
        }
	}
}