using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class viewNoteWithCursor : MonoBehaviour
{
    [Header("Модель записки в игре и ее спрайт")]
    [SerializeField] private GameObject noteModel;
    [SerializeField] private Image noteImage;

    [Header("Триггер на котором висит скрипт")]
    [SerializeField] private GameObject trigger;

    [Header("Активный курсор и панель с курсорами")]
    [Tooltip("Появится при наведении")]
    [SerializeField] private Image cursorActive;

    [Tooltip("При активации записки, все курсоры исчезнут")]
    [SerializeField] private GameObject cursorPanel;

    [Header("Камера игрока")]
    [SerializeField] private Camera cam;

    [Header("Источник звука и звуки")]
    [SerializeField] private AudioSource srcReplick;
    [SerializeField] private AudioClip clip_takeNoteForFirstTime;
    [SerializeField] private AudioClip clip_takeNoteAgain;

    [Header("Источник звука и звуки для эффектов")]
    [SerializeField] private AudioSource srcEffect;
    [SerializeField] private AudioClip clip_takeNoteEffect;

    [Header("Клавиша для активации записки")]
    private KeyCode key = KeyCode.E;

    [Header("Текст для задания и само задание")]
    [SerializeField] private Text text;
    [SerializeField] private string newTask;

    [Header("Теги (подробнее в наведении)")]
    [Tooltip("Тэг игрока")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Тэг записки")]
    [SerializeField] private string noteTag = "InteractItem";

    [Header("Нужное отметить галочкой")]

    [Tooltip("Нужно ли проигрывать диалог?")]
    [SerializeField] private bool _playAudio;

    [Tooltip("Нужно ли добавлять задание?")]
    [SerializeField] private bool _setNewTask;

    [Tooltip("Нужно ли удалить триггер и записку?")]
    [SerializeField] private bool _deleteTrigger;

    [Header("Что именно нужно удалить?")]
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

