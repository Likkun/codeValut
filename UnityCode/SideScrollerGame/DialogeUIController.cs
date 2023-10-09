using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogeUIController : MonoBehaviour
{

    public delegate void DialogueOpenComplete();
    public static event DialogueOpenComplete OnDialogueOpenComplete;

    public void SendDialogueOpenEvent()
    {
        OnDialogueOpenComplete();
    }

    private void Awake()
    {
        this.gameObject.GetComponent<Image>().color = new Color(
                        this.gameObject.GetComponent<Image>().color.r,
                        this.gameObject.GetComponent<Image>().color.g,
                        this.gameObject.GetComponent<Image>().color.b,
                        0);
    }

}
