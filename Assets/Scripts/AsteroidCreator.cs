using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCreator : Singleton<AsteroidCreator> {

    public List<GameObject> asteroidPrefabs;

    public float difficultyMultiplier = 1;

    private int _numAsteroidsToCreate;

    ExperienceManager experienceManager;

    float asteroidRangeCircumference = 1;

	void Start () {
        experienceManager = ExperienceManager.Instance;
        //CreateAsteroids();
	}

    public void CreateAsteroids()
    {
        _numAsteroidsToCreate = (int)(experienceManager.currentNumberOfBounces * difficultyMultiplier);

        for (int i = 0; i < _numAsteroidsToCreate; i++)
        {
            GameObject newAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count - 1)]) as GameObject;
            LerpAsteroidScale scaleLerp = newAsteroid.GetComponentInChildren<LerpAsteroidScale>();
            scaleLerp.BeginLerp(1);
            experienceManager.asteroids.Add(newAsteroid);
            Vector3 vecTowardHome = experienceManager.currentDestinationObject.transform.position - Camera.main.transform.position;
            Vector3 midPointPosition = Camera.main.transform.position + vecTowardHome.normalized * vecTowardHome.magnitude / 2;
            newAsteroid.transform.position = midPointPosition + new Vector3(
                Random.Range(-asteroidRangeCircumference, asteroidRangeCircumference),
                Random.Range(0, asteroidRangeCircumference),
                Random.Range(-asteroidRangeCircumference, asteroidRangeCircumference)
                );
            float randScale = Random.Range(0.5f*newAsteroid.transform.localScale.x, 1.1f * newAsteroid.transform.localScale.x);
            newAsteroid.transform.localScale = new Vector3(randScale, randScale, randScale);
            newAsteroid.transform.rotation = Random.rotation;


        }
    }
}
