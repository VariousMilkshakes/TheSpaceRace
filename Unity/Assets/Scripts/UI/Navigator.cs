using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Navigator : MonoBehaviour{

    public string TargetScene;

    public void MoveScene()
    {
        if (TargetScene != null)
        {
            SceneManager.LoadScene(TargetScene, LoadSceneMode.Single);
        }
    }

    public void AddScene()
    {
        if (TargetScene != null)
        {
            SceneManager.LoadScene(TargetScene, LoadSceneMode.Additive);
        }
    }

    public void RemoveCanvasHandler ()
    {
        string handler = TargetScene + "Handler";
        string canvas = TargetScene + "Canvas";

        Destroy(GameObject.Find(handler));
        Destroy(GameObject.Find(canvas));
    }

    public void RemoveScene ()
    {
        RemoveScene(SceneManager.GetActiveScene());
    }

    public void RemoveScene (string scene)
    {
        RemoveScene(SceneManager.GetSceneByName(scene));
    }

    public void RemoveScene (Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

}