using UnityEngine;
using UnityEngine.UI;

public class FloatingHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float current, float max)
    {
        slider.value = current / max;
    }
}
