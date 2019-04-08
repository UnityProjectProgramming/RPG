using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{

    public delegate void UIEventHandler(string newQuestDescription);
    public static event UIEventHandler onQuestUIChanged;

    public static void QuestDescUpdate(string newQuestDescription)
    {

    }

}


