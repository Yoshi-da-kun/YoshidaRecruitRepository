using UnityEngine;
using UnityEditor;

/// --------------------------------------------------
/// #RemoveComponentInChild.cs
/// 作成者:Qiitaの記事より引用　(https://qiita.com/OKsaiyowa/items/4380893a2a9653cf41e3)
/// 追記:吉田雄伍(コライダー部分)
/// 
/// ゲームオブジェクトからコンポーネントを一気に削除(子オブジェクト含)するスクリプト
/// --------------------------------------------------

public class RemoveComponentInChild : MonoBehaviour
{

    [Header("削除したいコンポーネントを文字列で指定"), SerializeField]
    string componentName;

    // コライダーのコンポーネント名
    string[] colliderComponentNames = { "BoxCollider", "SphereCollider", "CapsuleCollider", "MeshCollider"};

    //コンポーネントを取得して該当コンポーネントを削除
    void GetComAndDes()
    {
        Component[] components = GetComponentsInChildren<Component>();
        foreach (Component component in components)
        {
            if (component.GetType().Name == componentName)
            {
                DestroyImmediate(component);
            }

            #region ------------------コライダーすべてを削除する部分------------------

            foreach (string colliderName in colliderComponentNames)
            {
                if (component.GetType().Name == colliderName)
                {
                    DestroyImmediate(component);
                }
            }
            #endregion ------------------コライダーすべてを削除する部分------------------
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

            if (GUILayout.Button("一括削除"))
            {
                // 押下時に実行したい処理
                rootClass.GetComAndDes();
            }

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}