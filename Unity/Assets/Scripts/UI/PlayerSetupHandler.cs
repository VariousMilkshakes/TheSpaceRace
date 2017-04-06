using System.Collections;
using System.Collections.Generic;
using SpaceRace.Game;
using SpaceRace.PlayerTools;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupHandler : MonoBehaviour
{

    public GameObject PlayerGameObject;
    public GameObject PlayerAdderGameObject;
    public Text NewPlayerNameText;
    public Slider ColourSlider;

    private List<GameObject> playerBoxes;

	// Use this for initialization
	void Start () {
		
        playerBoxes = new List<GameObject>();

	}

    public void AddPlayer ()
    {
        int hueValue = (int)ColourSlider.value;
        string playerName = NewPlayerNameText.text;

        Color playerColor = Color.HSVToRGB((float)hueValue / 365f, 1, 1);
        Player newPlayer = new Player(playerName, playerColor);

        Vector3 pos = PlayerAdderGameObject.transform.position;

        GameObject newPlayerBox = Instantiate(PlayerGameObject, pos, Quaternion.identity);
        newPlayerBox.transform.SetParent(PlayerAdderGameObject.transform.parent);

        pos.y -= 100;
        if (GameManager.PLAYERS.Count % 3 == 0 && GameManager.PLAYERS.Count != 0) {
            pos.y = 390;
            pos.x += 230;
        }

        PlayerAdderGameObject.transform.position = pos;

        GameManager.PLAYERS.Add(newPlayer);
        setupPlayerBox(newPlayerBox, newPlayer);
    }

    private void setupPlayerBox (GameObject playerBox, Player newPlayer)
    {
        playerBox.GetComponentInChildren<Text>()
                 .text = newPlayer.Name;
        playerBox.GetComponentsInChildren<Image>()[1]
                 .color = newPlayer.Color;
        playerBox.GetComponentInChildren<Button>()
                 .onClick
                 .AddListener(delegate
                              {
                                  GameObject parent = playerBox;
                                  Player targetPlayer = newPlayer;
                                  GameManager.PLAYERS.Remove(targetPlayer);
                                  Destroy(parent);
                              });
    }

}
