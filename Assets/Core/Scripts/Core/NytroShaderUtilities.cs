using UnityEngine;

public static class NytroShaderUtilities
{
    public static Shader transparentShader = Shader.Find("Shader Graphs/Toon-v5-Transparent");
    public static Shader opaqueShader = Shader.Find("Shader Graphs/Toon-v5");

    public static class NytroToonKeywords
    {
        public static readonly int tint = Shader.PropertyToID("_tint");
        public static readonly int texture = Shader.PropertyToID("_tex");
        public static readonly int maskColor = Shader.PropertyToID("_alphaOverride");
        public static readonly int exposure = Shader.PropertyToID("_exposure");
        public static readonly int alpha = Shader.PropertyToID("_alpha");
        public static readonly int pulse = Shader.PropertyToID("_pulse");
        public static readonly int pulseColor = Shader.PropertyToID("_pulseColor");
    }

    public static class StandardKeywords
    {
        public static readonly int baseMap = Shader.PropertyToID("_BaseMap");
        public static readonly int baseColor = Shader.PropertyToID("_BaseColor");
    }

    public static class NytroPassKeywords
    {
        public static readonly int outlineColor = Shader.PropertyToID("_outlineColor");
        public static readonly int outlineThickness = Shader.PropertyToID("_outlineThickness");
        public static readonly int outlineAlpha = Shader.PropertyToID("_outlineAlpha");
    }
}
