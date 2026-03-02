using UnityEngine;

public class OverrideSunColor : MonoBehaviour
{
    private static readonly int ColorPropertyID = Shader.PropertyToID("_OverrideSunColor");

    void Start()
    {
        ApplyColor(new Color(0xCA / 255.0f, 0xAB / 255.0f, 0x53 / 255.0f));
    }

    public void ApplyColor(Color color)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach(var renderer in renderers)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor(ColorPropertyID, color);
            renderer.SetPropertyBlock(propBlock);
        }
        
    }
}
