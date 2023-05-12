using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class FadeSystem : MonoBehaviour
{
    [Tooltip("�t�F�[�h��Image������Object")]
    [SerializeField] GameObject _fadeObject;

    [Tooltip("�t�F�[�h�Ɏg��Image")]
    [SerializeField] Image _fadeImage;

    void Awake()
    {
        _fadeObject.SetActive(true);
    }


    public void StartFadeIn()//�t�F�[�h�C���֐�
    {
        _fadeImage.DOFade(endValue: 0f, duration: 1.5f).OnComplete(() => _fadeImage.gameObject.SetActive(false));
    }


    public void StartFadeOut(string scene)//�t�F�[�h�A�E�g�֐�
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(endValue: 1f, duration: 1.5f).OnComplete(() => SceneManager.LoadScene(scene));
    }


    public void Exit()
    {
        Application.Quit();
    }
    public void OnTitleSceneTo()
    {
        StartFadeOut("TitleScene");
    }


    void OnDisable()
    {
        DOTween.KillAll();
    }
}