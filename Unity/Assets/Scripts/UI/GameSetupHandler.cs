using System;
using System.Collections;
using System.Collections.Generic;
using SpaceRace.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetupHandler : MonoBehaviour
{

    private static readonly string ERR_INT_DIMENSIONS = "Map dimensions must be whole numbers";
    private static readonly string ERR_DIMENSION_SIZE = "Map dimensions must be between 20x20 - 120x120";
    private static readonly string ERR_NO_PLAYERS = "At least 1 player must be added";
    private static readonly string ERR_GOOD = "Ready to play";

    public Text MapHeightText;
    public Text MapWidthText;
    public Text MapSeedText;
    public Slider WaterCoverageSlider;
    public Text WaterCoverageText;
    public Text ErrorMessageText;

    private int mapHeight;
    private int mapWidth;
    private string mapSeed;
    private int waterCoverage;

    private bool ready = true;

    // Use this for initialization
    void Start () {}

    // Update is called once per frame
    void Update ()
    {

        ready = true;

        // Get user values
        try {
            mapHeight = Int32.Parse(MapHeightText.text);
            mapWidth = Int32.Parse(MapWidthText.text);
        } catch (Exception) {
            showError(ERR_INT_DIMENSIONS);
            ready = false;
        }

        if (GameManager.PLAYERS.Count == 0) {
            showError(ERR_NO_PLAYERS);
            ready = false;
        }

        mapSeed = MapSeedText.text;

        int dimCheck = mapHeight * mapWidth;
        if (dimCheck < 20 * 20 || dimCheck > 120 * 120) {
            showError(ERR_DIMENSION_SIZE);
            ready = false;
        }

	    waterCoverage = (int)WaterCoverageSlider.value;

	    WaterCoverageText.text = waterCoverage + "%";

        if (ready) showError(ERR_GOOD);

	}

    public void StartGame ()
    {
        if (ready) {

            GameManager.MAP_HEIGHT = mapHeight;
            GameManager.MAP_WIDTH = mapWidth;
            GameManager.MAP_WATER = waterCoverage;

            SceneManager.LoadScene("Main");

        }
    }

    private void showError (string error)
    {
        ErrorMessageText.text = error;
    }
}
