using UnityEngine;
using UnityEngine.UI;

public class FloatingStamina : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateStaminaBar(float current, float max)
    {
        slider.value = current / max;
    }
}
