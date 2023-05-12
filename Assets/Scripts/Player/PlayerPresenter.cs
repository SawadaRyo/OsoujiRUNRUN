using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーのオブジェクト1")]
    PlayerContoller _playerContoller = null;
    [SerializeField, Tooltip("プレイヤーのオブジェクト2")]
    PlayerInput _playerInput = null;
    [SerializeField, Tooltip("ゴミを吸い取るパーティクル")]
    ParticleSystem _particleSystem = null;
    [SerializeField, Tooltip("所持しているごみを表すゲージ")]
    Slider _slider = null;

    [SerializeField]
    private GameObject _gaugeObject;

    private void Start()
    {
        _particleSystem.Stop();

        //_playerContoller.CurrentTrashs
        //    .Subscribe(x => _slider.value = (float)(x.Count / _playerContoller.maxGarbage))
        //    .AddTo(gameObject);

        _playerInput.Vaccum
            .Subscribe(x =>
            {
                if (x)
                {
                    _particleSystem.Play();
                }
                else
                {
                    _particleSystem.Stop();
                }
            })
            .AddTo(this);
        _playerContoller.TrashCount
            .Subscribe(x =>
            {
                _slider.value = (float)(x / _playerContoller.maxGarbage);
            });


        _gaugeObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
