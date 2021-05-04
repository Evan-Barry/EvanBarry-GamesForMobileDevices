using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startScreenScript : MonoBehaviour
{
    public InputField name;
    public string nameString;

    // Start is called before the first frame update
    void Start()
    {
        name = GameObject.Find("Name").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void send()
    {
        //nameString = GameObject.Find("Name").GetComponent<InputField>().text;
        if(name.text != "")
        {
            PlayerPrefs.SetString("playerName",name.text);
            SceneManager.LoadScene("MainScreen");
        }
        else
        {

        }
        //Debug.Log(nameString);
    }
}
