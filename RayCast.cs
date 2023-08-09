using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RayCast : MonoBehaviour
{
    Grab grab;
    Vector3 rayDirection;                                          //Vector3 que guarda a dire��o que o Raycast estar� apontando
    bool agarrando;

    private void Awake()
    {
        grab = gameObject.AddComponent(typeof(Grab)) as Grab;
    }

    public void setRayDirection(Vector3 valor)
    {
        rayDirection = valor;
    }

    public Vector3 getRayDirection()
    {
        return rayDirection;
    }

    public void Grab(Animator animator, Rigidbody2D rb, GameObject objSeg)                                                     //Fun��o respons�vel por agarrar coisas
    {
        if (objSeg != null && Input.GetKeyDown(KeyCode.Space)) //Se o objeto a ser segurado n�o for nulo e o jogador apertar espa�o
        {
            agarrando = true;                                       //Avisa que o player est� agarrando algo
            animator.SetBool("isPushing", true);
            objSeg.GetComponentInParent<Rigidbody2D>().isKinematic = false;
            //Tira a "kinematicidade" do objeto segurado (ele volta a receber as leis da f�sica)
            objSeg.GetComponentInParent<FixedJoint2D>().enabled = true;//Ativa um componente que liga o Rigidbody2D do objeto ao do player
            objSeg.GetComponentInParent<FixedJoint2D>().connectedBody = rb;
            //Avisa que o Rigidbody2D � o do player

        }                                                           //Se o objeto a ser segurado n�o for nulo e o jogador soltar o espa�o
        else if (Input.GetKeyUp(KeyCode.Space)) //objSegurado != null &&
        {
            agarrando = false;                                      //N�o est� mais agarrando
            animator.SetBool("isPushing", false);
            GameObject[] Caixas = GameObject.FindGameObjectsWithTag("Box");
            //Procura todos os GameObjects na cena com a tag caixa (isso facilita meu trabalho)

            for (int i = 0; i < Caixas.Length; i++)                 //Para cada caixa encontrada
            {
                Caixas[i].GetComponent<FixedJoint2D>().enabled = false;
                //Desliga o FixedJoint da caixa
                Caixas[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                //Seta a velocidade da caixa para zero (pra que ela n�o saia deslizando quando o player soltar)
                Caixas[i].GetComponent<Rigidbody2D>().isKinematic = true;
                //Liga a "kinematicidade" da caixa
            }
        }
    }

    public void setAgarrando(bool agarrando)
    {
        this.agarrando = agarrando;
    }

    public bool getAgarrando()
    {
        return this.agarrando;
    }

    public RaycastHit2D RayCastCode(float distance, bool canTalk, Transform castPoint, Vector2 movimento)
        //Fun��o respons�vel para avisar se o meu Raycast "v�" uma caixa, usando o tamanho do Raycast como dado
    {
        Vector2 endPos;                                             //Crio um Vector2 para ser a posi��o final do Raycast

        Vector3 tempPos = new Vector3(movimento.x, movimento.y, 0).normalized;
        //Transformo o Vector2 de movimenta��o em um Vector3 chamado tempPos (que vai guardar a posi��o tempor�ria que o Raycast estar� apontando)

        if (tempPos != Vector3.zero)                                 //Se o player estiver andando
        {
            if (agarrando)                                         //Se n�o estiver agarrando nada
            {
                endPos = castPoint.position + rayDirection * distance;
            }
            else
            {
                rayDirection = tempPos;                             //A dire��o do Raycast segue a dire��o do jogador (Ele vai "olhar pra frente")
                endPos = castPoint.position + tempPos * distance;   //O ponto final do Raycast � a posi��o do ponto inicial + dire��o * tamanho do vetor

            }
            //Se ele estiver agarrando, o ponto final � a �ltima posi��o que ele esteve andando (Pra que ele puxe de "costas")
        }
        else endPos = castPoint.position + rayDirection * distance; //Se ele estiver parado tamb�m, o ponto final � a �ltima posi��o que ele esteve olhando

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Interactable"));
        //Eu juro que n�o conhe�o esse c�digo, peguei da net.
        //Pelo que eu entendi ele verifica se o Raycast est� colidindo com algo em uma LayerMask de coisas interag�veis (que eu criei no editor)

        //Se ele estiver captando algo e esse algo tiver a tag "Caixa" 

        Debug.DrawLine(castPoint.position, endPos, Color.blue);     //Isso � umm debug pra mostrar o Raycast no editor
        return hit;
    }
}
