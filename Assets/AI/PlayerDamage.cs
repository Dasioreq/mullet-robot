using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static GameController;

public class PlayerDamage : MonoBehaviour, IDamagable
{
    [SerializeField] public float maxLifeTime;
    [SerializeField] Camera[] camerasToBeDisabled;
    [SerializeField] Volume cameraVolume;
    [SerializeField] GameObject deathScreen;
    float lifeTime;

    void Start()
    {
        lifeTime = maxLifeTime;
        deathScreen.SetActive(false);
    }

    void Update()
    {
        if(gameController.GetGameState() == GameState.Normal)
        {
            if(lifeTime <= 1.0f)
            {
                Damage(Time.deltaTime * 0.5f);
            }
            else if(lifeTime <= 2.0f)
            {
                Damage(Time.deltaTime * 0.75f);
            }
            else
            {
                Damage(Time.deltaTime);
            }
        }
        else if(gameController.GetGameState() == GameState.DeathScreen)
        {
            if(Input.anyKeyDown)
            {
                Respawn();
                CloseDeathScreen();
            }
        }
    }

    virtual public void OnHit(RaycastHit hit, float damage)
    {
        Damage(damage);
    }

    virtual public void Damage(float damage)
    {
        lifeTime -= damage;
        if(lifeTime <= 0)
            Destroy();
        
        foreach(var comp in cameraVolume.profile.components)
        {
            if(comp is ChromaticAberration)
                ((ChromaticAberration)comp).intensity.value = (lifeTime < maxLifeTime * .5f)
                    ? 1 - lifeTime / (maxLifeTime * .5f)
                    : 0f;
            
            if(comp is FilmGrain)
                ((FilmGrain)comp).intensity.value = (lifeTime < maxLifeTime * .5f)
                    ? 1 - lifeTime / (maxLifeTime * .5f)
                    : 0f;
        }
    }
    public float GetLifeTime()
    {
        return lifeTime;
    }

    public float GetMaxLifeTime()
    {
        return maxLifeTime;
    }

    virtual public void Destroy() 
    {
        OpenDeathScreen();
    }

    public void OpenDeathScreen()
    {
        foreach(var cam in camerasToBeDisabled)
            cam.enabled = false;
        gameController.SetGameState(GameState.DeathScreen);

        deathScreen.SetActive(true);
    }

    public void CloseDeathScreen()
    {
        foreach(var cam in camerasToBeDisabled)
            cam.enabled = true;
        deathScreen.SetActive(false);
    }

    public void Respawn()
    {
        lifeTime = maxLifeTime;
        gameController.StartGame();
        GetComponent<EquipWeapon>().Equip(0);
    }
}
