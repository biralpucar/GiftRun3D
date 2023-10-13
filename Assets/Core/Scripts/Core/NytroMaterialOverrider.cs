using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static NytroShaderUtilities;

public enum NytroShaderType
{
    Opaque, Transparent
}

public class NytroMaterialOverrider : MonoBehaviour
{
    [HideInInspector] public Renderer overriddenRenderer;
    [HideInInspector] public Material overriddenMaterial;

    [Header("Render Queues")]
    public int opaqueQueue = 2450;
    public int transparentQueue = 3000;

    [Header("Default Outline")]
    public bool overrideDefaultOutlineProperties;
    public float defaultOutlineThickness;
    public float defaultOutlineAlpha;
    public Color defaultOutlineColor;

    private MaterialPropertyBlock outlinePropertyBlock;

    private void Awake()
    {
        overriddenRenderer = GetComponent<Renderer>();
        overriddenMaterial = overriddenRenderer.material; // this will clone the material

        outlinePropertyBlock = new MaterialPropertyBlock();

        ApplyDefaultOutlineProperties();
    }

    private void OnValidate()
    {
        defaultOutlineAlpha = Mathf.Clamp01(defaultOutlineAlpha);

        if (!Application.isPlaying) return;
        if (outlinePropertyBlock == null) return;

        ApplyDefaultOutlineProperties();
    }

    private void OnDestroy()
    {
        if (overriddenMaterial != null) Destroy(overriddenMaterial);
    }

    private void ApplyDefaultOutlineProperties()
    {
        if (overrideDefaultOutlineProperties)
        {
            SetOutlineColor(defaultOutlineColor);
            SetOutlineThickness(defaultOutlineThickness);
            SetOutlineAlpha(defaultOutlineAlpha);
        }
    }

    private void ApplyPropertyBlock()
    {
        overriddenRenderer.SetPropertyBlock(outlinePropertyBlock);
    }

    #region Setters & Getters
    public void SetTint(Color tint)
    {
        overriddenMaterial.SetColor(NytroToonKeywords.tint, tint);
    }

    public Color GetTint()
    {
        return overriddenMaterial.GetColor(NytroToonKeywords.tint);
    }

    public void SetMaskColor(Color maskColor)
    {
        overriddenMaterial.SetColor(NytroToonKeywords.maskColor, maskColor);
    }

    public Color GetMaskColor()
    {
        return overriddenMaterial.GetColor(NytroToonKeywords.maskColor);
    }

    public void SetTexture(Texture tex)
    {
        if (tex == null) tex = Texture2D.whiteTexture;
        overriddenMaterial.SetTexture(NytroToonKeywords.texture, tex);
    }

    public Texture GetTexture()
    {
        return overriddenMaterial.GetTexture(NytroToonKeywords.texture);
    }

    public void SetAlpha(float value)
    {
        if (value < 1 & overriddenMaterial.shader != transparentShader)
        {
            overriddenMaterial.SetNytroShader(NytroShaderType.Transparent);
            overriddenMaterial.renderQueue = transparentQueue;
        }

        else if ((Mathf.Approximately(value, 1f) | value > 1f) & overriddenMaterial.shader != opaqueShader)
        {
            overriddenMaterial.SetNytroShader(NytroShaderType.Opaque);
            overriddenMaterial.renderQueue = opaqueQueue;
        }

        overriddenMaterial.SetFloat(NytroToonKeywords.alpha, value);
    }

    public float GetAlpha()
    {
        return overriddenMaterial.GetFloat(NytroToonKeywords.alpha);
    }

    public void SetOutlineColor(Color color)
    {
        outlinePropertyBlock.SetColor(NytroPassKeywords.outlineColor, color);
        ApplyPropertyBlock();
    }

    public void SetOutlineThickness(float value)
    {
        outlinePropertyBlock.SetFloat(NytroPassKeywords.outlineThickness, value);
        ApplyPropertyBlock();
    }

    public void SetOutlineAlpha(float value)
    {
        outlinePropertyBlock.SetFloat(NytroPassKeywords.outlineAlpha, value);
        ApplyPropertyBlock();
    }

    public void SetExposure(float value)
    {
        overriddenMaterial.SetFloat(NytroToonKeywords.exposure, value);
    }

    public float GetExposure()
    {
        return overriddenMaterial.GetFloat(NytroToonKeywords.exposure);
    }

    public void SetPulse(float value)
    {
        overriddenMaterial.SetFloat(NytroToonKeywords.pulse, value);
    }

    public float GetPulse()
    {
        return overriddenMaterial.GetFloat(NytroToonKeywords.pulse);
    }

    public void SetPulseColor(Color color)
    {
        overriddenMaterial.SetColor(NytroToonKeywords.pulseColor, color);
    }

    public Color GetPulseColor()
    {
        return overriddenMaterial.GetColor(NytroToonKeywords.pulseColor);
    }
    #endregion

    #region Animators
    public Tween AnimateAlpha(float to, float duration)
    {
        Tween tween = DOTween.To(SetAlpha, GetAlpha(), to, duration);
        return tween;
    }

    public Tween AnimateExposure(float to, float duration)
    {
        Tween tween = DOTween.To(SetExposure, GetExposure(), to, duration);
        return tween;
    }

    public Tween AnimateTint(Color to, float duration)
    {
        Color from = GetTint(); // caching this so lambda can capture it
        Tween tween = DOTween.To((float t) =>
        {
            Color currentColor = Color.Lerp(from, to, t);
            SetTint(currentColor);
        }, 0, 1, duration);
        return tween;
    }

    public Tween AnimateOutlineThickness(float from, float to, float duration)
    {
        Tween tween = DOTween.To(SetOutlineThickness, from, to, duration);
        return tween;
    }

    public Tween AnimateOutlineAlpha(float from, float to, float duration)
    {
        Tween tween = DOTween.To(SetOutlineAlpha, from, to, duration);
        return tween;
    }

    public Tween AnimatePulse(float from, float to, float duration)
    {
        Tween tween = DOTween.To(SetPulse, from, to, duration);
        return tween;
    }

    Coroutine pulseRoutine;
    public void TriggerPulse(int count, float delay)
    {
        if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        pulseRoutine = StartCoroutine(PulseRoutine(count, delay));
    }
    #endregion

    IEnumerator PulseRoutine(int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            SetPulse(1);
            yield return new WaitForSeconds(delay);
            SetPulse(0);
            yield return new WaitForSeconds(delay);
        }

        pulseRoutine = null;
    }
}
