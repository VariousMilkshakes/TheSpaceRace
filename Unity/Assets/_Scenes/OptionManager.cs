using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {
    public InputField mapHeightInputField;
    public InputField mapWidthtInputField;
    public InputField worldSeedInputField;
    public Slider waterCoverageSlider;
    public Button multiPlayerSettingButton;
    public Button backButton;
    public Button startGameButton;
    public Button closeButton;
    

    //public Resolution resolutions;
    public MapGenerator mapGenerator;
    public GameOptions gameOptions;
   

    void OnEnable()
    {
        gameOptions = new GameOptions();


        mapHeightInputField.onValueChanged.AddListener(delegate { OnMapHeightChange(); });
        mapWidthtInputField.onValueChanged.AddListener(delegate { OnMapWidthChange(); });
        worldSeedInputField.onValueChanged.AddListener(delegate { OnWorldSeedChange(); });
        waterCoverageSlider.onValueChanged.AddListener(delegate { OnWaterCoverageChange(); });
        multiPlayerSettingButton.onClick.AddListener(delegate { OnMultiPlayerSettingClickEvent(); });
        startGameButton.onClick.AddListener(delegate { OnStartGameClickEvent(); });
        backButton.onClick.AddListener(delegate { OnBackClickEvent(); });
        //startGameButton.onClick.AddListener(delegate { OnStartGameButton(); });
        closeButton.onClick.AddListener(delegate { OnCloseClickEvent(); });



        //  mapGenerator = Screen.;

    }

    //GameOptions.SetMapHeight(int height) { 
    //  MapGenerator.SetMapWidth (String 



    public void OnMapHeightChange()
    {
     //MapGenerator.setMap
    }
    public void OnMapWidthChange()
    {
        
    }
    public void OnWorldSeedChange()
    {
      
    }
    public void OnWaterCoverageChange()
    {

    }
    public void OnMultiPlayerSettingClickEvent()
    {

    }
    public void OnStartGameClickEvent()
    {
        //Button start = startGameButton.GetComponent<Button>();
        //start.onClick.AddListener(OnStartGameClick);
    }
    public void OnCloseClickEvent()
    {

    }
    public void OnBackClickEvent()
    {

    }

}
//going to handle the logic/the UI of the options