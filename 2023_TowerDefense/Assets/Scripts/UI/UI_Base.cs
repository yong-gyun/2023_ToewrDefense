using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    bool _init;

    private void Awake()
    {
        Init();
    }

    protected virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < objects.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = gameObject.FindChild(names[i], true);
            else
                objects[i] = gameObject.FindChild<T>(names[i], true);

            if (objects[i] == null)
                Debug.Log($"Bind Faild {names[i]}");
        }
    }

    protected void BindText(Type type) { Bind<Text>(type); }
    protected void BindSlider(Type type) { Bind<Slider>(type); }
    protected void BindScrollbar(Type type) { Bind<Scrollbar>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindInputField(Type type) { Bind<InputField>(type); }

    protected T Get<T>(int idx) where T : UnityEngine.Object    
    {
        UnityEngine.Object[] objects = null;

        if (_objects.TryGetValue(typeof(T), out objects))
            return objects[idx] as T;

        return null;
    }

    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }
    protected Scrollbar GetScrollbar(int idx) { return Get<Scrollbar>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected InputField GetInputField(int idx) { return Get<InputField>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> evt, Define.UIEvent type)
    {
        UI_EventHandler evtHandler = go.GetOrAddComponent<UI_EventHandler>();

        switch(type)
        {
            case Define.UIEvent.Click:
                evtHandler.OnPointerClickAction -= evt;
                evtHandler.OnPointerClickAction += evt;
                break;
            case Define.UIEvent.PointerEnter:
                evtHandler.OnPointerEnterAction -= evt;
                evtHandler.OnPointerEnterAction += evt;
                break;
            case Define.UIEvent.PointerExit:
                evtHandler.OnPointerExitAction -= evt;
                evtHandler.OnPointerExitAction += evt;
                break;
        }
    }
}
