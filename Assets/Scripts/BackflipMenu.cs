using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackflipMenu : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float time;
    public Button myButton;
    private bool backlipping = false;
    private void Start()
    {
        myButton.onClick.AddListener(ButtonAction);
    }
    public void ButtonAction()
    {
        if (backlipping == false)
        {
            StartCoroutine(Backflip());
        }     
    }
    public IEnumerator Backflip()
    {
        animator.SetTrigger("TrBACKFLIP");
        backlipping = true;
        yield return new WaitForSeconds(time);
        backlipping=false;
    }

}
