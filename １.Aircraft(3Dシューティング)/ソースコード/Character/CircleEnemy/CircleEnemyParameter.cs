
using UnityEngine;

/// --------------------------------------------------
/// #CircleEnemyParameter.cs
/// �쐬��:�g�c�Y��
/// 
/// CircleEnemy�̃p�����[�^���܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

[CreateAssetMenu(menuName = "Parameters/CircleEnemyParameter", fileName = "NewCircleEnemyParameter")]
public class CircleEnemyParameter : CharacterBaseParameter
{
    [field: Header("�ړ��Ɋւ���ϐ�")]

    [field: SerializeField, Label("�ړ����x"), Range(0.01f,30)]
    public float _movingSpeed { get; private set; } = 0.5f;

    [field: SerializeField, Label("���񑬓x�ƕ���"), Range(-10, 10)]
    public float _turningSpeed { get; private set; } = 0.2f;

    [field: SerializeField, Label("�j�󂳂ꂽ�Ƃ��̃G�t�F�N�g��Prefab")]
    public GameObject _breakedEffectPrefab { get; private set; }

    [field: SerializeField, Label("�j�󂳂ꂽ����SE")]
    public AudioClip _breakedSound { get; private set; }


    private void OnEnable()
    {
        // CircleEnemy������\�ȑ��x�ɂȂ��Ă��邩���`�F�b�N����
        if (_turningSpeed == 0)
        {
            Debug.LogError("CircleEnemy��������s���܂���I");
        }
    }
}
