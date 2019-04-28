using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class MarkUp : MonoBehaviour {

    public string Markup_id;
    public MarkUpRecord markupData
    {
        get
        {
            return MarkupLibrarySingleton.instance.markupDict[Markup_id];
        }
        set
        {
            MarkupLibrarySingleton.instance.markupDict[Markup_id] = value;
        }
    }

    public TextMesh dataDisplay;

    public void SetStatus(string status)
    {
        markupData.status = status;
    }

    public void SetType(int typeInput)
    {
        markupData.type = (MarkUpRecord.MarkUpType)Enum.ToObject(typeof(MarkUpRecord.MarkUpType), typeInput);
    }

    public void CalculateLocation()
    {
        markupData.location = gameObject.transform.localPosition;
    }

    public void LoadRecord(MarkUpRecord markUpRecord)
    {
        markupData = markUpRecord;

        gameObject.transform.localPosition = markupData.location;
    }

    public void SaveMarkup()
    {
        DatabaseManager.instance.SaveMarkUpToDatabase(markupData, gameObject);
    }
}
