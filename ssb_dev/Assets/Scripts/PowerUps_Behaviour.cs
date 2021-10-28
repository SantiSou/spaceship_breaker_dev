using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps_Behaviour : MonoBehaviour
{
    public GameObject powerUpPrototype;
    public GameManager manager;

    void Start() {
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
    }

    // Update is called once per frame
    void Update() {
        if (Random.Range(0,1000) == 1) { // Condición para generar enemigos		
            createPowerUp ();
        }        
    }

    void createPowerUp () {
        float powerUpXPos = Random.Range(-2.6f, 2.7f);
        float powerUpYPos = 5;

        GameObject powerUpCopy = Instantiate(powerUpPrototype);
        powerUpCopy.transform.localScale = new Vector3(0.2f, 0.2f, 0); 
        powerUpCopy.transform.position = new Vector3(powerUpXPos, powerUpYPos, -1f);
        powerUpCopy.GetComponent<PowerUp_Behaviour>().direction = new Vector3(0, -transform.localScale.y, 0);  

      
    }
}
