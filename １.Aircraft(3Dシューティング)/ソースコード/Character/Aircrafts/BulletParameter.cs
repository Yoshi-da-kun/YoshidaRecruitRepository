
using UnityEngine;

/// --------------------------------------------------
/// #AircraftBullet.cs
/// �쐬��:�g�c�Y��
/// 
/// �e�̃p�����[�^���܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/BulletParameter" ,fileName = "NewBulletParameter")]
public class BulletParameter : ScriptableObject
{
    [field: SerializeField, Label("�e��Prefab")]
    public GameObject _bulletPrefab { get; private set; }

    [field: SerializeField, Label("�e�̑��x")]
    public float _bulletSpeed { get; private set; }

    [field: SerializeField, Label("�e�̍U����")]
    public int _bulletPower { get; private set; }

    [field: SerializeField, Label("�e�����˂���Ă��������܂ł̎���")]
    public int _bulletBreakTime { get; private set; }

    [field: SerializeField, Label("�e�������������̃G�t�F�N�g")]
    public GameObject _hitEffect { get; private set; }

    [field: SerializeField, Label("�e��������������SE")]
    public AudioClip _hitSound { get; private set; }

    [field: SerializeField, Label("�e�𔭎˂�������SE")]
    public AudioClip _shotSound { get; private set; }
}
