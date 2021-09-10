using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet_Behaviour : MonoBehaviour
{
    public GameManager manager;
    public int directionValue;
    public int directionValueY;

    void Start() {

        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
    }

    void Update() {
    }

}
