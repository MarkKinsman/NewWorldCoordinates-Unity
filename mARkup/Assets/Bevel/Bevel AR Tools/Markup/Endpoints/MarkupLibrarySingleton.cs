using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MarkupLibrarySingleton : MonoBehaviour {

    public Dictionary<string, MarkUpRecord> markupDict;
    public List<GameObject> markupList;
    public static MarkupLibrarySingleton instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        markupDict = new Dictionary<string, MarkUpRecord>();
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
