using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using URPGlitch;
using static GameController;

/// @class PlayerDamage
/// @brief Implements \ref IDamagable; Implements the Player's unique mechanic of losing health over time and their respawning
public class PlayerDamage : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxLifeTime;
    [SerializeField] Camera[] camerasToBeDisabled;
    [SerializeField] Volume cameraVolume;
    [SerializeField] GameObject deathScreen;
    [SerializeField] AudioMixer mixer;
    [SerializeField] private TMP_Text text1Number;
    [SerializeField] private TMP_Text text2Number;
    [SerializeField] private TextAnimation text1Effect;
    [SerializeField] private TextAnimation text2Effect;
    [SerializeField] private TextAnimation number1Effect;
    [SerializeField] private TextAnimation number2Effect;
    GameObject weapon;
    Vector3 baseWeaponPosition;
    Quaternion baseWeaponRotation;
    float lifeTime;
    [SerializeField] Cards cards;

    float damageFlashTimer = 0;

    float previousMusicVolume;
    float previousSfx;

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Respawn();
                CloseDeathScreen();
            }
        }

        UpdateFX();
    }

    virtual public void OnHit(RaycastHit hit, float damage) {}

    /// @brief Heals the player a given amount, accounting for their max health
    public void Heal(float lifetimeRestore)
    {
        lifeTime = Mathf.Min(lifeTime + lifetimeRestore, maxLifeTime);
    }

    /// @brief Updates post-processing effects for the player's camera based on their remaining health
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

    /// @brief Reduces the player's health; used exclusively for the DOT mechanic
    virtual public void Damage(float damage)
    {
        lifeTime -= damage;
        if (lifeTime <= 0)
            StartCoroutine(Destroy(Camera.main.transform.rotation, 1));
    }

    /// @brief Same as \ref Damage, but also adds a screenspace effect when damaged
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

    /// @brief Plays the player's death sequence and calls \ref PlayerDamage.OpenDeathScreen
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
                if (Input.GetKeyDown(KeyCode.Space)) // Restart
                {
                    Camera.main.transform.localPosition = new Vector3(0, .75f, 0);
                    yield break;
                }
            }

            yield return null;
        }

        OpenDeathScreen();
    }

    /// @brief Opens the death screen overlay
    public void OpenDeathScreen()
    {
        foreach (var cam in camerasToBeDisabled)
            cam.enabled = false;

        text1Number.text = Convert.ToString(gameController.level);
        text2Number.text = Convert.ToString(gameController.record);
        deathScreen.SetActive(true);
        text1Effect.StartTyping();
        text2Effect.StartTyping();
        number1Effect.StartTyping(gameController.level.ToString());
        number2Effect.StartTyping(gameController.record.ToString());
        mixer.GetFloat("MusicVolume", out previousMusicVolume);
        mixer.SetFloat("MusicVolume", -80);
        mixer.GetFloat("Sfx", out previousSfx);
        mixer.SetFloat("Sfx", -80);
    }

    IEnumerator MoveWeapon(float time, bool away)
    {
        float elapsed = 0;
        if (away)
        {
            while (elapsed < time)
            {
                if (Input.GetKeyDown(KeyCode.Space)) // Restart
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

    /// @brief Closes the death screen overlay
    public void CloseDeathScreen()
    {
        foreach (var cam in camerasToBeDisabled)
            cam.enabled = true;

        deathScreen.SetActive(false);
        mixer.SetFloat("MusicVolume", previousMusicVolume);
        mixer.SetFloat("Sfx", previousSfx);
    }

    /// @brief Resets the player's health-related stats and calls \ref GameController.StartGame and 'ref Cards.RestoreData to restore their position, upgrades, weapon etc.
    public void Respawn()
    {
        lifeTime = maxLifeTime;
        weapon.transform.localPosition = baseWeaponPosition;
        weapon.transform.localRotation = baseWeaponRotation;
        gameController.StartGame();
        cards.RestoreData();
    }
}
