using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class gameManager : MonoBehaviour
{
    public RawImage countyImage;
    public Texture2D image;
    private Text hintText;
    private Text scoreText;
    private int score = 0;
    private int hints = 0;
    private string filepath;
    private string imagepath;
    public List<string> countiesList;
    public List<string> remainingCounties;
    public string correctCounty;
    public List<string> threeCounties;
    public List<string> fourCounties;
    public Button buttonA, buttonB, buttonC, buttonD;

    // Start is called before the first frame update
    void Start()
    {
        hintText = GameObject.Find("hintText").GetComponent<Text>();
        scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        updateHints();
        updateScore();
        filepath = "Assets/countiesList.txt";
        loadCounties();
        pickCounty();
    }

    // Update is called once per frame
    void Update()
    {
        //countyImage.texture = image;
    }

    public void setScore(int s)
    {
        score += s;
        updateScore();
    }

    public int getScore()
    {
        return score;
    }

    void updateScore()
    {
        scoreText.text = "Score:\t" + getScore();
    }

    public void setHints(int h)
    {
        hints += h;
        updateHints();
    }

    public int getHints()
    {
        return hints;
    }

    void updateHints()
    {
        hintText.text = "Hints:\t" + getHints();
    }

    void loadCounties()
    {
        StreamReader reader = new StreamReader(filepath);
        string line;
        while((line = reader.ReadLine()) != null)
        {
            countiesList.Add(line);
            remainingCounties.Add(line);
        }
        reader.Close();
    }

    void pickCounty()
    {
        System.Random random = new System.Random();
        int index = random.Next(remainingCounties.Count);

        if(remainingCounties[index] != "")
        {
            correctCounty = remainingCounties[index];
            loadCountyImage();

            //remainingCounties.RemoveAt(index);
            // foreach (string county in remainingCounties)
            // {
            //     if(county == correctCounty)
            //     {
            //         remainingCounties[remainingCounties.IndexOf(county)] = "";
            //     }
            // }

            for(int i = 0; i < remainingCounties.Count; i++)
            {
                if(remainingCounties[i] == correctCounty)
                {
                    remainingCounties[i] = "";
                }
            }

            pick3Counties();
        }

        else
        {
            pickCounty();
        }

        
    }

    void pick3Counties()
    {
        System.Random random = new System.Random();
        int index;
        string[] pickedCounties = {"","","",correctCounty};
        for(int i = 0; i < 3; i++)
        {
            bool picked = false;
            // index = random.Next(countiesList.Count);
            // Debug.Log("Pick 3 counties index - " + index);
            // foreach (string ind in pickedCounties)
            // {
            //     if(countiesList[index] != ind)
            //     {
            //         pickedCounties[i] = countiesList[index];
            //         threeCounties.Add(countiesList[index]);
            //     }
            //     else
            //     {
            //         i--;
            //     }
            // }

            index = random.Next(countiesList.Count);
            Debug.Log("Pick 3 counties index - " + index);

            for(int j = 0; j <= 3; j++)
            {
                if(countiesList[index] == pickedCounties[j])
                {
                    picked = true;
                }
            }

            if(!picked)
            {
                pickedCounties[i] = countiesList[index];
                threeCounties.Add(countiesList[index]);
            }

            else
            {
                i--;
            }

        }
        setButtonText();
    }

    void loadCountyImage()
    {
        imagepath = "Assets/images/counties/" + correctCounty + ".png";
        if(File.Exists(imagepath))
        {
            byte[] img = System.IO.File.ReadAllBytes(imagepath);
            image = new Texture2D(650,650);
            image.LoadImage(img);
            countyImage.texture = image;
        }
        //countyImage.texture = image;
    }

    void setButtonText()
    {
        foreach (string county in threeCounties)
        {
            fourCounties.Add(county);
        }
        fourCounties.Add(correctCounty);
        fourCounties = shuffleList(fourCounties);
        
        buttonA.GetComponentInChildren<Text>().text = fourCounties[0];
        buttonB.GetComponentInChildren<Text>().text = fourCounties[1];
        buttonC.GetComponentInChildren<Text>().text = fourCounties[2];
        buttonD.GetComponentInChildren<Text>().text = fourCounties[3];

    }

    List<string> shuffleList(List<string> l)
    {
        System.Random rand = new System.Random();
        for(int i = 0; i < l.Count; i++)
        {
            int r = rand.Next(i, l.Count);
            string tmp = l[i];
            l[i] = l[r];
            l[r] = tmp;
        }

        return l;
    }

    public void optionButtonClicked(Button button)
    {
        string buttonText = button.GetComponentInChildren<Text>().text;
        Debug.Log(buttonText + " pressed");
        if(checkAnswer(buttonText))
        {
            setScore(1);
        }

        while(remainingCounties.Count > 0)
        {
            pickCounty();
        }
    }

    bool checkAnswer(string ans)
    {
        return(ans == correctCounty);
    }
}
