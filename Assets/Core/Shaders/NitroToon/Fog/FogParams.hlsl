#ifndef NYTRO_FOG_PARAMS
#define NYTRO_FOG_PARAMS


float4 _nytroAmbiance_fogColor;

float _nytroAmbiance_linearFogStart;
float _nytroAmbiance_linearFogDepth;
float _nytroAmbiance_linearFogBlend;

float _nytroAmbiance_verticalFogStartHeight;
float _nytroAmbiance_verticalFogDepth;
float _nytroAmbiance_verticalFogBlend;

void GetFogParams_float(
    out float4 fogColor,
    out float linearFogStart,
    out float linearFogDepth,
    out float linearFogBlend,
    out float verticalFogStart,
    out float verticalFogDepth,
    out float verticalFogBlend)
{
    fogColor = _nytroAmbiance_fogColor;

    linearFogStart = _nytroAmbiance_linearFogStart;
    linearFogDepth = _nytroAmbiance_linearFogDepth;
    linearFogBlend = _nytroAmbiance_linearFogBlend;

    verticalFogStart = _nytroAmbiance_verticalFogStartHeight;
    verticalFogDepth = _nytroAmbiance_verticalFogDepth;
    verticalFogBlend = _nytroAmbiance_verticalFogBlend;
}

#endif