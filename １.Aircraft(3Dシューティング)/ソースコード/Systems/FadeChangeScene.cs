
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// --------------------------------------------------
/// #FadeChangeScene.cs
/// �쐬��:�g�c�Y��
/// 
/// �V�[���`�F���W����Ƃ��̃t�F�[�h�C���A�A�E�g�ƃV�[���ړ����s���X�N���v�g
/// --------------------------------------------------

public class FadeChangeScene : MonoBehaviour
{
    [SerializeField, Label("�t�F�[�h�C���A�A�E�g�p�̉摜")]
    private Image _fadePanel;

    [SerializeField, Label("�t�F�[�h�C���A�A�E�g����Ƃ��̑��x"), Range(0.001f, 1)]
    private float _fadeSpeed = 0.001f;

    // ���݂̃A���t�@�l
    private float _currentAlpha = default;

    // �t�F�[�h�C�����̃t���O
    private bool _isFadeIn = default;

    [SerializeField, Label("Scene�J�n���Ƀt�F�[�h�A�E�g���邩")]
    private bool _isFadeOut;

    [SerializeField, Label("�ړ�����V�[���̖��O")]
    private string _changeSceneName;


    private void Start()
    {
        // �t�F�[�h�A�E�g����Ȃ�
        if (_isFadeOut)
        {
            // �A���t�@�l�̍ő�l
            int maxAlphaValue = 1;

            // �t�F�[�h�p�̉摜�̓����x���ő�ɂ���
            _fadePanel.color = new Color(0, 0, 0, maxAlphaValue);
        }

        // ���݂̃A���t�@�l���i�[
        _currentAlpha = _fadePanel.color.a;
    }


    private void Update()
    {
        // �t�F�[�h�A�E�g����
        if (_isFadeOut)
        {
            // ���݂̃A���t�@�l���瓧���ɂȂ�悤�Ɍ��Z����
            _currentAlpha -= _fadeSpeed;
        
            // �t�F�[�h�A�E�g�p�̉摜�̃A���t�@�l���X�V
            _fadePanel.color = new Color(0, 0, 0, _currentAlpha);

            // �t�F�[�h�A�E�g�����������Ƃ�
            if (_currentAlpha <= 0)
            {
                // �t�F�[�h�A�E�g���I������
                _isFadeOut = false;
            }
        
            return;
        }
        
        // �t�F�[�h�C������
        if (_isFadeIn)
        {
            // ���݂̃A���t�@�l����s�����ɂȂ�悤�ɉ��Z����
            _currentAlpha += _fadeSpeed;
            
            // �t�F�[�h�C���p�̉摜�̃A���t�@�l���X�V
            _fadePanel.color = new Color(0, 0, 0, _currentAlpha);
            
            // �A���t�@�l�̍ő�l
            int maxAlphaValue = 1;

            // �t�F�[�h�C�������������Ƃ�
            if (_currentAlpha >= maxAlphaValue)
            {
                // Scene���ړ�����
                SceneManager.LoadScene(_changeSceneName);
            }
        }
    }


    /// <summary>
    /// �t�F�[�h�A�E�g���J�n����
    /// </summary>
    public void FadeOutStart()
    {
        _isFadeOut = true;
    }


    /// <summary>
    /// �t�F�[�h�C�����J�n����
    /// </summary>
    public void FadeInStart()
    {
        _isFadeIn = true;
    }
}
