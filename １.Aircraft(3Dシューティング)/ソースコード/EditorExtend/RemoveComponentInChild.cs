using UnityEngine;
using UnityEditor;

/// --------------------------------------------------
/// #RemoveComponentInChild.cs
/// �쐬��:Qiita�̋L�������p�@(https://qiita.com/OKsaiyowa/items/4380893a2a9653cf41e3)
/// �ǋL:�g�c�Y��(�R���C�_�[����)
/// 
/// �Q�[���I�u�W�F�N�g����R���|�[�l���g����C�ɍ폜(�q�I�u�W�F�N�g��)����X�N���v�g
/// --------------------------------------------------

public class RemoveComponentInChild : MonoBehaviour
{

    [Header("�폜�������R���|�[�l���g�𕶎���Ŏw��"), SerializeField]
    string componentName;

    // �R���C�_�[�̃R���|�[�l���g��
    string[] colliderComponentNames = { "BoxCollider", "SphereCollider", "CapsuleCollider", "MeshCollider"};

    //�R���|�[�l���g���擾���ĊY���R���|�[�l���g���폜
    void GetComAndDes()
    {
        Component[] components = GetComponentsInChildren<Component>();
        foreach (Component component in components)
        {
            if (component.GetType().Name == componentName)
            {
                DestroyImmediate(component);
            }

            #region ------------------�R���C�_�[���ׂĂ��폜���镔��------------------

            foreach (string colliderName in colliderComponentNames)
            {
                if (component.GetType().Name == colliderName)
                {
                    DestroyImmediate(component);
                }
            }
            #endregion ------------------�R���C�_�[���ׂĂ��폜���镔��------------------
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RemoveComponentInChild))]
    public class ExampleInspector : Editor
    {
        RemoveComponentInChild rootClass;

        private void OnEnable()
        {
            rootClass = target as RemoveComponentInChild;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("�ꊇ�폜"))
            {
                // �������Ɏ��s����������
                rootClass.GetComAndDes();
            }

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}