using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class Endpoint_PostMarkup : System.Object
{
    public string markup_id;
    public int version_id;
    public string type;
    public string data;
    public string creator;
    public DateTime created_at;
    public DateTime updated_at;
    public Vector3 location;
    public string summary;

    public static string url = "http://markup.virtual-insights.com/v1/markup";
}



