using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(ControllerConnectionHandler))]
public class ControllerInputHandler : MonoBehaviour {

    private ControllerConnectionHandler _controllerConnectionHandler;

    private BounceRaycastController _rayComponent;

    private void Start()
    {
        _controllerConnectionHandler = GetComponent<ControllerConnectionHandler>();

        MLInput.OnControllerButtonUp += HandleOnButtonUp;
        MLInput.OnControllerButtonDown += HandleOnButtonDown;

        MLInput.OnTriggerDown += HandleOnTriggerDown;
        MLInput.OnTriggerUp += HandleOnTriggerUp;
    }

    private void HandleOnButtonDown(byte controllerId, MLInputControllerButton button)
    {
        if (_controllerConnectionHandler.IsControllerValid() && _controllerConnectionHandler.ConnectedController.Id == controllerId &&
            button == MLInputControllerButton.Bumper)
        {
            Debug.Log("bumper down");

            ExperienceManager.Instance.OnReset();
        }
    }
    
    private void HandleOnButtonUp(byte controllerId, MLInputControllerButton button)
    {
        if (_controllerConnectionHandler.IsControllerValid() && _controllerConnectionHandler.ConnectedController.Id == controllerId)
        {
            if (button == MLInputControllerButton.Bumper)
            {
                Debug.Log("bumper up");
            }

            else if (button == MLInputControllerButton.HomeTap)
            {
                Debug.Log("home tap down");
            }
        }
    }

    private void HandleOnTriggerDown(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {
            if (ExperienceManager.Instance.experienceHasBegun)
            {
                //Debug.Log("down: " + value);
                //_rayComponent = gameObject.AddComponent(typeof(BounceRaycastController)) as BounceRaycastController;
                //AddRaycastListener(_rayComponent);

                BlendShapeTrigger.Instance.SetFinger();
            }
            else
            {
                //ExperienceManager.Instance.experienceHasBegun = true;
                ExperienceManager.Instance.Begin();
            }

        }

        
        
    }

    IEnumerator AddRaycastListener(BounceRaycastController _rayComponent)
    {
        yield return new WaitForSeconds(1);
        _rayComponent.OnRaycastResult.AddListener(OnRaycastHit);

    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        StingerSFXController.Instance.PlayStingerHit(17.5f, 1);
    //    }
    //}

    private void HandleOnTriggerUp(byte controllerId, float value)
    {
        MLInputController controller = _controllerConnectionHandler.ConnectedController;
        if (controller != null && controller.Id == controllerId)
        {

            //Debug.Log("up: " +value);
            if (ExperienceManager.Instance.experienceHasBegun)
            {
                BlendShapeTrigger.Instance.Flick();
                StingerSFXController.Instance.PlayStingerHit(17.5f, 1);
            }
                
            else ExperienceManager.Instance.experienceHasBegun = true;
            
        }
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
        if (state != MLWorldRays.MLWorldRaycastResultState.RequestFailed && state != MLWorldRays.MLWorldRaycastResultState.NoCollision)
        {
            VectorCalculator.Instance.CreateRandomTargetPositions(result.point, result.point - transform.position, result.normal, 2);
            _rayComponent.OnRaycastResult.RemoveAllListeners();
            Destroy(_rayComponent);
        }
        else
        {
            
        }
    }
}
