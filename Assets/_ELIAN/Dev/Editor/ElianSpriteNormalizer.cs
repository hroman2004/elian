using UnityEditor;
using UnityEngine;

public static class ElianSpriteNormalizer
{
    [MenuItem("Tools/ELIAN/Normalizar sprites")]
    public static void NormalizeSprites()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/_ELIAN" });

        int modificados = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!path.EndsWith(".png"))
                continue;

            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer == null)
                continue;

            bool cambio = false;

            if (importer.spritePixelsPerUnit != 32f)
            {
                importer.spritePixelsPerUnit = 32f;
                cambio = true;
            }

            if (importer.filterMode != FilterMode.Point)
            {
                importer.filterMode = FilterMode.Point;
                cambio = true;
            }

            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                cambio = true;
            }

            if (cambio)
            {
                importer.SaveAndReimport();
                modificados++;
            }
        }

        Debug.Log($"ELIAN: normalización terminada. Sprites modificados: {modificados}");
    }
}
