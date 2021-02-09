using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunelOfDeath_Ai : MonoBehaviour
{
    const int map_size = Map_generatorV2.map_size;  //64

    GameObject player;
    GameObject me;
    EventV2 eventV2;

    Vector3 move_tunel;
    public int move_position;
    public int move_start;
    public int move_eventStart;

    //----------------------------------------------------------------

    void Start()
    {
        me = GameObject.Find("Tunel Of Death(Clone)");
        player = GameObject.Find("Player");
        eventV2 = GameObject.Find("Event").GetComponent<EventV2>();

        move_tunel = new Vector3(player.transform.position.x, 0, 0);
        move_position = 0;
        move_start = 0;
        move_eventStart = player.GetComponent<Map_generatorV2>().z_pos;
        
        RandomSwipe();
    }

    void Update()
    {
        if(eventV2.event_end == false)
        {
            //Synchronizacja z graczem
            transform.position = Vector3.forward * player.transform.position.z;
            transform.position += move_tunel;

            //Losuj przesunięcie
            if(move_eventStart + move_start <= transform.position.z)
            {
                move_position = Random.Range(-map_size, map_size + 1);
                move_start *= 2;
                Debug.Log("... o " + move_position + " jednostek");
            }

            //Przenieś tunel
            if(move_position < 0)
            {
                move_tunel += Vector3.left * 0.128f;

                if(move_position >= transform.position.x)   //Zatrzymaj
                    move_position = 0;
            }
            if(move_position > 0)
            {
                move_tunel += Vector3.right * 0.128f;

                if(move_position <= transform.position.x)   //Zatrzymaj
                    move_position = 0;
            } 
        }
        else
        {
            Destroy(me, 2f);
            Debug.Log("Destrukcja");
        }
    }

    //----------------------------------------------------------------

    void RandomSwipe()
    {
        move_start = eventV2.event_endPosition - move_eventStart;
        move_start /= 3;
        Debug.Log("Przesunięcie tunelu nastąpi w pozycji: " + (move_eventStart + move_start) + " ...");
    }

}
