using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoSingleton<AmbianceManager>
{
    public Color shadowColor;

    [Header("Skybox")]
    public Color horizonColor;
    public Color skyColor;
    [Range(0.01f, 1f)] public float horizonLength;
    [Range(-1f, 1f)] public float horizonShift;

    [Header("Fog")]
    public Color fogColor;

    public float linearFogStart = 100;
    public float linearFogDepth = 100;
    [Range(0.1f, 15)] public float linearFogBlend = 1;

    public float verticalFogStart = -1;
    public float verticalFogDepth = 10;
    [Range(0.1f, 15)] public float verticalFogBlend = 1;

    private void OnValidate()
    {
        ApplyParams();
    }

    protected override void Awake()
    {
        base.Awake();

        ApplyParams();
    }

    public void ApplyParams()
    {
        // shadow
        Shader.SetGlobalColor("_nytroAmbiance_shadowColor", shadowColor);

        // skybox
        Shader.SetGlobalColor("_nytroAmbiance_horizonColor", horizonColor.linear);
        Shader.SetGlobalColor("_nytroAmbiance_skyColor", skyColor.linear);
        Shader.SetGlobalFloat("_nytroAmbiance_horizonLength", horizonLength);
        Shader.SetGlobalFloat("_nytroAmbiance_horizonShift", -horizonShift);

        // fog
        Shader.SetGlobalColor("_nytroAmbiance_fogColor", fogColor.linear);

        Shader.SetGlobalFloat("_nytroAmbiance_linearFogStart", linearFogStart);
        Shader.SetGlobalFloat("_nytroAmbiance_linearFogDepth", linearFogDepth);
        Shader.SetGlobalFloat("_nytroAmbiance_linearFogBlend", linearFogBlend);

        Shader.SetGlobalFloat("_nytroAmbiance_verticalFogStartHeight", verticalFogStart);
        Shader.SetGlobalFloat("_nytroAmbiance_verticalFogDepth", verticalFogDepth);
        Shader.SetGlobalFloat("_nytroAmbiance_verticalFogBlend", verticalFogBlend);
    }
}
