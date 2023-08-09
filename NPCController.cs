using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    Transform targetPosition;
    Animator animator;
    [SerializeField] Vector2 distPlayer;


    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        animator = gameObject.GetComponent<Animator>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distPlayer = (targetPosition.position - this.transform.position).normalized;

        animator.SetFloat("DistXPlayer", distPlayer.x);
        //animator.SetFloat("Vertical", distPlayer.y);
    }
}
