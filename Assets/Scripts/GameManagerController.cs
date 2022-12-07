using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    public GameObject winPanelCanvasObject;
    public StageData previousData;
    public Text timeText;
    public Text levelText;
    public Button homeButton;
    public Button nextButton;
    public Image starsImage;
    public int count;
    public int level;
    public int stars;
    public bool isClear;
    // Start is called before the first frame update
    void Start()
    {
        createUserCar();
        timeText = GameObject.Find("Time_text").GetComponent<Text>();
        levelText = GameObject.Find("Level_text").GetComponent<Text>();
        starsImage = GameObject.Find("Stars_Image").GetComponent<Image>();
        homeButton = GameObject.Find("Home_Button").GetComponent<Button>();
        nextButton = GameObject.Find("Next_Button").GetComponent<Button>();
        winPanelCanvasObject = GameObject.Find("PanelCanvas");
        winPanelCanvasObject.SetActive(false);

        homeButton.onClick.AddListener(OnClickHomeButton);
        nextButton.onClick.AddListener(OnClickNextButton);

        Scene scene = SceneManager.GetActiveScene();


        level = int.Parse(scene.name.Substring(scene.name.Length-1, 1));

        isClear = false;
        count = 0;

        previousData = SaveSystem.LoadStage(level.ToString());

        StartCoroutine("CountRoutine");
    }

    void createUserCar()
    {
        MyCarData mycar = SaveSystem.LoadMyCar();
        string carName;
        if (mycar == null)
        {
            carName = "default";
        }
        else
        {
            carName = mycar.resourceName;
        }
        Debug.Log(mycar);
        Debug.Log(carName);
        GameObject userCarObject = Resources.Load<GameObject>(carName);
        userCarObject.AddComponent<BoxCollider>();
        userCarObject.AddComponent<CarMovement>();
        
        Transform userCarTransform = userCarObject.GetComponent<Transform>();
        userCarTransform.position = new Vector3(-1.5f, 0, -2);
        Instantiate(userCarObject);
    }

    IEnumerator CountRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            count += 1;
        }
    }

    public void OnClickHomeButton() {
        SceneManager.LoadScene("TitleMain");
    }

    public void OnClickNextButton() {
        Debug.Log("클릭");
        SceneManager.LoadScene("InGameScene_lvl" + (level + 1).ToString());
    }

    public void clearGame() {
        setLevel();
        calcClearTime();
        calcStars();
        winPanelCanvasObject.SetActive(true);

        isClear = true;

        saveGame();
        
    }

    public void saveGame() {
        if (previousData == null) {
            SaveSystem.SaveStage(this);
            saveStars(stars);
        } else
        {
            if (previousData.clearCount > count)
            {
                SaveSystem.SaveStage(this);
                saveStars(previousData.stars - stars);
            }
        }

    }

    public void saveStars(int amount) {
        StarsData totalStars = SaveSystem.LoadStars();
        if (totalStars != null) 
        {
            SaveSystem.SaveStars(totalStars.stars + amount);
        } else {
            SaveSystem.SaveStars(amount);
        }
    }

    public void calcClearTime() {
        StopCoroutine("CountRoutine");
        timeText.text = (count / 600).ToString("00") + " : " + (count / 10).ToString("00");
    }

    public void setLevel() {
        levelText.text = "Level " + level.ToString();
    }

    public void calcStars() {
        if (count < 100)
        {
            stars = 3;
            starsImage.fillAmount = 1;
        }
        else if (count < 300)
        {
            stars = 2;
            starsImage.fillAmount = 0.7f;
        }
        else 
        {
            stars = 1;
            starsImage.fillAmount = 0.3f;
        }

    }

}
