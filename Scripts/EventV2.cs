using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventV2 : MonoBehaviour
{
    const int speed_up = 6;
    const int event_quantity = 8;
    const int map_size = Map_generatorV2.map_size;
    const int map_distance = Map_generatorV2.map_distance;

    public int time;        //Używać zamiast Time.time!!!
    int time_beforeStart;
    int time_introduction;  //Czas rozgrzewki jest w "StartGame()"
    int time_freeTime;

    GameObject player_object;               Player_control player;
    public Canvas canvas;                   Ui_game_control Ui;
    public GameObject map_prefab;           Column_creator map;
    public GameObject column_prefab;        Add_force column;
    public GameObject tunel_prefab;         GameObject new_tunel;
    public GameObject rock;                 GameObject new_rock;
    public GameObject directional_light;    Light dir_light;
    public Camera main_camera;

    public bool play = false;

    int event_num;
    int event_columnUp;
    public int event_endPosition;   //Wymaga "TunelOfDeath_Ai.cs"
    public bool event_end;          // -||-

    int column_beforeEvent;
    bool column_randomRotation;
    bool column_randomSwipe;
    bool column_lean;

    bool player_cameraFlip;

    //----------------------------------------------------------------

    void Start()
    {
        play = false;
        StartGame();
    }

    void LateUpdate()
    {
        time = (int)Time.time - time_beforeStart;

        //Kiedy nie jesteś na scenie gry zatrzymaj eventy
        if(SceneManager.GetActiveScene().name == "Menu")
            play = false;
    
        //Przed startem gry wprowadź ustawienia początkowe
        if(play == true)
        {
            //Koniec rozgrzewki
            if(time == time_introduction)
            {
                time_introduction = 0;
                time_freeTime = time + 30;

                map.col_num = 1;
                player.speed_max += speed_up;

                Debug.Log("Koniec rozgrzewki");
            }

            //Rozpoczynanie evnetu
            if(time == time_freeTime)
            {
                time_freeTime = 0;
                event_num++;
                column_beforeEvent = map.col_num;

                RandomEvent();
                event_endPosition = player_object.GetComponent<Map_generatorV2>().z_pos;    //Pobranie pozycji z ostatniego wygenerowanego pasa
                event_endPosition += (map_size * (map_size / 4)) * Random.Range(1, 4);      //Losowanie odległości 16, 32 lub 48 plansz do pokonania

                Debug.Log("Rozpoczęcie eventu");
            }

            //Trwanie eventu
            if(column_randomRotation == true)   map.rotation = new Vector3(90, Random.Range(0, 360), 0);
            if(column_randomSwipe == true)      column.x_force = Random.Range(8, 33) * Random.Range(-1, 2);
            if(column_lean == true)             map.rotation = new Vector3(Random.Range(-90, -120), Random.Range(0, 360), 0);

            //Zakończenie eventu
            if(player_object.transform.position.z > event_endPosition && time > time_freeTime && time_introduction == 0)
            {
                ResetGameSetings();
                event_end = true;

                time_freeTime = time + (30 / Random.Range(1, 3));   //Swobodna jazda przez 15 lub 30 sekund
                player.speed_max += speed_up;

                //Zwiększanie liczby kolumn co 2, 4, 8, 16... eventów
                if(event_columnUp == event_num)
                {
                    map.col_num++;
                    event_columnUp *= 2;
                }

                Debug.Log("Koniec eventu");
            }

            //Nagroda za przetrwany event
            if(event_end == true && player_object.transform.position.z > (event_endPosition + (map_size * map_distance) + map_size))
            {
                Ui.PrintTitle(1, 4f);   //You survived!
                Ui.PrintInfo(0, 4f);    //You get 200 extra points
                    Ui.score_bonus += 200;

                event_end = false;

                Debug.Log("Przyznanie nagrody");
            }
        }
        else if(SceneManager.GetActiveScene().name == "Game")
            StartGame();
    }

    //----------------------------------------------------------------

    void StartGame()
    {
        player_object = GameObject.Find("Player");
        player = player_object.GetComponent<Player_control>();
        Ui = canvas.GetComponent<Ui_game_control>();
        map = map_prefab.GetComponent<Column_creator>();
        column = column_prefab.GetComponent<Add_force>();
        dir_light = directional_light.GetComponent<Light>();

        time_introduction = 8;
        time_freeTime = -1;     //Uniknięcie zrównania czasu po rozpocząciu gry
        event_num = 0;
        event_columnUp = 2;
        column_beforeEvent = 0;

        player.GetComponent<Player_control>().speed_max = 74;   //Po rozgrzewce 80
        ResetGameSetings();        

        play = true;
        time_beforeStart = (int)Time.time;
    }

    void ResetGameSetings()
    {
        column_randomRotation = false;
        column_randomSwipe = false;
        column_lean = false;

        map_prefab.GetComponent<BoxCollider>().enabled = true;

        map.col_num = column_beforeEvent;
        map.rotation = new Vector3(-90, 0, 0);
        map.y_pos = 0;
        map.rockDown = false;

        column_prefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        column_prefab.transform.localScale = new Vector3(1, 1, 1);

        column.rise = false;
        column.x_force = 0; column.x_axis = false;
        column.y_force = 0; column.y_axis = false;
        column.z_force = 0; column.z_axis = false;

        dir_light.intensity = 2.048f;
        dir_light.colorTemperature = 6570f;

        main_camera.backgroundColor = new Color(0.875f, 1f, 0.749f);    //223 225 191

        if(player_cameraFlip == true)
            player_object.GetComponent<Animator>().SetTrigger("Flip_out");

        if(GameObject.Find("Tunel Of Death(Clone)"))
            Destroy(new_tunel, 2f);
    }

    void RandomEvent()
    {
        int event_random = 7;//Random.Range(1, event_quantity + 1);

        Ui.PrintTitle(event_random + 1, 4f);

        switch(event_random)
        {
            default:    Debug.Log("Błąd losowania eventu");    break;

            case 1: ColumnColapse();    break;
            case 2: ColumnRises();      break;
            case 3: GiantLand();        break;
            case 4: ChaosSwipe();       break;
            case 5: UpIsDown();         break;
            case 6: TunelOfDeath();     break;
            case 7: RockDown();         break;
            case 8: RideAtNight();      break;
        }
    }

    //----------------------------------------------------------------

    void ColumnColapse()
    {
        //Limit kolumn na plansze
        if(column_beforeEvent > 2)  map.col_num = 2;

        column_prefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        column.y_force = 16;  column.y_axis = true;
        column.z_force = 16;  column.z_axis = true;

        map.rotation = new Vector3(90, Random.Range(0, 360), 0);
        map.y_pos = 32;

        column_randomRotation = true;
    }

    void ColumnRises()
    {
        map.y_pos = -32;

        column.rise = true;
    }

    void GiantLand()
    {
        //Limit kolumn na plansze
        if(column_beforeEvent > 3)  map.col_num = 3;

        column_prefab.transform.localScale = new Vector3(2, 2, 2);
    }

    void ChaosSwipe()
    {
        column_prefab.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX;
        map_prefab.GetComponent<BoxCollider>().enabled = false;

        column.x_force = Random.Range(4, 25) * Random.Range(-1, 2);
        column.x_axis = true;

        column_randomSwipe = true;
    }

    void UpIsDown()
    {
        player_object.GetComponent<Animator>().SetTrigger("Flip_in");

        player_cameraFlip = true;
    }

    void TunelOfDeath()
    {
        map.col_num = 0;
        new_tunel = Instantiate(tunel_prefab, player_object.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

    void RockDown()
    {
        map.col_num = 2;
        map.rockDown = true;
    }

    void RideAtNight()
    {
        map.y_pos = -4;
        map.rotation = new Vector3(Random.Range(-90, -120), Random.Range(0, 360), 0);

        dir_light.intensity = 0.128f;
        dir_light.colorTemperature = 20000f;

        main_camera.backgroundColor = new Color(0.634f, 0.749f, 0.498f);    //159 191 127

        column_lean = true;
    }
}
