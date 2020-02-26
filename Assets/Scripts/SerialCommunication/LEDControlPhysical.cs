using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDControlPhysical : MonoBehaviour
{
    //wait time range - how long till the next LED turns on
    public int waitTimeMin = 5;
    public int waitTimeMax = 10;

    static int NUMBER_OF_LEDS = 5;

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
            CodelabUtils._ShowAndroidToastMessage("Starting");
            IEnumerator coroutine = SelectRandomLED(Random.Range(this.waitTimeMin,this.waitTimeMax + 1), NUMBER_OF_LEDS);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator SelectRandomLED(float time, int numberOfLEDs)
    {
        while (true)
        {
            yield return (new WaitForSeconds(time));
            int number = Random.Range(0, numberOfLEDs);
            this.instance.Call("sendData", number.ToString());
        } 
    }
}
