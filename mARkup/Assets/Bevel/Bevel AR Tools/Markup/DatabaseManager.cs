using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;

public class DatabaseManager : MonoBehaviour {

    #region Variables
    //----------------------------------------------------------------------------------
    public static DatabaseManager instance;

    [SerializeField]
    private string projectName;
    [SerializeField]
    private int versionNumber;
    private int versionId;

    public delegate void DatabaseEvent(MarkUpRecord.MarkUpType markupType, MarkUpRecord databasePacket);
    public DatabaseEvent recievedMarkup;

    public Action databaseEventCompleted;

    private Queue<Action> databaseQueue = new Queue<Action>();
    private bool isDequeuing = false;
    private bool isWaitingForResponse = false;

    #endregion

    #region Singleton Pattern
    //----------------------------------------------------------------------------------

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

        StartCoroutine(GetProjects_FromDatabase());
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    #endregion

    #region Public Methods

    //----------------------------------------------------------------------------------

    // Used to start the SendMarkupToDatabase coroutine from Markup scripts
    public void SaveMarkUpToDatabase(MarkUpRecord record, GameObject markupGameObject)
    {
        databaseQueue.Enqueue(() => { StartCoroutine(PostMarkup_ToDatabase(record, markupGameObject)); });
        if (!isDequeuing)
        {
            StartCoroutine(ExecuteDatabaseQueue());
        }
    }

    public void deleteFromDatabase(string markupID)
    {
        databaseQueue.Enqueue(() => { StartCoroutine(Delete_FromDatabase(markupID)); });
        if (!isDequeuing)
        {
            StartCoroutine(ExecuteDatabaseQueue());
        }
    }

    #endregion

    #region Private Methods
    //----------------------------------------------------------------------------------

    private void InjectMarkupParams(string markupId, string jBlob)
    {
        MarkUpRecord myMarkupInstance;
        MarkupLibrarySingleton.instance.markupDict.TryGetValue(markupId, out myMarkupInstance);
        if(myMarkupInstance == null)
        {
            return;
        }
        MarkUpRecord markupParams = JsonUtility.FromJson<MarkUpRecord>(jBlob as string);
        myMarkupInstance.updated_at = markupParams.updated_at;
        myMarkupInstance.created_at = markupParams.created_at;
        myMarkupInstance.version_id = markupParams.version_id;
    }

#endregion

    #region Coroutines
    //----------------------------------------------------------------------------------

    //Attempts to get all of the project records in the database.
    IEnumerator GetProjects_FromDatabase()
    {
        var download = UnityWebRequest.Get(Endpoint_GetProjects.url);
        yield return download.SendWebRequest();
        var projectList = Endpoint_GetProjects.Parse(download.downloadHandler.text);
        foreach (ProjectRecord record in projectList)
        {
            if (record.name == projectName)
            {
                StartCoroutine(PostVersions_FromDatabase(record.project_id));
            }
        }
    }

    //Attempts to get all of the version records for a specifc porjectId.
    IEnumerator PostVersions_FromDatabase(int projectId)
    {
        WWWForm versionForm = new WWWForm();
        versionForm.AddField("project_id", projectId);
        var download = UnityWebRequest.Post(Endpoint_PostVersions.url, versionForm);
        yield return download.SendWebRequest();
        var versionList = Endpoint_PostVersions.parse(download.downloadHandler.text);
        foreach (VersionRecord v in versionList)
        {
            if(v.version_number == versionNumber.ToString())
            {
                versionId = v.version_id;
            }
        }

        StartCoroutine(PostMarkups_FromDatabase(versionId));
    }

     //Attempts to get all of the markups for the specified versions.
    IEnumerator PostMarkups_FromDatabase(int versionId)
    {
        WWWForm markupForm = new WWWForm();
        markupForm.AddField("version", versionId);
        var download = UnityWebRequest.Post(Endpoint_PostMarkups.url, markupForm);
        yield return download.SendWebRequest();
        if (download.isNetworkError || download.isHttpError)
        {
            print("Error downloading: " + download.error);
        }
        var markupList = Endpoint_PostMarkups.parse(download.downloadHandler.text);

        foreach (MarkUpRecord m in markupList)
        {
            if (recievedMarkup != null)
            {
                recievedMarkup(m.type, m);
            }
        }
    }

    //Attempts to create a new markup record.
    IEnumerator PostMarkup_ToDatabase(MarkUpRecord record, GameObject markupGameObject)
    {
        isWaitingForResponse = true;

        WWWForm markupForm = new WWWForm();
        markupForm.AddField("version_id", versionId);
        markupForm.AddField("markup_id", record.markup_Id);
        markupForm.AddField("type", record.type.ToString());
        markupForm.AddField("data", record.data.Serialize());
        markupForm.AddField("creator", record.creator);
        markupForm.AddField("assigned", record.assigned);
        markupForm.AddField("location", JsonUtility.ToJson(record.location));
        markupForm.AddField("status", record.status);

        var download = UnityWebRequest.Post(Endpoint_PostMarkup.url, markupForm);

        yield return download.SendWebRequest();

        if (markupGameObject != null)
        {
            MarkUp myMarkupInstance = markupGameObject.GetComponent<MarkUp>();

            if (download.isNetworkError || download.isHttpError)
            {
                print("Error downloading: " + download.error);
                myMarkupInstance.markupData.DatabaseStatus = MarkUpRecord.databaseStatus.failedToRegister;
            }
            else
            {
                myMarkupInstance.markupData.DatabaseStatus = MarkUpRecord.databaseStatus.registered;
                InjectMarkupParams(myMarkupInstance.markupData.markup_Id, download.downloadHandler.text);
            }
        }

        isWaitingForResponse = false;
    }

    //Attempts to delete a markup record.
    IEnumerator Delete_FromDatabase(string markupID)
    {
        isWaitingForResponse = true;

        var res = UnityWebRequest.Delete(Endpoint_DeleteMarkup.url + markupID);

        yield return res.SendWebRequest();

        if (res.isNetworkError || res.isHttpError)
        {
            print("Error downloading: " + res.error);
        }

        isWaitingForResponse = false;
    }

    IEnumerator ExecuteDatabaseQueue()
    {
        isDequeuing = true;
        while (databaseQueue.Count > 0)
        {
            if (!isWaitingForResponse)
            {
                Action a = databaseQueue.Dequeue();
                a();
            }
            yield return null;         
        }
        isDequeuing = false;
    }
    #endregion

    
}

