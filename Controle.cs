using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Controle : MonoBehaviour
{
    Walk walk;
    RayCast rc;
    Vector2 movimento;                                              //Vector2 que guarda a movimentação aplicada ao player


    public bool PCIsOpen;
    public bool canTalk;
    public bool isTalking;

    RaycastHit2D hit;


    [SerializeField] DialogueTrigger dialogueTrigger;
    public DialogueManager dialogueManager;
    Rigidbody2D rb;                                                 //Referência a um Rigidbody
    Animator animator;                                              //Referência a um Animator
    [SerializeField] Text textoDialogado;
    public string textoEscrito;

    string textoDialogo;
    int i;

    [SerializeField] GameObject objSegurado;                        //Referência publica a um GameObject
    public GameObject DialogueCanvas;
    [SerializeField] Transform castPoint;                           //O ponto de início do Raycast
    [SerializeField] float speed, pushingSpeed, runningSpeed, viewRay;            //Variáveis float para velocidade, velociade quando empurrando e o tamanho do Raycast

    /// <summary>
    /// 
    /// Quanto ao Raycast:
    ///     Entenda o Raycast como um Vetor, com um ponto de início (castPoint), uma direção (rayDirection) e um tamanho (viewRay).
    ///     
    /// Quanto ao [SerializeField]:
    ///     É um jeito bem maneiro de mostrar variaveis privadas no editor da Unity. ELAS CONTINUAM SENDO PRIVADAS, ou seja, outros
    ///     scripts não conseguirão acessá-las, mas facilita a visualização e controle, além de poupar memória (mesmo que bem pouco).
    ///     
    /// </summary>

    private void Awake()                                            //Antes do primeiro frame do jogo
    {
        rb = GetComponent<Rigidbody2D>();                           //rb faz referência ao Rigidbody2D do player
        animator = GetComponent<Animator>();                        //animator faz referência ao Animator do player
        dialogueManager = FindObjectOfType<DialogueManager>();

        walk = gameObject.AddComponent(typeof(Walk)) as Walk;
        rc = gameObject.AddComponent(typeof(RayCast)) as RayCast;
        rc.setRayDirection(new Vector3(1, 0, 0));                        //Direção inicial do Raycast
    }

    void Update()                                                   //Ocorre a cada frame do jogo
    {

        if (!PCIsOpen && !isTalking)
        {
            movimento = walk.Walking(movimento, rc.getRayDirection(), rc.getAgarrando(), animator);  //Chama a função responsável por andar
        }
        else animator.SetFloat("velocidade", 0);


        hit = rc.RayCastCode(viewRay, canTalk, castPoint, movimento);


        if (!PCIsOpen && canTalk && Input.GetKeyDown(KeyCode.Space))
        {
            Talk();
        }

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("GrabBox"))
            {
                objSegurado = hit.collider.gameObject;                  //O objeto a ser segurado será o objeto que está sendo "visto"
            }
            else
            {
                objSegurado = null;                                     //O objeto a ser segurado é nulo
            }

            if (hit.collider.gameObject.CompareTag("PC") || hit.collider.gameObject.CompareTag("Dialogo"))
            {

                dialogueTrigger = hit.collider.gameObject.GetComponent<DialogueTrigger>();

                if (hit.collider.gameObject.CompareTag("PC"))
                {
                    if (dialogueManager.isPC == false)
                    {
                        dialogueManager.isPC = true;
                    }

                    if (dialogueTrigger.text != null)
                    {
                        textoEscrito = dialogueTrigger.text;
                    }
                }

                if (hit.collider.gameObject.CompareTag("Dialogo"))
                {
                    dialogueManager.isPC = false;
                }

                canTalk = true;
            }
            else
            {
                canTalk = false;
                isTalking = false;
                dialogueManager.isPC = false;
            }

        }
        else
        {
            objSegurado = null;
            dialogueTrigger = null;
            canTalk = false;
        }

        rc.Grab(animator, rb, objSegurado);

    }  

    private void FixedUpdate()                                      //Ocorre a cada frame do jogo, se ajustando ao framerate de cada PC
    {
        rb.MovePosition(rb.position + movimento * (rc.getAgarrando() ? pushingSpeed : (Input.GetKey(KeyCode.LeftShift) ? runningSpeed : speed)) * Time.fixedDeltaTime);
        
        /// <summary>
        /// 
        /// Modifica a posição do Rigidbody2D do jogador para: posição atual + variação de posição * velocidade * cada segundo (se não tiver isso fica estranho).
        ///     Essa parte dentro dos parenteses é uma "função if abreviada".
        ///     
        /// Função if abreviada: (condição ? instruçãoSeForVerdadeira : instruçãoSeForFalsa);
        ///     Ela literalmente pergunta: "Está agarrando?", se sim, usa a velociade de "empurro", se não, usa a velocidade normal.
        /// 
        /// </summary>
    }

    //------------------------------------- FUNÇÕES -------------------------------------------------


    

    void Talk()
    {
        if (isTalking)
        {
            dialogueTrigger.NextDialogue();
            return;
        }

        dialogueTrigger.TriggerDialogue();
        isTalking = true;
    }
}