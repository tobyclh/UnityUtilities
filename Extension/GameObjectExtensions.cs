using System;
using UnityEngine;
using System.Linq;

public static class GameObjectExtensions
{

    public static Vector3 GetNormailizedLocalPosition(this Transform trans)
    {
        if(trans.parent == null)
        {
            throw new SystemException("transform does not have parent");
        }
        var rectTrans = trans.parent.GetComponent<RectTransform>();
        if (rectTrans == null)
        {
            throw new SystemException("transform parent does not have recttransform");
        }
        return new Vector3(trans.localPosition.x / (rectTrans.rect.width * 2), trans.localPosition.y / (rectTrans.rect.height * 2));
    }

    public static Vector3 GetAbsoluteLocalPosition(this Transform trans, Vector3 normalPos)
    {
        if (trans.parent == null)
        {
            throw new SystemException("transform does not have parent");
        }
        var rectTrans = trans.parent.GetComponent<RectTransform>();
        if (rectTrans == null)
        {
            throw new SystemException("transform parent does not have recttransform");
        }
        return new Vector3(normalPos.x * (rectTrans.rect.width / 2), normalPos.y * (rectTrans.rect.height / 2));
    }

    /// <summary>
    /// Returns all monobehaviours (casted to T)
    /// </summary>
    /// <typeparam name="T">interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfaces<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        var mObjs = gObj.GetComponents<MonoBehaviour>();

        return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
    }

    /// <summary>
    /// Returns the first monobehaviour that is of the interface type (casted to T)
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterface<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        return gObj.GetInterfaces<T>().FirstOrDefault();
    }

    /// <summary>
    /// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterfaceInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets all monobehaviours in children that implement the interface of type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

        var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();

        return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
    }




    /// <summary>
    /// Gets all monobehaviours in scene that implement the interface of type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] FindObjectOfInterface<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        var types = UnityEngine.GameObject.FindObjectsOfType<MonoBehaviour>();

        return (from a in types where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();

    }
}
