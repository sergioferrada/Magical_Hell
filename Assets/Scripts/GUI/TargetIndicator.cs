using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public GameObject target;
    public float offScreenThreshold = 10f;
    private Camera mainCamera;
    private bool isIndicatorActivate = true;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(isIndicatorActivate && RoomsManager.Instance.GetEnemiesEnScene() == 1)
        {
            target = FindFirstObjectByType<Enemy>().gameObject;
            Vector3 targetDirection = target.transform.position - transform.position;
            float distanceToTarget = targetDirection.magnitude;

            if(distanceToTarget < offScreenThreshold)
            {
                gameObject.SetActive(false);
                isIndicatorActivate = false;
            }
            else
            {
                Vector3 targetViewportPosition = mainCamera.WorldToViewportPoint(target.transform.position);

                if(targetViewportPosition.z > 0 && targetViewportPosition.x >0 && targetViewportPosition.x < 1 && targetViewportPosition.y > 0 && targetViewportPosition.y < 1)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                    Vector3 screenEdge = mainCamera.ViewportToWorldPoint(new Vector3(Mathf.Clamp(targetViewportPosition.x, .1f, .9f), Mathf.Clamp(targetViewportPosition.y, .1f, .9f), mainCamera.nearClipPlane));
                    transform.position = new Vector3(screenEdge.x, screenEdge.y, 0);
                    Vector3 direction = target.transform.position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0,0,angle);
                }
            }
        }
    }
}
