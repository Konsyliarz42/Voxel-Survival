using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_generator : MonoBehaviour
{
    public GameObject map;
    private GameObject new_map;

    const int map_size = 64;
    const int map_distance = 6;

    int i, j;

    public float x_pos;
    public float z_pos;

    public int z_num = 1;
    public int x_num = 0;
    int x_num_bef;

    bool crate = false;
    
    void Start()
    {
        //Generowanie startowego pola
        for(i = 1; i <= map_distance + 1; i++)  //W osi Z
        {
            x_pos = 0;
            z_pos = map_size * i;

            for(j = map_distance / -2; j <= map_distance / 2; j++) //W osi X
                new_map = Instantiate(map, new Vector3(x_pos + map_size * j, 0, z_pos), Quaternion.Euler(-90, 0, 0)) as GameObject;
        }
    }

    void Update()
    {
        //Rozpoznanie planszy nad którą jest gracz w osi X
        if(transform.position.x > (map_size * x_num) + (map_size / 2))  //Poruszając się w prawo
        {    x_num_bef = x_num; x_num++;    crate = true;    }

        if(transform.position.x < (map_size * x_num) - (map_size / 2))  //Poruszając się w lewo
        {    x_num_bef = x_num; x_num--;    crate = true;    }

        //Generowanie plansz przed graczem
        if(transform.position.z >= map_size*z_num)
        {
            x_pos = map_size * x_num;
            z_pos = map_size * (z_num + (map_distance - 1));

            for(i = map_distance / -2; i <= map_distance / 2; i++) //W osi X
                new_map = Instantiate(map, new Vector3(x_pos + map_size * i, 0, z_pos), Quaternion.Euler(-90, 0, 0)) as GameObject;

            z_num++;

        //    Debug.Log("Wygenerowano pas poziomo (" + x_pos + ", " + z_pos + ")");
        }

        //Generowanie plansz bocznych
        if(crate == true)
        {
            x_pos = (map_size * x_num_bef) + (map_size * (map_distance / 2));
            z_pos = map_size * z_num;

            for(i = 0; i < map_distance - 1; i++)   //W osi Z
                new_map = Instantiate(map, new Vector3(x_pos, 0, z_pos + (map_size * i)), Quaternion.Euler(-90, 0, 0)) as GameObject;
    
            crate = false;
        //    Debug.Log("Wygenerowano pas pionowo (" + x_pos + ", " + z_pos + ")");
        }
    }
}