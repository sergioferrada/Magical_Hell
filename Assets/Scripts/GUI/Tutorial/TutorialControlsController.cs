using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControlsController : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (key == KeyCode.D)
                GetComponent<Animator>().Play("Fade_Out_WASD_Animation");
            else if (key == KeyCode.Space)
                GetComponent<Animator>().Play("Fade_Out_Space_Animation");

            Destroy(gameObject, .75f);
        }
    }
}
