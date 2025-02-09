using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScript : MonoBehaviour
{
    [SerializeField] CarController carController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ResetDriftCollision")
        {
            carController.TouchSmth = true;
        }
    }

}
