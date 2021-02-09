using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column_creator : MonoBehaviour
{
    public GameObject me;
    private GameObject player;
    public GameObject column;       GameObject new_column;
    public GameObject rock;         GameObject new_rock;
    public GameObject cube_bonus;   GameObject new_cube_bonus;
    public GameObject cube_slow;    GameObject new_cube_slow;
    public GameObject cube_free;    GameObject new_cube_free;

    public Mesh variant0;
    public Mesh variant1;
    public Mesh variant2;
    public Mesh variant3;

    public Vector3 rotation = new Vector3(-90, 0, 0);

    int ran_num0;
    int ran_num1;

    int i;

    int x_pos = 0;
    public int y_pos = 0;
    int z_pos = 0;

    const int column_size = 8;
    const int map_size = 64;
    const int map_pos_to_null = 28; //Połowa wielkości mapy (32) - połowa wielkości kolumny (4)

    public int col_num = 0;
    public bool activeEvent = false;
    public bool rockDown = false;

    public int z_num;

    void Start()
    {
        //Szukanie gracza na scenie
        player = GameObject.Find("Player");

        //Tworzenie Modelu i wybranie wariantu
        MeshFilter meshFilter = me.AddComponent<MeshFilter>() as MeshFilter;
        ran_num0 = Random.Range(0, 3);

        //    Debug.Log("Wylosowano wariant: " + ran_num0);
        if(ran_num0 == 0)   meshFilter.sharedMesh = variant0;
        if(ran_num0 == 1)   meshFilter.sharedMesh = variant1;
        if(ran_num0 == 2)   meshFilter.sharedMesh = variant2;
        if(ran_num0 == 3)   meshFilter.sharedMesh = variant3;

        //Limity ilości generowanych kolumn
        if(col_num > 16)    col_num = 16;
        if(col_num < 0)     col_num = 0;

        //Generowanie kolumn
        if(col_num != 0)
        {
            for(i = Random.Range(1, col_num); i > 0; i--)
            {
            //    Debug.Log("Tworzenie kolumny");
                ran_num1 = Random.Range(0, 8);

                x_pos = ((int)transform.position.x - map_pos_to_null) + (column_size * ran_num1);
                z_pos = ((int)transform.position.z - map_pos_to_null) + (column_size * ran_num1);

                if(Random.Range(0, 40) == 4)  //2.5% szans na wygenerowanie kostki
                {
                    switch(Random.Range(0, 3))
                    {
                        default:    Debug.Log("Błąd losowania kostki");    break;

                        case 0: CubeBonus();    break;
                        case 1: CubeSlow();     break;
                        case 2: CubeFree();     break;
                    }
                }
                else
                {
                    if(rockDown == false)
                        NewColumn();
                    else
                        NewRock();
                }
            }
        }
    }

    void FixedUpdate()
    {
        //Usuwanie plansz i kolumn po minięciu gracza
        if(((int)me.transform.position.z + map_size) < (int)player.transform.position.z)
        {
        //    Debug.Log("Destrukcja");
            Destroy(me);
        }
    }
    void NewColumn()
    {
        new_column = Instantiate(column, new Vector3(x_pos, y_pos, z_pos), Quaternion.Euler(rotation)) as GameObject;
        new_column.transform.parent = me.transform;
    }

    void NewRock()
    {
        z_num = player.GetComponent<Map_generatorV2>().z_num;

        new_rock = Instantiate(rock, new Vector3(x_pos, y_pos, z_pos), Quaternion.Euler(Vector3.zero)) as GameObject;
        new_rock.transform.name = "Rock" + z_num;
    }

    void CubeBonus()
    {
        new_cube_bonus = Instantiate(cube_bonus, new Vector3(x_pos, player.transform.position.y, z_pos), Quaternion.Euler(rotation)) as GameObject;
        new_cube_bonus.transform.parent = me.transform;
    }
    void CubeSlow()
    {
        new_cube_slow = Instantiate(cube_slow, new Vector3(x_pos, player.transform.position.y, z_pos), Quaternion.Euler(rotation)) as GameObject;
        new_cube_slow.transform.parent = me.transform;
    }
    void CubeFree()
    {
        new_cube_free = Instantiate(cube_free, new Vector3(x_pos, player.transform.position.y, z_pos), Quaternion.Euler(rotation)) as GameObject;
        new_cube_free.transform.parent = me.transform;
    }
}
