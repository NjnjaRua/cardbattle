using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(SpriteAtlasScript))]
public class SpriteAtlasScriptEditor : Editor
{
    private SpriteAtlasScript spriteAtlasScript;

    void OnEnable()
    {
        spriteAtlasScript = (SpriteAtlasScript)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Apply"))
        {
            if(spriteAtlasScript != null )
            {
                spriteAtlasScript.OnChangeSprite();
            }
        }
    }
}
#endif
