using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedBehaviour : MonoBehaviour
{

    //Camera
    Camera mainCamera;

    Vector3 relativePosition;

    public void setRelativePosition(Vector3 pos)
    {
        this.relativePosition = pos;
    }

    private void Awake()
    {
        this.mainCamera = GameObject.Find("ARCore Device").GetComponentInChildren<Camera>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = mainCamera.ViewportToWorldPoint(this.relativePosition);
        transform.LookAt(mainCamera.transform);
    }
}
