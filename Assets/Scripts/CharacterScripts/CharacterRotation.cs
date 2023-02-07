using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterRotation : MonoBehaviour
{
    /*public float mouseSens;
    public bool invX = false, invY = false;*/
    
    [SerializeField] private float mouseSens, xNegativeClamp, xPositiveClamp, virtCamXRotationXNegativeClamp, virtCamXRotationxPositiveClamp, 
        composerVirtcamNegativeClamp, composerVirtcamPositiveClamp, speedVirtComposer;

    [SerializeField] private Transform playerChar, camTransformLookat;
    private float virtCamXRotation, virtCamYRotation;

    [SerializeField] private float raycastLength;
    [SerializeField] private Transform raycastStartPoint;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactableKeyCode;

    [SerializeField] private Vector3 virtCamComposerOffset;

    private CinemachineVirtualCamera virtCam;
    private CinemachineTransposer virtCamTransposer;
    private CinemachineComposer virtCamComposer;

    private float xRotation = 0f;
    private Camera camera;

    private void Start()
    {
        virtCam = GetComponent<CinemachineVirtualCamera>();
        virtCamTransposer = virtCam.GetCinemachineComponent<CinemachineTransposer>();
        virtCamComposer = virtCam.GetCinemachineComponent<CinemachineComposer>();

        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main;

        //VCT test
        //virtCamTransposer.m_FollowOffset = new Vector3(4f, 1f, -10f);
        //virtCamComposer.m_TrackedObjectOffset = new Vector3(4f, 1f, -10f);

    }

    private void Update()
    {
        Rotate();

        DetectInteractables();
    }

    private void DetectInteractables()
    {
        //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(raycastStartPoint.position, transform.forward);


        if (Physics.Raycast(ray, out RaycastHit hit, raycastLength, interactableLayer))
        {
            //<JPK> safety check, just in case I dont have one
            if (PlayerMessagesUI.Instance != null)
            {
                PlayerMessagesUI.Instance.SetPlayerText("E to interact with " + hit.collider.gameObject.name);

                if (Input.GetKeyDown(interactableKeyCode))
                {
                    hit.collider.gameObject.GetComponent<Interactable>().Interact();
                }
            }
        }
        else if (PlayerMessagesUI.Instance)             //<JPK> safety check, just in case I dont have one
            PlayerMessagesUI.Instance.HideText();
        

    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        /*if (invX)
            mouseX * -1;
         
         if (invY)
            xRotation += mouseY;
        else*/
            xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, xNegativeClamp, xPositiveClamp);

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        virtCamXRotation -= mouseY;
        virtCamXRotation = Mathf.Clamp(virtCamXRotation, virtCamXRotationXNegativeClamp, virtCamXRotationxPositiveClamp);

        Vector3 virtFollowOffsetCamVector = new Vector3(0f, virtCamXRotation, -20f);

        virtCamTransposer.m_FollowOffset = virtFollowOffsetCamVector;

        virtCamYRotation += mouseX;
        virtCamYRotation = Mathf.Clamp(virtCamYRotation, composerVirtcamNegativeClamp, composerVirtcamPositiveClamp);
        
        virtCamComposer.m_TrackedObjectOffset = new Vector3(virtCamYRotation, virtCamComposerOffset.y, virtCamComposerOffset.z);


        playerChar.Rotate(Vector3.up * mouseX * speedVirtComposer);
    }

}
