
using UnityEngine;

/// --------------------------------------------------
/// #TargetBomb.cs
/// �쐬��:�g�c�Y��
/// 
/// ���e�^�̓I�̏������s���X�N���v�g
/// --------------------------------------------------

public class TargetBomb : TargetManager
{
    [SerializeField, Label("���e�̃p�����[�^")]
    TargetBombParameter _bombParameter;

    [SerializeField, Label("���𗬂��X�N���v�g")]
    private SoundController _soundController;


    private void Update()
    {
        // �󂳂ꂽ���̏���
        if (_isDead)
        {
            // �G�t�F�N�g���o��
            Instantiate(_bombParameter._breakedEffectPrefab, this.transform.position, Quaternion.identity);

            // �����o��
            _soundController.PlaySeSound(_bombParameter._breakedSound);

            // �I�u�W�F�N�g�𖳌�������
            this.gameObject.SetActive(false);
        }
    }
}
