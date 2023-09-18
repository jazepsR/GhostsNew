using ARLocation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    [HideInInspector] public bool disappearing = false;
    [HideInInspector] public bool collected = false;
    [HideInInspector] public bool gotHit = false;
    [HideInInspector] public Animator anim;
    [SerializeField] private float disableTime = 0.5f;
    [SerializeField] private float randomDisplacement = 7f;
    public GameObject ghostLight;
    public Transform rootBone; 
    public float moveSpeed = 1f;
    public int health = 3;
    Vector3 target=Vector3.one;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        disappearing = false;
    }
    private void OnEnable()
    {
        //startPosition = Camera.main.transform.position;
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        // Vector2 randomDir = Random.insideUnitCircle * randomDisplacement;
        // rootBone.localPosition = new Vector3(randomDir.x, 0, randomDir.y);
        disappearing = false;
        //startPosition = transform.position;
        anim.ResetTrigger("Disappear");
        SetTarget();
    }
    private void Update()
    {
        if (target == null)
            SetTarget();
        float dist = Vector3.Distance(transform.position, target);
        //Debug.LogError("dist: " + dist);
        if (dist > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
        }
        else
        {
            SetTarget();
        }
    }
    public void GetHit()
    {
        if (health <= 0 && collected == false)
        {
            ObjectManager.instance.NextGhost();
            GameManager.instance.IncreaseScore();
            collected = true;
            MusicController.instance.PlayHitSound();
            Disappear();
        }
        if (health >=1 && gotHit== false)
        {
            health--;
            gotHit = true;
            anim.SetTrigger("Hit");
            Invoke("ResetHit", 1f);
            MusicController.instance.PlayHitSound();
        }
    }

    private void ResetHit()
    {
        gotHit = false;
    }
    private void SetTarget()
    {
        Vector2 randomDir = Random.insideUnitCircle * randomDisplacement;
        target = Camera.main.transform.position + new Vector3(randomDir.x, 0, randomDir.y);
    }
    // Update is called once per frame
    

    public void Disappear()
    {
        if(!disappearing)
        {
            disappearing = true;
            if(anim)
                anim.SetTrigger("Disappear");
            Invoke("DisableObj", disableTime);
        }
    }

    private void DisableObj()
    {
        disappearing = false;
        gameObject.SetActive(false);
    }
}
