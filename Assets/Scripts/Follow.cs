using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private Camera firstPersonCamera;

    private bool isCameraSet;
    private Vector3 offset;


    private void Awake()
    {
        this.isCameraSet = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.isCameraSet)
        {
            this.transform.position = this.firstPersonCamera.transform.position + this.offset;
        }
    }

    public void SetCamera(Camera camera)
    {
        this.firstPersonCamera = camera;
        this.SetOffsetToCamera();
        this.isCameraSet = true;
    }

    private void SetOffsetToCamera()
    {
        this.offset = this.transform.position - this.firstPersonCamera.transform.position;
    }
}
