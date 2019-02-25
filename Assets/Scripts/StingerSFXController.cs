using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StingerSFXController : Singleton<StingerSFXController> {

    private AudioSource _sfxSource;

	void Start () {
        _sfxSource = GetComponent<AudioSource>();    
	}

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StingerSFXController.Instance.PlayStingerHit(66, 3);
        //}
    }

    // 17.5, 25
    public void PlayStingerHit(float startTime, float playbackTime)
    {
        _sfxSource.time = startTime;
        _sfxSource.Play();
        StartCoroutine(StopAfterSeconds(playbackTime));
    }
    IEnumerator StopAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _sfxSource.Stop();
    }
}
