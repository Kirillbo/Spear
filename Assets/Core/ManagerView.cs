using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ManagerView  {

    private static Dictionary<Type, MonoBehaviour> _views = new Dictionary<Type, MonoBehaviour>();

    public static T Get<T>() where T : MonoBehaviour
    {
        var needType = typeof(T);

        if (_views.ContainsKey(needType))
        {
            return _views[needType] as T;
        }

            var result = Object.FindObjectOfType(needType) as T;
            if(result == null) Debug.LogFormat("Object type {0} not find on scene", needType);
            _views.Add(needType, result);
            return result;
        
    }
}
