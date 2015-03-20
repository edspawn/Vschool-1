﻿using UnityEngine;
using System.Collections;

public class PictureLib_LeftArrow : MonoBehaviour
{

    private GameObject Wall;
    void OnMouseDown()
    {
        var scr = Wall.GetComponent<PictureLib>();
        scr.current_page = scr.current_page - 1;
        if (scr.current_page == 0) scr.current_page = scr.pages_num;
        scr.loadPics(scr.current_page);
    }
}
