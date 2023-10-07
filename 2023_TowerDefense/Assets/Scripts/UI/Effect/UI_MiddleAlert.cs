using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MiddleAlert : UI_Base
{
    enum Texts
    {
        AlertText
    }

    enum Images
    {
        Image
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        return true;
    }

    public void SetAlert(string alert, float duration)
    {
        GetText((int)Texts.AlertText).text = alert;
        StartCoroutine(CoFade(duration));
    }

    IEnumerator CoFade(float duration)
    {
        Color color = GetImage((int)Images.Image).color;
        float a = color.a;
        float f_time = duration;
        float curTime = 0f;

        while(color.a > 0.25f)
        {
            curTime += Time.deltaTime / f_time;
            color.a = Mathf.Lerp(a, 0.25f, curTime);
            GetImage((int)Images.Image).color = color;

            Color textColor = GetText((int)Texts.AlertText).color;
            textColor.a = color.a;
            GetText((int)Texts.AlertText).color = textColor;
            yield return null;
        }

        Managers.Resource.Destory(gameObject);
    }
}