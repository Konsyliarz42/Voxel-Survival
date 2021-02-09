using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour
{
    GameObject player;
    public Canvas canvas;
    public GameObject map;
    public GameObject column;
    public GameObject tunel;
    GameObject new_tunel;

    public bool play = false;
    public bool first_event = true;

    public int event_time;
    public int time;
    public int next_event_time;
    public int random_event;
    int event_num = -1;
    int next_column;

    int column_side;
    float z_rotation = 0;
    bool reset_rot;
    public int col_num = 1;
    bool check_col;
    bool tunel_create;

    //----------------------------------------------------------------

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        player = GameObject.Find("Player");

        ResetGameSetings();
        next_event_time = 8 + (int)Time.time;   //8s rozgrzewki
        next_column = 2;
        play = true;
        first_event = true;

        player.GetComponent<Player_control>().speed_max = 64;   //Po rozgrzewce 80
        map.GetComponent<Column_creator>().col_num = 0;
    }

    void LateUpdate()
    {
        time = (int)Time.time;

        if(SceneManager.GetActiveScene().name == "Menu")
            play = false;

        if(play == true)
        {
            //Rozpoczęcie eventu
            if(next_event_time <= (int)Time.time)
            {
                event_num++;
                next_event_time += 30 * Random.Range(1, 3); //Następny event bedzie za pół minuty lub minutę

                if(first_event == false)    //Losowanie rozpoczyna się od drugiego eventu
                {
                    random_event = Random.Range(1, 7);
                    event_time = next_event_time - 15;   //Event kończy się 15s przed wylosowaniem następnego

                    PrintNameEvent(random_event);
                }
                else    //Koniec rozgrzewki
                {
                    first_event = false;
                    map.GetComponent<Column_creator>().col_num = 1;
                    player.GetComponent<Player_control>().speed_max += 12;  //Na rozgrzewce poruszanie następuje wolno
                }

                player.GetComponent<Player_control>().speed_max += 4;
            }

            //Uruchomienie eventu
            if(random_event == 1)   ColumnColapse();
            if(random_event == 2)   ColumnRise();
            if(random_event == 3)   GiantLand();
            if(random_event == 4)   ChaosSwipe();
            if(random_event == 5)   UpIsDown();
            if(random_event == 6)   TunelOfDeath();

            //Koniec eventu
            if(random_event > 0 && event_time < (int)Time.time)
            {
                next_event_time = (int)Time.time + 15;
                event_time = next_event_time - 15;

                canvas.GetComponent<Ui_game_control>().PrintTitle(1, 4f);   //You survived!
                canvas.GetComponent<Ui_game_control>().PrintInfo(0, 4f);    //You get 200 extra points
                canvas.GetComponent<Ui_game_control>().score_bonus += 200;

                //Zwiększenie liczby kolumn co 2, 4, 8, 16, 32... eventy
                if(event_num >= next_column)
                {
                    col_num++;
                    next_column *= 2;
                }

                ResetGameSetings();
            }
        }
        else
        {
            if(SceneManager.GetActiveScene().name == "Game")
                StartGame();
        }

        //Powrót do rotacji 0
        if(reset_rot == true)
            if(z_rotation != 0)
                UpIsDownEnd();
            else
                reset_rot = false;
    }

    //----------------------------------------------------------------

    public void ResetGameSetings()
    {
        random_event = 0;
        map.GetComponent<Column_creator>().col_num = col_num;

        //ColumnColapse
        column.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        column.GetComponent<Add_force>().y_axis = false;
        column.GetComponent<Add_force>().z_axis = false;
        map.GetComponent<Column_creator>().y_pos = 0;
        map.GetComponent<Column_creator>().rotation = new Vector3(-90, 0, 0);

        //ColumnRise
        map.GetComponent<BoxCollider>().enabled = true;
        column.GetComponent<Add_force>().rise = false;

        //GiantLand
        column.transform.localScale = new Vector3(1, 1, 1);

        //ChaosSwipe
        column.GetComponent<Add_force>().x_axis = false;

        //UpIsDown
        reset_rot = true;

        //TunelOfDeath
        check_col = true;
        tunel_create = false;
    }

    void PrintNameEvent(int ran_eve)
    {
        if(ran_eve == 1)    canvas.GetComponent<Ui_game_control>().PrintTitle(2, 4f);   //Column Colapse
        if(ran_eve == 2)    canvas.GetComponent<Ui_game_control>().PrintTitle(3, 4f);   //Column Rises
        if(ran_eve == 3)    canvas.GetComponent<Ui_game_control>().PrintTitle(4, 4f);   //Giant Land
        if(ran_eve == 4)    canvas.GetComponent<Ui_game_control>().PrintTitle(5, 4f);   //Chaos Swipe
        if(ran_eve == 5)    canvas.GetComponent<Ui_game_control>().PrintTitle(6, 4f);   //Up Is Down
        if(ran_eve == 6)    canvas.GetComponent<Ui_game_control>().PrintTitle(7, 4f);   //Tunel Of Death
    }

    void ColumnColapse()
    {
        col_num = map.GetComponent<Column_creator>().col_num;
        if(col_num > 2)    map.GetComponent<Column_creator>().col_num = 2;

        column.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        
        column.GetComponent<Add_force>().y_force = 16;
        column.GetComponent<Add_force>().z_force = 16;

        column.GetComponent<Add_force>().y_axis = true;
        column.GetComponent<Add_force>().z_axis = true;
        
        map.GetComponent<Column_creator>().rotation = new Vector3(90, Random.Range(1, 13) * 30, 0);
        map.GetComponent<Column_creator>().y_pos = 32;
    }

    void ColumnRise()
    {
        col_num = map.GetComponent<Column_creator>().col_num;

        map.GetComponent<BoxCollider>().enabled = false;
        map.GetComponent<Column_creator>().y_pos = -32;

        column.GetComponent<Add_force>().rise = true;
    }

    void GiantLand()
    {
        column.transform.localScale = new Vector3(2, 2, 2);
        col_num = map.GetComponent<Column_creator>().col_num;
        if(col_num > 4)    map.GetComponent<Column_creator>().col_num = 4;
    }

    void ChaosSwipe()
    {
        column.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX;
        col_num = map.GetComponent<Column_creator>().col_num;

        map.GetComponent<BoxCollider>().enabled = false;

        column.GetComponent<Add_force>().x_force = Random.Range(4, 17) * Random.Range(-1, 2);
        column.GetComponent<Add_force>().x_axis = true;
    }

    void UpIsDown()
    {
        if(z_rotation < 180f)
            z_rotation += 0.5f;

        player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z_rotation));
        col_num = map.GetComponent<Column_creator>().col_num;
    }
    void UpIsDownEnd()
    {
        if(z_rotation > 0)
        {
            z_rotation -= 0.5f;
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, z_rotation));
            col_num = map.GetComponent<Column_creator>().col_num;
        }
    }

    void TunelOfDeath()
    {
        if(check_col == true)
        {
            check_col = false;
            col_num = map.GetComponent<Column_creator>().col_num;
        }
        else
            map.GetComponent<Column_creator>().col_num = 0;

        if(tunel_create == false)
        {
            new_tunel = Instantiate(tunel, new Vector3((int)player.transform.position.x + (64 * Random.Range(-1, 2)), 512, player.transform.position.z + 512), Quaternion.Euler(0, 0, 0)) as GameObject;
            tunel_create = true;
        }
        
        if(player.transform.position.z > new_tunel.transform.position.z + 320)
        {
            event_time = (int)Time.time - 1;
            Destroy(new_tunel);
            map.GetComponent<Column_creator>().col_num = col_num;
        }
    }
}
