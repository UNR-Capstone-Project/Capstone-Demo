using System.Collections;
using UnityEngine;

public class TempoManager : MonoBehaviour
{
    public enum HIT_QUALITY
    {
        BAD = 0,
        GOOD,
        EXCELLENT
    }
    public void SetTempo(float new_tempo)
    {
        StopUpdateBeatTick();
        _tempo = new_tempo;

        _timeBetweenBeats = 60 / _tempo;
        _excellentHitTime = _timeBetweenBeats * _excellentPercent;
        _goodHitTime = _timeBetweenBeats * _goodPercent;
        _badHitTime = _timeBetweenBeats * _badPercent;

        StartUpdateBeatTick();
    }
    private void BeatTick()
    {
        recentBeatTime = Time.time;
        Debug.Log("Tick");
    }
    private IEnumerator UpdateBeatTickTimer()
    {
        yield return new WaitForSeconds(_timeBetweenBeats);
        BeatTick();
        StartUpdateBeatTick();
    }
    public void StartUpdateBeatTick()
    {
        StartCoroutine(UpdateBeatTickTimer());
    }
    public void StopUpdateBeatTick()
    {
        StopCoroutine(UpdateBeatTickTimer());
    }
    public void CheckBeatHitTime(float playerHitTime, ref HIT_QUALITY hitQuality)
    {
        
    }

    void Start()
    {
        _timeBetweenBeats = 60/_tempo;
        _excellentHitTime = _timeBetweenBeats * _excellentPercent;
        _goodHitTime = _timeBetweenBeats * _goodPercent;
        _badHitTime = _timeBetweenBeats * _badPercent;

        StartUpdateBeatTick();
    }
    void Update()
    {
        Debug.Log(Time.time);
    }

    private float _tempo = 85f;
    [SerializeField] private float _timeBetweenBeats;
    private float recentBeatTime;

    private float _excellentPercent = 0.05f;
    private float _goodPercent = 0.1f;
    private float _badPercent = 0.25f;
    private float _excellentHitTime;
    private float _goodHitTime;
    private float _badHitTime;
}
