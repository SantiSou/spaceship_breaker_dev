using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MongoAPI : MonoBehaviour
{
    void Start()
    {

        // Player playerInstance = new Player();
        // playerInstance.username = "test2";
        // playerInstance.email = "test2@test.com";
        // playerInstance.password = "test2";

        // Structure Data Type

        // requestMDB request = new requestMDB();
        // request.username = "test";
        // request.points = "0";
        // request.geolocation = "";
        // request.datetime = "2021-10-04 00:00:00";
        
    }

    void Update()
    {
        
    }

    public void UploadMDB(string url, string request) {

        StartCoroutine(Upload("<ip>/<route>/", request));

    }

    IEnumerator Upload(string url, string request) {

        string requestToJson = JsonUtility.ToJson(request);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requestToJson);

        var uwr = new UnityWebRequest(url, "POST");
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();
 
        if(uwr.isNetworkError) {
            Debug.Log(uwr.error);
        }
        else {
            
            Debug.Log(uwr.downloadHandler.text);
 
            byte[] results = uwr.downloadHandler.data;
        }
    }   

    public class requestMDB {

        public string username;
        public string points;
        public string geolocation;
        public string datetime;        

    } 
}
