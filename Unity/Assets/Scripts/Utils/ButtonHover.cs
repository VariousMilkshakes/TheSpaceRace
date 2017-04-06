using System.Collections;
using System.Collections.Generic;
using SpaceRace.Utils;
using UnityEngine;
using System;
using System.Net.Mime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    public static void SHOW_TOOL_TIP (bool show)
    {
//        GameObject.Find("BuildingInfo")
//                  .GetComponent<Image>()
//                  .enabled = show;

        if (!show) {
            GameObject.Find("BuildingInfo")
                      .GetComponent<Text>()
                      .text = "";
        }
    }

    public GameObject UiHandler;
    public Type TargetBuilding;

    private UiHack uiHandler;

    void Start ()
    {
        uiHandler = UiHandler.GetComponent<UiHack>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SHOW_TOOL_TIP(true);
        uiHandler.DisplayToolTop(TargetBuilding);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiHandler.ClearToolTip();
        SHOW_TOOL_TIP(false);
    }
}
