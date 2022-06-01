using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private float _deltaTime;
    private bool _stopClock = false;
    private TMP_Text _clockText;

    public static Clock Instance;


    private void Awake()
    {
        if (Instance)
            Destroy(Instance);
        Instance = this;
        _clockText = GetComponent<TMP_Text>();
        _deltaTime = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _stopClock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stopClock)
        {
            _deltaTime += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(_deltaTime);

            string hour = LeadingZero(span.Hours);
            string minute = LeadingZero(span.Minutes);
            string second = LeadingZero(span.Seconds);

            _clockText.text = hour + ":" + minute + ":" + second;
        }
    }


    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }


    public void OnGameOver() { _stopClock = true; }


    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }


    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }


    public TMP_Text GetCurrentTimeText() { return _clockText; }
}
