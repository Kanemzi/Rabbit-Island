using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtinctionScreen : MonoBehaviour
{
    public event EventHandler onEnd;

    [Header("References")]
    public RabbitsManager Rabbits;
    public Image Overlay;
    public TextMeshProUGUI MessageLabel;
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI RestartLabel;

    private Inputs _inputs;
    private bool _canRestart;

    private int _maxPopulation = 0;
    public int MaxPopulation {
        get => _maxPopulation;
        set
		{
            _maxPopulation = value;
            UpdateMaxPopulationLabel(value);
		}
    }

	private void UpdateMaxPopulationLabel(int score)
	{
        ScoreLabel.text = "Maximum Population : " + score;
    }

	private void Start()
    {
        _canRestart = false;
        Rabbits.onEndangered += OnRabbitsEndangered;

        MessageLabel.gameObject.SetActive(false);
        ScoreLabel.gameObject.SetActive(false);
        RestartLabel.gameObject.SetActive(false);

        _inputs = new Inputs();
        _inputs.Enable();
        _inputs.Endmenu.Restart.performed += OnRestartAction;
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

    private void OnRabbitsEndangered(object sender, EventArgs data)
	{
        MaxPopulation = Rabbits.MaxRabbitsCount;
        Show();
	}

	private void Show()
	{
        LeanTween.alpha(Overlay.rectTransform, 0.9f, 0.5f).setFrom(0.0f).setOnComplete(() =>
        {
            MessageLabel.gameObject.SetActive(true);
            ScoreLabel.gameObject.SetActive(true);
            MessageLabel.alpha = 0.0f;
            ScoreLabel.alpha = 0.0f;

            LeanTween.moveY(MessageLabel.rectTransform, 30.0f, 1.0f).setFrom(0.0f).setEaseInOutCirc();
            LeanTween.moveY(ScoreLabel.rectTransform, -15.0f, 1.0f).setFrom(0.0f).setEaseInOutCirc();

            LeanTween.value(MessageLabel.gameObject, 0.0f, 1.0f, 1.5f).setEaseInOutCirc()
                .setOnUpdate((float a) =>
                {
                    MessageLabel.alpha = a;
                    ScoreLabel.alpha = a;
                }).setOnComplete(() =>
                {
                    RestartLabel.gameObject.SetActive(true);
                    _canRestart = true;
                    onEnd?.Invoke(this, EventArgs.Empty);
                });
        }); 
    }

    private void OnRestartAction(InputAction.CallbackContext obj)
    {
        if (!_canRestart) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
