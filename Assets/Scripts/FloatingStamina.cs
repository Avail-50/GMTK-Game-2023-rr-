using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingStamina : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform hero;
    public Vector3 offset;

    //private Vector3 

    public void UpdateStaminaBar(float current, float max)
    {
        slider.value = current / max;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        //transform.rotation = camera.transform.rotation;
        transform.position = hero.transform.position + offset;
    }
}
