using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/**
* @class BackflipMenu
* @brief Script for playing the backfip animation it the main menu
*/
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

    /// @brief Helper function called on Button press
    public void ButtonAction()
    {
        if (backlipping == false)
        {
            StartCoroutine(Backflip());
        }     
    }

    /// @brief plays the animation
    public IEnumerator Backflip()
    {
        animator.SetTrigger("TrBACKFLIP");
        backlipping = true;
        yield return new WaitForSeconds(time);
        backlipping=false;
    }
}
