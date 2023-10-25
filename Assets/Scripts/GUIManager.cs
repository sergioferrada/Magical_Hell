using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public static class GUIManager
{
    private static GameObject currentGUI;
    private static Animator animatorGUI;


    public static void SetCurrentCanvas(GameObject newGUI) {

        currentGUI = newGUI;

        if(newGUI.GetComponent<Animator>() != null)
            animatorGUI = newGUI.GetComponent<Animator>();
        else
            animatorGUI = null;
    }

    public static GameObject GetCurrentCanvas()
    {
        return currentGUI;
    }
}
