using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleAnimation : MonoBehaviour
{
    public Animator animator;
    public bool toggleplay = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Playtoggle()
    {
        if (toggleplay == true)
        { animator.SetTrigger("opentr"); }
        else
        {
            animator.SetTrigger("closetr");

        }
        if (toggleplay == true) { toggleplay = false; }
        else { toggleplay = true; }
    }

}