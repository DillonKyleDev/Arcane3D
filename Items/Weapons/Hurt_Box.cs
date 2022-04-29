using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt_Box : MonoBehaviour
{
    private bool canDetect = false;
    private List<string> hitList = new List<string>();
    private Weapon_Item weapon;
    public Animator animator;
    public float timeFactor;
    public float speed;

    void Start()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        weapon = gameObject.GetComponentInParent<Weapon_Item>();
    }

    void Update()
    {
        if(canDetect == false)
        {
            hitList = new List<string>();
        }
    }

    public void SetDetect(bool detect)
    {
        canDetect = detect;
        gameObject.GetComponent<CapsuleCollider>().enabled = detect;
    }

    private IEnumerator SlowAnimation()
    {
        float startTime = Time.time; // need to remember this to know how long to dash
        while(Time.time < startTime + timeFactor)
        {
            animator.speed = speed;
            yield return null; // this will make Unity stop here and continue next frame
        }
        animator.speed = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(canDetect)
        {
            Debug.Log(hitList);
            if(other.GetComponent<Tags>() != null && other.GetComponent<Tags>().HasTag("Enemy") && !hitList.Contains(other.name))
            {
                Debug.Log("Has enemy tag");
                StartCoroutine(SlowAnimation());
                float damage = weapon.GetDamage();
                hitList.Add(other.name);
                other.GetComponent<Take_Damage>().TakeDamage(damage);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
