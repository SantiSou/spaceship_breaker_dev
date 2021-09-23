using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleet_Behaviour : MonoBehaviour
{
    public GameManager manager;
    public int directionValue;
    public int directionValueY;
    public float enemyXPos;
    public float enemyYPos;
    public bool reArrange;
    public bool fleeCreated;

    void Start() {

        reArrange = true;
        enemyXPos = 0.0f;
        enemyYPos = 0.0f;
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();

    }

    void Update() {

        if (transform.childCount < 1) {
            Destroy(gameObject);
        } 
    }

    public void reArrangeFleet () {

        bool newLine = false;
        int childQty = 0;
        float firstXPos = 0.0f;

        foreach (Transform child in transform) {

            childQty += 1;
            if (childQty == 1) {

                enemyXPos = child.transform.position.x;
                enemyYPos = child.transform.position.y;

                firstXPos = enemyXPos;

            }
            else if ((childQty%5==0)) {
                enemyYPos += child.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                newLine = true;
            } else {

                if (!newLine) {
                    enemyXPos += child.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
                } else {
                    enemyXPos = firstXPos;
                    newLine = false;
                }
            }

            child.transform.position = new Vector3(enemyXPos, enemyYPos, child.transform.position.z);
            
        }
    }
}
