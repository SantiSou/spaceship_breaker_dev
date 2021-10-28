using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public GameManager manager;
    public GameObject leaderboard_panel;
    public GameObject list_panel;
    public GameObject list;

    void Start() {
        
        manager = GameObject.Find("GameManager").GetComponent<GameManager> ();

        leaderboard_panel = gameObject.transform.parent.gameObject.transform.Find("LeaderboardMenu").gameObject;
        list_panel = leaderboard_panel.transform.Find("Panel").gameObject;
        list = list_panel.transform.Find("List").gameObject;
        list.GetComponent<Text>().text = "";

    }

    public void PlayGame () {

        LoadNextLevel(SceneManager.GetActiveScene().buildIndex +1);

    }

    public void Leaderboard () {

        manager.mongo.GetComponent<MongoAPI>().GetDatabase();

    }

    public void updateLeaderboard (string response) {

        Debug.Log(response);
        list.GetComponent<Text>().text = response;

    }

    public void QuitGame () {

        Application.Quit();

    }  

    public void LoadNextLevel(int levelIndex) {

        StartCoroutine(LoadLevel(levelIndex));

    }

    IEnumerator LoadLevel(int levelIndex) {

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);


    }

    public class highscore {

        public string nickname;
        public string points;
        public string date;

    }
}
