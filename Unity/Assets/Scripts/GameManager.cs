using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceRace.PlayerTools;
using SpaceRace.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceRace
{
    class GameManager
    {
        public static GameManager MANAGER
        {
            get { return MANAGER; }
            set
            {
                if (MANAGER == null) {
                    MANAGER = value;
                }
            }
        }

        public static void RUN_INSTRUCTION (String command)
        {
            new InlineScript(command);
        }

        public static Dictionary<String, Delegate> INSTRUCTION_SET = new Dictionary<string, Delegate>();

        private Game currentGame;

        private GameManager()
        {
            // Get game modifier commands
            GetType().GetMethods()
                     .ToList()
                     .ForEach(e =>
                     INSTRUCTION_SET
                     .Add(e.Name, Delegate
                     .CreateDelegate(e.GetType(), e)));

            SceneManager.LoadScene("Main");
            List<GameObject> goList = SceneManager.GetActiveScene()
                                           .GetRootGameObjects()
                                           .ToList();

            foreach (GameObject go in goList) {
                if (go.tag == "GameController") {
                    currentGame = go.GetComponent<Game>();
                }
            }
        }

        private void addResource (Resource resource, int volume)
        {
            Player p = currentGame.GetActivePlayer();
            p.Inventory.AddResource(resource, volume);
        }
    }
}
