using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Ui_menu_control : MonoBehaviour
{
    public Camera mainCamera;
    PostProcessVolume   volume;
    Vignette            vignetteLayer           = null;
    MotionBlur          motionBlurLayer         = null;
    ColorGrading        colorGradingLayer       = null;
    AmbientOcclusion    ambientOcclusionLayer   = null;
    Bloom               bloomLayer              = null;

    public Image options_menu;
    bool options_open = false;
    public double slider_value;
    public bool vignette, motionBlur, colorGrading, ambietOcclusion, bloom;
    public string launge;
    public int type_control;

    public Image score_board_menu;
    public Text podium;
    public Text score_list;
    bool score_open = false;
    string score_text;

    public Image blackScreen;
    bool settings_is_loaded;
    public int[] score_tab = new int[16];
    const int score_tab_lenght = 16;

    //----------------------------------------------------------------

    void Start()
    {
        slider_value = 1f;
        vignette = true;
        motionBlur = true;
        colorGrading = true;
        ambietOcclusion = true;
        bloom = true;
        launge = "EN";

        //Włączenie gestów jeśli nie obsługiwany jest akcelerometr
        if(SystemInfo.supportsAccelerometer == false)
        {
            type_control = 1;   //Dotyk
            GameObject.Find("Control Accelerometr").GetComponent<Button>().interactable = false;

        }
        else
        {
            type_control = 0;   //Akcelerometr
            GameObject.Find("Control Accelerometr").GetComponent<Button>().interactable = true;
        }

        GameObject.Find("Version").GetComponent<Text>().text = "v" + Application.version;
        GameObject.Find("Game Name").GetComponent<Text>().text = Application.productName;

        settings_is_loaded = false;
    }

    void Update()
    {
        if(settings_is_loaded == false)
            LoadSettings();

        if(SceneManager.GetActiveScene().name == "Game")
            settings_is_loaded = false;
    }

    void LoadSettings()
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
            type_control = data.type_control;

            Debug.Log("Wczytano ustawienia");
        }
        else
        {
            Debug.Log("Nie odnaleziono pliku Save.vsf! Korzystanie z ustawień domyślnych");
            SaveSystem.SaveGame(this);
        }
        
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        settings_is_loaded = true;
        SetLaunge(launge);
    }
    
    void LoadScoreTab()
    {
        if(File.Exists(Application.persistentDataPath + "/Score.vsf"))
        {
            GameStatus status = SaveSystem.LoadStatus();
            for(int i = 0; i < score_tab_lenght; i++)
                score_tab[i] = status.score_tab[i];
            Debug.Log("Wczytano tablice wyników");
        }
        else
        {
            Debug.Log("Brak tablicy wyników!");
        }
    }

    void SetLaunge(string launge)
    {
        launge = launge.ToUpperInvariant();
        GetComponent<LaungeData>().ChangeLaunge(launge);

        GameObject.Find("Button Play").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_play;
        GameObject.Find("Button Score Board").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_scoreBoard;
        GameObject.Find("Button Options").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_options;
        GameObject.Find("Button Exit").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_exit;

        GameObject.Find("ScoreBoard Name").GetComponent<Text>().text = GetComponent<LaungeData>().name_scoreBoard;
            GameObject.Find("ScoreBoard Info").GetComponent<Text>().text = GetComponent<LaungeData>().info_scoreBoard;

        GameObject.Find("Options Name").GetComponent<Text>().text = GetComponent<LaungeData>().name_options;
        GameObject.Find("Options Info").GetComponent<Text>().text = GetComponent<LaungeData>().other_optionsInfo;
            GameObject.Find("Options Post-Proces").GetComponent<Text>().text = GetComponent<LaungeData>().name_postProces;
                GameObject.Find("PostProces Name").GetComponent<Text>().text = GetComponent<LaungeData>().name_postProcesSlider;
                GameObject.Find("Vignette Toggle").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().toggle_vignette;
                GameObject.Find("Motion Blur Toggle").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().toggle_motionBlur;
                GameObject.Find("Color Grading Toggle").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().toggle_colorGrading;
                GameObject.Find("Ambient Occlusion Toggle").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().toggle_ambientOcclusion;
                GameObject.Find("Bloom Toggle").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().toggle_bloom;
        GameObject.Find("Options Launge").GetComponent<Text>().text = GetComponent<LaungeData>().name_launge;
            GameObject.Find("Launge EN").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_laungeEN;
            GameObject.Find("Launge PL").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_laungePL;
            GameObject.Find("Launge GER").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_laungeGER;
        GameObject.Find("Options Control").GetComponent<Text>().text = GetComponent<LaungeData>().name_control;
            if(SystemInfo.supportsAccelerometer == true)
            {
                GameObject.Find("Control Info").GetComponent<Text>().color =  Color.white;
                if(type_control == 0)   GameObject.Find("Control Info").GetComponent<Text>().text = GetComponent<LaungeData>().other_controlInfoAccelerometr;
                else                    GameObject.Find("Control Info").GetComponent<Text>().text = GetComponent<LaungeData>().other_controlInfoTouch;
            }
            else
            {
                GameObject.Find("Control Info").GetComponent<Text>().color =  Color.red;
                GameObject.Find("Control Info").GetComponent<Text>().text = GetComponent<LaungeData>().other_controlWarring;
            }
            GameObject.Find("Control Accelerometr").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_accelerometer;
            GameObject.Find("Control Touch").GetComponentInChildren<Text>().text = GetComponent<LaungeData>().button_touch;
    }

    void SetPostProces(GameData data)
    {
        volume.weight = (float)data.slider_value;
        vignetteLayer.enabled.value = data.vignette;
        motionBlurLayer.enabled.value = data.motionBlur;
        colorGradingLayer.enabled.value = data.colorGrading;
        ambientOcclusionLayer.enabled.value = data.ambietOcclusion;
        bloomLayer.enabled.value = data.bloom;

        GameObject.Find("PostProces Slider").GetComponent<Slider>().value = (float)data.slider_value;
        GameObject.Find("Vignette Toggle").GetComponent<Toggle>().isOn = data.vignette;
        GameObject.Find("Motion Blur Toggle").GetComponent<Toggle>().isOn = data.motionBlur;
        GameObject.Find("Color Grading Toggle").GetComponent<Toggle>().isOn = data.colorGrading;
        GameObject.Find("Ambient Occlusion Toggle").GetComponent<Toggle>().isOn = data.ambietOcclusion;
        GameObject.Find("Bloom Toggle").GetComponent<Toggle>().isOn = data.bloom;

        slider_value = data.slider_value;
        vignette = data.vignette;
        motionBlur = data.motionBlur;
        colorGrading = data.colorGrading;
        ambietOcclusion = data.ambietOcclusion;
        bloom = data.bloom;

    //    Debug.Log("data: " + data.vignette + " , gra: " + vignette);
    }

    //----------------------------------------------------------------

    IEnumerator waitwithaction(string action)
    {
        yield return new WaitForSecondsRealtime(2);

        if(action == "Exit")    Application.Quit();
        if(action == "Play")    SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //----------------------------------------------------------------

    public void Play()
    {
        Debug.Log("Rozpoczęcie gry");
        if(score_open == true)    ScoreBoard();
        if(options_open == true)    Options();
            
        blackScreen.GetComponent<Animator>().SetTrigger("Fade_in");
        StartCoroutine(waitwithaction("Play"));
        //Application.LoadLevel("Game");
    }

    //================================================================================================================================

    public void ScoreBoard()
    {
        if(score_open == false)
        {
            if(options_open == true)    Options();

            PrintScoreBoard();
            score_board_menu.GetComponent<Animator>().SetTrigger("Swipe_in");
            score_open = true;
        }
        else
        {
            score_board_menu.GetComponent<Animator>().SetTrigger("Swipe_out");
            score_open = false;
        }
    }
    void PrintScoreBoard()
    {
        int i, j;

        LoadScoreTab();

        if(File.Exists(Application.persistentDataPath + "/Score.vsf"))
        {
            score_text = "";
            for(i = 0; i < 3; i++)
                if(score_tab[i] != 0)
                    score_text +=  (i + 1) + ". " + score_tab[i] + Environment.NewLine;
                else
                    i = 2;
            podium.text = score_text;

            score_text = "";
            for(j = i; j < score_tab_lenght; j++)
                if(score_tab[j] > 0)
                {
                    if((j + 1) < 10)
                        score_text += "  " + (j + 1) + ". " + score_tab[j] + Environment.NewLine;
                    else
                        score_text += (j + 1) + ". " + score_tab[j] + Environment.NewLine;
                }
                else
                    j = score_tab_lenght;
            
            score_list.text = score_text;
        }
        else
        {
            podium.text = GetComponent<LaungeData>().other_noData;
            score_list.text = "";
        }
        
    }

    //================================================================================================================================

    public void Options()
    {
        if(options_open == false)
        {
            if(score_open == true)    ScoreBoard();

            PostProcesSlider();
            options_menu.GetComponent<Animator>().SetTrigger("Swipe_in");
            options_open = true;
        }
        else
        {
            SaveSystem.SaveGame(this);
            options_menu.GetComponent<Animator>().SetTrigger("Swipe_out");
            options_open = false;
        }
    }

    public void PostProcesSlider()
    {
        slider_value = Math.Round((double)GameObject.Find("PostProces Slider").GetComponent<Slider>().value, 3);    //Ograniczenie do 3 miejsc po przecinku
        GameObject.Find("PostProces Slider").GetComponentInChildren<Text>().text = (slider_value * 100) + "%";      //Wyświetlanie procentowe
        volume.weight = (float)slider_value;
    }

    public void ToggleVignette()
    {
        if(vignette == false)   vignette = true;
        else                    vignette = false;

        vignetteLayer.enabled.value = vignette;
    }
    public void ToggleMotionBlur()
    {
        if(motionBlur == false) motionBlur = true;
        else                    motionBlur = false;

        motionBlurLayer.enabled.value = motionBlur;
    }
    public void ToggleColorGrading()
    {
        if(colorGrading == false)   colorGrading = true;
        else                        colorGrading = false;

        colorGradingLayer.enabled.value = colorGrading;
    }
    public void ToggleAmbientOcclusion()
    {
        if(ambietOcclusion == false)    ambietOcclusion = true;
        else                            ambietOcclusion = false;

        ambientOcclusionLayer.enabled.value = ambietOcclusion;
    }
    public void ToggleBloom()
    {
        if(bloom == false)  bloom = true;
        else                bloom = false;

        bloomLayer.enabled.value = bloom;
    }

    public void LaungeEN()
    {
        launge = "EN";
        SetLaunge(launge);
        SaveSystem.SaveGame(this);
    }
    public void LaungePL()
    {
        launge = "PL";
        SetLaunge(launge);
        SaveSystem.SaveGame(this);
    }
    public void LaungeGER()
    {
        launge = "GER";
        SetLaunge(launge);
        SaveSystem.SaveGame(this);
    }

    public void ControlAccelerometr()
    {
        if(SystemInfo.supportsAccelerometer == true)
            GameObject.Find("Control Info").GetComponent<Text>().text = GetComponent<LaungeData>().other_controlInfoAccelerometr;
        type_control = 0;
    }
    public void ControlTouch()
    {
        if(SystemInfo.supportsAccelerometer == true)
            GameObject.Find("Control Info").GetComponent<Text>().text = GetComponent<LaungeData>().other_controlInfoTouch;
        type_control = 1;
    }

    //================================================================================================================================

    public void Exit()
    {
        Debug.Log("Koniec gry");
        if(score_open == true)    ScoreBoard();
        if(options_open == true)    Options();

        blackScreen.GetComponent<Animator>().SetTrigger("Fade_in");
        StartCoroutine(waitwithaction("Exit"));
    }
}
