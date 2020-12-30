using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public event EventHandler onStart;

    public Image Overlay;
    public CinemachineVirtualCamera IntroCamera;

    public TextMeshProUGUI TitleLabel;
    public TextMeshProUGUI StartLabel;
    
    private Inputs _inputs;

    void Start()
    {
        LeanTween.alpha(Overlay.rectTransform, 0.0f, 1f).setFrom(1.0f).setEaseInOutCirc().setOnComplete(() =>
        {
            Overlay.gameObject.SetActive(false);
        });

        _inputs = new Inputs();
        _inputs.Enable();
        _inputs.Endmenu.Restart.performed += OnStartAction;
    }

	private void OnStartAction(InputAction.CallbackContext obj)
	{
        Debug.Log("Start action");
        // Hide the menu, start game
        onStart?.Invoke(this, EventArgs.Empty);
        IntroCamera.gameObject.SetActive(false);

        LeanTween.value(gameObject, 1.0f, 0.0f, 1.0f).setEaseInOutCirc()
                .setOnUpdate((float a) =>
                {
                    TitleLabel.alpha = a;
                    StartLabel.alpha = a;
                }).setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
    }

	private void OnEnable()
    {
        if (_inputs == null) return;
        _inputs.Enable();
    }

    private void OnDisable()
    {
        if (_inputs == null) return;
        _inputs.Disable();
    }
}
