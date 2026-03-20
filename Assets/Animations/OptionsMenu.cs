using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class OptionsMenu : MonoBehaviour
{
    Animator animator;
    bool open = false;
    [SerializeField] float openTime;
    float timer;
    [SerializeField] Camera uiCamera;
    [SerializeField] Volume blurVolume;
    GameObject weapon;
    Vector3 baseWeaponPosition;
    Quaternion baseWeaponRotation;
    public GameObject Player;
    public GameObject Camera;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 0)
        {
            timer -= Time.unscaledDeltaTime;
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(!open)
                {
                    animator.SetTrigger("TrOpen");
                    Time.timeScale = 0;
                    uiCamera.gameObject.SetActive(false);
                    StartCoroutine(BlurLerp(blurVolume.weight, 1, .5f));
                    StartCoroutine(MoveWeapon(.5f, true));
                    Camera.GetComponent<CameraContoller>().enabled = false;
                    Player.GetComponent<MovementHandler>().rb.linearVelocity = Vector3.zero;
                    Player.GetComponent<MovementHandler>().enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    weapon.GetComponent<Animator>().enabled = false;
                    weapon.GetComponent<AudioSource>().enabled = false;
                    weapon.GetComponent<Gun>().enabled = false;
                }
                else
                {
                    animator.SetTrigger("TrClose");
                    Time.timeScale = 1;
                    uiCamera.gameObject.SetActive(true);
                    StartCoroutine(BlurLerp(blurVolume.weight, 0, .5f));
                    StartCoroutine(MoveWeapon(.5f, false));
                    Camera.GetComponent<CameraContoller>().enabled = true;
                    Player.GetComponent<MovementHandler>().enabled = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = false;
                    weapon.GetComponent<Animator>().enabled = true;
                    weapon.GetComponent<AudioSource>().enabled = true;
                    weapon.GetComponent<Gun>().enabled = true;
                }
                open = !open;
                timer = openTime;
            }
        }

        if(weapon == null)
        {
            Gun comp =  transform.parent.parent.GetComponentInChildren<Gun>();
            if(comp != null)
            {
                weapon = comp.gameObject;
                baseWeaponRotation = weapon.transform.localRotation;
                baseWeaponPosition = weapon.transform.localPosition;
            }
        }
    }

    IEnumerator BlurLerp(float start, float end, float time) {
        float elapsed = 0;
        while (elapsed < time) {
            elapsed += Time.unscaledDeltaTime;
            if(open)
                blurVolume.weight = Mathf.Lerp(start, end, Mathf.Sqrt(elapsed / time));
            else
                blurVolume.weight = Mathf.Lerp(start, end, (elapsed / time) * (elapsed / time));
            yield return null;
        }
    }

    IEnumerator MoveWeapon(float time, bool away)
    {
        float elapsed = 0;
        if(away)
        {
            while(elapsed < time)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / time;
                weapon.transform.localPosition = Vector3.Lerp(baseWeaponPosition, baseWeaponPosition + new Vector3(0, -.75f, -.25f), t * t * (3f - 2f * t));
                weapon.transform.localRotation = Quaternion.Lerp(baseWeaponRotation, baseWeaponRotation * Quaternion.Euler(Vector3.up * -15) * Quaternion.Euler(Vector3.right * -30), t * t * (3f - 2f * t));
                yield return null;
            }
        }
        else
        {
            while(elapsed < time)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / time;
                weapon.transform.localPosition = Vector3.Lerp(baseWeaponPosition + new Vector3(0, -.75f, -.25f), baseWeaponPosition, t * t * (3f - 2f * t));
                weapon.transform.localRotation = Quaternion.Lerp(baseWeaponRotation * Quaternion.Euler(Vector3.up * -15) * Quaternion.Euler(Vector3.right * -30), baseWeaponRotation, t * t * (3f - 2f * t));
                yield return null;
            }
        }
    }
}
