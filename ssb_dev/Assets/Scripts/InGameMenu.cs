using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public GameManager manager;
    public GameObject leaderboard_panel;
    public GameObject list_panel;
    public GameObject list;
    public GameObject nickname_panel;
    public GameObject nickname_text;
    public GameObject nickname_inputfield;
    public GameObject submit_button;
    public GameObject record_panel;
    public bool submited;

    void Start() {

        // GameObject mongo = Instantiate(MongoAPI);
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        submited = false;

        leaderboard_panel = gameObject.transform.Find("LeaderboardMenu").gameObject;
        list_panel = leaderboard_panel.transform.Find("Panel").gameObject;
        list = list_panel.transform.Find("List").gameObject;
        list.GetComponent<Text>().text = "";    

        record_panel = gameObject.transform.Find("RecordScore").gameObject;
        nickname_panel = record_panel.transform.Find("Nickname").gameObject;
        submit_button = record_panel.transform.Find("SubmitButton").gameObject;

        nickname_panel.SetActive(false);
        submit_button.SetActive(false);
        
    }

    public void MainMenu () {

        LoadNextLevel(SceneManager.GetActiveScene().buildIndex -1);

    }

    public void LoadNextLevel(int levelIndex) {

        StartCoroutine(LoadLevel(levelIndex));

    }

    IEnumerator LoadLevel(int levelIndex) {

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);


    }    

    public void RestartGame () {

        manager.RestartGame();

    }

    public void SubmitScoreAvailable (string points) {

        nickname_panel.SetActive(true);
        nickname_text = nickname_panel.transform.Find("Text").gameObject;
        nickname_text.GetComponent<Text>().text = "";
        nickname_inputfield = nickname_panel.transform.Find("InputField").gameObject;
        nickname_inputfield.SetActive(false);
        int pointInt;
        int.TryParse(points, out pointInt);

        if (manager.score > pointInt) {
            
            nickname_text.GetComponent<Text>().text = "Nickname";
            submit_button.SetActive(true);
            nickname_inputfield.SetActive(true);

        }
        else {

            nickname_text.GetComponent<Text>().text = "Not enough points";

        }

    }

    public void SubmitScore () {

        if (!submited && manager.InputField.text.Length > 0) {

            submited = true;
            nickname_panel.SetActive(false);
            submit_button.SetActive(false);
		    manager.mongo.GetComponent<MongoAPI>().SaveScore(manager.InputField.text, manager.score.ToString());
        }

		// mongoResponse = manager.mongo.GetComponent<MongoAPI>().SaveScore();
		// endgamePanel = Instantiate(endgamePanelPrototype);
    }

    public void Leaderboard () {

        manager.mongo.GetComponent<MongoAPI>().GetDatabase();

    }

    public void updateLeaderboard (string response) {

        Debug.Log(response);
        if (response == "") {

            list.GetComponent<Text>().text = "No records";

        }
        else {
            list.GetComponent<Text>().text = response;
        }

    }
    // public void QuitGame () {

    //     Application.Quit();

    // }  

    // public void LoadNextLevel(int levelIndex) {

    //     StartCoroutine(LoadLevel(levelIndex));

    // }

    // IEnumerator LoadLevel(int levelIndex) {

    //     yield return new WaitForSeconds(transitionTime);

    //     SceneManager.LoadScene(levelIndex);


    // }
}
