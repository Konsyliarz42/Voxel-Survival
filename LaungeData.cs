using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaungeData : MonoBehaviour
{
    //================================================================================
    //Po zmianie języka konieczne jest powtórzenie przypisania stringów elementom!!!
    //================================================================================

    //  Title
    public string title_gameOver = "Game Over";             //0
    public string title_endEvent = "You survived!";         //1

    public string title_columnColapse = "Column Colapse";   //2
    public string title_columnRises = "Column Rises";       //3
    public string title_giantLand = "Giant Land";           //4
    public string title_chaosSwipe = "Chaos Swipe";         //5
    public string title_upIsDown = "Up Is Down";            //6
    public string title_tunelOfDeath = "Tunel Of Death";    //7
    public string title_rockDown = "Rock Down";             //8
    public string title_rideAtNight = "Ride At Night";      //9

    //  Info
    public string info_endEvent = "You get 200 extra points";   //0
    public string info_scoreBoard = "The best score ever:";     //1

    public string info_bonus = "You get 100 extra points";      //2
    public string info_slow = "Slow motion is active";          //3
    public string info_free = "No colider active";              //4

    public string info_accelerationControl = "Move your device to control game";    //10
    public string info_touchControl = "Touch on screen to control game";            //11

    //  Button
    public string button_play = "Play";
    public string button_resume = "Resume";
    public string button_playAgain = "Play Again";
    public string button_options = "Options";
    public string button_scoreBoard = "Score Board";
    public string button_backToMenu = "Back To Menu";
    public string button_exit = "Exit";
    public string button_laungeEN = "English";
    public string button_laungePL = "Polish";
    public string button_laungeGER = "German";
    public string button_accelerometer = "Accelerometer";
    public string button_touch = "Touch";

    //  Other text
    public string other_speed = "Speed:";
    public string other_score = "Score:";
    public string other_endScore = "Your Score:";
    public string other_noData = "No Data";
    public string other_optionsInfo = "To confirm, close the menu by clicking the options button";
    public string other_controlInfoAccelerometr = "To control use accelerometer";
    public string other_controlInfoTouch = "To control use touch";
    public string other_controlWarring = "Your device has not support accelerometer";
 
    //  Name
    public string name_scoreBoard = "Score Board";
    public string name_pause = "Pause";
    public string name_options = "Options";
    public string name_postProces = "Post-Process";
    public string name_postProcesSlider = "Weight";
    public string name_launge = "Language";
    public string name_control = "Control";

    //  Toggle
    public string toggle_vignette = "Vignette";
    public string toggle_motionBlur = "Motion Blur";
    public string toggle_colorGrading = "Color Grading";
    public string toggle_ambientOcclusion = "Ambient Occlusion";
    public string toggle_bloom = "Bloom";

    //----------------------------------------------------------------

    public void ChangeLaunge(string launge)
    {
        LaungeEN(); //Domyślny język

        if(launge == "EN" || launge == "ENGLISH")       LaungeEN();
        else if(launge == "PL" || launge == "POLISH")   LaungePL();
        else if(launge == "GER" || launge == "GERMAN")  LaungeGER();
        else
            Debug.LogError("Wybrany język jest nieobsługiwany");
    }

    //----------------------------------------------------------------

    void LaungeEN()
    {
        //-------- Menu --------
        button_play = "Play";
        button_options = "Options";
        button_scoreBoard = "Score Board";
        button_exit = "Exit";

        name_scoreBoard = "Score Board";
        info_scoreBoard = "The best score ever:";
        other_noData = "No Data";

        name_options = "Options";
        other_optionsInfo = "To confirm, close the menu by clicking the options button";
        name_postProces = "Post-Process";
        name_postProcesSlider = "Weight";
        toggle_vignette = "Vignette";
        toggle_motionBlur = "Motion Blur";
        toggle_colorGrading = "Color Grading";
        toggle_ambientOcclusion = "Ambient Occlusion";
        toggle_bloom = "Bloom";

        name_launge = "Language";
        button_laungeEN = "English";
        button_laungePL = "Polish";
        button_laungeGER = "German";

        name_control = "Control";
        other_controlInfoAccelerometr = "To control use accelerometer";
        other_controlInfoTouch = "To control use touch";
        other_controlWarring = "Your device has not support accelerometer";
        button_accelerometer = "Accelerometer";
        button_touch = "Touch";

        //-------- Game --------
        other_speed = "Speed: ";
        other_score = "Score: ";
        other_endScore = "Your Score:";

        title_endEvent = "You survived!";
        title_gameOver = "Game Over";
        title_columnColapse = "Column Colapse";
        title_columnRises = "Column Rises";
        title_giantLand = "Giant Land";
        title_chaosSwipe = "Chaos Swipe";
        title_upIsDown = "Up Is Down";
        title_tunelOfDeath = "Tunel Of Death";
        title_rockDown = "Rock Down";

        info_endEvent = "You get 200 extra points";
        info_bonus = "You get 100 extra points";
        info_slow = "Slow motion is active";
        info_free = "No colider active"; 

        name_pause = "Pause";
        button_resume = "Resume";
        button_playAgain = "Play Again";
        button_backToMenu = "Back To Menu";
    }

    void LaungePL() //Bez polskich znaków
    {
        //-------- Menu --------
        button_play = "Graj";
        button_scoreBoard = "Tablica Wynikow";
        button_options = "Ustawienia";
        button_exit = "Wyjscie";

        name_scoreBoard = "Tablica Wynikow";
        info_scoreBoard = "Najlepsze wyniki:";
        other_noData = "Brak Danych";

        name_options = "Ustawienia";
        other_optionsInfo = "Aby potwierdzic, zamknij menu, klikajac przycisk ustawien";
        name_postProces = "Post-Procesy";
        name_postProcesSlider = "Waga";
        toggle_vignette = "winieta";
        toggle_motionBlur = "Rozmycie W Ruchu";
        toggle_colorGrading = "Gradient Koloru";
        toggle_ambientOcclusion = "Okluzja otoczenia";
        toggle_bloom = "Bloom";

        name_launge = "Jezyk";
        button_laungeEN = "Angielski";
        button_laungePL = "Polski";
        button_laungeGER = "Niemiecki";

        name_control = "Sterowanie";
        other_controlInfoAccelerometr = "Uzywaj akcelerometru";
        other_controlInfoTouch = "Uzywaj dotyku";
        other_controlWarring = "Twoje urzadzenie nie obsluguje akcelerometru";
        button_accelerometer = "Akcelerometr";
        button_touch = "Dotyk";

        //-------- Gra --------
        other_speed = "Predkosc: ";
        other_score = "Wynik: ";
        other_endScore = "Twoj Wynik:";

        title_endEvent = "Przetrwales!";
        title_gameOver = "Koniec Gry";
        title_columnColapse = "Kolumnolipsa";
        title_columnRises = "Powstanie";
        title_giantLand = "Ziemia Gigantow";
        title_chaosSwipe = "Niekontrolowane przesuniecie";
        title_upIsDown = "Gora To Dol";
        title_tunelOfDeath = "Tunel Zaglady";
        title_rockDown = "Upadek skały";

        info_endEvent = "Otrzymujesz Dodatkowe 200 Punktow";
        info_bonus = "Otrzymujesz Dodatkowe 100 Punktow";
        info_slow = "Spowolnienie czasu";
        info_free = "Bez kolizji"; 

        name_pause = "Pauza";
        button_resume = "Wroc Do Gry";
        button_playAgain = "Od Nowa";
        button_backToMenu = "Powrot Do Menu";
    }

    public void LaungeGER()
    {
      //-------- Menu --------
        button_play = "Abspielen";
        button_scoreBoard = "Anzeigetafel";
        button_options = "die Einstellungen";
        button_exit = "Ausgang";

        name_scoreBoard = "Anzeigetafel";
        info_scoreBoard = "die besten Ergebnisse:";
        other_noData = "Keine Daten";

        name_options = "die Einstellungen";
        other_optionsInfo = "Schliessen Sie zur Bestatigung das Menu, indem Sie auf die Schaltflache Einstallungen klicken";
        name_postProces = "Post-Prozess";
        name_postProcesSlider = "Gewicht";
        toggle_vignette = "Vignette";
        toggle_motionBlur = "Bewegungsunscharfe";
        toggle_colorGrading = "Farbkorrektur";
        toggle_ambientOcclusion = "Umgebungsokklusion";
        toggle_bloom = "Bloom";

        name_launge = "Sprache";
        button_laungeEN = "Englisch";
        button_laungePL = "Polnisch";
        button_laungeGER = "Deutsche";

        name_control = "Steuerung";
        other_controlInfoAccelerometr = "Zur Steuerung Beschleunigungsmesser verwenden";
        other_controlInfoTouch = "Zur Steuerung verwenden Sie touch";
        other_controlWarring = "Ihre Gerät unterstützt keinen Beschleunigungsmesser";
        button_accelerometer = "Beschleunigungsmesser";
        button_touch = "Berühren";

        //-------- Spiel --------
        other_speed = "Geschwindigkeit: ";
        other_score = "Ergebnis: ";
        other_endScore = "Ihr Ergebnis:";

        title_endEvent = "Du hast uberlebt!";
        title_gameOver = "Spiel ist aus";
        title_columnColapse = "Saule collapse";
        title_columnRises = "Saule steigt";
        title_giantLand = "Riesenland";
        title_chaosSwipe = "Chaotisch Verschiebung";
        title_upIsDown = "Kopfuber";
        title_tunelOfDeath = "Tunnel des Todes";

        info_endEvent = "Sie erhalten zusatzlich 200 Punkte";
        info_bonus = "Sie erhalten zusatzlich 200 Punkte";
        info_slow = "Zeit verlangsamen";
        info_free = "Keine Kollision"; 

        name_pause = "Pause";
        button_resume = "Fortsetzen";
        button_playAgain = "Nochmal abspielen";
        button_backToMenu = "Zuruck zum Menu";
    }
}