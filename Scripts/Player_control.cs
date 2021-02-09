using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_control : MonoBehaviour
{
    GameObject control;

    GameObject slowStatus;
    GameObject freeStatus;

    public GameObject map;
    public Canvas canvas;

    Vector3 touchPosition;

    public int speed, speed_max, speed_bost;    //Speed_max jest ustawiany w "Event.cs" (StartGame())
    public bool colision = false;
    public bool options_active = false;
    bool mobile_control = false;
    public int type_control;    //Typ sterowania jest pobierany z pliku "Saves.vsf", wczytuje go "Ui_game_control.cs" (LoadGame())

    int time = 0;
    int timeSlow;
    int timeFree;

    //----------------------------------------------------------------

    void OneOnSecond()
    {
        if(time < (int)Time.time)
        {
            time = (int)Time.time;

            timeSlow--;
            timeFree--;
        }
    }

    void Start()
    {
        //Rozpoznanie środowiska
        if(Application.platform == RuntimePlatform.Android) mobile_control = true;
        else                                                mobile_control = false;
            
        //Pozycja początkowa
        transform.position = new Vector3(0.0f, 4.096f, -32.0f);

        //Połączenie z Event
        control = GameObject.Find("Event");

        //Połączenie z wyświetlaczami
        slowStatus = GameObject.Find("Cube Slow Status");
        freeStatus = GameObject.Find("Cube Free Status");
    }

    void Update()
    {
        //Płynna zmiana prędkości
        if(speed < speed_max + speed_bost)  speed++;
        else  if(speed > speed_max + speed_bost)  speed--;
    
        if( colision == false
        &&  options_active == false)
        {
            //Stałe poruszanie się do przodu
            transform.position += Vector3.forward * (Time.deltaTime * speed);

            //Sterowanie klawiszami jeśli uruchamiany w edytorze
            if(mobile_control == false)
                Control_keyboard();

            //Sterowanie na platformie mobilnej
            if(type_control == 0)   Control_acceleration();
            else                    Control_touch();
        }

        OneOnSecond();
        slowStatus.GetComponentInChildren<Text>().text = "" + timeSlow;
        freeStatus.GetComponentInChildren<Text>().text = "" + timeFree;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Wykryto kolizję z " + other.gameObject.name);

        if( other.gameObject.name == "Column(Clone)"
        ||  other.gameObject.name == "Left" || other.gameObject.name == "Right"
        ||  other.gameObject.name == "Rock")
        {
            speed_max = 0;
            speed_bost = 0;
            colision = true;

            canvas.GetComponent<Ui_game_control>().PrintTitle(0, 4f); //Koniec gry
            canvas.GetComponent<Ui_game_control>().OpenGameOverMenu();

            Debug.Log("Koniec gry");

            control.GetComponent<Event>().ResetGameSetings();
            map.GetComponent<Column_creator>().col_num = 0;
        }
        
        if( other.gameObject.name == "Bonus cube(Clone)"
        ||  other.gameObject.name == "Bonus cube")
        {
            canvas.GetComponent<Ui_game_control>().score_bonus += 100;
            canvas.GetComponent<Ui_game_control>().PrintInfo(2, 4f);    //Dodatkowe 100 punktów
        }
        if( other.gameObject.name == "Slow cube(Clone)"
        ||  other.gameObject.name == "Slow cube")
        {
            StartCoroutine(waitslow(8));
            canvas.GetComponent<Ui_game_control>().PrintInfo(3, 4f);    //Slow motion
        }
        if( other.gameObject.name == "Free cube(Clone)"
        ||  other.gameObject.name == "Free cube")
        {
            StartCoroutine(waitfree(8));
            canvas.GetComponent<Ui_game_control>().PrintInfo(4, 4f);    //Brak kolizji
        }
    }

    //----------------------------------------------------------------

    IEnumerator waitslow(int t)
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
        timeSlow = t + 1;

        slowStatus.GetComponent<Animator>().SetTrigger("Swipe_in");
        yield return new WaitForSeconds(t);
        slowStatus.GetComponent<Animator>().SetTrigger("Swipe_out");

        Time.timeScale = 1;
    //    Time.fixedDeltaTime = Time.fixedDeltaTime * Time.timeScale;
    }

    IEnumerator waitfree(int t)
    {
        GetComponent<BoxCollider>().enabled = false;
        timeFree = t + 1;

        freeStatus.GetComponent<Animator>().SetTrigger("Swipe_in");
        yield return new WaitForSeconds(t);
        freeStatus.GetComponent<Animator>().SetTrigger("Swipe_out");

        GetComponent<BoxCollider>().enabled = true;
    }

    //----------------------------------------------------------------

    void Control_acceleration()
    {
        //Przyśpieszenie
        if(-Input.acceleration.z > 0.512f)
            speed_bost = speed_max / 2;

        //Zwolnienie
        if(-Input.acceleration.z < 0.064f)
            speed_bost = speed_max / -4;

        //Brak boost'u
        if(-Input.acceleration.z < 0.512f && -Input.acceleration.z > 0.064)
            speed_bost = 0;
        
        //Skręcanie
        if(Input.acceleration.x > 0.064f)   //W prawo
            transform.position += Vector3.right * (Time.deltaTime * Input.acceleration.x * speed);
        
        if(Input.acceleration.x < -0.064f)  //W lewo
            transform.position += Vector3.left * (Time.deltaTime * -Input.acceleration.x * speed);
    }

    void Control_keyboard()
    {
        //Przyśpieszenie
        if(Input.GetKeyDown(KeyCode.UpArrow))
           speed_bost = speed_max / 2;
        
        //Zwolnienie
        if(Input.GetKeyDown(KeyCode.DownArrow))
            speed_bost = speed_max / -4;
        
        //Brak boost'u
        if( Input.GetKeyUp(KeyCode.DownArrow)
        ||  Input.GetKeyUp(KeyCode.UpArrow))
           speed_bost = 0;

        //Skręcanie
        if(Input.GetKey(KeyCode.RightArrow))    //W prawo
            transform.position += Vector3.right * (Time.deltaTime * speed_max / 4);
        
        if(Input.GetKey(KeyCode.LeftArrow))     //W lewo
            transform.position += Vector3.left * (Time.deltaTime * speed_max / 4);
    }

    void Control_touch()
    {
        if(Input.GetMouseButtonDown(0))
        {
            touchPosition = Vector3.zero;
        //    touchPosition = Input.mousePosition;
        //    touchPosition /= Screen.dpi;
        }

        if(Input.GetMouseButton(0))
        {
            touchPosition = Input.mousePosition;

            //Przyśpieszenie
            if(touchPosition.y > Screen.height - (Screen.height / 3))
                speed_bost = speed_max / 2;

            //Zwolnienie
            if(touchPosition.y < Screen.height - (Screen.height / 3) * 2)
                speed_bost = speed_max / -4;

            //Skręcanie
            if(touchPosition.x > Screen.width - (Screen.width / 3))    //W prawo
                transform.position += Vector3.right * (Time.deltaTime * speed_max / 4);

            if(touchPosition.x < (Screen.width / 3))     //W lewo
                transform.position += Vector3.left * (Time.deltaTime * speed_max / 4);
        }

        if(Input.GetMouseButtonUp(0))
        {
            //Brak boost'u
            speed_bost = 0;
            touchPosition = Vector3.zero;
        }
    }
}
