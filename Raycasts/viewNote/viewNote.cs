using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class viewNote : MonoBehaviour
{
    [Header("Note model(3d) and Image(panel) in Canvas")]
    [SerializeField] private GameObject noteModel;
    [SerializeField] private Image noteImage;

    [Tooltip("Sprite, which change noteImage")]
    [SerializeField] private Sprite newSpriteNote;

    [Header("Trigger")]
    [SerializeField] private GameObject trigger;

    [Tooltip("Panel with all crosshairs")]
    [SerializeField] private GameObject cursorPanel;

    [Header("Audio Source for Replick")]
    [SerializeField] private AudioSource srcReplick;
    [SerializeField] private AudioClip clip_takeNoteForFirstTime;
    [SerializeField] private AudioClip clip_takeNoteAgain;

    [Header("Audio Source for Effect")]
    [SerializeField] private AudioSource srcEffect;
    [SerializeField] private AudioClip clip_takeNoteEffect;

    [Header("Key for interact")]
    private KeyCode key = KeyCode.E;

    [Header("Text(gameObj) and task")]
    [SerializeField] private Text text;
    [SerializeField] private string newTask;

    [Header("Tags (check desription)")]
    [Tooltip("Player Tag")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Note Tag")]
    [SerializeField] private string noteTag = "InteractItem";

    [Header("Checkbox")]

    [Tooltip("Need play replick?")]
    [SerializeField] private bool _playAudio;

    [Tooltip("Need add task?")]
    [SerializeField] private bool _setNewTask;

    [Tooltip("Need delete something?...")]
    [SerializeField] private bool _deleteTrigger;

    [Header("...Delete what?")]
    [SerializeField] private WhatDelete needDelete;

    private enum WhatDelete
    {
        TriggerOnly = 0,
        ModelOnly = 1,
        Both = 2,
        Nothing = 3
    }

    private Camera cam => Camera.main;

    private RaycastHit hit;

    private bool _isInTrigger = false;
    private bool _isTaked = false;

    private void Start()
    {
        noteImage.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _isInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _isInTrigger = false;
        }
    }

    private void LateUpdate()
    {
        if (_isInTrigger)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag(noteTag) && Input.GetKeyDown(key))
            {
                ShowNoteDefault();
                PlayAudio();
                SetNewTask();
                DeleteSomething();
            }
        }
    }

    private void ShowNoteDefault()
    {
        _isTaked = true;
        noteImage.enabled = true;
        noteImage.sprite = newSpriteNote;

        cursorPanel.SetActive(false);

        srcEffect.Stop();
        srcEffect.PlayOneShot(clip_takeNoteEffect);

        Time.timeScale = 0;

        if (Input.GetKeyDown(key | KeyCode.Escape)) //if dont work remove that "|"
        {
            noteImage.enabled = false;
            cursorPanel.SetActive(true);
            Time.timeScale = 1;
        }

    }

    private void DeleteSomething()
    {
        if (_deleteTrigger)
        {
            switch (needDelete)
            {
                case WhatDelete.TriggerOnly:
                    Destroy(trigger);
                    break;

                case WhatDelete.ModelOnly:
                    Destroy(noteModel);
                    break;

                case WhatDelete.Both:
                    Destroy(noteModel);
                    Destroy(trigger);
                    break;

                case WhatDelete.Nothing:
                    break;

                default:
                    break;
            }
        }
    }

    private void PlayAudio()
    {
        if (_playAudio)
        {
            if (!_isTaked)
            {
                srcReplick.Stop();
                srcReplick.PlayOneShot(clip_takeNoteForFirstTime);
            }
            else
            {
                srcReplick.Stop();
                srcReplick.PlayOneShot(clip_takeNoteAgain);
            }
        }
    }

    private void SetNewTask()
    {
        if (_setNewTask)
        {
            text.enabled = true;
            text.text = newTask;
        }
    }
}

