using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunel_control : MonoBehaviour
{
    GameObject left;
    GameObject right;
    GameObject player;

    Vector3 rotationL;
    Vector3 rotationR;
    float rot_x;

    Vector3 position;
    int pos_x;
    float pos_y;
    float pos_z;

    void Start()
    {
        left = GameObject.Find("Block left");
        right = GameObject.Find("Block right");
        player = GameObject.Find("Player");

        rot_x = -45f;
        pos_x = (int)player.transform.position.x + (64 * Random.Range(-1, 2));
        pos_y = 256f;
        pos_z = player.transform.position.z + 512f;
        

        rotationL = new Vector3(rot_x, -90, 0);
        rotationR = new Vector3(rot_x, 90, 0);
        position = new Vector3(pos_x, pos_y, pos_z);
    }

    void Update()
    {
        //Obracanie bloków
        if(rot_x < 0)
        {
            rot_x += 0.25f;

            rotationL = new Vector3(rot_x, -90, 0);
            rotationR = new Vector3(rot_x, 90, 0);

            left.transform.rotation = Quaternion.Euler(rotationL);
            right.transform.rotation = Quaternion.Euler(rotationR);
        }

        //Opuszczanie bloków
        if(transform.position.y >= 33)
        {
            pos_y -= 1.25f;
            pos_z = player.transform.position.z + 448f;

            position = new Vector3(pos_x, pos_y, pos_z);
            transform.position = position;
        }
    }
}
