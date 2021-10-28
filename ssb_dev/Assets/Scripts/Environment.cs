using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public GameObject environmentPrototype;
    public GameObject backgroundPrototype;
    public float time;
	public float cameraX;
	public float cameraY;

    public RectTransform rt;

    // void Start() {

	// 	cameraX = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
	// 	cameraY = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y;

    //     GameObject environment = Instantiate(environmentPrototype);
    //     environment.transform.parent = this.gameObject.transform;
    //     environment.transform.SetSiblingIndex(0);

    //     for (int i = 0; i < 2; i++) {

    //         GameObject background = Instantiate(backgroundPrototype);
    //         background.transform.parent = environment.gameObject.transform;
    //         background.transform.SetSiblingIndex(0);            

    //         rt = background.GetComponent<RectTransform>();
    //         rt.sizeDelta = new Vector2(8, 8);
    //         print(rt.sizeDelta.y);

    //         switch (i) {
    //             case 0:
    //                 background.transform.position = new Vector3(0.0f, (rt.sizeDelta.y/2), 0.0f);
    //                 break;
    //             case 1:
    //                 background.transform.position = new Vector3(0.0f, ((rt.sizeDelta.y/2)*-1), 0.0f);
    //                 break;
    //         }
    //     }

    // }
    
    // void Update() {

    //     time += Time.deltaTime;

        

    // }

    // void MovementControl () {




    // }
}
