
//scene 3
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManageMultiplayer : MonoBehaviour {
    
    public InputField playerIdInputField;
    public Button nextButton;


   // public MapGenerator mapGenerator;
    public GameOptions gameOptions;


    void OnEnable()
    {
        gameOptions = new GameOptions();


        playerIdInputField.onValueChanged.AddListener(delegate { OnPlayerIdChange(); });
        nextButton.onClick.AddListener(delegate { OnNextClickEvent(); });
    } 
    
    public void OnPlayerIdChange() //allows users to input the player ID 
    {

    }
    public void OnNextClickEvent() //takes player back to option menu upon clicking the button. 
    {
         
    }
    


}
