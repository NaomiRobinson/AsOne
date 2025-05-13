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
}
