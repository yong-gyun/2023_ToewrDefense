using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Minimap : UI_Base
{
    enum RawImages
    {
        RawImage
    }

    Camera _miniMapCam;

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        _miniMapCam = GameObject.Find("@MinimapCamera").GetComponent<Camera>();
        Bind<RawImage>(typeof(RawImages));
        Get<RawImage>((int)RawImages.RawImage).gameObject.BindEvent(OnClickMinimap, Define.UIEvent.Click);
        return true;
    }

    void OnClickMinimap(PointerEventData evt)
    {
        RawImage rawImage = Get<RawImage>((int)RawImages.RawImage);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImage.rectTransform, evt.position, null, out localPoint);
        localPoint.x = (rawImage.rectTransform.rect.xMin * -1) - (localPoint.x * -1);
        localPoint.y = (rawImage.rectTransform.rect.yMin * -1) - (localPoint.y * -1);

        Vector2 viewPort = new Vector2(localPoint.x / rawImage.rectTransform.rect.size.x, localPoint.y / rawImage.rectTransform.rect.size.y);
        Ray ray = _miniMapCam.ViewportPointToRay(viewPort);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Camera.main.transform.position = new Vector3(hit.point.x, Camera.main.transform.position.y, hit.point.z);
        }
    }
}
