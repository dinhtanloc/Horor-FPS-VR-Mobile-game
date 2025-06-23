using System;
using UnityEngine;
using UnityEngine.Events;

namespace Alteruna
{
	/// <summary>
	/// Event used for when you want a something to only apply for a single client.
	/// </summary>
	/// <remarks>
	///	When on an avatar, the controller will be the avatar owner.
	/// Otherwise, the controller will be the client with the lowest user index.
	/// </remarks>
	public class SingleClientEvent : CommunicationBridge
	{
		/// <summary>
		/// Gets if the local client is the controller.
		/// </summary>
		public bool IsControlled { get; private set; }

		private bool _isAvatar;
		private bool _haveCalled;

		/// <summary>
		/// Runs when the controlling client is changed.
		/// </summary>
		/// <remarks>
		///	True when controlled on this client.
		/// </remarks>
		[Tooltip("Runs when the controlling client is changed. True when controlled on this client.")]
		public UnityEvent<bool> OnClientChanged = new UnityEvent<bool>();

		public void Start()
		{
			Multiplayer.OnRoomJoined.AddListener(OnRoomJoined);
			Multiplayer.OnOtherUserJoined.AddListener(OnOtherUserJoined);
			Multiplayer.OnOtherUserLeft.AddListener(OnOtherUserLeft);
			Multiplayer.OnRoomLeft.AddListener(OnRoomLeft);
		}

		public void OnDestroy()
		{
			Multiplayer.OnRoomJoined.RemoveListener(OnRoomJoined);
			Multiplayer.OnOtherUserJoined.RemoveListener(OnOtherUserJoined);
			Multiplayer.OnOtherUserLeft.RemoveListener(OnOtherUserLeft);
			Multiplayer.OnRoomLeft.RemoveListener(OnRoomLeft);
		}

		private void OnOtherUserLeft(Multiplayer arg0, User arg1) => UpdateEvent(arg0);
		private void OnOtherUserJoined(Multiplayer arg0, User arg1) => UpdateEvent(arg0);
		private void OnRoomLeft(Multiplayer arg0) => UpdateEvent(arg0);
		private void OnRoomJoined(Multiplayer arg0, Room arg1, User arg2) => UpdateEvent(arg0);


		public void UpdateEvent() => UpdateEvent(Multiplayer);

		public void UpdateEvent(Multiplayer m)
		{
			if (_isAvatar) return;
			bool isMe = m.LowestUserIndex == m.GetUser();
			if (_haveCalled && isMe == IsControlled) return;
			IsControlled = isMe;
			_haveCalled = true;
			OnClientChanged.Invoke(IsControlled);
		}

		public override void Possessed(bool isMe, User user)
		{
			if (_haveCalled && IsControlled == isMe) return;
			IsControlled = isMe;
			_isAvatar = true;
			_haveCalled = true;
			OnClientChanged.Invoke(IsControlled);
		}

		public override void Unpossessed()
		{
			if (IsControlled)
				OnClientChanged.Invoke(false);
			IsControlled = false;
		}
	}
}