using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthManager2 : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent healthManagerDeath;
    //public HealthbarScript healthbarScriptReference;
    public UIManager uiManager;
    public GameObject coinPrefab, xpCrystalPrefab;
    Animator animator;
    [SerializeField]
    private int _maxHealth = 100;
    public bool coinSpawned = false;

    Slider floatingHpBarSlider;
    private float lerpSpeed = 0.05f;

    public int MaxHealth
    {
        get { return _maxHealth; }

        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            if (tag.Equals("Enemy"))
            {
                UpdateFloatingHealthbar();

            }
            if (_health <= 0)
            {
                IsAlive = false;
                if (!coinSpawned && !tag.Equals("Player"))
                {
                    floatingHpBarSlider.gameObject.active = false;
                    Rigidbody2D coinRB = Instantiate(coinPrefab, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity).GetComponent<Rigidbody2D>();
                    Rigidbody2D xpRB = Instantiate(xpCrystalPrefab, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity).GetComponent<Rigidbody2D>();

                    coinRB.velocity = new Vector2(-transform.localScale.x, coinRB.velocity.y + 3);
                    xpRB.velocity = new Vector2(transform.localScale.x, coinRB.velocity.y + 3);

                    coinSpawned = true;
                }
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    public bool isInvincible;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.5f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        private set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            print($"IsAlive Set: {value}");

            if (value == false)
            {
                healthManagerDeath.Invoke();

                if (name.Equals("Frost Guardian"))
                {
                    uiManager.UpdateQuestObjectiveText(" -Get the key and find a way through the dungeon.");
                }
            }

        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        uiManager = GameObject.Find("UI Manager").GetComponent<UIManager>();
        if (tag.Equals("Enemy"))
        {
            floatingHpBarSlider = GetComponentInChildren<Slider>();
            floatingHpBarSlider.maxValue = MaxHealth;
            floatingHpBarSlider.value = Health;
        }
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }

        //Hit(10);

    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            if (gameObject.tag.Equals("Player"))
            {
                uiManager.UpdatePlayerHP();
            }

            return true;
        }
        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed(gameObject, actualHeal);

            if (gameObject.tag.Equals("Player"))
            {
                uiManager.UpdatePlayerHP();
            }
            return true;
        }

        return false;
    }

    public void UpdateFloatingHealthbar()
    {
        //floatingHpBarSlider.value = Health;
        floatingHpBarSlider.value = Health;

        
    }
}
