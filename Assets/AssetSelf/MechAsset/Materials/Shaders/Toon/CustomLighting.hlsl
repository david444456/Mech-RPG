
/*
#ifndef MAINLIGHT_INCLUDED
#define MAINLIGHT_INCLUDED

void MainLight_float
    (
    float3 WorldPos, out
    float3 Direction, out
    float3 Color, out
    float DistanceAtten, out
    float ShadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
        Direction = float3(0.5, 0.5, 0);
        Color = 1;
        DistanceAtten = 1;
        ShadowAtten = 1;
#else
    //float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    
    
    //#if defined(UNIVERSAL_LIGHTING_INCLUDED)
    
        // GetMainLight defined in Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl
        Light mainLight = GetMainLight();
        Direction = -mainLight.direction;
        Color = mainLight.color;
        DistanceAtten = mainLight.distanceAttenuation;
        ShadowAtten = mainLight.shadowAttenuation;
    
//#elif defined(HD_LIGHTING_INCLUDED) 
        // ToDo: make it work for HDRP (check define above)
        // Packages/com.unity.render-pipelines.high-definition/Runtime/Lighting/LightDefinition.cs.hlsl
        // if (_DirectionalLightCount > 0)
        // {
        //     DirectionalLightData light = _DirectionalLightDatas[0];
        //     lightDir = -light.forward.xyz;
        //     color = light.color;
        //     ......
        
    //#endif

#endif
}
#endif*/
