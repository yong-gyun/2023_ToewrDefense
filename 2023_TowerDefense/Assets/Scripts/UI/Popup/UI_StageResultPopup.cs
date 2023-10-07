using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageResultPopup : UI_Popup
{
    enum Images
    {
        ResultImage
    }

    enum Texts
    {
        ScoreText,
        TimeText,
    }

    enum Buttons
    {
        ConfirmButton
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetText((int)Texts.ScoreText).text = $"점수 : {Managers.Game.CurrentScore}";
        int min = (int)Managers.Game.CurrentTime / 60;
        int sec = (int)Managers.Game.CurrentTime % 60;
        GetText((int)Texts.TimeText).text = $"진행 시간 : {string.Format("{0:00}:{1:00}", min, sec)}";
        
        if(Managers.Game.IsStageClear == false)
        {
            GetImage((int)Images.ResultImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Defeat");
            GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent((evt) => { Time.timeScale = 1f; Managers.Scene.LoadAsync(Define.SceneType.InputRank,
                () => { Managers.UI.MakeEffectUI<UI_Fade>().Enter(true, 0.5f); }); }, 
                Define.UIEvent.Click);
        }
        else
        {
            GetImage((int)Images.ResultImage).sprite = Managers.Resource.Load<Sprite>("Sprites/Victory");

            if (Managers.Game.CurrentStage == 2)
                GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent((evt) => { Time.timeScale = 1f; Managers.Scene.LoadAsync(Define.SceneType.InputRank); }, Define.UIEvent.Click);
            else
                GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent((evt) => { Time.timeScale = 1f; Managers.Scene.LoadAsync(Define.SceneType.Stage2, () => { 
                Managers.UI.MakeEffectUI<UI_Fade>().Enter(true, 0.5f); }); }, Define.UIEvent.Click);
        }

        return true;
    }
}