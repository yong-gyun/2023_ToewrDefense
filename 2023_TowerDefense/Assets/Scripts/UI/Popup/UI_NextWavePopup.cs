using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NextWavePopup : UI_Popup
{
    enum Texts
    {
        NextWaveCountdownText
    }

    enum Buttons
    {
        NextButton
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        StartCoroutine(CoCountdown());
        GetButton((int)Buttons.NextButton).gameObject.BindEvent((evt) =>
        {
            Managers.Sound.Play("Interaction/ButtonClick");
            if (Managers.Object.SpawnPool.IsStarted == false)
                Managers.Object.SpawnPool.OnStart();
            else
                Managers.Object.SpawnPool.OnChangeWave(++Managers.Game.CurrentWave);
            ClosePopupUI();
        }, Define.UIEvent.Click);
        return true;
    }

    IEnumerator CoCountdown()
    {
        int[] counts = new int[10] { 120, 22, 20, 18, 15, 12, 10, 8, 5, 3 };
        int count = counts[Managers.Game.CurrentWave];

        while(count > 0)
        {
            GetText((int)Texts.NextWaveCountdownText).text = $"웨이브 시작까지 {count}초";
            yield return new WaitForSeconds(1f);
            count--;
        }
        if (Managers.Object.SpawnPool.IsStarted == false)
            Managers.Object.SpawnPool.OnStart();
        else
            Managers.Object.SpawnPool.OnChangeWave(++Managers.Game.CurrentWave);
        ClosePopupUI();
    }
}
