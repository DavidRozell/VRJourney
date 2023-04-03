using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayController : MonoBehaviour
{
    private XRInteractorLineVisual lineVisual;
    public bool isLeft;
    public bool isRight;

    void Start()
    {
        lineVisual = gameObject.GetComponent<XRInteractorLineVisual>();
    }

    public void Hide()
    {
        lineVisual.enabled = false;
        StartCoroutine(WaitToEnable());
    }

    public void Show()
    {
        lineVisual.enabled = true;
    }

    public void OnHoverRay()
    {
        if (gameObject.name == "LeftController")
        {
            isLeft = true;
        }
        else
        {
            if (gameObject.name == "RightController")
            {
                isRight = true;
            }
        }
    }

    public void OnHoverRayExit()
    {
        isLeft = false;
        isRight = false;
    }

    private IEnumerator WaitToEnable()
    {
        yield return new WaitForSeconds(0.1f);
        if (gameObject.name == "LeftController")
        {
            isLeft = true;
        }
        else
        {
            if (gameObject.name == "RightController")
            {
                isRight = true;
            }
        }
    }
}
