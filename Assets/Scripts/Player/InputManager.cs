using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class InputManager : MonoBehaviour
	{
		public Action<int> InvenotyKeyPress;

		PlayerInput _playerInput;

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool Grab;
		public bool LeftClick;
		public bool RightClick;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        void Awake()
        {
			_playerInput = GetComponent<PlayerInput>();
		}

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnMouseLeftInput(InputValue value)
		{
			LeftClick = value.isPressed;
		}

		public void OnMouseRightInput(InputValue value)
		{
			RightClick = value.isPressed;
		}

		public void OnInventory(InputValue value)
        {
			InvenotyKeyPress?.Invoke((int)(float)value.Get());
		}

		public void OnGrab(InputValue value)
		{
			Grab = value.isPressed;
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		public void TrySetupInputState(bool state)
        {
			if(_playerInput.enabled != state)
            {
				_playerInput.enabled = state;
			}
		}
	}
	
}