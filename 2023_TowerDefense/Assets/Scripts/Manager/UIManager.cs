using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public UI_Scene SceneUI { get; private set; }
    int _order = 10;

    public void SetCanvas(GameObject go, bool sort)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        if(sort)
            canvas.sortingOrder = _order++;
        else
            canvas.sortingOrder = 0;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");

        if (go == null)
            return null;

        T popup = go.GetOrAddComponent<T>();
        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");

        if (go == null)
            return null;

        T sceneUI = go.GetOrAddComponent<T>();
        SceneUI = sceneUI;
        return sceneUI;
    }


    public T MakeSubitemUI<T>(Transform parent, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Subitem/{name}");

        if (go == null)
            return null;

        T subitem = go.GetOrAddComponent<T>();
        subitem.transform.SetParent(parent);
        return subitem;
    }

    public T MakeWorldSpcaeUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        
        T worldSpace = go.GetOrAddComponent<T>();
        worldSpace.transform.SetParent(parent);

        //if (parent == null)
        //    worldSpace.transform.SetParent(Managers.Scene.CurrentScene.transform);

        return worldSpace;
    }

    public T MakeEffectUI<T>(string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Effect/{name}");

        if (go == null)
            return null;

        SetCanvas(go, true);
        T effectUI = go.GetOrAddComponent<T>();
        return effectUI;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        Managers.Resource.Destory(popup.gameObject);
        _order--;
    }

    public void Clear()
    {
        SceneUI = null;
    }
}
