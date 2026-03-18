using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

public class SmoothnessResetter : AssetPostprocessor
{
    void OnPreprocessMaterialDescription(MaterialDescription description, Material material, AnimationClip[] animations)
    {
        material.SetFloat("_Glossiness", 0.0f);
        
        if (material.HasProperty("_Smoothness"))
        {
            material.SetFloat("_Smoothness", 0f);
        }

        material.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
        material.SetFloat("_SpecularHighlights", 0f);

        material.EnableKeyword("_GLOSSYREFLECTIONS_OFF");
        material.SetFloat("_GlossyReflections", 0f);

        material.EnableKeyword("_ENVIRONMENTREFLECTIONS_OFF");
        material.SetFloat("_EnvironmentReflections", 0f);
    }
}