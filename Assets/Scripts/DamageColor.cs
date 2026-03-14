using UnityEngine;
using UnityEngine.UI;

public class DamageColor : MonoBehaviour
{

    [SerializeField] private PlayerDamage player;
    [SerializeField] private Image batteryFill;

    void Update()
    {
        if (player == null || batteryFill == null) return;
        float hpPercent = Mathf.Clamp01(player.GetLifeTime() / player.GetMaxLifeTime());
        batteryFill.fillAmount = hpPercent;
        Color color = Color.Lerp(Color.red, Color.green, hpPercent);
        color.a = 0.5f; 
        batteryFill.color = color;
    }
}
