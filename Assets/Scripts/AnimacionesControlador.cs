using System;
using System.Collections;
using UnityEngine;

public static class AnimacionesControlador
{
    public static void SetBool(Animator animator, string parametro, bool valor)
    {
        if (animator != null)
        {
            animator.SetBool(parametro, valor);
        }
    }

    public static void SetTrigger(Animator animator, string parametro)
    {
        if (animator != null)
        {
            animator.SetTrigger(parametro);
        }
    }

    public static void Play(Animator animator, string nombreAnimacion)
    {
        if (animator != null)
        {
            animator.Play(nombreAnimacion);
        }
    }

   public static void SetTriggerConCallback(MonoBehaviour caller, Animator animator, string parametro, float duracion, Action callback)
{
    if (animator != null && caller != null)
    {
        animator.ResetTrigger(parametro); 
        animator.SetTrigger(parametro);
        caller.StartCoroutine(EjecutarDespuesDeTiempo(duracion, callback));
    }
}

    private static IEnumerator EjecutarDespuesDeTiempo(float tiempo, Action callback)
    {
        yield return new WaitForSeconds(tiempo);
        callback?.Invoke();
    }
}
