using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_Behaviour : MonoBehaviour
{
    public float speed = 1;
    public bool isFaster = false;
    public float faster = 5;
    public Vector3 direction = new Vector3(0,-1,0);
    public GameManager manager;

    Vector3 stepVector;
    Rigidbody2D rb;    
    // Start is called before the first frame update
    void Start() {
        manager = GameObject.Find("Game Manager").GetComponent<GameManager> ();
        rb = GetComponent<Rigidbody2D> ();
        
    }

    // Update is called once per frame
    void Update() {      

        stepVector = manager.spaceObjSpeed * direction.normalized;

        rb.velocity = stepVector;

        if(transform.position.y < -5)
        {
			Destroy(gameObject);
        }
    }
}
