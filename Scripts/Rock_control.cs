using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_control : MonoBehaviour
{
    GameObject player;
    public GameObject me;
    public GameObject area;
    private GameObject rock;

    Vector3 fix_position;

    float scale = 8.0f;
    float hight = 32.0f;
    float angle;

    bool stop_rotate = false;

    void Start()
    {
        player = GameObject.Find("Player");
        rock = GameObject.Find("/" + this.transform.name + "/Rock");
        fix_position = new Vector3(Random.Range(-64, 65), 0, 128);

        area.transform.localScale = ((Vector3.right + Vector3.up) * scale) + Vector3.forward;

        rock.transform.position = new Vector3(0, hight, 0);
        rock.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        angle += Time.deltaTime * 64;
        if(stop_rotate != true) rock.transform.rotation = Quaternion.Euler(angle, -angle, 0);

        if(area.transform.localScale.x > 1.0f)
        {
            area.transform.localScale -= ((Vector3.right + Vector3.up) * scale) * (Time.deltaTime * 0.1f);
            this.transform.position = player.transform.position + fix_position;
            rock.transform.position = area.transform.position + Vector3.up * hight;
        }
        else
        {
            if(rock.transform.position.y <= 4.096f) stop_rotate = true;
            else                                    rock.transform.position += Vector3.down * (Time.deltaTime * 32);
        }

    }
}
