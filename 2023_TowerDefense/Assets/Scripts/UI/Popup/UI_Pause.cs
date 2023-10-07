using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Pause : UI_Popup
{
    enum Buttons
    {
        ContinueButton,
        ExitButton
    }

    float _curerntTimeScale = 1f;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _curerntTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        BindButton(typeof(Buttons));
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton, Define.UIEvent.Click);
        return true;
    }

    void OnClickContinueButton(PointerEventData evtData)
    {
        Managers.Sound.Play("Interaction/ButtonClick");
        Time.timeScale = _curerntTimeScale;
        ClosePopupUI();
    }

    void OnClickExitButton(PointerEventData evtData)
    {
        Managers.Sound.Play("Interaction/ButtonClick");
        Time.timeScale = 1f;
        Managers.Scene.LoadAsync(Define.SceneType.Menu);
    }
}
