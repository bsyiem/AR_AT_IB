using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public Camera firstPersonCamera;

    public List<GameObject> catcherList;

    private int passedNumber = 0;

    public float zDist = 3.0f;

    public float minDistToPass = 0.5f;


    private int catcherNumber;

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
        if(this.transform.position == this.catcherList[this.catcherNumber].transform.position)
        {
            this.catcherNumber = GetNextCatcherNumber(this.catcherNumber);
            this.passedNumber++;
            Debug.Log(this.passedNumber);
        }
        else
        {
            //this.transform.position = Vector3.Lerp(this.transform.position, this.catcherList[this.catcherNumber].transform.position, 0.1f);
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.catcherList[this.catcherNumber].transform.position, Time.deltaTime * 1.5f);
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
