using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManageMultiplayer : MonoBehaviour {
    public InputField numberOfPlayerInputField;
    public InputField userIdInputField;
    public Button closeButton;


   // public MapGenerator mapGenerator;
    public GameOptions gameOptions;


    void OnEnable()
    {
        gameOptions = new GameOptions();


        numberOfPlayerInputField.onValueChanged.AddListener(delegate { OnNumberOfPlayerChange(); });
        userIdInputField.onValueChanged.AddListener(delegate { OnUserIdChange(); });
        closeButton.onClick.AddListener(delegate { OnCloseClickEvent(); });
    } 
    public void OnNumberOfPlayerChange() //allows user to input number of player
                                         //they want to play with
    {

    }
    public void OnUserIdChange() //allows users to input the player ID 
    {

    }
    public void OnCloseClickEvent() //takes player back to option menu upon clicking the button. 
    {
         
    }
    


}
