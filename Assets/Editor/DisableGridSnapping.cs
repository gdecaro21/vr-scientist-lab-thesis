using UnityEditor;
using UnityEngine;
using System.Linq;

[InitializeOnLoad]
public static class DisableGridSnapping
{
    static DisableGridSnapping()
    {
        var dataType = typeof(EditorSnapSettings).Assembly
            .GetType("UnityEditor.EditorSnapSettingsData");
        if (dataType == null) return;

        var instance = Resources.FindObjectsOfTypeAll(dataType).FirstOrDefault() as Object;
        if (instance == null) return;

        var so = new SerializedObject(instance);
        var prop = so.FindProperty("m_SnapSettings.m_SnapToGrid");
        if (prop == null) return;

        prop.boolValue = false;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
