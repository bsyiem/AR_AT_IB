using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionController : MonoBehaviour
{

    //instruction for each condition
    private string trial = "The next level is a tutorial.\n\n" +
        "- In this level, you have to react as fast as possible to red lights that turn on and are shown on the screen.\n" +
        "- These lights turn on at RANDOM INTERVALS.\n" +
        "- These lights can either be images of REAL lights or may be VIRTUAL lights.\n" +
        "- you have to react to any RED LIGHT turning on regardless of whether they are virtual or real.\n"+
        "- You can react to them by CLICKING THE LEFT MOUSE BUTTON.\n" +
        "- These light are small and placed in the top-left, top-right, bottom-left, bottom-right and center of the screen.\n\n"+
        "Left click to continue";

    private string pnn = "- React to the REAL red lights turning on by clicking the left mouse button.\n" +
        "- Try not to click the left mouse button when no lights are on.\n\n" +
        " Left click to continue.";
    private string pyn = "-This level contains some added elements.\n" +
        "- You only have to react to the REAL red lights by clicking the left mouse button.\n" +
        "- Try not to click the left mouse button when no lights are on.\n\n" +
        "Left click to continue.";
    private string pyy = "- This level contains some added elements.\n" +
        "- You have to count the number of times the red sphere is passed (every time it leaves a cube counts as 1 pass).\n" +
        "- You also have to react to the REAL red lights by clicking the left mouse button.\n\n" +
        "Left click to continue.";

    private string vnn = "- React to the VIRTUAL red lights turning on by clicking the left mouse button.\n" +
        "- Try not to click the left mouse button when no lights are on.\n\n" +
        " Left click to continue.";
    private string vyn = "- This level contains some added elements.\n" +
        "- You only have to react to the VIRTUAL red lights by clicking the left mouse button.\n" +
        "- Try not to click the left mouse button when no lights are on.\n\n" +
        "Left click to continue.";
    private string vyy = "- This level contains some added elements.\n" +
        "- You have to count the number of times the red sphere is passed (every time it leaves a cube counts as 1 pass).\n" +
        "- You also have to react to the VIRTUAL red lights by clicking the left mouse button.\n\n" +
        " Left click to continue.";

    private MTurkSettings settings;
    private PHPCommunicationManager commManager;

    private UnityEngine.UI.Text canvasText;

    private static InstructionController _instance;



    private void Awake()
    {
        this.canvasText = GameObject.Find("Instructions").GetComponentInChildren<UnityEngine.UI.Text>();
        this.settings = GameObject.Find("Settings").GetComponent<MTurkSettings>();
        this.commManager = GameObject.Find("CommunicationManager").GetComponent<PHPCommunicationManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        if(settings.currentScene == MTurkSettings.StudyScenes.OPN)
        {
            SetUpParticipantValues();
        }
 
        SetInstructions();
    }

    //sets participant ID and starting conditions
    void SetUpParticipantValues()
    {
        string response = commManager.getLastParticipantNumber();
        int lastPID;
        if (response != null && !response.Equals(""))
        {
            int.TryParse(response, out lastPID);
            settings.pId = lastPID + 1;
        }

        if (settings.pId != -1)
        {
            //sets up the participant number, so that the next pId can be determined
            this.commManager.SetParticipantNumber(this.settings.pId);
        }

        Debug.Log(settings.pId);

        if (settings.pId % 2 == 0)
        {
            settings.startingEventType = "physicalEvent";
        }
        else
        {
            settings.startingEventType = "virtualEvent";
        }
    }

    //void SetInstructions(Scene scene, LoadSceneMode mode)
    void SetInstructions()
    {
        //Debug.Log(this.settings.currentScene.ToString());
        //Debug.Log(this.settings.currentCondition);

        switch (this.settings.currentScene)
        {
            case MTurkSettings.StudyScenes.OPN:
                canvasText.text = "Left Click to read instructions for the next task.";
                break;
            case MTurkSettings.StudyScenes.INS_1:
                if (this.settings.startingEventType == "physicalEvent")
                {
                    canvasText.text = pnn;
                }
                else
                {
                    canvasText.text = vnn;
                }
                break;
            case MTurkSettings.StudyScenes.INS_2:
                if (this.settings.startingEventType == "physicalEvent")
                {
                    canvasText.text = pyn;
                }
                else
                {
                    canvasText.text = vyn;
                }
                break;
            case MTurkSettings.StudyScenes.INS_3:
                if (this.settings.startingEventType == "physicalEvent")
                {
                    canvasText.text = pyy;
                }
                else
                {
                    canvasText.text = vyy;
                }
                break;
            case MTurkSettings.StudyScenes.INS_4:
                if (this.settings.startingEventType == "physicalEvent")
                {
                    canvasText.text = vnn;
                }
                else
                {
                    canvasText.text = pnn;
                }
                break;
            case MTurkSettings.StudyScenes.INS_5:
                if (this.settings.startingEventType == "physicalEvent")
                {
                    canvasText.text = vyn;
                }
                else
                {
                    canvasText.text = pyn;
                }
                break;
            case MTurkSettings.StudyScenes.INS_6:
                if (this.settings.startingEventType == "physicalEvent")
                {
                    canvasText.text = vyy;
                }
                else
                {
                    canvasText.text = pyy;
                }
                break;
            default:
                canvasText.text = "";
                break;
        }
    }
        

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            switch (settings.currentScene)
            {
                case MTurkSettings.StudyScenes.OPN:
                    settings.currentScene = MTurkSettings.StudyScenes.TUTOTIAL;
                    canvasText.text = trial;
                    break;
                case MTurkSettings.StudyScenes.TUTOTIAL:
                    settings.currentScene = MTurkSettings.StudyScenes.EXP;
                    SceneManager.LoadScene("MTurk Tutorial");
                    break;
                case MTurkSettings.StudyScenes.INS_1:
                    settings.currentScene = MTurkSettings.StudyScenes.EXP;
                    SceneManager.LoadScene("MTurkNoAR");
                    break;
                case MTurkSettings.StudyScenes.INS_2:
                    settings.currentScene = MTurkSettings.StudyScenes.EXP;
                    SceneManager.LoadScene("MTurkAR");
                    break;
                case MTurkSettings.StudyScenes.INS_3:
                    settings.currentScene = MTurkSettings.StudyScenes.EXP;
                    SceneManager.LoadScene("MTurkAR");
                    break;
                case MTurkSettings.StudyScenes.INS_4:
                    settings.currentScene = MTurkSettings.StudyScenes.EXP;
                    SceneManager.LoadScene("MTurkNoAR");
                    break;
                case MTurkSettings.StudyScenes.INS_5:
                    settings.currentScene = MTurkSettings.StudyScenes.EXP;
                    SceneManager.LoadScene("MTurkAR");
                    break;
                case MTurkSettings.StudyScenes.INS_6:
                    settings.currentScene = MTurkSettings.StudyScenes.EXP;
                    SceneManager.LoadScene("MTurkAR");
                    break;
                default:
                    break;
            }
        }
    }
}
