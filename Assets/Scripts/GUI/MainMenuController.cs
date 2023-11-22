using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static GUIManager;

public class MainMenuController : GUIBase
{
    public TMP_Text versionText;

    private void Start()
    {
        versionText.SetText(" Version " + GameManager.Instance.versionNumber);
    }                                                                                                                                                                                             
}
