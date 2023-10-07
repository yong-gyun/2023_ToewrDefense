using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fade : UI_Base
{
    public Action OnEnterAction = null;
    public Action OnExitAction = null;
    
    enum Images
    {
        Image
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        return true;
    }

    public void Enter(bool isFadeOut, float duration)
    {
        StartCoroutine(CoFade(isFadeOut, duration));
    }

    IEnumerator CoFade(bool isFadeOut, float duration)
    {
        if (OnEnterAction != null)
            OnEnterAction.Invoke();

        if(isFadeOut)
        {
            Color color = GetImage((int)Images.Image).color;
            color.a = 1f;
            float t = 0f;
            while (color.a > 0f)
            {
                t += Time.deltaTime / duration;
                float alpha = Mathf.Lerp(1f, 0f, t);
                color.a = alpha;
                GetImage((int)Images.Image).color = color;
                yield return null;
            }

            if (OnExitAction != null)
                OnExitAction.Invoke();
            Managers.Resource.Destory(gameObject);
        }
        else
        {
            Color color = GetImage((int)Images.Image).color;
            color.a = 0f;
            float t = 0f;
            while(color.a < 1f)
            {
                t += Time.deltaTime / duration;
                float alpha = Mathf.Lerp(0f, 1f, t);
                color.a = alpha;
                GetImage((int)Images.Image).color = color;
                yield return null;
            }

            if (OnExitAction != null)
                OnExitAction.Invoke();
        }

    }
}
