using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Effect : MonoBehaviour
{
    public static Effect Instance;

    //building
    [SerializeField] private GameObject building;

    private Animator animator;
    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        building = GameObject.FindGameObjectWithTag(Tags.Building);
    }
    public void BlackEffect()
    {

        building.SetActive(false);
    }
    public void NormalEffect()
    {
        building.SetActive(true);
    }
}
