using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class viewNoteWithCursor : MonoBehaviour
{
    [Header("������ ������� � ���� � �� ������")]
    [SerializeField] private GameObject noteModel;
    [SerializeField] private Image noteImage;

    [Header("������� �� ������� ����� ������")]
    [SerializeField] private GameObject trigger;

    [Header("�������� ������ � ������ � ���������")]
    [Tooltip("�������� ��� ���������")]
    [SerializeField] private Image cursorActive;

    [Tooltip("��� ��������� �������, ��� ������� ��������")]
    [SerializeField] private GameObject cursorPanel;

    [Header("������ ������")]
    [SerializeField] private Camera cam;

    [Header("�������� ����� � �����")]
    [SerializeField] private AudioSource srcReplick;
    [SerializeField] private AudioClip clip_takeNoteForFirstTime;
    [SerializeField] private AudioClip clip_takeNoteAgain;

    [Header("�������� ����� � ����� ��� ��������")]
    [SerializeField] private AudioSource srcEffect;
    [SerializeField] private AudioClip clip_takeNoteEffect;

    [Header("������� ��� ��������� �������")]
    private KeyCode key = KeyCode.E;

    [Header("����� ��� ������� � ���� �������")]
    [SerializeField] private Text text;
    [SerializeField] private string newTask;

    [Header("���� (��������� � ���������)")]
    [Tooltip("��� ������")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("��� �������")]
    [SerializeField] private string noteTag = "InteractItem";

    [Header("������ �������� ��������")]

    [Tooltip("����� �� ����������� ������?")]
    [SerializeField] private bool _playAudio;

    [Tooltip("����� �� ��������� �������?")]
    [SerializeField] private bool _setNewTask;

    [Tooltip("����� �� ������� ������� � �������?")]
    [SerializeField] private bool _deleteTrigger;

    [Header("��� ������ ����� �������?")]
    [SerializeField] private WhatDelete needDelete;

    private enum WhatDelete
    {
        TriggerOnly = 0,
        ModelOnly = 1,
        Both = 2,
        Nothing = 3
    }

    private bool _isInTrigger = false;
    private bool _isTaked = false;

    private RaycastHit hit;

    private void Start()
    {
        noteImage.enabled = false;
        cursorActive.enabled = false;
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
            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag(noteTag))
            {
                cursorActive.enabled = true;
            }
            else
            {
                cursorActive.enabled = false;
            }

            //call all functions
            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag(noteTag) && Input.GetKeyDown(key))
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

        cursorPanel.SetActive(false);

        srcEffect.Stop();
        srcEffect.PlayOneShot(clip_takeNoteEffect);

        Time.timeScale = 0;

        if(Input.GetKeyDown(key | KeyCode.Escape)) //if dont work remove that "|"
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

