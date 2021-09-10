using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class init_MainCamera : MonoBehaviour
{
    public Transform target;
    Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
		print(screenPos.x);
		print(screenPos.y);
        // Screen.orientation = ScreenOrientation.LandscapeLeft;
        // Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
