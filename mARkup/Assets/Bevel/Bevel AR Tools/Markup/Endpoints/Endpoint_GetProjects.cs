using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class Endpoint_GetProjects
{
    public static string url = "http://markup.virtual-insights.com/v1/projects";

    public static List<ProjectRecord> Parse(string JBlob)
    {
        List<ProjectRecord> projectList = new List<ProjectRecord>();
        projectList = Helper_JSON.FromJson<ProjectRecord>(Helper_JSON.FixJsonArray(JBlob)).ToList();
        return projectList;
    }
}
