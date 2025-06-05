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
        skipButton.SetActive(false);
        videoPlayer.loopPointReached += OnVideoEnd;
        double duration = videoPlayer.length;
        targetTime = Mathf.Max(0f, (float)(duration - 5.0));

        StartCoroutine(WaitForVideoStart());
    }

    private System.Collections.IEnumerator WaitForVideoStart()
    {
        yield return new WaitForSeconds(2f);
        skipButton.SetActive(true);
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
