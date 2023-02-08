using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{

    private void Start()
    {
        Invoke("DisableTutorial", 10f);
    }

    private void Update()
    {
        if (CharacterMovement.Instance.IsMoving())
            DisableTutorial();
    }

    private void DisableTutorial()
    {
        gameObject.SetActive(false);
    }

}
