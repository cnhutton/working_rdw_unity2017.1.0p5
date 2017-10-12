using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneSwitcher : MonoBehaviour
{

    private int _index = 0;

    public void LoadNextScene()
    {
        if (_index >= SceneManager.sceneCountInBuildSettings) return;
        SceneManager.LoadSceneAsync(_index + 1);
        
        ++_index;
        Debug.LogError("Loaded: " + _index);
    }

    public int CurrentSceneIndex()
    {
        return _index;
    }

    public void LoadNextScene(SceneName name)
    {
        switch (name)
        {
            case SceneName.Zero:
                SceneManager.LoadSceneAsync("0 Preload");
                break;
            case SceneName.One:
                SceneManager.LoadSceneAsync("1 Intro");
                break;
            case SceneName.Two:
                SceneManager.LoadSceneAsync("2 RDW Training");
                break;
            case SceneName.Three:
                SceneManager.LoadSceneAsync("3 Calibration Training");
                break;
            case SceneName.Four:
                SceneManager.LoadSceneAsync("4 Calibration");
                break;
            case SceneName.Five:
                SceneManager.LoadSceneAsync("5 Walkthrough");
                break;
            default:
                throw new ArgumentOutOfRangeException("name", name, null);
        }
    }

}
