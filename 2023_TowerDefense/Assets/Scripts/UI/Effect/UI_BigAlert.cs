using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BigAlert : UI_Base
{
    enum Images
    {
        BG,
        AlertImage
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        return true;
    }

    Action _onDestroyAction = null;

    public void SetInfo(string path, float duration, Action evt)
    {
        _onDestroyAction += evt;
        GetImage((int)Images.AlertImage).sprite = Managers.Resource.Load<Sprite>($"Sprites/{path}");
        StartCoroutine(CoFade(duration));
    }

    private void OnDestroy()
    {
        if (_onDestroyAction != null)
            _onDestroyAction.Invoke();
    }
    IEnumerator CoFade(float duration)
    {
        Color color = GetImage((int)Images.AlertImage).color;
        float a = color.a;
        float f_time = duration;
        float curTime = 0f;

        while (color.a > 0.25f)
        {
            curTime += Time.unscaledDeltaTime / f_time;
            color.a = Mathf.Lerp(a, 0.25f, curTime);
            GetImage((int)Images.BG).color = color;

            Color alertColor = GetImage((int)Images.AlertImage).color;
            alertColor.a = color.a;
            GetImage((int)Images.AlertImage).color = alertColor;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        Managers.Resource.Destory(gameObject);
    }
}
