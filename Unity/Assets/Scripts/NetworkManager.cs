using UnityEngine;
using System.Collections;

// Code sourced from multiplayer tutorial.
// multiplayer tutorial: http://www.paladinstudios.com/2013/07/10/how-to-create-an-online-multiplayer-game-with-unity/

public class NetworkManager : MonoBehaviour {

	private const string typeName = "SpaceRace";
	private const string gameName = "level1";
	private HostData[] hostList;

	private void StartServer () {
		Network.InitializeServer (8, 10000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (typeName, gameName);
	}

	void onServerInitialised () {
		Debug.Log ("Server has initialised");
	}



	private void RefreshHostList() {
		MasterServer.RequestHostList (typeName);
	}

	void OnMasterServerEvent (MasterServerEvent msEvent) {
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
	}

	private void JoinServer (HostData hostData) {
		Network.Connect (hostData);
	}

	void onConnectedToServer() {
		Debug.Log ("Server has been joined");
	}

	void onGUI() {
		if (!Network.isClient && !Network.isServer) {
			if (GUI.Button (new Rect (100, 100, 250, 100), "Create a server"))
				StartServer ();
			if (GUI.Button (new Rect (100, 250, 250, 100), "Refresh hosts"))
				RefreshHostList ();
			if (hostList != null) {
				for (int i = 0; i < hostList.Length; i++) {
					if (GUI.Button (new Rect (400, 100 + (100 * i), 300, 100), hostList [i].gameName))
						JoinServer (hostList [i]);
				}
			}
		}
	}
}
