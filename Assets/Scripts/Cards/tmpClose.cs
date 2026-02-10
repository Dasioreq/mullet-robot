using UnityEngine;

public class tmpClose : MonoBehaviour
{
    public GameObject CardPanel;
    public void CloseWin()
    {
        CardPanel.SetActive(false);
    }

}
