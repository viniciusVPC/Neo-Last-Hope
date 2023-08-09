using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Controller : MonoBehaviour
{
    PublicTriggerController pTC;
    public string buttonType;                                           //String publica que dir� ao script qual o tipo do bot�o
    public Sprite[] OrStates;                                           //Sprites vermelhos
    public Sprite[] AndStates;                                          //Sprites azuis
    public Sprite[] FixOrStates;
    public Sprite[] FixAndStates;
    bool isFixed = false;
    Sprite[] States;                                                    //Sprites do bot�o em si (o script definir� se ele ter� os sprites vermelhos ou azuis)

    bool isDemo;


    public bool pisou;                                                  //Booleana que avisa se o bot�o foi pisado
    [HideInInspector] public bool isPlayer;
    [SerializeField] List<GameObject> parentBoxes = new List<GameObject>();
    public string conectionId;                                          //id do bot�o (para conectar ao port�o)

    private void Start()                                                //No primeiro frame
    {
        pTC = GetComponent<PublicTriggerController>();
        switch (buttonType)                                             //"L�" o tipo de bot�o da string p�blica
        {
            case "OR":                                                  //Se for "OR"
                {
                    States = OrStates;                                  //O array de estados do bot�o segurar� os sprites vermelhos
                }
                break;

            case "AND":                                                 //Se for "AND"
                {
                    States = AndStates;                                 //O array de estados do bot�o segurar� os sprites azuis
                }
                break;

            case "FOR":
                {
                    States = FixOrStates;
                    isFixed = true;

                }
                break;

            case "FAND":
                {
                    States = FixAndStates;
                    isFixed = true;
                }
                break;

            case "DEMO":                                                 
                {
                    States = AndStates;                                 
                    isDemo = true;
                }
                break;
        }


        this.GetComponent<SpriteRenderer>().sprite = States[0];         //Seta o sprite "Aberto" como o inicial do bot�o
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Feet"))
        {
            isPlayer = true;
            pisou = true;
            if (isDemo)
            {
                Application.Quit();
            }
            this.GetComponent<SpriteRenderer>().sprite = States[1];
            pTC.publicTrigger = true;
        }

        if (col.gameObject.CompareTag("Box"))
        {
            parentBoxes.Add(col.gameObject);
            pisou = true;
            if (isDemo)
            {
                Application.Quit();
            }
            this.GetComponent<SpriteRenderer>().sprite = States[1];
            pTC.publicTrigger = true;

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Feet"))
        {
            isPlayer = false;
        }

        if (col.gameObject.CompareTag("Box"))
        {
            parentBoxes.Remove(col.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (parentBoxes.Count == 0 && !isPlayer)
        {
            if (pisou && !isFixed)
            {
                pisou = false;
                this.GetComponent<SpriteRenderer>().sprite = States[0];
                if (buttonType == "OR" || buttonType == "AND")
                {
                }
                else pTC.publicTrigger = false;

            }
        }
    }
}
