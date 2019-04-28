using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Endpoint_PostVersions
{
    public static string url = "http://nwc.virtual-insights.com/v1/versions";

    public static List<VersionRecord> parse(string JBlob)
    {
        List<VersionRecord> versionList = new List<VersionRecord>();
        versionList = Helper_JSON.FromJson<VersionRecord>(Helper_JSON.FixJsonArray(JBlob)).ToList();
        return versionList;
    }
}
