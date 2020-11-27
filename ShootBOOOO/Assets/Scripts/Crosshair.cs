using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private RectTransform crosshairRect;

    [SerializeField] private float restingSize = 65f;
    [SerializeField] private float maxSize = 250f;
    [SerializeField] private float speed;

    private float currentSize = 65f;

    private void Start()
    {
        crosshairRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (IsMoving())
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
        }
        else
        {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }

        crosshairRect.sizeDelta = new Vector2(currentSize, currentSize);
    }

    private bool IsMoving()
    {
        if (Input.GetAxis("Horizontal") != 0 ||
            Input.GetAxis("Vertical") != 0 ||
            Input.GetAxis("Mouse X") != 0 ||
            Input.GetAxis("Mouse Y") != 0)
            return true;
        else
            return false;
    }
}
