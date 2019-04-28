using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarkupDataPacket_Question : System.Object, IMarkupDataPacket
{
    public Vector3[] linePoints;
    public Vector3[] lineDirections;
    public Vector3 location;
    public float width;
    public Color color;
    public string markup_Id;
}
