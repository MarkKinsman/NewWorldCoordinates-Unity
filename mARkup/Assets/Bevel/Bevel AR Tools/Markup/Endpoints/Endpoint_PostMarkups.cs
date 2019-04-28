using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Endpoint_PostMarkups 
{
    public static string url = "http://markup.virtual-insights.com/v1/markups";

    static public List<MarkUpRecord> parse(string JBlob)
    {
        List<MarkUpRecord> MarkupList = new List<MarkUpRecord>();
        MarkupList = Helper_JSON.FromJson<MarkUpRecord>(Helper_JSON.FixJsonArray(JBlob)).ToList();
        return MarkupList;
    }
}



