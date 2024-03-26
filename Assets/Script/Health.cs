using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void HitEvent(GameObject source);
    public HitEvent OnHit;

    public delegate void ResetEvent();
    public ResetEvent OnHitReset;

    public float MaxHealth = 10f;
    public Cooldown Invulnerability;

    public float CurrenHealth
    {
        get
        {
            return _currentHealth;
        }
    }

    private float _currentHealth = 10f;

    private bool _canDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        ResetHealthToMax();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetInvulnerable()
    {
        if (_canDamage)
            return;

        if (Invulnerability.IsOnCooldown && _canDamage == false)
            return;

        _canDamage = true;
        OnHitReset?.Invoke();
    }

    public void Damage(float damage, GameObject source)
    {
        if (!_canDamage)
            return;

        _currentHealth -= damage;

        if(_currentHealth <= 0f)
        {
            _currentHealth = 0f;
            Die();
        }

        Invulnerability.StartCooldown();
        _canDamage = false;

        OnHit?.Invoke(source);
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    void ResetHealthToMax()
    {
        _currentHealth = MaxHealth;
    }
}
