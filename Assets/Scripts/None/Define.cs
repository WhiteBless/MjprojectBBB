using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum MouseEvent
    {
        Press,
        Click,
    }
    public enum CameraMode
    {
        QarterView,
    }

    // 캐릭터 구분
    public enum Character_Name
    {
        Assensin,
    }

    public enum Choice_Floor
    {
        Floor_1,
        Floor_2,
        Floor_3,
        Floor_4,
        Floor_5,
    }

    public enum Cur_Scene
    { 
        NONE,
        MAIN,
        FLOOR_1,
        FLOOR_2,
        FLOOR_3,
    }


    public enum Cur_Character
    { 
        ASSASIN,
        SAMURAI,
        WARRIOR,
    }

    public enum Reaper_Pattern_Color
    {
        NORMAL,
        RED,
        YELLOW,
        GREEN,
        BLUE,
    }
}
