using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(BoxCollider))]
public class PlayVideoOnEnter : MonoBehaviour
{
    [Tooltip("The object to hide when the video ends. If left empty, the VideoPlayer's GameObject is hidden.")]
    [SerializeField] private GameObject targetToHide;

    private VideoPlayer videoPlayer;
    private bool hasPlayed;

    void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        videoPlayer.isLooping = false;
        videoPlayer.playOnAwake = false;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

    void OnTriggerEnter(Collider other)
    {
        StartVideo();
    }

    [ContextMenu("Start Video")]
    void StartVideo()
    {
        if (hasPlayed) return;
        hasPlayed = true;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        (targetToHide != null ? targetToHide : vp.gameObject).SetActive(false);
    }
}