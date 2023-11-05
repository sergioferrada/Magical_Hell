using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public void CallChangeSceneEvent(string sceneName)
    {
        // Llama al evento estático de GameManager aquí.
        GameManager.Instance.ChangeScene(sceneName);
    }
}
