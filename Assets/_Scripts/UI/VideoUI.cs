using UnityEngine;
using UnityEngine.Video;

public class VideoUI : BaseUI
{
    [SerializeField] private GameObject skipButton;
    VideoPlayer videoPlayer;
    double targetTime;

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
    }

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        double duration = videoPlayer.length;
        targetTime = Mathf.Max(0f, (float)(duration - 3.0));
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video terminato!");
        // Azioni da fare alla fine del video
        TurnManager.Instance.StartGame();
        UIManager.instance.ShowUI(UIManager.GameUI.HUD);
    }

    private void OnEnable()
    {
        videoPlayer.Play();
    }

    public void SkipVideo()
    {
        skipButton.SetActive(false);
        videoPlayer.time = targetTime;
    }

    private void Update()
    {
        if (videoPlayer.time >= targetTime)
        {
            skipButton.SetActive(false);
        }
    }
}
