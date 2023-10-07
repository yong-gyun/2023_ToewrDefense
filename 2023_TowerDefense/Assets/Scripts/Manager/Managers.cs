using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers Instance { get { Init(); return s_instance; } }
    static Managers s_instance;

    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    GameManager _game = new GameManager();
    ObjectManager _object = new ObjectManager();
    DataManager _data = new DataManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();

    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static GameManager Game { get {  return Instance._game; } }
    public static ObjectManager Object { get { return Instance._object; } }
    public static DataManager Data { get {  return Instance._data; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }

    static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if(go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<Managers>();
            }

            s_instance = go.GetComponent<Managers>();
            s_instance._data.Init();
            s_instance._game.Init();
            s_instance._sound.Init();
            DontDestroyOnLoad(go);
        }
    }

    private void OnApplicationQuit()
    {
        Data.SaveData();
    }

    public static void Clear()
    {
        UI.Clear();
        Game.Clear();
        Object.Clear();
        Sound.Clear();
    }
}
