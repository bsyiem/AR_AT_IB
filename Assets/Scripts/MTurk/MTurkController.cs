using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MTurkController : MonoBehaviour
{

    private MTurkSettings settings;
    //private InstructionController instructionController;
    private PHPCommunicationManager commManager;



    // to be set by opening scene
    //either 
    // physicalEvent
    // virtualEvent
    public string eventType = "physicalEvent";

    public UnityEngine.UI.Image bgimage;
    public UnityEngine.UI.Image startInstructions;


    //list containing background images for corresponding events
    public List<Sprite> virtualEvents;
    public List<Sprite> physicalEvents;

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

    //timer
    bool isLedOn; //bool to donote if an LED is on
    float reactionTime; // records reaction time; keeps adding deltaTime since the frame where isLedON


    // Start is called before the first frame update
    void Start()
    {
        this.settings = GameObject.Find("Settings").GetComponent<MTurkSettings>();
        //this.instructionController = GameObject.Find("InstructionController").GetComponent<InstructionController>();
        this.commManager = GameObject.Find("CommunicationManager").GetComponent<PHPCommunicationManager>();

        if(this.settings.currentCondition == 0)
        {
            this.eventType = "physicalEvent";
        }
        else
        {
            this.setEventType();
        }

        if(this.eventType == "physicalEvent")
        {
            this.bgimage.sprite = this.physicalEvents[5];
        }
        else
        {
            this.bgimage.sprite = this.virtualEvents[5];
        }

        //webcall php script to write to file
    }

    // Update is called once per frame
    void Update()
    {
        if (isLedOn)
        {
            this.reactionTime += Time.deltaTime;
        }
        ProcessInput();
    }

    void setEventType()
    {
        if (this.settings.startingEventType == "physicalEvent")
        {
            if (this.settings.currentCondition <= 3)
            {
                this.eventType = "physicalEvent";
            }
            else
            {
                this.eventType = "virtualEvent";
            }
        }
        else
        {
            if (this.settings.currentCondition <= 3)
            {
                this.eventType = "virtualEvent";
            }
            else
            {
                this.eventType = "physicalEvent";
            }
        }
    }

    void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!this.isStarted)
            {
                IEnumerator coroutine = SelectRandomLED(NUMBER_OF_LEDS);
                StartCoroutine(coroutine);
                this.startInstructions.gameObject.SetActive(false);
                this.isStarted = true;
                if (this.ball != null)
                {
                    this.ball.resetPassedNumber();
                }
            }
            else
            {
                //Record reaction.

                //false positive
                if (!isLedOn)
                {
                    //webcll to write a false positive event
                }
                else
                {
                    Debug.Log("reacted = " + this.reactionTime);
                    //webcall write this.reaction time via webcall
                    this.reactionTime = 0.0f;//reset reaction time

                    //reset background = no LED on
                    //last index hold the image with no LED on
                    if (this.eventType == "physicalEvent")
                    {
                        this.bgimage.sprite = this.physicalEvents[NUMBER_OF_LEDS];
                    }
                    else
                    {
                        this.bgimage.sprite = this.virtualEvents[NUMBER_OF_LEDS];
                    }
                }
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

            //test
            //number+=1;
            //number = number > numberOfLEDs ? 0 : number;


            if (isLedOn)
            {
                Debug.Log(this.reactionTime);

                //write missed event/ false negative reaction and seconds with this.eventType
                //this will be a webcall 
                this.reactionTime = 0.0f; //reset reaction time
            }

            if (this.eventType == "physicalEvent")
            {
                this.bgimage.sprite = this.physicalEvents[number];
            }
            else
            {
                this.bgimage.sprite = this.virtualEvents[number];
            }

            this.isLedOn = true;

            numberOfEvents--;
            lastLedNumber = number;
        }

        //wait waitTimeMin second before sending terminate signal
        yield return (new WaitForSeconds(Random.Range(this.waitTimeMin, this.waitTimeMin + 1)));

        
        if(this.ball != null)
        {
            //write ball passes via webcall
        }


        //figure out and set up next scene
        this.settings.currentCondition += 1;

        switch (this.settings.currentCondition)
        {
            case 1:
                this.settings.currentScene = MTurkSettings.StudyScenes.INS_1;
                SceneManager.LoadScene("MTurk Instructions");
                break;
            case 2:
                this.settings.currentScene = MTurkSettings.StudyScenes.INS_2;
                SceneManager.LoadScene("MTurk Instructions");
                break;
            case 3:
                this.settings.currentScene = MTurkSettings.StudyScenes.INS_3;
                SceneManager.LoadScene("MTurk Instructions");
                break;
            case 4:
                this.settings.currentScene = MTurkSettings.StudyScenes.INS_4;
                SceneManager.LoadScene("MTurk CountSubmit");
                break;
            case 5:
                this.settings.currentScene = MTurkSettings.StudyScenes.INS_5;
                SceneManager.LoadScene("MTurk Instructions");
                break;
            case 6:
                this.settings.currentScene = MTurkSettings.StudyScenes.INS_6;
                SceneManager.LoadScene("MTurk Instructions");
                break;
            default:
                SceneManager.LoadScene("MTurk CountSubmit");
                break;
        }
        //load end scene

      
    }
}
