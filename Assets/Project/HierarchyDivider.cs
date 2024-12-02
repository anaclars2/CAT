using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class HierarchyDivider : MonoBehaviour
{
    [SerializeField] private string dividerName = "=== Divider ===";
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private FontStyle fontStyle = FontStyle.Bold;

    // Propriedades públicas para evitar problemas de acesso
    public string DividerName => dividerName;
    public Color TextColor => textColor;
    public FontStyle FontStyle => fontStyle;
}

// Customização da Hierarchy
[InitializeOnLoad]
public static class HierarchyDividerDrawer
{
    static HierarchyDividerDrawer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyItem;
    }

    private static void DrawHierarchyItem(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;

        var divider = obj.GetComponent<HierarchyDivider>();
        if (divider == null) return;

        // Evitar que o Unity renderize o texto padrão
        GUI.backgroundColor = Color.clear;
        EditorGUI.LabelField(selectionRect, "", new GUIStyle());

        // Estilo visual do texto personalizado
        var style = new GUIStyle
        {
            normal = { textColor = divider.TextColor },
            fontStyle = divider.FontStyle,
            alignment = TextAnchor.MiddleCenter,
            fontSize = 12 // Ajuste o tamanho da fonte se necessário
        };

        // Desenhar o texto do divisor
        Rect adjustedRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, selectionRect.height);
        EditorGUI.LabelField(adjustedRect, divider.DividerName, style);
    }
}
