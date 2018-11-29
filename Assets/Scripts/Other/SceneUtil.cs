using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WongJJ.Game.Core;
using WongJJ.Game.Core.StrangeExtensions;

public class SceneUtil : KeepSingletion<SceneUtil>
{
    void Awake()
    {
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.isLoaded && scene.buildIndex == 2)
        {
            iocViewManager.OpenView((int)UiId.Login, Vector2.zero, false);
        }
        else if (scene.isLoaded && scene.buildIndex == 3)
        {
            iocViewManager.OpenView((int)UiId.Preach, Vector2.zero, false);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
    }
}
