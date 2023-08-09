using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Controller : MonoBehaviour
{
    PublicTriggerController pTC;
    public string buttonType;                                           //String publica que dirá ao script qual o tipo do botão
    public Sprite[] OrStates;                                           //Sprites vermelhos
    public Sprite[] AndStates;                                          //Sprites azuis
    public Sprite[] FixOrStates;
    public Sprite[] FixAndStates;
    bool isFixed = false;
    Sprite[] States;                                                    //Sprites do botão em si (o script definirá se ele terá os sprites vermelhos ou azuis)

    bool isDemo;


    public bool pisou;                                                  //Booleana que avisa se o botão foi pisado
    [HideInInspector] public bool isPlayer;
    [SerializeField] List<GameObject> parentBoxes = new List<GameObject>();
    public string conectionId;                                          //id do botão (para conectar ao portão)

    private void Start()                                                //No primeiro frame
    {
        pTC = GetComponent<PublicTriggerController>();
        switch (buttonType)                                             //"Lê" o tipo de botão da string pública
        {
            case "OR":                                                  //Se for "OR"
                {
                    States = OrStates;                                  //O array de estados do botão segurará os sprites vermelhos
                }
                break;

            case "AND":                                                 //Se for "AND"
                {
                    States = AndStates;                                 //O array de estados do botão segurará os sprites azuis
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


        this.GetComponent<SpriteRenderer>().sprite = States[0];         //Seta o sprite "Aberto" como o inicial do botão
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
