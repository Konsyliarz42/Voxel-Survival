using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Ui_game_control : MonoBehaviour
{
    public Camera mainCamera;
    PostProcessVolume   volume;
    Vignette            vignetteLayer           = null;
    MotionBlur          motionBlurLayer         = null;
    ColorGrading        colorGradingLayer       = null;
    AmbientOcclusion    ambientOcclusionLayer   = null;
    Bloom               bloomLayer              = null;

    public double slider_value;
    public bool vignette, motionBlur, colorGrading, ambietOcclusion, bloom;

    public Text speedtext;
    public Text scoretext;
    public Text endScoretext;
    public Text infotext;
    public Text titletext;
    public Image pauseMenu;
    public Image gameOverMenu;
    public Image blackScreen;

    GameObject player;
    public GameObject map;

    public int score;
    public int score_bonus;
    public int[] score_tab;

    public string launge;
    string info;
    string title;
    string txt_info, txt_title;

    const int time_end = 2;
    
    bool settings_is_loaded;
    bool options_active = false;
    
    //----------------------------------------------------------------

    void Start()
    {
        player = GameObject.Find("Player");
        settings_is_loaded = false;

        score = 0;
        score_bonus = 0;
        info = " ";
        title = " ";
    }

    void Update()
    {
        if(settings_is_loaded == false)    LoadGame();
        if(SceneManager.GetActiveScene().name == "Menu")    settings_is_loaded = false;
        score = score_bonus + (player.GetComponent<Map_generatorV2>().z_num - 1);

        if(score == 1)  if(player.GetComponent<Player_control>().type_control == 0) PrintInfo(10, 3f);  else PrintInfo(11, 3f);
        
        speedtext.text = GetComponent<LaungeData>().other_speed + player.GetComponent<Player_control>().speed;
        scoretext.text = GetComponent<LaungeData>().other_score + score;
        infotext.text = info;
        titletext.text = title;
    }

    public void LoadGame()
    {
        volume = mainCamera.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignetteLayer);
        volume.profile.TryGetSettings(out motionBlurLayer);
        volume.profile.TryGetSettings(out colorGradingLayer);
        volume.profile.TryGetSettings(out ambientOcclusionLayer);
        volume.profile.TryGetSettings(out bloomLayer);

        if(File.Exists(Application.persistentDataPath + "/Save.vsf"))
        {
            GameData data = SaveSystem.LoadGame();
            SetPostProces(data);
            launge = data.launge;
            player.GetComponent<Player_control>().type_control = data.type_control;
            SetLaunge(launge);
        }

        if(File.Exists(Application.persistentDataPath + "/Score.vsf"))
        {
            GameStatus status = SaveSystem.LoadStatus();
            for(int i = 0; i < 16; i++)
                score_tab[i] = status.score_tab[i];
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void SetLaunge(string launge)
    {
        launge = launge.ToUpperInvariant();
        GetComponent<LaungeData>().ChangeLaunge(launge);

        GameObject.Find("Pause Name").GetComponent<Text>().text = GetComponent<LaungeData>().name_pause;
        GameObject.Find("Button Resume").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_resume;
        GameObject.Find("Button Play Again").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_playAgain;
        GameObject.Find("Button Back To Menu").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_backToMenu;
        GameObject.Find("Button Exit").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_exit;

        GameObject.Find("Text End Score").GetComponent<Text>().text = GetComponent<LaungeData>().other_endScore;
        GameObject.Find("Button Play Again 2").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_playAgain;
        GameObject.Find("Button Back To Menu 2").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_backToMenu;
        GameObject.Find("Button Exit 2").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_exit;
    }

    void SetPostProces(GameData data)
    {
        volume.weight = (float)data.slider_value;
        vignetteLayer.enabled.value = data.vignette;
        motionBlurLayer.enabled.value = data.motionBlur;
        colorGradingLayer.enabled.value = data.colorGrading;
        ambientOcclusionLayer.enabled.value = data.ambietOcclusion;
        bloomLayer.enabled.value = data.bloom;

        slider_value = data.slider_value;
        vignette = data.vignette;
        motionBlur = data.motionBlur;
        colorGrading = data.colorGrading;
        ambietOcclusion = data.ambietOcclusion;
        bloom = data.bloom;
    }

    //----------------------------------------------------------------

    IEnumerator waitinfo(string txt, float t)
    {
        info = txt;
        yield return new WaitForSeconds(t);
        info = " ";
    }

    IEnumerator waittitle(string txt, float t)
    {
        title = txt;
        yield return new WaitForSeconds(t);
        title = " ";
    }

    IEnumerator waitonanimation()
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 0;
    }

    IEnumerator waitwithaction(string action)
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;

        if(action == "Exit")    Application.Quit();
        if(action == "Back")    SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        if(action == "Again")   SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //----------------------------------------------------------------

    public void PrintInfo(int number_info, float time)
    {
        if(number_info == 0)    txt_info = GetComponent<LaungeData>().info_endEvent;
        if(number_info == 1)    txt_info = GetComponent<LaungeData>().info_scoreBoard;

        if(number_info == 2)    txt_info = GetComponent<LaungeData>().info_bonus;
        if(number_info == 3)    txt_info = GetComponent<LaungeData>().info_slow;
        if(number_info == 4)    txt_info = GetComponent<LaungeData>().info_free;

        if(number_info == 10)   txt_info = GetComponent<LaungeData>().info_accelerationControl;
        if(number_info == 11)   txt_info = GetComponent<LaungeData>().info_touchControl;


        StartCoroutine(waitinfo(txt_info, time));
    }

    public void PrintTitle(int number_title, float time)
    {
        if(number_title == 0)   txt_title = GetComponent<LaungeData>().title_gameOver;
        if(number_title == 1)   txt_title = GetComponent<LaungeData>().title_endEvent;

        if(number_title == 2)   txt_title = GetComponent<LaungeData>().title_columnColapse;
        if(number_title == 3)   txt_title = GetComponent<LaungeData>().title_columnRises;
        if(number_title == 4)   txt_title = GetComponent<LaungeData>().title_giantLand;
        if(number_title == 5)   txt_title = GetComponent<LaungeData>().title_chaosSwipe;
        if(number_title == 6)   txt_title = GetComponent<LaungeData>().title_upIsDown;
        if(number_title == 7)   txt_title = GetComponent<LaungeData>().title_tunelOfDeath;
        if(number_title == 8)   txt_title = GetComponent<LaungeData>().title_rockDown;
        if(number_title == 9)   txt_title = GetComponent<LaungeData>().title_rideAtNight;

        StartCoroutine(waittitle(txt_title, time));
    }

    public void OpenGameOverMenu()
    {
        Debug.Log("Menu końca gry aktywne");
        endScoretext.text = "" + score;
        SaveSystem.SaveStatus(this);
        
        gameOverMenu.GetComponent<Animator>().SetTrigger("Visual_in");
        StartCoroutine(waitonanimation());
    }

    public void CloseGameOverMenu()
    {
        Debug.Log("Menu końca gry nie aktywne");
        gameOverMenu.GetComponent<Animator>().SetTrigger("Visual_out");
    }

    public void Options()
    {
        if(options_active == false && player.GetComponent<Player_control>().colision == false)
        {
            Debug.Log("Menu opcji aktywne");
            options_active = true;
            
            player.GetComponent<Player_control>().options_active = true;
            pauseMenu.rectTransform.anchoredPosition = new Vector2(0, 0);
            pauseMenu.GetComponent<Animator>().SetTrigger("Swipe_in");

            StartCoroutine(waitonanimation());
        }
    }
    public void Resume()
    {
        Debug.Log("Powrót do gry");
        options_active = false;
        Time.timeScale = 1; 

        player.GetComponent<Player_control>().options_active = false;
        pauseMenu.rectTransform.anchoredPosition = new Vector2(0, 0);
        pauseMenu.GetComponent<Animator>().SetTrigger("Swipe_out");
        Time.timeScale = 1;
    }

    public void PlayAgain()
    {
        Debug.Log("Gra od nowa");
        Time.timeScale = 1;
        options_active = false;

        CloseGameOverMenu();
        map.GetComponent<Column_creator>().col_num = 0;
        blackScreen.GetComponent<Animator>().SetTrigger("Fade_in");
        StartCoroutine(waitwithaction("Again"));
    }

    public void BackToMenu()
    {
        Debug.Log("Przejście do menu");
        Time.timeScale = 1;

        if(options_active == false)
            CloseGameOverMenu();
        
        map.GetComponent<Column_creator>().col_num = 0;
        blackScreen.GetComponent<Animator>().SetTrigger("Fade_in");
        StartCoroutine(waitwithaction("Back"));
        //Application.LoadLevel("Menu");
    }

    public void Exit()
    {
        Debug.Log("Wyjscie z gry");
        Time.timeScale = 1;
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        if(options_active == false)
            CloseGameOverMenu();

        map.GetComponent<Column_creator>().col_num = 0;
        blackScreen.GetComponent<Animator>().SetTrigger("Fade_in");
        StartCoroutine(waitwithaction("Exit")); 
    }
}
