using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotoController : MonoBehaviour
{
    public SendBackButton sendBackButton;
    public RayController rayControllerLeft, rayControllerRight;
    public RawImage fadeEffectImage;
    public Vector3 photoScale;
    public bool moveWithBoat;
    public Transform lerpBackPosition;
    public bool isLooped;
    public bool isHovered;
    public bool isControllable;
    public InputActionReference buttonA;
    public InputActionReference buttonB;
    public InputActionReference buttonX;
    public InputActionReference buttonY;
    public RawImage image;
    public AudioSource rootAudioSource;
    public AudioSource audioSource;
    public Text playStatus;
    public Text description;
    public GameObject frame;
    public bool activated;
    public Color startColor, endColor;
    public VideoPlayer videoPlayer;
    public VideoPlayer effectVideoPlayer;
    public float textdelay = 0.1f;
    public string fullText;
    private string currentText = "";
    // Start is called before the first frame update
    public void Start()
    {
        photoScale = gameObject.transform.localScale;
        StartCoroutine(LerpBack());
        moveWithBoat = true;
        buttonA.action.performed += VideoStatusRight;
        buttonB.action.performed += StopVideoRight;
        buttonX.action.performed += VideoStatusLeft;
        buttonY.action.performed += StopVideoLeft;
        rootAudioSource = gameObject.GetComponent<AudioSource>();
        if (videoPlayer != null)
        {
            if (isLooped)
            {
                videoPlayer.isLooping = true;
            }
            audioSource = frame.GetComponent<AudioSource>();
            audioSource.volume = 0f;
            videoPlayer.loopPointReached += FinishedVideo;
            videoPlayer.SetTargetAudioSource(0, videoPlayer.gameObject.GetComponent<AudioSource>());
            //videoPlayer.Prepare();
            //StartCoroutine(ShowVideo());
        }
    }

    public void Update()
    {
        if (sendBackButton.sendBack && gameObject.CompareTag("Picture"))
        {
            if (gameObject.transform.localScale == photoScale)
            {
                gameObject.transform.localScale *= 7;
            }

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, lerpBackPosition.transform.position, 8 * Time.deltaTime);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, lerpBackPosition.rotation, 8 * Time.deltaTime);
            StartCoroutine(DisableLerp());
        }

        if (sendBackButton.sendBack && gameObject.CompareTag("Video"))
        {
            if (gameObject.transform.localScale == photoScale)
            {
                gameObject.transform.localScale *= 7;
            }

            if (videoPlayer.isPlaying && activated)
            {
                videoPlayer.Pause();
                StartCoroutine(ShowStatus());
            }

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, lerpBackPosition.transform.position, 8 * Time.deltaTime);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, lerpBackPosition.rotation, 8 * Time.deltaTime);
            StartCoroutine(DisableLerp());
        }

        if (sendBackButton.sendBack && gameObject.CompareTag("Epic"))
        {
            if (gameObject.transform.localScale == photoScale)
            {
                gameObject.transform.localScale *= 7;
            }

            if (videoPlayer != null && videoPlayer.isPlaying && activated)
            {
                videoPlayer.Pause();
                StartCoroutine(ShowStatus());
            }

            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, lerpBackPosition.transform.position, 8 * Time.deltaTime);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, lerpBackPosition.rotation, 8 * Time.deltaTime);
            StartCoroutine(DisableLerp());
        }


        if (moveWithBoat)
        {
            gameObject.transform.position = lerpBackPosition.transform.position;
            gameObject.transform.rotation = lerpBackPosition.transform.rotation;
        }
    }

    private void VideoStatusRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        if (videoPlayer != null && isControllable && rayControllerRight.isRight)
        {
            if (isHovered)
            {
                if (videoPlayer.isPlaying && activated)
                {
                    videoPlayer.Pause();
                    StartCoroutine(ShowStatus());
                }
                else
                {
                    if (activated)
                    {
                        videoPlayer.Play();
                        StartCoroutine(ShowStatus());
                    }
                }
            }
        }
    }

    private void StopVideoRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (videoPlayer != null && isControllable && activated && rayControllerRight.isRight)
        {
            if (isHovered)
            {
                videoPlayer.Stop();
                playStatus.text = "■";
                playStatus.enabled = true;
                StartCoroutine(DisplayStopped());
            }
        }
    }

    private void VideoStatusLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (videoPlayer != null && isControllable && rayControllerLeft.isLeft)
        {
            if (isHovered)
            {
                if (videoPlayer.isPlaying && activated)
                {
                    videoPlayer.Pause();
                    StartCoroutine(ShowStatus());
                }
                else
                {
                    if (activated)
                    {
                        videoPlayer.Play();
                        StartCoroutine(ShowStatus());
                    }
                }
            }
        }
    }

    private void StopVideoLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (videoPlayer != null && isControllable && activated && rayControllerLeft.isLeft)
        {
            if (isHovered)
            {
                videoPlayer.Stop();
                playStatus.text = "■";
                playStatus.enabled = true;
                StartCoroutine(DisplayStopped());
            }
        }
    }

    private IEnumerator ShowStatus()
    {
        if (videoPlayer.isPlaying)
        {
            playStatus.text = "▶";
            playStatus.enabled = true;
            yield return new WaitForSeconds(1f);
            if (videoPlayer.isPaused || playStatus.text == "■")
            {
                playStatus.enabled = true;
            }
            else
            {
                playStatus.enabled = false;
            }
        }
        else
        {
            playStatus.enabled = true;
            playStatus.text = "II";
        }
    }

    public void start()
    {
        if(!activated && !isControllable)
        {
            activated = true;
            StartCoroutine(FrameFadeOut());
        }
    }

    public void OnHover()
    {
        isHovered = true;
    }

    public void OnHoverExit()
    {
        isHovered = false;
    }

    public void OnGrab()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        if (gameObject.transform.localScale != photoScale)
        {
            gameObject.transform.localScale /= 7;
        }
        if (videoPlayer != null && !videoPlayer.isPrepared)
        {
            videoPlayer.Prepare();
            StartCoroutine(ShowVideo());
        }
        moveWithBoat = false;

    }

    public void OnDrop()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void FinishedVideo(VideoPlayer vp)
    {
        if (vp == videoPlayer && !isLooped)
        {
            videoPlayer.Stop();
            playStatus.text = "■";
            playStatus.enabled = true;
            StartCoroutine(DisplayStopped());
        }
    }

    private IEnumerator FrameFadeOut()
    {
        rootAudioSource.Play();
        effectVideoPlayer.Play();

        if (image != null)
        {
            image.enabled = true;
        }
        float tick = 0f;
        while (frame.GetComponent<Renderer>().material.color != Color.white)
        {
            tick += Time.deltaTime * 0.4f;
            frame.GetComponent<Renderer>().material.color = Color.Lerp(Color.black, Color.white, tick);
            if (image != null)
            {
                image.color = Color.Lerp(Color.black, Color.white, tick);
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float tick = 0f;
        while (fadeEffectImage.color != endColor)
        {
            tick += Time.deltaTime * 0.2f;
            fadeEffectImage.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowVideo()
    {
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        videoPlayer.Play();
        yield return new WaitForSeconds(0.1f);
        videoPlayer.Pause();
    }

    private IEnumerator DisableLerp()
    {
        gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        moveWithBoat = true;
        sendBackButton.sendBack = false;
        gameObject.GetComponent<XRGrabInteractable>().enabled = true;
    }

    private IEnumerator LerpBack()
    {
        if (gameObject.transform.localScale == photoScale)
        {
            gameObject.transform.localScale *= 7;
        }

        while (Vector3.Distance(lerpBackPosition.transform.position, gameObject.transform.position) > 0.1f)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, lerpBackPosition.transform.position, 8 * Time.deltaTime);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, lerpBackPosition.rotation, 8 * Time.deltaTime);
            yield return null;
        }
        moveWithBoat = true;
    }

    private IEnumerator DisplayStopped()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds(0.0001f);
        videoPlayer.Pause();
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            description.text = currentText;
            yield return new WaitForSeconds(textdelay);
        }
        if (videoPlayer != null)
        {
            yield return new WaitForSeconds(0.5f);
            audioSource.volume = 1f;
            videoPlayer.Play();
            isControllable = true;
        }
    }
}
