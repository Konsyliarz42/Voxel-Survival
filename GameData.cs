using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public double slider_value;
    public bool vignette, motionBlur, colorGrading, ambietOcclusion, bloom;
    public string launge;

    public int type_control;

    public GameData(Ui_menu_control canvas)
    {
        slider_value = canvas.slider_value;

        vignette = canvas.vignette;
        motionBlur = canvas.motionBlur;
        colorGrading = canvas.colorGrading;
        ambietOcclusion = canvas.ambietOcclusion;
        bloom = canvas.bloom;
        
        launge = canvas.launge;
        type_control = canvas.type_control;
    }
}
