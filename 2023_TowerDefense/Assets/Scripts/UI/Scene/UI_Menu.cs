using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Menu : UI_Scene
{
    enum Buttons
    {
        StartButton,
        IntroButton,
        ManualButton,
        RankButton,
        ExitButton
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnClickStartButton, Define.UIEvent.Click);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton, Define.UIEvent.Click);
        GetButton((int)Buttons.RankButton).gameObject.BindEvent(OnClickRankButton, Define.UIEvent.Click);
        //GetButton((int)Buttons.IntroButton).gameObject.BindEvent(OnClickIntroButton, Define.UIEvent.Click);
        //GetButton((int)Buttons.ManualButton).gameObject.BindEvent(OnClickManualButton, Define.UIEvent.Click);
        return true;
    }

    void OnClickStartButton(PointerEventData evtData)
    {
        Managers.Sound.Play("Interaction/ButtonClick");
        Managers.Game.CurrentTime = 0;
        Managers.Game.CurrentGold = 100;
        Managers.Game.CurrentScore = 0;
        Managers.Scene.LoadAsync(Define.SceneType.Stage1, () => { Managers.UI.MakeEffectUI<UI_Fade>().Enter(true, 0.5f); });
    }

    void OnClickIntroButton(PointerEventData evtData)
    {
        Managers.Sound.Play("Interaction/ButtonClick");

    }

    void OnClickManualButton(PointerEventData evtData) 
    {
        Managers.Sound.Play("Interaction/ButtonClick");
    }

    void OnClickRankButton(PointerEventData evtData)
    {
        Managers.Sound.Play("Interaction/ButtonClick");
        Managers.UI.ShowPopupUI<UI_RankPopup>();
    }

    void OnClickExitButton(PointerEventData evtData)
    {
        Managers.Sound.Play("Interaction/ButtonClick");
        Application.Quit();
    }
}