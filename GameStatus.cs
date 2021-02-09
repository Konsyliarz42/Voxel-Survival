using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStatus
{
    public int last_score;
    public int[] score_tab = new int[16];
    const int score_tab_lenght = 16;

    public GameStatus(Ui_game_control canvas)
    {
        last_score = canvas.score;

        //Pobranie tablicy z gry
        for(int i = 0; i < score_tab_lenght; i++)
            score_tab[i] = canvas.score_tab[i];

        //Dodanie wyniku
        for(int j = 0; j < score_tab_lenght; j++)
            if(last_score > score_tab[j])
            {
                for(int k = score_tab_lenght - 1; k > j; k--)
                    score_tab[k] = score_tab[k - 1];

                score_tab[j] = last_score;
                j = score_tab_lenght;
            }
            else if(last_score == score_tab[j])
                j = score_tab_lenght;
    }
}
