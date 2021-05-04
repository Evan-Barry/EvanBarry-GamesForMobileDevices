using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using GooglePlayGames;

public class gameManager : MonoBehaviour
{
    public RawImage countyImage;
    public Texture2D image;
    private Text hintText;
    private Text scoreText;
    private int score = 0;
    private int hints = 0;
    public int hintsUsed = 0;
    private string filepath;
    private string imagepath;
    public List<string> countiesList;
    public List<string> remainingCounties;
    public string correctCounty;
    public List<string> threeCounties;
    public List<string> fourCounties;
    public Button buttonA, buttonB, buttonC, buttonD, useHintButton;
    private System.Random random;
    private int pickCountyIndex;
    public int streak = 0;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        hintText = GameObject.Find("hintText").GetComponent<Text>();
        scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        updateHints();
        updateScore();
        //filepath = "Assets/Resources/countiesList.txt";
        filepath = "countiesList";
        loadCounties();
        pickCounty();
        Debug.Log("Welcome, " + PlayerPrefs.GetString("playerName") + "!");
    }

    // Update is called once per frame
    void Update()
    {
        //countyImage.texture = image;
        if(hints > 0)
        {
            Debug.Log("There are hints");
            useHintButton.interactable = true;
        }

        else
        {
            useHintButton.interactable = false;
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            hints++;
        }
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
        TextAsset file = Resources.Load<TextAsset>(filepath);
        //StreamReader reader = new StreamReader(filepath);
        List<string> lines = new List<string>(file.text.Split('\n'));
        //while((line = reader.ReadLine()) != null)
        for(int i = 0 ; i < lines.Count; i++)
        {
            if(i == lines.Count-1)
            {
                countiesList.Add(lines[i]);
                remainingCounties.Add(lines[i]);
            }
            else
            {
                countiesList.Add(lines[i].Substring(0, lines[i].Length-1));
                remainingCounties.Add(lines[i].Substring(0, lines[i].Length-1));
            }
        }
        // foreach (string line in lines)
        // {
        //     countiesList.Add(line.Substring(0, line.Length-1));
        //     remainingCounties.Add(line.Substring(0, line.Length-1));
        // }
        //reader.Close();
    }

    void pickCounty()
    {
        pickCountyIndex = random.Next(remainingCounties.Count);

        if(remainingCounties[pickCountyIndex] != null)
        {
            correctCounty = remainingCounties[pickCountyIndex];
            //correctCounty = correctCounty.Substring(0, correctCounty.Length-1);
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
                    remainingCounties[i] = null;
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
        //System.Random random = new System.Random();
        threeCounties = new List<string>();
        int index;
        string[] pickedCounties = {"","","",correctCounty};
        for(int i = 0; i < 3; i++)
        {
            bool picked = false;

            index = random.Next(countiesList.Count);

            for(int j = 0; j <= 3; j++)
            {
                if(countiesList[index] == pickedCounties[j])
                {
                    picked = true;
                    Debug.Log("Pick 3 counties index - " + index + " - already picked");
                }
            }

            if(!picked)
            {
                Debug.Log("Pick 3 counties index - " + index);
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
        //correctCounty = correctCounty.Substring(0, correctCounty.Length-1);
        imagepath = "images/"+correctCounty;
        image = Resources.Load<Texture2D>(imagepath);
        countyImage.texture = image;
        //Debug.Log(imagepath);
        //Debug.Log(Application.dataPath);
        // if(image != null)
        // {
        //     byte[] img = System.IO.File.ReadAllBytes(imagepath);
        //     image = new Texture2D(650,650);
        //     image.LoadImage(img);
        //     countyImage.texture = image;
        // }
        // else
        // {
        //     Debug.Log("Image doesn't exist!?!");
        // }
        //countyImage.texture = image;
    }

    void setButtonText()
    {
        fourCounties = new List<string>();
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
        //System.Random rand = new System.Random();
        for(int i = 0; i < l.Count; i++)
        {
            int r = random.Next(i, l.Count);
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
            streak++;
            checkStreak();
        }

        else
        {
            streak = 0;
        }

        if(countiesRemain())
        {
            pickCounty();
            setAllOptionButtonsInteractable(true);
        }

        else
        {
            gameOver();
        }
    }

    public void hintButtonClicked(Button button)
    {
        hints--;
        hintsUsed++;
        updateHints();

        string incorrectCounty1 = threeCounties[0];
        string incorrectCounty2 = threeCounties[1];

        if(buttonA.GetComponentInChildren<Text>().text == incorrectCounty1 || buttonA.GetComponentInChildren<Text>().text == incorrectCounty2)
        {
            buttonA.interactable = false;
        }

        if(buttonB.GetComponentInChildren<Text>().text == incorrectCounty1 || buttonB.GetComponentInChildren<Text>().text == incorrectCounty2)
        {
            buttonB.interactable = false;
        }

        if(buttonC.GetComponentInChildren<Text>().text == incorrectCounty1 || buttonC.GetComponentInChildren<Text>().text == incorrectCounty2)
        {
            buttonC.interactable = false;
        }

        if(buttonD.GetComponentInChildren<Text>().text == incorrectCounty1 || buttonD.GetComponentInChildren<Text>().text == incorrectCounty2)
        {
            buttonD.interactable = false;
        }

    }

    void setAllOptionButtonsInteractable(bool i)
    {
        buttonA.interactable = i;
        buttonB.interactable = i;
        buttonC.interactable = i;
        buttonD.interactable = i;
    }

    bool countiesRemain()
    {
        foreach (string county in remainingCounties)
        {
            if(county != null)
            {
                Debug.Log("There are counties remaining");
                return true;
            }
        }
        Debug.Log("There are NO counties remaining");
        return false;
    }

    void gameOver()
    {
        setAllOptionButtonsInteractable(false);
        updateLeaderboardScore();
        Debug.Log("Game Over! You scored " + score + "/32");
    }

    bool checkAnswer(string ans)
    {
        return(ans == correctCounty);
    }

    void checkStreak()
    {
        switch (streak)
        {
            case 5:
                Debug.Log("You got 5 in a row!");
                Social.ReportProgress(GPGSIds.achievement_5_in_a_row, 100f, null);
                break;
            case 10:
                Debug.Log("You got 10 in a row!");
                Social.ReportProgress(GPGSIds.achievement_10_in_a_row, 100f, null);
                break;
            case 20:
                Debug.Log("You got 20 in a row!");
                Social.ReportProgress(GPGSIds.achievement_20_in_a_row, 100f, null);
                break;
            case 32:
                if(hintsUsed > 0)
                {
                    Debug.Log("You got 32 in a row!");
                    Social.ReportProgress(GPGSIds.achievement_32_in_a_row, 100f, null);
                }
                else if(hintsUsed == 0)
                {
                    Debug.Log("Perfect Game!");
                    Social.ReportProgress(GPGSIds.achievement_perfect_game, 100f, null);
                }
                break;
        }
    }

    public void openAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    public void openLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void updateLeaderboardScore()
    {
        Social.ReportScore(score, GPGSIds.leaderboard_high_score, null);
    }
}
