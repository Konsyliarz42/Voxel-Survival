using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_random_down : MonoBehaviour
{
    GameObject player;
    Animator animator;

    void Start()
    {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();

        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        if(player.GetComponent<Player_control>().speed < 80)        yield return new WaitForSeconds(Random.Range(2, 6));
        else if(player.GetComponent<Player_control>().speed < 128)  yield return new WaitForSeconds(Random.Range(1, 3));
        else if(player.GetComponent<Player_control>().speed < 144)  yield return new WaitForSeconds(1);
        else                                                        yield return new WaitForSeconds(0.5f);

        switch(Random.Range(1, 7))
        {
            default:    Debug.Log("Błąd losowania skały");  break;

            case 1: animator.SetTrigger("Start_01");    break;
            case 2: animator.SetTrigger("Start_02");    break;
            case 3: animator.SetTrigger("Start_03");    break;
            case 4: animator.SetTrigger("Start_04");    break;
            case 5: animator.SetTrigger("Start_05");    break;
            case 6: animator.SetTrigger("Start_06");    break;
        }
    }
}
