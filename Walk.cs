using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Walk : MonoBehaviour
{   
    public Vector2 Walking(Vector2 movimento, Vector3 rayDirection, bool agarrando, Animator animator)  //Fun��o respons�vel por captar o Input de movimento
    {
        movimento = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        //A varia��o de movimento � um Vector2 com os valores do Input horizontal (-1, 0 ou 1) e o Input vertical (-1, 0 ou 1)


        if (agarrando)                                             //Se ele estiver agarrando algo 
        {
            if (rayDirection.y == 0)
            {
                if (rayDirection.x == 1)
                {
                    animator.SetFloat("horizontal", 1);
                }
                else if (rayDirection.x == -1)
                {
                    animator.SetFloat("horizontal", -1);
                }
                animator.SetFloat("vertical", 0);
            }
            else
            {
                animator.SetFloat("horizontal", 0);
                if (rayDirection.y == 1)
                {
                    animator.SetFloat("vertical", 1);
                }
                else if (rayDirection.y == -1)
                {
                    animator.SetFloat("vertical", -1);
                }
            }

        }
        else
        {
            animator.SetFloat("horizontal", movimento.x);           //Atualiza a vari�vel do animador para a varia��o de movimento no eixo X
            animator.SetFloat("vertical", movimento.y);             //Atualiza a vari�vel do animador para a varia��o de movimento no eixo Y

        }

        animator.SetFloat("velocidade", movimento.sqrMagnitude);//Atualiza a vari�vel do animador para a velocidade do player



        if (movimento != Vector2.zero)                          //Se ele se mover
        {
            animator.SetFloat("horizontalIdle", movimento.x);
            //Atualiza a vari�vel Idle do animador para a �ltima dire��o que ele estava se movendo no X
            animator.SetFloat("verticalIdle", movimento.y);     //Atualiza a vari�vel Idle do animador para a �ltima dire��o que ele estava se movendo no y
        }

        return movimento;

    }
}
