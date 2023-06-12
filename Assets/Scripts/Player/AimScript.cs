using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{   
    public GameObject Gun;
    private Animator gunAnimator;
    private bool hasReloaded = true;

    void Start()
    {
        gunAnimator = Gun.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gunAnimator.Play("aim");
        }
        if (Input.GetKeyDown(KeyCode.R) && hasReloaded)
        {
            gunAnimator.Play("Reload");
            hasReloaded = false;
            StartCoroutine(ResetReload());
        }
        if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.R))
        {
            gunAnimator.Play("New State");
        }
    }

    IEnumerator ResetReload()
    {
        yield return new WaitForSeconds(2f);
        hasReloaded = true;
    }
}
