
using UnityEngine;

/// --------------------------------------------------
/// #OriginalSphereCollider.cs
/// �쐬��:�g�c�Y��
/// 
/// ��(Sphere)�̃R���C�_�[�̃f�[�^�Ə��������X�N���v�g
/// --------------------------------------------------

public class OriginalSphereCollider : OriginalCollider
{
    // ���̃R���C�_�[��Transform 
    public Transform _thisTransform { get; private set; } = default;

    [field: SerializeField, Header("�����蔻��̑傫��")]
    public float _colliderRadius { get; private set; }


    private void Awake()
    {
        // ���g�̃R���C�_�[�̌`����i�[����
        _colliderShape = _typeOfColliderShape.Sphere;

        _thisTransform = this.transform;
    }
}
