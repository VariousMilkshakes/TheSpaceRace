using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scene1_OptionMenu : MonoBehaviour {

    
    public Button worldSeedButton;
    public Slider waterCoverageSlider;
    public Button playerSettingButton;
    public Button backButton;
    
    


        //public Resolution resolutions;
        public MapGenerator mapGenerator;
        public Scene1_OptionMenu optionMenu;


        void OnEnable()
        {
            optionMenu = new Scene1_OptionMenu();




            worldSeedButton.onClick.AddListener(delegate { OnWorldSeedClickEvent(); });
            waterCoverageSlider.onValueChanged.AddListener(delegate { OnWaterCoverageChange(); });
            playerSettingButton.onClick.AddListener(delegate { OnPlayerSettingClickEvent(); });
            backButton.onClick.AddListener(delegate { OnBackClickEvent(); });



            //  mapGenerator = Screen.;

        }

        //GameOptions.SetMapHeight(int height) { 
        //  MapGenerator.SetMapWidth (String 



        
        public void OnWorldSeedClickEvent() //allows the user to enter the world seed
        {

        }
        public void OnWaterCoverageChange() //user able to input the water level depending on the coverage they want. 
        {

        }
        public void OnPlayerSettingClickEvent()//click on event which will bring out multiplayer menu upon clicking the button
        {

        }
        public void OnBackClickEvent() //user is taken to the game
        {
          
        
        }
    }

