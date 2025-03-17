using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMoveController : MonoBehaviour
{
    

    [SerializeField] private CharacterController characterController;
    public float speed = 5.0f;
    public float sensitivity = 2.0f;
    public Camera cam;
    [SerializeField] private float rayDistance;
    public bool isMoving;
    [SerializeField] Transform playerModel;
    public CameraShake camShake;
    [HideInInspector]
    private float verticalRotation = 0f;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private Transform root;
    private void OnEnable()
    {
        StartCoroutine(ShowTutorialWithDelay());
    }
    void Update()
    {
        MoveControl();
        CameraControl();

        playerModel.position = root.position ;

        playerModel.rotation = cam.transform.rotation;
    }

    void MoveControl()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        moveDirection.y = -3;
        if(characterController.velocity.magnitude > 0.1f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        characterController.Move(moveDirection * speed * Time.deltaTime);
    }
    void CameraControl()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    public void ToggleControls(bool isEnable)
    {
        isMoving = isEnable;
        camShake.enabled = isEnable;
        enabled = isEnable;
       
    }
    public void MoveInstantly(Transform targetPos)
    {
        characterController.enabled = false;
        transform.position = targetPos.position;
        transform.rotation = targetPos.rotation;
        characterController.enabled = true;
        
    }
    IEnumerator ShowTutorialWithDelay()
    {
        yield return new WaitForSeconds(2f);
        tutorial.ShowTutorialStep(TutorialStepNames.wasd, true);
        yield return new WaitForSeconds(3f);
        tutorial.ShowTutorialStep(TutorialStepNames.wasd, false);
    }
}
