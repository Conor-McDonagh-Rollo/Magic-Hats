using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    static public Player instance;

    public Rigidbody2D body;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float runSpeed = 20.0f;

    Transform cam;

    public static Vector2 lookDirection;

    // SPELLS
    public GameObject basicCast;
    float basicCastTimer = 0.0f;
    const float basicCastCooldown = 1.0f;

    public GameObject fireball;
    public GameObject fireballDecal;
    public LayerMask decalLayer;
    float fireballTimer = 0.0f;
    const float fireballCooldown = 5.0f;
    public bool protectOn = false;

    GameObject protect;
    float protectTimer = 0.0f;
    const float protectCooldown = 2.0f;
    AudioSource parryAudio;

    public GameObject iceCast;
    float iceCastTimer = 0.0f;
    const float iceCastCooldown = 3.0f;

    ParticleSystem teleport;
    float teleportTimer = 0.0f;
    const float teleportCooldown = 5.0f;
    public LayerMask mask;

    // HEALTH
    int health = 100;
    public Slider healthSlider;
    public bool frozen = false;
    float frozenCooldown = 2.0f;

    // PAUSE

    public GameObject pauseMenu;
    public GameObject deadMenu;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        cam = Camera.main.transform;
        healthSlider.maxValue = 100;
        healthSlider.value = 100;
        protect = transform.GetChild(0).gameObject;
        teleport = transform.GetChild(1).GetComponent<ParticleSystem>();
        parryAudio = protect.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (frozen)
        {
            frozenCooldown -= Time.deltaTime;
            if (frozenCooldown <= 0.0f)
            {
                frozenCooldown = 2.0f;
                frozen = false;
            }
            return;
        }

        // ROTATE TOWARDS MOUSE
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos -= objectPos;

        lookDirection = mousePos.normalized;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        

        // MOVE WASD
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // SHOOT BASIC CAST
        basicCastTimer -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && basicCastTimer <= 0.0f)
        {
            ShootInDirection s = Instantiate(basicCast, transform.position, transform.rotation).GetComponent<ShootInDirection>();
            s.direction = lookDirection;
            s.friendly = true;
            basicCastTimer = basicCastCooldown;
        }

        // POWERFUL SPELLS

        fireballTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1) && fireballTimer <= 0.0f)
        {
            // FIRE SPELL
            Vector2 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(fireball, v, Quaternion.identity);
            Collider2D col = Physics2D.OverlapCircle(v, 0.82f, decalLayer);
            if (col != null)
            {
                if (!col.CompareTag("decal"))
                {
                    NextScene.instance.decalsArr.Add(Instantiate(fireballDecal, v, Quaternion.identity));
                }
            }
            else
            {
                NextScene.instance.decalsArr.Add(Instantiate(fireballDecal, v, Quaternion.identity));
            }
            
            fireballTimer = fireballCooldown;
        }

        protectTimer -= Time.deltaTime;
        if (protectOn && protectTimer < 1.0f)
        {
            protectOn = false;
            protect.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && protectTimer <= 0.0f)
        {
            // BLOCK SPELL
            protect.SetActive(true);
            protectOn = true;
            protectTimer = protectCooldown;
        }

        iceCastTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha3) && iceCastTimer <= 0.0f)
        {
            // FREEZE SPELL
            ShootInDirection s = Instantiate(iceCast, transform.position, transform.rotation).GetComponent<ShootInDirection>();
            s.direction = lookDirection;
            s.friendly = true;
            iceCastTimer = iceCastCooldown;
        }

        teleportTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha4) && teleportTimer <= 0.0f)
        {
            // TELEPORT SPELL
            teleportTimer = teleportCooldown;

            RaycastHit2D hit;
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -transform.up, 100.0f, mask);

            if (hit.point != null) // VALID
            {

                if (hit.transform.name == "Floor")
                {
                    transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    teleport.Play();
                }
                else
                {
                    teleportTimer = 0.0f;
                }
            }
            else // INVALID LOCATION
            {
                teleportTimer = 0.0f;
            }

        }

        // PAUSE

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }

    }

    void FixedUpdate()
    {
        // MOVE WASD
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);

        // CAMERA FOLLOW
        cam.position = new Vector3(transform.position.x, transform.position.y, cam.position.z);
    }

    public void Hurt(int _amount)
    {
        health -= _amount;
        healthSlider.value = health;

        if (health < 0)
        {
            Time.timeScale = 0.0f;
            deadMenu.SetActive(true);
            Destroy(gameObject);
        }
    }

    public void Heal(int _amount)
    {
        health += _amount;
        healthSlider.value = health;
    }

    public void ScreenShake()
    {
        StartCoroutine(Shake(0.15f, 0.8f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Camera cam = Camera.main;
        Vector3 orignalPosition = cam.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            cam.transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        cam.transform.position = orignalPosition;
    }

    public void ParrySound()
    {
        parryAudio.PlayOneShot(parryAudio.clip, parryAudio.volume);
    }
}
