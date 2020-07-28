using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubmitController : MonoBehaviour
{
    public UnityEngine.UI.InputField inputCount;


    private PHPCommunicationManager commManager;
    private MTurkSettings settings;

    private string folderName;
    private string fileName;

    // Start is called before the first frame update
    void Start()
    {
        this.commManager = GameObject.Find("CommunicationManager").GetComponent<PHPCommunicationManager>();
        this.settings = GameObject.Find("Settings").GetComponent<MTurkSettings>();

        this.fileName = SetFileName();
        //the name of the folder is the last three characters of the filename
        //example: p0pyy will be in folder pyy
        this.folderName = this.fileName.Substring(this.fileName.Length - 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // the filname will always be for "yy"
    string SetFileName()
    {
        string fileName = "";
        if(this.settings.startingEventType == "physicalEvent")
        {
            if(this.settings.currentCondition == 4)
            {
                fileName = "p" + this.settings.pId.ToString() + "pyy";
            }
            else
            {
                fileName = "p" + this.settings.pId.ToString() + "vyy";
            }
        }
        else
        {
            if (this.settings.currentCondition == 4)
            {
                fileName = "p" + this.settings.pId.ToString() + "vyy";
            }
            else
            {
                fileName = "p" + this.settings.pId.ToString() + "pyy";
            }
        }

        return fileName;
    }

    public void SubmitCount()
    {
        int count;
        if(!int.TryParse(inputCount.text, out count))
        {
            inputCount.text = "";
            inputCount.placeholder.GetComponent<UnityEngine.UI.Text>().text = "Please enter a whole number";
        }
        else
        {
            this.commManager.sendPassCount(this.folderName, this.fileName, count, "counted");

            if (this.settings.currentCondition > 6)
            {
                SceneManager.LoadScene("MTurkEnd");
            }
            else
            {
                SceneManager.LoadScene("MTurk Instructions");
            }            
        }
    }
}
