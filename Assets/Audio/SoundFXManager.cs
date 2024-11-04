using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager Instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClip (AudioClip audioClip, Transform spawnTransform, float volume) {
        // spawn in
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        // assign
        audioSource.clip = audioClip;

        // volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();
        // get length of clip

        float clipLength = audioSource.clip.length; 
        // destrpy the clip

        Destroy(audioSource.gameObject, clipLength);
    }


}
