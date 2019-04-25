using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VirtiualCameraController : MonoBehaviour {

    [SerializeField] Cinemachine.CinemachineVirtualCamera PlayerVirtualCamera;

    private Cinemachine.CinemachineBrain CinemachineBrain;

    // Use this for initialization
    void Start ()
    {
        CinemachineBrain = GetComponent<Cinemachine.CinemachineBrain>();
        PlayableDirector playableDirector = GetComponent<PlayableDirector>();

    }

    void SetOriginalPlayerVirtualCamera(PlayableDirector pd)
    {
       
    }

}
