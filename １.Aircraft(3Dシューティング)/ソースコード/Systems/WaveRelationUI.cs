
using UnityEngine;
using UnityEngine.UI;

/// --------------------------------------------------
/// #WaveRelationUI.cs
/// �쐬��:�g�c�Y��
/// 
/// �E�F�[�u�i�s�Ɋ֘A����UI�̏���������X�N���v�g
/// --------------------------------------------------

public class WaveRelationUI : MonoBehaviour
{
    [SerializeField, Label("�ڕW(mission)��\������e�L�X�g")]
    private Text _missionText;

    [SerializeField, Label("�G�̐���\������e�L�X�g")]
    private Text _targetCountText;

    [SerializeField, Header("�G�̐���\������e�L�X�g�̐����̑O�ƌ��ɂ��镶��(��:�O�@��:���)")]
    private string _startOfTargetCountText;
    [SerializeField]
    private string _endOfTargetCountText;

    [Header("")]
    [SerializeField, Label("�E�F�[�u�̏I�����ɕ\������e�L�X�g")]
    private Text _waveFinishText;

    // ���݂̎c��^�[�Q�b�g�̐�
    private int _currentRemainingTargetCount;


    private void Start()
    {
        // �E�F�[�u�I���̃e�L�X�g���\���ɂ���
        _waveFinishText.enabled = false;

        // �ڕW�̃e�L�X�g��\������
        _missionText.enabled = true;
    }


    private void Update()
    {
        // �c��^�[�Q�b�g�̐����ω��������̏���
        if (_currentRemainingTargetCount != WaveState._remainingTargets)
        {
            // ���݂̎c��^�[�Q�b�g�̐����X�V����
            _currentRemainingTargetCount = WaveState._remainingTargets;

            // �c��^�[�Q�b�g�̕ω����ɍs���e�L�X�g�̕ύX����
            RemainingTargetTextUpdate();

            // �c��^�[�Q�b�g�����O�ɂȂ����Ƃ�
            if (_currentRemainingTargetCount == 0)
            {
                // �E�F�[�u�I���̃e�L�X�g��\������
                _waveFinishText.enabled = true;

                // �ڕW�̃e�L�X�g���\���ɂ���
                _missionText.enabled = false;
            }
        }
    }


    /// <summary>
    /// �c��^�[�Q�b�g�̕ω����ɍs���e�L�X�g�̕ύX����
    /// </summary>
    private void RemainingTargetTextUpdate()
    {
        // �c��G���̃e�L�X�g���Ȃ��ꍇ�ȉ��̏������s��Ȃ�
        if (!_targetCountText)
        {
            return;
        }

        // �c��G���̃e�L�X�g�̐����X�V����
        _targetCountText.text = _startOfTargetCountText + _currentRemainingTargetCount + _endOfTargetCountText;
    }
}
