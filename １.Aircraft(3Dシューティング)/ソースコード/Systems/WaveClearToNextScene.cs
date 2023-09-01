
using UnityEngine;

/// --------------------------------------------------
/// #WaveClearToNextScene.cs
/// �쐬��:�g�c�Y��
/// 
/// �E�F�[�u�̃N���A����Scene�ړ��������s���X�N���v�g
/// --------------------------------------------------

public class WaveClearToNextScene : MonoBehaviour
{
    [SerializeField, Label("FadeChangeScene�̃X�N���v�g")]
    private FadeChangeScene _fadeChangeScene;

    [SerializeField, Label("�t�F�[�h�C�����J�n����܂łɂ����鎞��")]
    private float _startFadeInTime = 1;

    // �t�F�[�h�C���J�n����܂ł̎��Ԍv���p�̕ϐ�
    private float _startFadeInElapsedTime = default;

    // �t�F�[�h�C�����J�n�������������t���O
    private bool _fadeInStarted = default;


    private void Update()
    {
        // �t�F�[�h�C�����J�n���Ă����珈�����Ȃ�
        if (_fadeInStarted)
        {
            return;
        }

        // �S�Ă̓G��j�󂵂Ă���Ƃ��AScene���ړ����J�n����
        if (WaveState._isAllTargetBreaked)
        {
            // �o�ߎ��Ԃ̌v�����s��
            _startFadeInElapsedTime += Time.deltaTime;

            // �t�F�[�h�C���̊J�n���Ԃ𒴂����Ƃ�
            if (_startFadeInElapsedTime >= _startFadeInTime)
            {
                // �t�F�[�h�C�����J�n����Scene���ړ�����
                _fadeChangeScene.FadeInStart();

                // �t�F�[�h�C���J�n�t���O���Z�b�g
                _fadeInStarted = true;
            }
        }
    }
}
