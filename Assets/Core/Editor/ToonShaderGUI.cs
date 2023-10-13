using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToonShaderGUI : ShaderGUI
{
    const int indentLevel = 1;

    protected Material target;
    protected MaterialEditor editor;
    protected MaterialProperty[] properties;
    protected GUIContent content;

    bool useAdvanced = false;

    protected bool hideOpacitySlider = true;
    protected bool hideAlphaOverride = false;
    protected string mainLabel = "NytroToon V4 - Opaque";

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        if (useAdvanced)
        {
            base.OnGUI(materialEditor, properties);
            DoUIButton();
            return;
        }

        target = materialEditor.target as Material;
        editor = materialEditor;
        this.properties = properties;
        content = new GUIContent();

        bool fresnel = System.Array.IndexOf(target.shaderKeywords, "NITROTOON_FRESNEL_ON") != -1;
        bool specular = System.Array.IndexOf(target.shaderKeywords, "NITROTOON_SPECULAR_ON") != -1;

        GUILayout.Label(mainLabel, EditorStyles.boldLabel);

        EditorGUILayout.Space();
        GUILayout.Label("Main");
        AddIndent();
        {
            MaterialProperty palette = FindProperty("_tex", properties);
            MaterialProperty tint = FindProperty("_tint", properties);
            GUIContent paletteLabel = new GUIContent("Palette");
            materialEditor.TexturePropertySingleLine(paletteLabel, palette, tint);

            if(!hideAlphaOverride) DoDefaultProperty("_alphaOverride", "Mask Color");

            DoDefaultProperty("_exposure", "Exposure");

            if (!hideOpacitySlider) DoDefaultProperty("_alpha", "Alpha");

        }
        RemoveIndent();

        EditorGUILayout.Space();
        GUILayout.Label("Shine");
        AddIndent();
        {
            EditorGUI.BeginDisabledGroup(!(specular | fresnel));
            DoDefaultProperty("_fresnelColor", "Shine Color");
            DoDefaultProperty("_fresnelCutoffSize", "Smoothness");
            EditorGUI.EndDisabledGroup();
        }
        RemoveIndent();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!DoKeyword("NITROTOON_TOON_ON", "Toon Shading"));
        {
            AddIndent();
            DoDefaultProperty("_shadowCutoff", "Cutoff");
            DoDefaultProperty("_shadowCutoffSize", "Size");
            RemoveIndent();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!DoKeyword("NITROTOON_FRESNEL_ON", "Fresnel"));
        {
            AddIndent();
            DoDefaultProperty("_fresnelCutoff", "Limit");
            RemoveIndent();
        } EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(!DoKeyword("NITROTOON_SPECULAR_ON", "Specular"));
        {
            AddIndent();
            DoDefaultProperty("_specularPower", "Power");
            RemoveIndent();
        } EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        DoKeyword("NITROTOON_FOG_ON", "Fog");

        EditorGUILayout.Space();
        GUILayout.Label("Misc. Options", EditorStyles.boldLabel);
        materialEditor.EnableInstancingField();
        materialEditor.RenderQueueField();
        DoUIButton();
    }

    void DoUIButton()
    {
        if (GUILayout.Button("Toggle UI"))
        {
            useAdvanced = !useAdvanced;
        }
    }

    protected void AddIndent()
    {
        EditorGUI.indentLevel += indentLevel;
    }

    protected void RemoveIndent()
    {
        EditorGUI.indentLevel -= indentLevel;
    }

    protected void DoDefaultProperty(string key, string label)
    {
        content.text = label;
        editor.ShaderProperty(FindProperty(key, properties), content);
    }

    protected bool DoKeyword(string keyword, string label)
    {
        bool state = System.Array.IndexOf(target.shaderKeywords, keyword) != -1;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        state = EditorGUILayout.Toggle(state, GUILayout.MaxWidth(15));
        EditorGUILayout.LabelField(label);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            SetKeyword(keyword, state);
        }

        return state;
    }
    protected void SetKeyword(string keyword, bool state)
    {
        if (state)
        {
            foreach (Material material in editor.targets)
            {
                material.EnableKeyword(keyword);
            }

        }
        else
        {
            foreach (Material material in editor.targets)
            {
                material.DisableKeyword(keyword);
            }
        }
    }
}
