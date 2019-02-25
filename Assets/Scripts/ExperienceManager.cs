using MagicLeap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : Singleton<ExperienceManager> {

    public int currentLevel;
    public int currentNumberOfBounces = 3;
    
    public List<GameObject> asteroids;
    public GameObject homePrefab;
    public GameObject characterPrefab;
    public GameObject startingAsteroidPrefab;

    public GameObject currentStartObject;
    public GameObject currentDestinationObject;
    public GameObject currentCharacter;

    private GameObject _controller;

    public bool experienceHasBegun;

    private void Start()
    {
        _controller = (FindObjectOfType(typeof(ControllerTransform)) as ControllerTransform).gameObject;
    }

    public void Begin()
    {
        StartLevel(0);
    }

    public void OnSuccess()
    {
        StartCoroutine(SuccessSequence());
    }

    IEnumerator SuccessSequence()
    {
        //Display success message
        StingerSFXController.Instance.PlayStingerHit(66, 3);

        yield return new WaitForSeconds(3);
        //dismiss and destroy all asteroids
        float waitTime = 0;
        foreach (GameObject asteroid in asteroids)
        {
            LerpAsteroidScale scaleLerp = asteroid.GetComponent<LerpAsteroidScale>();
            if (scaleLerp != null)
            {
                waitTime = scaleLerp.duration;
                scaleLerp.BeginLerp(-1);
            }
        }

        yield return new WaitForSeconds(waitTime);
        currentLevel++;
        asteroids.Clear();
        Destroy(currentCharacter);
        Destroy(currentDestinationObject);
        Destroy(currentStartObject);
        StartLevel(currentLevel);
        yield return null;
    }

    public void StartLevel(int levelIndex)
    {
        // display level intro message
        // Use vector calc to create ideal path
        Instantiate(startingAsteroidPrefab, _controller.transform.position + _controller.transform.forward * 0.3f, _controller.transform.rotation);

        currentCharacter = Instantiate(characterPrefab, _controller.transform.position + _controller.transform.forward * 0.3f + new Vector3(0,0.1f,0), _controller.transform.rotation);
        currentDestinationObject = Instantiate(homePrefab, _controller.transform.position + _controller.transform.forward * 2f,  Quaternion.Inverse(_controller.transform.rotation));
        StartCoroutine(CreateAsteroidsWhenReady());
    }

    IEnumerator CreateAsteroidsWhenReady()
    {
        yield return new WaitUntil(() => (currentDestinationObject!=null));
        AsteroidCreator.Instance.CreateAsteroids();
    }

    public void OnReset()
    {
        // Dismiss all level objects
        // re-init current level
        // re-display level intro message

    }
}
