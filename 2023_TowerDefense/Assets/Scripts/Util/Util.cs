using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }
    
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if(recursive)
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                T component = transform.GetComponent<T>();

                if(component != null)
                {
                    if (string.IsNullOrEmpty(name) || component.name == name)
                        return component;
                }
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        if (go == null)
            return null;

        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static float GetDistance(float a)
    {
        return a * Define.TILE_SIZE;
    }

    public static T GetShortestDistance<T>(GameObject from, List<T> objects) where T : MonoBehaviour
    {
        if (objects.Count == 0)
            return null;

        float maxDinstance = Mathf.Infinity;
        T target = null;

        foreach (T item in objects)
        {
            Vector3 interval = item.transform.position - from.transform.position;
            interval.y = 0f;
            float distance = interval.magnitude;

            if(distance < maxDinstance)
            {
                target = item;
                maxDinstance = distance;
            }
        }

        return target;
    }
}
