using UnityEngine;
using System.Collections;

namespace SpaceRace.PlayerTools
{
	public partial class Player : MonoBehaviour
	{
		public float speed = 10f;

		private float lastSynchronizationTime = 0f;
		private float syncDelay = 0f;
		private float syncTime = 0f;
		private Vector3 syncStartPosition = Vector3.zero;
		private Vector3 syncEndPosition = Vector3.zero;

		void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
		{
			Vector3 syncPosition = Vector3.zero;
			Vector3 syncVelocity = Vector3.zero;
			if (stream.isWriting)
			{
				syncPosition = GetComponent<Rigidbody>().position;
				stream.Serialize(ref syncPosition);

				syncPosition = GetComponent<Rigidbody>().velocity;
				stream.Serialize(ref syncVelocity);
			}
			else
			{
				stream.Serialize(ref syncPosition);
				stream.Serialize(ref syncVelocity);

				syncTime = 0f;
				syncDelay = Time.time - lastSynchronizationTime;
				lastSynchronizationTime = Time.time;

				syncEndPosition = syncPosition + syncVelocity * syncDelay;
				syncStartPosition = GetComponent<Rigidbody>().position;
			}
		}

		void Awake()
		{
			lastSynchronizationTime = Time.time;
		}

		void Update()
		{
			if (GetComponent<NetworkView>().isMine)
			{
				InputMovement();
				InputColorChange();
			}
			else
			{
				SyncedMovement();
			}
		}

		// movement for the player.
		private void InputMovement()
		{
			if (Input.GetKey(KeyCode.W))
				GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + Vector3.forward * speed * Time.deltaTime);

			if (Input.GetKey(KeyCode.S))
				GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position - Vector3.forward * speed * Time.deltaTime);

			if (Input.GetKey(KeyCode.D))
				GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + Vector3.right * speed * Time.deltaTime);

			if (Input.GetKey(KeyCode.A))
				GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position - Vector3.right * speed * Time.deltaTime);
		}

		//code for syncing the player
		private void SyncedMovement()
		{
			syncTime += Time.deltaTime;

			GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
		}


		private void InputColorChange()
		{
			if (Input.GetKeyDown(KeyCode.R))
				ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
		}

		[RPC]
		void ChangeColorTo(Vector3 color)
		{
			GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z, 1f);

			if (GetComponent<NetworkView>().isMine)
				GetComponent<NetworkView>().RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
		}
	}

}