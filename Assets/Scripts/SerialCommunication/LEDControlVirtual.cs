using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class LEDControlVirtual : MonoBehaviour
{

    //first person camer
    Camera mainCamera;

    //ball obj
    //to get the number of passes if AR content is present
    public BallBehaviour ball;

    //current selectedLED
    int currentLed = -1;

    //number of events
    public int numberOfEvents = 10;

    //wait time range - how long till the next LED turns on
    public int waitTimeMin = 5;
    public int waitTimeMax = 10;

    static int NUMBER_OF_LEDS = 5;

    //virtual LEDs
    public float led_z_distance = 15f; 
    public GameObject ledPrefab;
    Dictionary<int, GameObject> ledMap;

    //led position
    // 0; // top left
    // 1; // top right
    // 2; // bottom right
    // 3; // bottom left
    // 4; // center
    Vector3[] led_pos;


    bool isStarted = false;

    AndroidJavaClass serialCommunicationManagerClass;
    AndroidJavaObject instance { get { return serialCommunicationManagerClass.GetStatic<AndroidJavaObject>("INSTANCE"); } }

    private void Awake()
    {
        led_pos = new Vector3[5];
        //led positions
        led_pos[0] = new Vector3(0.1f, 0.9f, led_z_distance);
        led_pos[1] = new Vector3(0.9f, 0.9f, led_z_distance);
        led_pos[2] = new Vector3(0.9f, 0.1f, led_z_distance);
        led_pos[3] = new Vector3(0.1f, 0.1f, led_z_distance);
        led_pos[4] = new Vector3(0.5f, 0.5f, led_z_distance);

        //set up map
        ledMap = new Dictionary<int, GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //instantiate the LEDs from ledPrefab
        //place led in 4 corners + center
        //create hashmap <int,gameobj> to keep track of leds
        for (int i = 0; i < 5; i++)
        {
            GameObject led = Instantiate(ledPrefab, Vector3.zero, Quaternion.identity);
            led.GetComponent<LedBehaviour>().setRelativePosition(led_pos[i]);
            ledMap.Add(i, led);
            SwitchOffLed(i);
        }

        this.serialCommunicationManagerClass = new AndroidJavaClass("com.bsyiem.serialcommunicationplugin.SerialCommunication");
        this.serialCommunicationManagerClass.CallStatic("instantiate", this.gameObject.name);

        //set up connection with baudRate 9600
        this.instance.Call("createPhysicaloid", 9600);

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
        if (Input.touchCount != 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
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
        while (numberOfEvents > 0)
        {
            yield return (new WaitForSeconds(Random.Range(this.waitTimeMin, this.waitTimeMax + 1)));
            do
            {
                currentLed = Random.Range(0, numberOfLEDs);
            } while (currentLed == lastLedNumber);

            //send message to start timer to arduino
            this.instance.Call("sendData", currentLed.ToString());
            // when arduino receives start it will startTime and send back a "$" 
            // then wait for button press or another "start"
            numberOfEvents--;
            lastLedNumber = currentLed;
        }

        //wait five second before sending terminate signal
        yield return (new WaitForSeconds(Random.Range(this.waitTimeMin, this.waitTimeMax + 1)));
        this.instance.Call("sendData", ("#").ToString());

    }

    //turn emission of material on
    void SwitchOnLed(int ledNumber)
    {
        //ledMap[ledNumber].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        Transform ledLight = ledMap[ledNumber].transform.GetChild(0);

        ledLight.GetComponent<MeshRenderer>().enabled = true;
        
        //the light reflects on the virtual objects so I will keep it off
        //ledLight.GetComponent<Light>().enabled = true;
    }

    //turn emission of material off
    void SwitchOffLed(int ledNumber)
    {
        //ledMap[ledNumber].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        Transform ledLight = ledMap[ledNumber].transform.GetChild(0);

        ledLight.GetComponent<MeshRenderer>().enabled = false;

        //the light reflects on the virtual objects so I will keep it off
        //ledLight.GetComponent<Light>().enabled = false;
    }

    void SwitchOffAllLeds()
    {
        for (int i = 0; i < 5; i++)
        {
            SwitchOffLed(i);
        }
    }

    //all reaction times are handled by the arduino, this should 
    // switch on specific LED - when receiving a special character (say "$")
    // switch off all leds - when receiving a reaction time or terminating char "#"
    //update Arduino code to send UnityPlayer.UnitySendMessage("ObjectName","thisFunctionName", "message");
    void HandleArduinoMessage(string message)
    {
        switch (message)
        {
            case "$": //received start code "$" - switch on LED
                SwitchOnLed(currentLed);
                break;
            case "@": // receive reaction time "@" - switch off leds
                SwitchOffAllLeds();
                break;
            case "#":
                if (this.ball != null)
                {
                    this.instance.Call("writeToFile", this.ball.getPassedNumber().ToString());             
                }
                this.instance.Call("closeFile");
                this.instance.Call("closeConnection");
                SwitchOffAllLeds();
                SceneManager.LoadScene("Opening");
                break;
                   
        }
    }
}
