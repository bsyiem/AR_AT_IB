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

    //records the current LED number reacted to
    int currentLEDNumber;

    //keeps track of scene condition
    int currenSceneCondtion;

    //sets folderName
    private string folderName;
    //sets fileName
    private string fileName;

    // Start is called before the first frame update
    void Start()
    {
        this.settings = GameObject.Find("Settings").GetComponent<MTurkSettings>();
        this.commManager = GameObject.Find("CommunicationManager").GetComponent<PHPCommunicationManager>();

        //if its the trial, use phhysical leds
        //else set up based on Participant ID
        if (this.settings.currentCondition == 0)
        {
            this.eventType = "physicalEvent";
        }
        else
        {
            this.setEventType();
        }

        //load appropriate background
        if (this.eventType == "physicalEvent")
        {
            this.bgimage.sprite = this.physicalEvents[5];
        }
        else
        {
            this.bgimage.sprite = this.virtualEvents[5];
        }

        this.fileName = this.SetFileName();

        //the name of the folder is the last three characters of the filename
        //example: p0pyy will be in folder pyy
        if (this.settings.currentCondition != 0)
            this.folderName = this.fileName.Substring(this.fileName.Length - 3);
    }

    //sets the fileName
    string SetFileName()
    {
        if(this.settings.currentCondition == 0)
        {
            return null;
        }

        // 1 = nn
        // 2 = yn
        // 0 = yy
        int eventBasedConditionNumber = this.settings.currentCondition % 3;

        switch (eventBasedConditionNumber)
        {
            case 0:
                return "p" + this.settings.pId.ToString() + this.eventType[0] + "yy";
            case 1:
                return "p" + this.settings.pId.ToString() + this.eventType[0] + "nn";
            case 2:
                return "p" + this.settings.pId.ToString() + this.eventType[0] + "yn";
        }

        return "";

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

                //starts ball counting
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
                    // -1 denotes NA
                    if(this.settings.currentCondition != 0)
                    {
                        this.commManager.sendReactionTimeEvent(this.folderName, this.fileName, "FP", -1, -1);
                    }
                    
                }
                else
                {
                    //Debug.Log("reacted = " + this.reactionTime);

                    //webcall write this.reaction time via webcall
                    if (this.settings.currentCondition != 0)
                    {
                        this.commManager.sendReactionTimeEvent(this.folderName, this.fileName, "TP", this.reactionTime, this.currentLEDNumber);
                    }

                    this.reactionTime = 0.0f;//reset reaction time
                    this.isLedOn = false;

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
            this.currentLEDNumber = number;

            if (isLedOn)
            {
                //Debug.Log(this.reactionTime);

                //write missed event/ false negative reaction and seconds with this.eventType
                //this will be a webcall 
                if (this.settings.currentCondition != 0)
                {
                    this.commManager.sendReactionTimeEvent(this.folderName, this.fileName, "FN", this.reactionTime, lastLedNumber);
                }

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
        
        //wait before writing
        yield return (new WaitForSeconds(Random.Range(this.waitTimeMin, this.waitTimeMin)));

        //check if the last led was on
        if (isLedOn)
        {
            //Debug.Log(this.reactionTime);

            //write missed event/ false negative reaction and seconds with this.eventType
            //this will be a webcall 
            if (this.settings.currentCondition != 0)
            {
                this.commManager.sendReactionTimeEvent(this.folderName, this.fileName, "FN", this.reactionTime, lastLedNumber);
            }

            this.reactionTime = 0.0f; //reset reaction time
        }

        //wait before writing
        //fixing order issues
        yield return (new WaitForSeconds(Random.Range(1f, 1f)));

        if (this.ball != null)
        {
            //write ball passes via webcall
            this.commManager.sendPassCount(this.folderName, this.fileName, this.ball.getPassedNumber(), "actual");
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
