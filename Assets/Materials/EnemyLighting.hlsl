void GetMainLightData_float(out float3 Direction, out float3 Color)
{
    #ifdef SHADERGRAPH_PREVIEW
        Direction = float3(0.5, 0.5, 0);
        Color = float3(1, 1, 1);
    #else
        Light light = GetMainLight();
        Direction = light.direction;
        Color = light.color;
    #endif
}