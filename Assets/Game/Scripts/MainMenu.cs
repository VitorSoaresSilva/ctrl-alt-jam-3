using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

//By FJB and Romeu
public class MainMenu : MonoBehaviour
{
    [Header("Jam Logo")]
    [SerializeField] private RectTransform jamLogo;
    [SerializeField] private float logoRotationSpeed = 50f;

    [Header("Game Scene Config")]
    public string sceneName = "Game";

    [Header("Audio"), FormerlySerializedAs("AudioClip")]
    public AudioClip[] AudioClips;
    public AudioSource audioSource;
    public float fadeInDuration = 2.0f;
    [Range(0, 1)] public float Volume = 1f;
    private int currentAudioIndex = 0;

    [Header("Background Image")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backgroundImages;

    [SerializeField, Header("Controls Dialog")]
    private RectTransform controlsDialog;
    [SerializeField] private Button openDialogBtn;
    [SerializeField] private Button closeDialogBtn;

    [SerializeField, Header("Credits Dialog")]
    private RectTransform creditsDialog;
    [SerializeField] private Button openCreditsBtn;
    [SerializeField] private Button closeCreditsBtn;
    [SerializeField] private ScrollRect creditsScrollRect;
    [SerializeField] private float creditsScrollSpeed = 0.035f;
    private bool creditsScrollDirection = false;

    #region Unity Methods
    private void OnEnable()
    {
            
    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        SetRandomBackgroundImage();
        StartCoroutine(FadeInMusic());
        PlayNextAudioClip();

        if(creditsScrollRect)
            creditsScrollRect.verticalNormalizedPosition = (creditsScrollDirection) ? 0 : 1;
    }

    void Update()
    {
        if(jamLogo != null)
            jamLogo.transform.Rotate(0, logoRotationSpeed * Time.deltaTime, 0);

        if (audioSource != null && !audioSource.isPlaying)
        {
            PlayNextAudioClip();
        }

        // credits auto scroll
        if(creditsScrollRect != null && creditsScrollRect.gameObject.activeSelf)
        {
            creditsScrollRect.verticalNormalizedPosition = Mathf.MoveTowards(
                creditsScrollRect.verticalNormalizedPosition, 
                (creditsScrollDirection) ? 0 : 1,
                Time.deltaTime * creditsScrollSpeed
            );

            if (creditsScrollRect.verticalNormalizedPosition <= 0.01f)
                creditsScrollDirection = false;
            else if(creditsScrollRect.verticalNormalizedPosition >= 0.99f)
                creditsScrollDirection = true;
        }

        
    }
    #endregion

    private void SetRandomBackgroundImage()
    {
        if (backgroundImages.Length > 0 && backgroundImage != null)
        {
            int index = Random.Range(0, backgroundImages.Length);
            backgroundImage.sprite = backgroundImages[index];
        }
    }

    public void StartGame()
    {
        Debug.Log("OnStartButton was called.");
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    public void ChangeScene(string scene)
    {
        StartCoroutine(FadeOutAndLoadScene(scene));
    }

    public void QuitGame()
    {
        Debug.Log("OnQuitButton was called.");
        Application.Quit();
    }

    public void ShowControls()
    {
        if(controlsDialog != null)
            controlsDialog.gameObject.SetActive(true);
        if(closeDialogBtn != null)
            closeDialogBtn.Select();
    }

    public void HideControls()
    {
        if (controlsDialog != null)
            controlsDialog.gameObject.SetActive(false);
        if(openDialogBtn != null)
            openDialogBtn.Select();
    }

    public void ToggleDialog(Dialog dialog)
    {
        if(dialog.dialog.gameObject.activeSelf)
        {
            dialog.dialog.gameObject.SetActive(false);
            dialog.openBtn.Select();
        }
        else
        {
            dialog.dialog.gameObject.SetActive(true);
            dialog.closeBtn.Select();
        }
    }


    #region Private Methods
    private IEnumerator FadeOutAndLoadScene(string scene)
    {
        StartCoroutine(FadeOutMusic());
        yield return new WaitForSeconds(fadeInDuration);
        SceneManager.LoadSceneAsync(scene);
    }

    private void PlayNextAudioClip()
    {
        if (AudioClips.Length > 0)
        {
            int rndIndex = Random.Range(0, AudioClips.Length);
            if(rndIndex == currentAudioIndex)
            {
                rndIndex = (rndIndex + 1) % AudioClips.Length;
            }
            audioSource.clip = AudioClips[rndIndex];
            currentAudioIndex = rndIndex;
            audioSource.Play();
        }
    }

    private IEnumerator FadeInMusic()
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeInDuration)
        {
            float t = (Time.time - startTime) / fadeInDuration;
            audioSource.volume = t * Volume;

            yield return null;
        }
        audioSource.volume = Volume;
    }

    private IEnumerator FadeOutMusic()
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeInDuration)
        {
            float t = (Time.time - startTime) / fadeInDuration;
            audioSource.volume = Volume - t * Volume;

            yield return null;
        }
        audioSource.volume = 0;
    }
    #endregion

}

[System.Serializable]
public class Dialog
{
    public RectTransform dialog;
    public Button openBtn;
    public Button closeBtn;
}