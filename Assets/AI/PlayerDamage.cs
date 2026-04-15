using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Diagnostics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using URPGlitch;
using static GameController;

public class PlayerDamage : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxLifeTime;
    [SerializeField] Camera[] camerasToBeDisabled;
    [SerializeField] Volume cameraVolume;
    [SerializeField] GameObject deathScreen;
    [SerializeField] AudioMixer mixer;
    GameObject weapon;
    Vector3 baseWeaponPosition;
    Quaternion baseWeaponRotation;
    float lifeTime;
    [SerializeField] Cards cards;

    float damageFlashTimer = 0;

    float previousMusicVolume;

    public void Destroy() { }

    void FindWeapon()
    {
        weapon = GetComponent<EquipWeapon>().gunHolder.GetComponentInChildren<Gun>().gameObject;
        baseWeaponPosition = weapon.transform.localPosition;
        baseWeaponRotation = weapon.transform.localRotation;
    }

    void Start()
    {
        lifeTime = maxLifeTime;
        deathScreen.SetActive(false);
        FindWeapon();
    }

    void Update()
    {
        if(damageFlashTimer > 0)
            damageFlashTimer = Mathf.Max(damageFlashTimer - Time.unscaledDeltaTime, 0);

        if(!weapon)
            FindWeapon();

        if(gameController.GetGameState() == GameState.UpgradeSelection || gameController.GetGameState() == GameState.Normal)
            if(gameController.intermission)
            {
                Heal(Time.deltaTime * (maxLifeTime * .5f));
            }

        if (gameController.GetGameState() == GameState.Normal)
        {
            if(!gameController.intermission)
            {
                if (lifeTime <= 1.0f)
                {
                    Damage(Time.deltaTime * 0.5f);
                }
                else if (lifeTime <= 2.0f)
                {
                    Damage(Time.deltaTime * 0.75f);
                }
                else
                {
                    Damage(Time.deltaTime);
                }
            }
        }
        else if (gameController.GetGameState() == GameState.DeathScreen)
        {
            if (Input.anyKeyDown)
            {
                Respawn();
                CloseDeathScreen();
            }
        }

        UpdateFX();
    }

    virtual public void OnHit(RaycastHit hit, float damage) {}

    public void Heal(float lifetimeRestore)
    {
        lifeTime = Mathf.Min(lifeTime + lifetimeRestore, maxLifeTime);
    }

    public void UpdateFX()
    {
        foreach (var comp in cameraVolume.profile.components)
        {
            if (comp is ChromaticAberration)
                ((ChromaticAberration)comp).intensity.value = (lifeTime <= 5)
                    ? 1 - lifeTime / 5
                    : 0f;

            if (comp is FilmGrain)
                ((FilmGrain)comp).intensity.value = (lifeTime <= 5)
                    ? 1 - lifeTime / 5
                    : 0f;

            if(comp is AnalogGlitchVolume)
            {
                if(damageFlashTimer > 0)
                {
                    ((AnalogGlitchVolume)comp).colorDrift.value = .5f;
                }
                else
                {
                    float intensity = (lifeTime <= .5f)
                        ? 1 - lifeTime - .5f
                        : 0;

                    ((AnalogGlitchVolume)comp).scanLineJitter.value = .5f * intensity;
                    ((AnalogGlitchVolume)comp).verticalJump.value = .1f * intensity;
                    ((AnalogGlitchVolume)comp).horizontalShake.value = .2f * intensity;
                    ((AnalogGlitchVolume)comp).colorDrift.value = .25f * intensity;
                }
            }
        }
    }

    virtual public void Damage(float damage)
    {
        lifeTime -= damage;
        if (lifeTime <= 0)
            StartCoroutine(Destroy(Camera.main.transform.rotation, 1));
    }

    public void DamageWithEffect(float damage)
    {
        Damage(damage);
        damageFlashTimer += 0.05f * damage;
    }

    public float GetLifeTime()
    {
        return lifeTime;
    }

    public float GetMaxLifeTime()
    {
        return maxLifeTime;
    }

    virtual public IEnumerator Destroy(Quaternion baseCameraRotation, float time)
    {
        gameController.SetGameState(GameState.DeathScreen);
        StartCoroutine(MoveWeapon(.5f, true));

        float elapsed = 0;
        while (elapsed < time)
        {
            foreach (var comp in cameraVolume.profile.components)
            {
                if (comp is AnalogGlitchVolume)
                {
                    float intensity = (elapsed + .5f * time) / time;

                    ((AnalogGlitchVolume)comp).scanLineJitter.value = .5f * intensity;
                    ((AnalogGlitchVolume)comp).verticalJump.value = .1f * intensity;
                    ((AnalogGlitchVolume)comp).horizontalShake.value = .2f * intensity;
                    ((AnalogGlitchVolume)comp).colorDrift.value = .25f * intensity;
                }
            }

            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / time;
            Camera.main.transform.localPosition = Vector3.Lerp(new Vector3(0, .75f, 0), new Vector3(0, .75f, 0) + new Vector3(0, -1f, -.25f), t * t * (3f - 2f * t));
            Camera.main.transform.localRotation = Quaternion.Lerp(baseCameraRotation, baseCameraRotation * Quaternion.Euler(Vector3.up * -30) * Quaternion.Euler(Vector3.right * -60), t * t * (3f - 2f * t));

            if (gameController.GetGameState() == GameState.DeathScreen)
            {
                if (Input.anyKeyDown) // Restart
                {
                    Camera.main.transform.localPosition = new Vector3(0, .75f, 0);
                    yield break;
                }
            }

            yield return null;
        }

        OpenDeathScreen();
    }

    public void OpenDeathScreen()
    {
        foreach (var cam in camerasToBeDisabled)
            cam.enabled = false;

        deathScreen.SetActive(true);
        mixer.GetFloat("MusicVolume", out previousMusicVolume);
        mixer.SetFloat("MusicVolume", -80);
    }

    IEnumerator MoveWeapon(float time, bool away)
    {
        float elapsed = 0;
        if (away)
        {
            while (elapsed < time)
            {
                if (Input.anyKeyDown) // Restart
                {
                    yield break;
                }

                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / time;
                weapon.transform.localPosition = Vector3.Lerp(baseWeaponPosition, baseWeaponPosition + new Vector3(0, -.75f, -.25f), t * t * (3f - 2f * t));
                weapon.transform.localRotation = Quaternion.Lerp(baseWeaponRotation, baseWeaponRotation * Quaternion.Euler(Vector3.up * -15) * Quaternion.Euler(Vector3.right * -30), t * t * (3f - 2f * t));
                yield return null;
            }
        }
        else
        {
            while (elapsed < time)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / time;
                weapon.transform.localPosition = Vector3.Lerp(baseWeaponPosition + new Vector3(0, -.75f, -.25f), baseWeaponPosition, t * t * (3f - 2f * t));
                weapon.transform.localRotation = Quaternion.Lerp(baseWeaponRotation * Quaternion.Euler(Vector3.up * -15) * Quaternion.Euler(Vector3.right * -30), baseWeaponRotation, t * t * (3f - 2f * t));
                yield return null;
            }
        }
    }

    public void CloseDeathScreen()
    {
        foreach (var cam in camerasToBeDisabled)
            cam.enabled = true;

        deathScreen.SetActive(false);
        mixer.SetFloat("MusicVolume", previousMusicVolume);
    }

    public void Respawn()
    {
        lifeTime = maxLifeTime;
        weapon.transform.localPosition = baseWeaponPosition;
        weapon.transform.localRotation = baseWeaponRotation;
        gameController.StartGame();
        cards.RestoreData();
    }
}
