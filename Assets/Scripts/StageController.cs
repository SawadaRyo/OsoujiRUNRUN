using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;
using Cinemachine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private GameObject _viewCamera;

    private void Start()
    {
        GameManager.Instance.SetTrashValue(
            FindObjectsOfType<Trash>().Length);

        SoundManager.Instance.AudioPlay(SoundType.BGM, 0);
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsReady)
        {
            _viewCamera.SetActive(true);
        }
        else
        {
            _viewCamera.SetActive(false);
        }
    }
}
