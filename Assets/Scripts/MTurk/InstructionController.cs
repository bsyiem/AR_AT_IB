using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionController : MonoBehaviour
{

    //instruction for each condition
    private string trial = "The next level is a tutorial. " +
        "In this level, you have to react as fast as possible to red lights that turn on and are shown on the screen." +
        "You can react to them by clicking the left mouse button." +
        "These light are small and placed in the top-left, top-right, bottom-left, bottom-right and center of the screen." +
        "Left click to continue";

    private string pnn = "React to the red lights turning on by clicking the left mouse button." +
        "Try not to click the left mouse button when no lights are on." +
        " Left click to continue.";
    private string pyn = "This level contains some added elements. " +
        "You only have to react to the red lights by clicking the left mouse button. " +
        "Try not to click the left mouse button when no lights are on. " +
        "Left click to continue.";
    private string pyy = "This level contains some added elements. You have to count the number of times the red sphere" +
        " is passed (every time it leaves a cube counts as 1 pass)." +
        "You also have to react to the red lights by clicking the left mouse button. Left click to continue.";

    private string vnn = "React to the red lights turning on by clicking the left mouse button." +
        "Try not to click the left mouse button when no lights are on." +
        " Left click to continue.";
    private string vyn = "This level contains some added elements. " +
        "You only have to react to the red lights by clicking the left mouse button. " +
        "Try not to click the left mouse button when no lights are on. " +
        "Left click to continue.";
    private string vyy = "This level contains some added elements. You have to count the number of times the red sphere" +
        " is passed (every time it leaves a cube counts as 1 pass)." +
        "You also have to react to the red lights by clicking the left mouse button. Left click to continue.";

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
        string response = commManager.getLastParticipantNumber(); //i'm unsure if this is blocking. could manually block with a while loop
        int lastPID;
        if (response != null && !response.Equals(""))
        {
            int.TryParse(response, out lastPID);
            settings.pId = lastPID + 1;
        }

        if (settings.pId % 2 == 0)
        {
            settings.startingEventType = "physicalEvent";
        }
        else
        {
            settings.startingEventType = "virtualEvent";
        }

        SetInstructions();
    }

    //void SetInstructions(Scene scene, LoadSceneMode mode)
    void SetInstructions()
    {
        Debug.Log(this.settings.currentScene.ToString());
        Debug.Log(this.settings.currentCondition);

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
