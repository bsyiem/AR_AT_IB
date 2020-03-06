using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LEDControlPhysical : MonoBehaviour
{
    //ball obj
    //to get the number of passes if AR content is present
    public BallBehaviour ball;

    //number of events
    public int numberOfEvents = 10;

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
        
        //set up connection with baudRate 9600
        this.instance.Call("createPhysicaloid",9600);

        //set the shared preference file name to write data into
        FileSetting fileSetting = GameObject.Find("FileSetting").GetComponent<FileSetting>();
        this.instance.Call("setFileName", fileSetting.fileName);
        
        //open the connection    
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
            IEnumerator coroutine = SelectRandomLED(NUMBER_OF_LEDS);
            StartCoroutine(coroutine);
            this.isStarted = true;
            if (this.ball != null)
            {
                this.ball.resetPassedNumber();
            }
        }
    }

    IEnumerator SelectRandomLED(int numberOfLEDs)
    {
        int lastLedNumber = -1;
        int number = -1;
        while (numberOfEvents > 0)
        {
            yield return (new WaitForSeconds(Random.Range(this.waitTimeMin, this.waitTimeMax + 1)));
            do
            {
                number = Random.Range(0, numberOfLEDs);
            } while (number == lastLedNumber);
            
            this.instance.Call("sendData", number.ToString());
            numberOfEvents--;
            lastLedNumber = number;
        }

        //wait five second before sending terminate signal
        yield return (new WaitForSeconds(Random.Range(this.waitTimeMin, this.waitTimeMax + 1)));
        this.instance.Call("sendData", ("#").ToString());

    }

    //all reaction times are handled by the arduino, 
    // for the physical controller, this does nothing but is still called from the java code when a message is received
    void HandleArduinoMessage(string message)
    {
        switch (message)
        {
            case "#":
                if (this.ball != null)
                {
                    this.instance.Call("writeToFile", "\n"+this.ball.getPassedNumber().ToString());
                }
                this.instance.Call("closeFile");
                break;
        }

        SceneManager.LoadScene("Opening");
    }
}
