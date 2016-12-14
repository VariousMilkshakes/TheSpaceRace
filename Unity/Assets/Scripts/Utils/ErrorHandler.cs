using System;
using System.Collections;
using System.Collections.Generic;
using SpaceRace.PlayerTools;
using SpaceRace.World.Buildings;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{

    public GameObject ErrorDisplay;
    private Text errorMessage;

    private int lifetime;
    private int fadeTick;

    public void Handle(BuildingException e)
    {
        hideError();
        string errorString = e.ToString() + "\nBuilding: " + e.Building.Name;
        errorMessage.text = errorString;
        displayError();
    }

    public void Handle(PlayerException e)
    {
        hideError();
        string errorString = e.Message;
        errorMessage.text = errorString;
        displayError();
    }

    private void displayError()
    {
        errorMessage.CrossFadeAlpha(1f, 0f, false);
        fadeTick = 30*lifetime;
    }

    private void hideError()
    {
        errorMessage.CrossFadeAlpha(0f, 0f, false);
    }

	// Use this for initialization
	void Start ()
	{
	    errorMessage = ErrorDisplay.GetComponent<Text>();
	    lifetime = 2;
	    fadeTick = -1;
        hideError();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    switch (fadeTick)
	    {
	        case 0:
	            fadeTick = -1;

	            errorMessage.CrossFadeAlpha(0f, 1f, false);
	            break;
            case -1:
                break;
            default:
	            fadeTick--;
	            break;
	    }
	}
}
