using System.Collections;
using UnityEngine;
using UnityEngine.Video;
public enum VideoType
{
    Intro,
    Win,
    Lose
}

public class VideoUI : BaseUI
{
    public VideoType currentVideoType;

    [Header("Videos")]
    [SerializeField] public VideoClip introVideo;
    [SerializeField] public VideoClip winVideo;
    [SerializeField] public VideoClip loseVideo;

    [Header("Video UI Settings")]
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
    }

    public void PlayVideo(VideoType videoClip)
    {
        skipButton.SetActive(false);
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        switch (videoClip)
        {
            case VideoType.Intro:
                videoPlayer.clip = introVideo;
                break;
            case VideoType.Win:
                videoPlayer.clip = winVideo;
                break;
            case VideoType.Lose:
                videoPlayer.clip = loseVideo;
                break;
            default:
                Debug.LogError("Tipo di video non supportato: " + videoClip);
                return;
        }
        currentVideoType = videoClip;

        double duration = videoPlayer.length;
        targetTime = Mathf.Max(0f, (float)(duration - 5.0));

        StartCoroutine(WaitForVideoStart());
        videoPlayer.Play();
    }

    private IEnumerator WaitForVideoStart()
    {
        yield return new WaitForSeconds(2f);
        skipButton.SetActive(true);
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video terminato!");
        // Azioni da fare alla fine del video

        switch (currentVideoType)
        {
            case VideoType.Intro:
                TurnManager.Instance.StartGame();
                UIManager.instance.ShowUI(UIManager.GameUI.HUD);
                break;
            case VideoType.Win:
                UIManager.instance.ShowUI(UIManager.GameUI.Win);
                break;
            case VideoType.Lose:
                UIManager.instance.ShowUI(UIManager.GameUI.Lose);
                break;
            default:
                break;
        }
    }

    public void SkipVideo()
    {
        skipButton.SetActive(false);
        videoPlayer.time = targetTime + 2.0f;
    }

    private void Update()
    {
        if (videoPlayer.time >= targetTime)
        {
            skipButton.SetActive(false);
        }
    }
}
