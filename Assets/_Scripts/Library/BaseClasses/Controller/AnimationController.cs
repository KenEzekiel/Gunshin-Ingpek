using System;
using UnityEngine;

[Serializable]
public abstract class AnimationController 
{
    // Attributes
    [HideInInspector] public Model model;
    [HideInInspector] public Renderer renderer;
    [HideInInspector] public Animator animator;

    // Constructor
    public void Init(MonoBehaviour monobehaviour)
    {
        model = monobehaviour.GetComponentInChildren<Model>();
        
        #if STRICT
        if(model == null) 
        {
            Debug.LogError($"Animated object of {monobehaviour.name} does not have a model. How to resolve: create a gameObject with a model.cs script as its child");
        }
        else
        {
            animator = model.GetComponent<Animator>();
            renderer = model.GetComponent<Renderer>();
            if(animator == null)
            {
                Debug.LogError($"Animated object of {monobehaviour.name} does not have an animator in its model. How to resolve: add an animator to its child containing the model.cs script");
            }
            if(renderer == null)
            {
                Debug.LogError($"Animated object of {monobehaviour.name} does not have an renderer in its model. How to resolve: add an renderer to its child containing the model.cs script");
            }
        }
        #else
        animator = model.GetComponent<Animator>();
        renderer = model.GetComponent<Renderer>();
        #endif
    }

    public void DetectVisibility()
    {
        if(renderer.isVisible)
        {
            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }
    }
}