
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// --------------------------------------------------
/// #Label.cs
/// 作成者:吉田雄伍
/// 
/// Inspectorに表示する変数名を入力した文字列([Label("文字列")])に変換するスクリプト
/// --------------------------------------------------

public class Label : PropertyAttribute
{
    //Inspectorに表示される文字列を格納する(読み取り専用)
    public readonly string _label;

    /// <param name="label"> Inspectorに表示される文字列</param>
    public Label(string label)
    {
        _label = label;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Label))]
public class LabelAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Labelクラスを取得
        Label newLabel = attribute as Label;

        // Labelクラスに引数のstringを渡す
        label.text = newLabel._label;

        // Inspector上での表示を上書きする
        EditorGUI.PropertyField(position, property, label, true);
    }


    /// <param name="property"> Inspectorの要素 </param>
    /// <param name="label"> Inspectorに表示する文字列 </param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 呼び出した変数のInspector座標を返す
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
#endif
