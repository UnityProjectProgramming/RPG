using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    //Serlized
    [SerializeField] AudioClip clip;
    [SerializeField] int layerFilter = 0;
    [SerializeField] float playerDistanceThreshold = 5f;
    [SerializeField] bool isOneTimeOnly = true;

    //
    bool hasPlayed = false;
    GameObject player; // will only work for players , Make it general if Needed
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clip;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceToPlayer <= playerDistanceThreshold)
        {
            RequestPlayAudioClip();
        }
    }

    void RequestPlayAudioClip()
    {
        if (isOneTimeOnly && hasPlayed)
        {
            return;
        }
        else if (audioSource.isPlaying == false)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, playerDistanceThreshold);
    }
}