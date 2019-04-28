using UnityEngine;
using System;

[System.Serializable]
public class MarkUpRecord
{
    public enum MarkUpType { question, issue, testResult }
    public enum databaseStatus { unregistered, failedToRegister, registered }
    public databaseStatus DatabaseStatus = databaseStatus.unregistered;

    public string markup_Id;
    public int version_id;
    public MarkUpType type;
    public IMarkupDataPacket data;
    public string creator;
    public string assigned;
    public Vector3 location;
    public string status;
    public DateTime created_at;
    public DateTime updated_at;
}
