using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RFX4_OnEnableResetTransform : MonoBehaviour {

    public GameObject Target;

    //   Transform t;
    //   public Vector3 startPosition;
    //   Quaternion startRotation;
    //   Vector3 startScale;
    //   bool isInitialized;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player");
    }

    //void OnEnable () {
    //    if(!isInitialized)
    //       {
    //           isInitialized = true;
    //           t = transform;
    //           startPosition = t.position;
    //           startRotation = t.rotation;
    //           startScale = t.localScale;
    //       }
    //       else
    //       {
    //           t.position = startPosition;
    //           t.rotation = startRotation;
    //           t.localScale = startScale;
    //       }
    //}

    //   void OnDisable()
    //   {
    //       if (!isInitialized)
    //       {
    //           isInitialized = true;
    //           t = transform;
    //           startPosition = t.position;
    //           startRotation = t.rotation;
    //           startScale = t.localScale;
    //       }
    //       else
    //       {
    //           t.position = startPosition;
    //           t.rotation = startRotation;
    //           t.localScale = startScale;
    //       }
    //   }

    //private void OnDisable()
    //{
      
    //}
}
