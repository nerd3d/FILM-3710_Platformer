using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
  
  public AudioSource efxSource;
  public AudioSource musicSource;
  public static SoundManager instance = null;

  public float lowPitchRange = 0.95f;
  public float highPitchRange = 1.05f;

  // Use this for initialization
  void Awake() {
    if (instance == null)
      instance = this;
    else if (instance != this)
      Destroy(gameObject);

    DontDestroyOnLoad(gameObject);
  }

  public void PlaySingle(AudioClip _clip) {
    efxSource.clip = _clip;
    efxSource.Play();
  }

  public void RandomizeSfx(params AudioClip [] _clips) {
    int randomIndex = Random.Range(0, _clips.Length);
    float randomPitch = Random.Range(lowPitchRange, highPitchRange);

    efxSource.pitch = randomPitch;
    efxSource.clip = _clips[randomIndex];
    efxSource.Play();
  }
}
