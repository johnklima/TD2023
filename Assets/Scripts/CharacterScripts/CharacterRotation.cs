using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    /*public float mouseSens;
    public bool invX = false, invY = false;*/
    
    [SerializeField] private float mouseSens, xNegativeClamp, xPositiveClamp;
    [SerializeField] private Transform playerChar;
    

    [SerializeField] private float raycastLength;
    [SerializeField] private Transform raycastStartPoint;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactableKeyCode;


    private float xRotation = 0f;
    private Camera camera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main;
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

            PlayerMessagesUI.Instance.SetPlayerText("E to interact with " + hit.collider.gameObject.name);
            
            if (Input.GetKeyDown(interactableKeyCode))
            {
                hit.collider.gameObject.GetComponent<Interactable>().Interact();
            }
        }
        else
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

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerChar.Rotate(Vector3.up * mouseX);
    }

}
