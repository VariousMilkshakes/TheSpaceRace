using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {
    public InputField mapHeightInputField;
    public InputField mapWidthtInputField;
    public InputField worldSeedInputField;
    public Slider waterCoverageSlider;
    

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
   

}
//going to handle the logic/the UI of the options