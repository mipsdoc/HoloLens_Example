using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class BallControl : MonoBehaviour {

    GestureRecognizer gestureRecognizer;
    CatControl control;

    void Start () {
        // 제스처 체크 설정.
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.TappedEvent += OnTappedEvent;
        gestureRecognizer.StartCapturingGestures();

        control = GameObject.FindObjectOfType<CatControl>();

    }

    private void OnTappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {

        Debug.Log("##Cat Position : " + control.transform.position.ToString());
        Debug.Log("##Camera Position : " + Camera.main.transform.position.ToString());
        RaycastHit hit;
        if (Physics.Raycast(headRay, out hit, 100))
        {
            transform.position = hit.point + new Vector3(0, 0.1f, 0);
        }
    }

}
