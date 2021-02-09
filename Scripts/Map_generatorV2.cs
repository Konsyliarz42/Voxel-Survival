using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_generatorV2 : MonoBehaviour
{
    public const int map_size = 64;
    public const int map_distance = 6;

    public GameObject map;
    GameObject new_horizontal;
    GameObject horizontal_children;
    GameObject new_vertical;
    GameObject vertical_children;

    int i, j;

    public int x_num = 0;
    public int z_num = 1;
    public int x_pos;
    public int z_pos;

    Vector3 position;
    Vector3 rotation;

    //----------------------------------------------------------------

    void Start()
    {
        map.GetComponent<Column_creator>().col_num = 0;
        CreateStart();
    }

    void Update()
    {
        //Wykrycie nad którą planszą znajduje się gracz
        if(transform.position.z >= map_size*z_num)
        {   z_num++;    CreateForward();    }

        if(transform.position.x > (map_size * x_num) + (map_size / 2))
        {   x_num++;    CreateSide();   }

        if(transform.position.x < (map_size * x_num) - (map_size / 2))
        {   x_num--;    CreateSide();   }
    }

    //----------------------------------------------------------------

    void CreateStart()
    {
        //Wyznaczenie stanu początkowego
        position = new Vector3(x_num, 0, z_num);
        rotation = new Vector3(-90, 0, 0);

        //Generowanie pola
        Debug.Log("Generowanie pola startowego w pozcyji: " + position);

        for(i = 1; i <= map_distance; i++)  //W osi Z
        {
            z_pos = map_size * i;

            position = new Vector3(x_num, 0, z_pos);

            new_horizontal = Instantiate(map, position, Quaternion.Euler(rotation)) as GameObject;

            for(j = (map_distance / -2); j <= (map_distance / 2); j++)  //W osi X
            {
                x_pos = map_size * j;

                position = new Vector3(x_pos, 0, z_pos);

                //Ominięcie obiektu "new_map"
                if(j == -1)  j++;

                horizontal_children = Instantiate(map, position, Quaternion.Euler(rotation)) as GameObject;
                horizontal_children.transform.parent = new_horizontal.transform;
            }
        }
    }

    void CreateForward()
    {
        //Wyznaczenie pozycji początkowej
        x_pos = map_size * x_num;
        z_pos = map_size * (z_num + (map_distance - 1));

        position = new Vector3(x_pos, 0, z_pos);
        rotation = new Vector3(-90, 0, 0);

        //Generowanie pasa
    //    Debug.Log("Generowanie pasa poziomego w pozcyji: " + position);
        new_horizontal = Instantiate(map, position, Quaternion.Euler(rotation)) as GameObject;

        for(i = (map_distance / -2); i <= (map_distance / 2); i++)
        {
            if(i != 0)  //Ominięcie obiektu "new_map"
            {
                x_pos = (int)new_horizontal.transform.position.x + map_size * i;
                z_pos = (int)new_horizontal.transform.position.z;

                position = new Vector3(x_pos, 0, z_pos);

                horizontal_children = Instantiate(map, position, Quaternion.Euler(rotation)) as GameObject;
                horizontal_children.transform.parent = new_horizontal.transform;
            }
        }
    }

    void CreateSide()
    {
        //Wyznaczenie pozycji początkowej
        x_pos = (int)new_horizontal.transform.position.x;
        z_pos = (int)new_horizontal.transform.position.z;

        //Korekta przesunięcia
        if(x_num < 0)   x_pos -= (map_size * (map_distance / 2)) + map_size;
        else            x_pos += (map_size * (map_distance / 2)) + map_size;

        position = new Vector3(x_pos, 0, z_pos);
        rotation = new Vector3(-90, 0, 0);

        //Generowanie pasa
    //    Debug.Log("Generowanie pasa pionowego w pozcyji: " + position);
        new_vertical = Instantiate(map, position, Quaternion.Euler(rotation)) as GameObject;

        for(i = 1; i <= (map_distance / 2) + 1; i++)
        {
            z_pos = (int)new_vertical.transform.position.z - map_size * i;

            position = new Vector3(x_pos, 0, z_pos);

            vertical_children = Instantiate(map, position, Quaternion.Euler(rotation)) as GameObject;
            vertical_children.transform.parent = new_vertical.transform;
        }
    }
}
