using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

public class SmoothnessResetter : AssetPostprocessor
{
    // Wywoływane automatycznie przy każdym imporcie modelu FBX
    void OnPreprocessMaterialDescription(MaterialDescription description, Material material, AnimationClip[] animations)
    {
        // Ustawienie parametru gładkości na 0
        // W standardowym shaderze Unity odpowiada za to "_Glossiness" lub "_GlossMapScale"
        material.SetFloat("_Glossiness", 0.0f);
        
        if (material.HasProperty("_Smoothness"))
        {
            material.SetFloat("_Smoothness", 0f);
        }
    }
}