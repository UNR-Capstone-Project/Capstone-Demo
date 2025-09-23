using System.Collections;
using UnityEngine;

public class TempoManager : MonoBehaviour
{
    public enum HIT_QUALITY
    {
        MISS = 0,
        BAD,
        GOOD,
        EXCELLENT
    }

    public void SetTempo(float new_tempo)
    {
        _tempo = new_tempo;

        _timeBetweenBeats = 60 / _tempo;

        _excellentHitTimeStart = _timeBetweenBeats * _excellentPercent;
        _goodHitTimeStart = _timeBetweenBeats * _goodPercent;
        _badHitTimeStart = _timeBetweenBeats * _badPercent;

        _excellentHitTimeEnd = _timeBetweenBeats - _excellentHitTimeStart;
        _goodHitTimeEnd = _timeBetweenBeats - _goodHitTimeStart;
        _badHitTimeEnd = _timeBetweenBeats - _badHitTimeStart;

    }
    private void BeatTick()
    {
        
        _currentBeatTime = 0;
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
    public HIT_QUALITY CheckHitQuality()
    {
        if (_currentBeatTime < _excellentHitTimeStart || _currentBeatTime > _excellentHitTimeEnd) return HIT_QUALITY.EXCELLENT;
        else if (_currentBeatTime < _goodHitTimeStart || _currentBeatTime > _goodHitTimeEnd)      return HIT_QUALITY.GOOD;
        else if (_currentBeatTime < _badHitTimeStart || _currentBeatTime > _badHitTimeEnd)        return HIT_QUALITY.BAD;
        return HIT_QUALITY.MISS;
    }

    void Start()
    {
        SetTempo(_tempo);

        StartUpdateBeatTick();
    }
    void Update()
    {
        _currentBeatTime += Time.deltaTime;
    }

    private float _tempo = 85f;
    [SerializeField] private float _timeBetweenBeats = 0;
    private float _currentBeatTime = 0;

    private float _excellentPercent = 0.05f;
    private float _goodPercent = 0.1f;
    private float _badPercent = 0.25f;

    //    Start                     End
    //      |------------------------|
    //   BEAT 1                    BEAT 2
    //These HitTime measurements are used to compare the _currentBeatTime with whether it lands within the thresholds of Excellent, Good, Bad or Miss
    private float _excellentHitTimeStart = 0;
    private float _goodHitTimeStart = 0;
    private float _badHitTimeStart = 0;
    private float _excellentHitTimeEnd = 0;
    private float _goodHitTimeEnd = 0;
    private float _badHitTimeEnd = 0;
}
