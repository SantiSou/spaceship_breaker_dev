using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MongoAPI : MonoBehaviour
{
    public UnityWebRequest uwr;
    public MainMenu mainMenu;
    public InGameMenu inGameMenu;
    public scoreItem[] scoreItems;
    public int lastpoints;

    void Start() {

        // Player playerInstance = new Player();
        // playerInstance.username = "test2";
        // playerInstance.email = "test2@test.com";
        // playerInstance.password = "test2";

        // Structure Data Type

        // requestMDB request = new requestMDB();
        // request.username = "test";
        // request.points = "0";
        // request.datetime = "2021-10-04 00:00:00";
        
    }

    public void GetLastPoints() {

        StartCoroutine(request_lastscore("http://mongoapi-ssb.herokuapp.com/users/lastscore", "GET", ""));

        if (SceneManager.GetActiveScene().buildIndex != 0) {
            inGameMenu = GameObject.Find("Canvas(Clone)").GetComponent<InGameMenu> ();
        }
    }

    public void SaveScore(string nickname, string points) {

        saveScore score = new saveScore();
        score.nickname = nickname;
        score.points = points;
        Debug.Log(score.nickname);
        StartCoroutine(request_saveHighscores("http://mongoapi-ssb.herokuapp.com/users/savehighscore", "POST", score));

    }

    public UnityWebRequest GetDatabase() {

        StartCoroutine(request_getHighscores("http://mongoapi-ssb.herokuapp.com/users/gethighscores", "GET", ""));
        
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            mainMenu = GameObject.Find("MainMenu").GetComponent<MainMenu> ();
        }
        else {
            inGameMenu = GameObject.Find("Canvas(Clone)").GetComponent<InGameMenu> ();
        }        

        return uwr;

    }

    IEnumerator request_getHighscores(string url, string method, string request) {

        string requestToJson = JsonUtility.ToJson(request);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requestToJson);

        uwr = new UnityWebRequest(url, method);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();
 
        if(uwr.isNetworkError) {
            Debug.Log(uwr.error);
        }
        else {
                      
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                mainMenu.updateLeaderboard(uwr.downloadHandler.text);
            }
            else {
                inGameMenu.updateLeaderboard(uwr.downloadHandler.text);
            }

            // Debug.Log(uwr.downloadHandler.text);
 
            byte[] results = uwr.downloadHandler.data;
        }
    } 

    IEnumerator request_saveHighscores(string url, string method, saveScore request) {

        string requestToJson = JsonUtility.ToJson(request);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requestToJson);

        uwr = new UnityWebRequest(url, method);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();
 
        if(uwr.isNetworkError) {
            Debug.Log(uwr.error);
        }
        else {
            
            Debug.Log(uwr.downloadHandler.data);
 
            byte[] results = uwr.downloadHandler.data;
        }
    }  

    IEnumerator request_lastscore(string url, string method, string request) {

        string requestToJson = JsonUtility.ToJson(request);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(requestToJson);

        uwr = new UnityWebRequest(url, method);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();
 
        if(uwr.isNetworkError) {
            Debug.Log(uwr.error);
        }
        else {

            inGameMenu.SubmitScoreAvailable(uwr.downloadHandler.text);
 
            byte[] results = uwr.downloadHandler.data;
        }
    }       
    
    public class saveScore {

        public string nickname;
        public string points;

    }

    [Serializable]
    public class scoreItem {

        public string username;
        public string points;
        public string datetime;   

    }    

    [Serializable]
    public class scoreItemCollection
    {
        [SerializeField]
        public scoreItem[] sprites;
    }    
}
