using System.Collections;
using System.Collections.Generic;
using SpaceRace.Game;
using UnityEngine;
using UnityEngine.UI;

public class Winner : MonoBehaviour
{

    public Text GameWinner;

    public void Start()
    {
        GameWinner.text = GameManager.Winner.Name;
    }
}
