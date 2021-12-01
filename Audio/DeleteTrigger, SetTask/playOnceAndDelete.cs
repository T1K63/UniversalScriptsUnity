using UnityEngine;
using UnityEngine.UI;

public class playOnceAndDelete : MonoBehaviour
{
    [Header("Trigger, which be deleted (if choose)")]
    [SerializeField] private GameObject   trigger;

    [Header("Audio source and replick")]
    [SerializeField] private AudioSource  srcAudio;
    [SerializeField] private AudioClip    acAudio;

    [Header("Need change the task?")]
    [SerializeField] private bool         haveTask = false;

    [Header("Need delete trigger?")]
    [SerializeField] private bool         deleteTrigger = false;

    [Header("New task")]
    [SerializeField] private Text         _text;
    [SerializeField] private string       newTask;

    [Header("Player tag")]
    [SerializeField] private string       tagOfPlayer;

    private int i;

    private void Start()
    {
        i = 1;

        if (haveTask)
        {
            i = i + 1;
        }

        if (deleteTrigger)
        {
            i = i + 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagOfPlayer))
        {
            switch (i)
            {
                case 1:
                    NoOneCheckbox();
                    break;

                case 2:
                    OnlyHaveTask();
                    break;

                case 3:
                    OnlyDeleteTrigger();
                    break;

                case 4:
                    BothCheckbox();
                    break;

                default:
                    break;
            }
        }
    }

    private void NoOneCheckbox() {
        srcAudio.Stop();
        srcAudio.PlayOneShot(acAudio);
    }

    private void OnlyHaveTask() {
        srcAudio.Stop();
        srcAudio.PlayOneShot(acAudio);

        _text.enabled = true;
        _text.text = newTask;
    }

    private void OnlyDeleteTrigger()
    {
        srcAudio.Stop();
        srcAudio.PlayOneShot(acAudio);

        Destroy(trigger);
    }

    private void BothCheckbox()
    {
        srcAudio.Stop();
        srcAudio.PlayOneShot(acAudio);

        _text.enabled = true;
        _text.text = newTask;

        Destroy(trigger);
    }
}
