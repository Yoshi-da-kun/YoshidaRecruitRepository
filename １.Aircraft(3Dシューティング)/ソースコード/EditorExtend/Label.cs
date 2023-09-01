
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// --------------------------------------------------
/// #Label.cs
/// �쐬��:�g�c�Y��
/// 
/// Inspector�ɕ\������ϐ�������͂���������([Label("������")])�ɕϊ�����X�N���v�g
/// --------------------------------------------------

public class Label : PropertyAttribute
{
    //Inspector�ɕ\������镶������i�[����(�ǂݎ���p)
    public readonly string _label;

    /// <param name="label"> Inspector�ɕ\������镶����</param>
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
        // Label�N���X���擾
        Label newLabel = attribute as Label;

        // Label�N���X�Ɉ�����string��n��
        label.text = newLabel._label;

        // Inspector��ł̕\�����㏑������
        EditorGUI.PropertyField(position, property, label, true);
    }


    /// <param name="property"> Inspector�̗v�f </param>
    /// <param name="label"> Inspector�ɕ\�����镶���� </param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // �Ăяo�����ϐ���Inspector���W��Ԃ�
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
#endif
