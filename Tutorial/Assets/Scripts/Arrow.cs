using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 3f;
    [SerializeField] float arrowLifetime = 4f;
    // Start is called before the first frame update
    void Awake()
    {
        Skeleton skeleton = FindObjectOfType<Skeleton>();
        //Make sure the direction of velocity is the same as the direction the skeleton is facing
        GetComponent<Rigidbody2D>().velocity = new Vector2(arrowSpeed * Mathf.Sign(skeleton.transform.localScale.x), 0f);
        //Make sure the orientation of the arrow matches that of the Skeleton
        transform.localScale = new Vector2(transform.localScale.x * Mathf.Sign(skeleton.transform.localScale.x), transform.localScale.y);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        arrowLifetime -= Time.deltaTime;
        if(arrowLifetime < Mathf.Epsilon)
        {
            Destroy(gameObject);
        }
    }
}
