using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialCommunicationManager : MonoBehaviour
{
    bool isStarted = false;

    AndroidJavaClass serialCommunicationManagerClass;
    AndroidJavaObject instance { get { return serialCommunicationManagerClass.GetStatic<AndroidJavaObject>("INSTANCE"); } }

    // Start is called before the first frame update
    void Start()
    {
        this.serialCommunicationManagerClass = new AndroidJavaClass("com.bsyiem.serialcommunicationplugin.SerialCommunication");
        this.serialCommunicationManagerClass.CallStatic("instantiate", this.gameObject.name);
        //this.instance.Call("showText", "is this working?");
        this.instance.Call("createPhysicaloid",9600);
        this.instance.Call("openConnection");
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTouch();
    }

    void ProcessTouch()
    {
        Touch touch;
        if(Input.touchCount != 1 || (touch =Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        if (!this.isStarted)
        {
            IEnumerator coroutine = SelectRandomLED(5, 5);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator SelectRandomLED(float time, int numberOfLEDs)
    {
        while (true)
        {
            yield return (new WaitForSeconds(time));
            int number = Random.Range(0, numberOfLEDs - 1);
            this.instance.Call("sendData", number.ToString());
        } 
    }
}
