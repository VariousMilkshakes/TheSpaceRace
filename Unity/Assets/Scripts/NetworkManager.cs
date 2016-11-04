using UnityEngine;
using System.Collections;

// Code sourced from multiplayer tutorial.
// multiplayer tutorial: http://www.paladinstudios.com/2013/07/10/how-to-create-an-online-multiplayer-game-with-unity/

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "UniqueGameName";
	private const string gameName = "RoomName";

	private bool isRefreshingHostList = false;
	private HostData[] hostList;

	public GameObject playerPrefab;

	// adding the buttons to the GUI
	void OnGUI() 
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();

			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();

			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	//Starting server with 5 players (max), on a port of 25000, and checking if the machine has a public address
	// the server is then registered to the master server, a server held by unity.
	private void StartServer()
	{
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	// when a server is initialized, a player is automatically spawned.
	void OnServerInitialized()
	{
		SpawnPlayer();
	}

	// updating the host list
	void Update()
	{
		if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
		{
			isRefreshingHostList = false;
			hostList = MasterServer.PollHostList();
		}
	}

	//requesting the host list
	private void RefreshHostList()
	{
		if (!isRefreshingHostList)
		{
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);
		}
	}

	// code to join the server once the server is available.
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	// if a server is joined, a player is spawned using the SpawnPlayer method.
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}

	// method used to instantiate a player.
	private void SpawnPlayer()
	{
		Network.Instantiate(playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
	}
}
