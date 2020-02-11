using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherBehaviour : MonoBehaviour
{

    public Camera firstPersonCamera;

    //is the next position to be moved to set
    private bool isSetPosition = false;

    //the next position the catcher moves to
    private Vector3 nextPosition;

    //distance from the camera
    public float zDist = 3.0f;

    //on arriving at a new location, catcher should wait the following seconds
    public float pauseTime = 1.5f;

    //var used to measure pause time
    private float stopWatch = 0.0f;

    //current pause state
    private bool pauseState = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 centerCamera = new Vector3(0.5f, 0.5f, this.firstPersonCamera.nearClipPlane + this.zDist);

        Vector3 randomCamera;
        do
        {
            randomCamera = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), this.firstPersonCamera.nearClipPlane + this.zDist);
        } while (randomCamera == centerCamera);

        this.transform.position = this.firstPersonCamera.ViewportToWorldPoint(randomCamera);
    }

    // Update is called once per frame
    void Update()
    {
        if(this.firstPersonCamera != null)
        {
            /*
             * if catcher is not in a paused state
             * seelect new direction and move towards it
             */ 
            if (!this.pauseState)
            {
                /*
             * if direction is not set, set a new direction to move to
             */
                if (!isSetPosition)
                {
                    //this.nextDirection = new Vector3(0, 0, this.firstPersonCamera.nearClipPlane + 1);

                    this.nextPosition = this.firstPersonCamera.ViewportToWorldPoint(
                        new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), this.firstPersonCamera.nearClipPlane + this.zDist));

                    this.isSetPosition = true;
                }
                /*
                 * else move to the set direction
                 */
                else
                {
                    if (this.transform.position != this.nextPosition)
                    {
                        //this.transform.position = Vector3.Lerp(this.transform.position, this.nextDirection, 0.04f);
                        this.transform.position = Vector3.MoveTowards(this.transform.position, this.nextPosition, Time.deltaTime * 1.0f);
                    }
                    else
                    {
                        this.isSetPosition = false;
                        this.pauseState = true;
                    }
                }
            }
            /*
             * if catcher is in a paused state
             * set up stopwatch
             */ 
            else
            {
                if (this.stopWatch >= this.pauseTime)
                {
                    this.pauseState = false;
                    this.stopWatch = 0.0f;
                }
                else
                {
                    this.stopWatch += Time.deltaTime;    
                }
            }          
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //second working
        //Vector3 relativeCamPos = this.firstPersonCamera.WorldToViewportPoint(this.transform.position);
        //Vector3 otherRelativeCamPos = this.firstPersonCamera.WorldToViewportPoint(other.transform.position);

        //Vector3 oppositeDirection = relativeCamPos - otherRelativeCamPos;
        //oppositeDirection.z = this.firstPersonCamera.nearClipPlane + this.zDist;

        //this.transform.position = Vector3.MoveTowards(this.transform.position, this.firstPersonCamera.ViewportToWorldPoint(oppositeDirection), Time.deltaTime * 1.0f);


        //First working 
        this.nextPosition = this.firstPersonCamera.ViewportToWorldPoint(
                    new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), this.firstPersonCamera.nearClipPlane + this.zDist));

        this.isSetPosition = true;
    }
}
