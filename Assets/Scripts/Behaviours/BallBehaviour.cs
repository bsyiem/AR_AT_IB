using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public Camera firstPersonCamera;

    public List<GameObject> catcherList;

    private int passedNumber = 0;

    //dist from camera
    public float zDist = 3.0f;

    //min distance to next catcher
    public float minDistToPass = 0.5f;

    private int catcherNumber;

    //speed of ball
    public float speed = 1.0f;

    //stay with catcher for a set time
    public float pauseTime = 2.0f;
    private float stopWatch = 0.0f;

    //has the experiment started
    public bool started = false;


    public void resetPassedNumber()
    {
        this.passedNumber = 0;
        this.started = true;
    }

    public int getPassedNumber()
    {
        return this.passedNumber;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 centerCamera = new Vector3(0.5f, 0.5f, this.firstPersonCamera.nearClipPlane + this.zDist);
        this.transform.position = this.firstPersonCamera.ViewportToWorldPoint(centerCamera);

        this.catcherNumber = GetNextCatcherNumber(this.catcherNumber);
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.started)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.catcherList[this.catcherNumber].transform.position, Time.deltaTime * this.speed);
        }
        else
        {
            if (this.transform.position == this.catcherList[this.catcherNumber].transform.position)
            {
                if (this.stopWatch >= this.pauseTime)
                {
                    this.catcherNumber = GetNextCatcherNumber(this.catcherNumber);
                    this.passedNumber++;

                    this.stopWatch = 0.0f;
                    //Debug.Log(this.passedNumber);
                }
                else
                {
                    this.stopWatch += Time.deltaTime;
                }

            }
            else
            {
                //this.transform.position = Vector3.Lerp(this.transform.position, this.catcherList[this.catcherNumber].transform.position, 0.1f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.catcherList[this.catcherNumber].transform.position, Time.deltaTime * this.speed);
            }
        }       
    }

    private int GetNextCatcherNumber(int currentCatcherNumber)
    {
        int newCatcherNumber;
        float distToCatcher;
        do
        {
            newCatcherNumber = Random.Range(0, 5);
            distToCatcher = Vector3.Distance(this.transform.position, this.catcherList[newCatcherNumber].transform.position);

        } while (newCatcherNumber == currentCatcherNumber && distToCatcher < this.minDistToPass);

        return newCatcherNumber; 
    }
}
