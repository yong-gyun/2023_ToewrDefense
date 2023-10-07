using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene 
    { 
        get 
        {
            if (_currentScene == null)
                _currentScene = GameObject.FindObjectOfType<BaseScene>();
            return _currentScene; 
        } 
    }
    BaseScene _currentScene;

    public Define.SceneType CurrentSceneType { get { return CurrentScene.SceneType; } }

    public void Load(Define.SceneType type)
    {
        SceneManager.LoadScene(GetSceneName(type));
        Managers.Clear();
    }

    public void LoadAsync(Define.SceneType type, Action completed = null)
    {
        Managers.Clear();
        UI_Fade fade = Managers.UI.MakeEffectUI<UI_Fade>();
        fade.OnExitAction += () => 
        { 
            AsyncOperation op = SceneManager.LoadSceneAsync(GetSceneName(type));
            op.completed += (operation) => 
            {
                if (completed != null)
                    completed.Invoke();
            };
        };
        fade.Enter(false, 0.25f);
    }

    string GetSceneName(Define.SceneType type)
    {
        string name = Enum.GetName(typeof(Define.SceneType), type);
        char[] chars = name.ToCharArray();
        char.ToUpper(chars[0]);
        return new string(chars);
    }
}
