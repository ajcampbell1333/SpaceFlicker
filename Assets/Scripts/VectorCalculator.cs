using MagicLeap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class VectorCalculator : Singleton<VectorCalculator> {

    private Vector3 _startPosition, _startNormal, _startApproach;

    private Vector3 _currentHitPos;

    private float _confidence = 0.0f;

    [SerializeField]
    private GameObject _debugInstance;

    [SerializeField, Tooltip("Raycast from controller.")]
    private BounceRaycastController _raycastController;
    
    private Renderer _render;

    private int _bounceCount;

    private int _bounceTotal;

    public List<GameObject> bounces;

    private GameObject _controller;

    private int _hitCheck;

    private BounceRaycastController _rayComponent;

    private void Awake()
    {
        _controller = (FindObjectOfType(typeof(ControllerTransform)) as ControllerTransform).gameObject;
        
        List<GameObject> bounces = new List<GameObject>();
        //_raycastController.gameObject.SetActive(true);
        //_raycastController.Controller.gameObject.SetActive(true);
        //_render = _raycastController.transform.GetComponent<Renderer>();
        //if (_render == null)
        //{
        //    Debug.LogError("Error: RaycastVisualizer._render is not set, disabling script.");
        //    enabled = false;
        //    return;
        //}
    }

    /// <summary>
    /// For each level, this method creates a set of game objects whose
    /// transform positions mark the bounce points of the character during an ideal trajectory toward home.
    /// Position of home is always set to the last item in this array.
    /// </summary>
    public void CreateRandomTargetPositions(Vector3 startingHit, Vector3 startingDirection, Vector3 startingNormal, int numNewBounces)
    {
        _startPosition = startingHit;
        _startNormal = startingNormal;
        _startApproach = startingDirection;
        _bounceCount = numNewBounces;

        GameObject dummyController = new GameObject();

        // raycast to first hit
        Vector3 currentHitPos = startingHit;// new Vector3(Random.Range(0,3.0f), Random.Range(0, 3.0f), Random.Range(0, 3.0f));
        Vector3 currentHitNormal = new Vector3(Random.Range(0, 3.0f), Random.Range(0, 3.0f), Random.Range(0, 3.0f));
        Vector3 approachDirection = startingDirection; // currentHitPos - dummyController.transform.position;
        BounceRaycastController rayComponent = _controller.AddComponent(typeof(BounceRaycastController)) as BounceRaycastController;
        rayComponent.OnRaycastResult.AddListener(OnRaycastHit);

        //IterateBounce(approachDirection, currentHitNormal);
        
        StartCoroutine(CheckHit());

        // calc angle of approach - compare to hit normal to find departure angle
        //start from current controller position
        // cast a ray at a random rotation
        // if it hit world mesh, start with that point, else try again
        // get the vector direction by subtracting start from destination
        // 
        // randomly choose whether to instantiate a target
        // use the ratio of num targets to bounces to determine likelihood a target gets created at this bounce
        // track total num targets created - if numNewTargets - totalTargetsCreated == numNewBounces - numBouncesCreated
        // then stop randomly choosing whether to create a target and just create one for each remaining
        // create a game object for the current bounce if appropriate and orient forward vector along hit normal
        // add metadata to the game object (approach angle, departure angle, hasTarget, etc.)
        _hitCheck = 0;
    }

    IEnumerator CheckHit()
    {
        yield return new WaitForSeconds(0.1f);
        if (_hitCheck > 0)
        {
            // raycast didn't hit any valid surface
            // start the bounces array over
            bounces.Clear();
            CreateRandomTargetPositions(_startPosition, _startApproach, _startNormal, _bounceTotal);
        }
    }

    private void IterateBounce(Vector3 newHitPos, Vector3 approachDirection, Vector3 currentHitNormal)
    {
        Vector3 departureDirection = Vector3.Reflect(approachDirection, currentHitNormal);
        GameObject newHit = new GameObject();
        newHit.transform.position = newHitPos;
        Bounce bounce = newHit.AddComponent(typeof(Bounce)) as Bounce;
        bounce.departureDirection = departureDirection;
        bounce.approachDirection = approachDirection;
        // Instantiate debug visualizer objects here and attach them as children to newHit 
        GameObject dInstance = Instantiate(_debugInstance, newHit.transform.position, newHit.transform.rotation);
        dInstance.transform.SetParent(newHit.transform);
        bounces.Add(newHit);

        if (_bounceCount < _bounceTotal - 1)
        {
            _raycastController.transform.position = newHit.transform.position;
            _raycastController.transform.rotation = Quaternion.FromToRotation(_raycastController.transform.forward, departureDirection);
            _rayComponent = newHit.AddComponent(typeof(BounceRaycastController)) as BounceRaycastController;
            _rayComponent.OnRaycastResult.AddListener(OnRaycastHit);
        }
        _bounceCount++;

        
    }

    
    /// <summary>
    /// Callback handler called when raycast has a result.
    /// Updates the confidence value to the new confidence value.
    /// </summary>
    /// <param name="state"> The state of the raycast result.</param>
    /// <param name="result">The hit results (point, normal, distance).</param>
    /// <param name="confidence">Confidence value of hit. 0 no hit, 1 sure hit.</param>
    public void OnRaycastHit(MLWorldRays.MLWorldRaycastResultState state, RaycastHit result, float confidence)
    {
        _hitCheck++;
        _confidence = confidence;
        if (state != MLWorldRays.MLWorldRaycastResultState.RequestFailed && state != MLWorldRays.MLWorldRaycastResultState.NoCollision)
        {
            ////Debug.Log(string.Format("result point: {0} {1} {2}", result.point.x, result.point.y, result.point.z));
            //// Update the cursor position and normal.
            //_raycastController.transform.position = result.point;
            //_raycastController.transform.LookAt(result.normal + result.point);
            //_raycastController.transform.localScale = Vector3.one;

            //// Set the color to yellow if the hit is unobserved.
            //_render.material.color = (state == MLWorldRays.MLWorldRaycastResultState.HitObserved) ? Color.green : Color.yellow;
            _hitCheck = 0;

            if (_bounceCount < _bounceTotal)
            {
                IterateBounce(result.point, result.point - _currentHitPos, result.normal);
                _currentHitPos = result.point;
            }
            Destroy(_rayComponent);
        }
        else
        {
            //// Update the cursor position and normal.
            //_raycastController.transform.position = (_raycastController.RayOrigin + (_raycastController.RayDirection * 10));
            //_raycastController.transform.LookAt(_raycastController.RayOrigin);
            //_raycastController.transform.localScale = Vector3.one;

            //_render.material.color = Color.red;

           
        }
    }
    

}
