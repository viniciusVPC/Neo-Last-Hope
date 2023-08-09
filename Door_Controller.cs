using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Controller : MonoBehaviour
{
    PublicTriggerController pTC;
    [SerializeField] GameObject[] Buttons;                                  //Array que guardar� todos os bot�es da cena
    [SerializeField] EdgeCollider2D[] doorCollider;
    /// <summary>
    /// 0 - Right
    /// 1 - Left
    /// </summary>

    [SerializeField] BoxCollider2D[] colliders;
    [SerializeField] GameObject[] doorSidesCorrection;
    // 0 - RIGHT; 1 - LEFT

    EdgeCollider2D chosenDoorCollider;

    Animator animator;

    [SerializeField] GameObject ConnectedButton;                                             //Refer�ncia a um bot�o conectado ao port�o
    [SerializeField]List<GameObject> ConnectedAndButtons = new List<GameObject>();
    public string conectionId = null;                                              //string publica para conectar a um bot�o
    public bool isAnd;
    public string doorDirection;
    public bool isClosed;

    string doorOpenAnimation;
    string doorCloseAnimation;

    private void Awake()                                                    //Antes do primeiro frame
    {
        Buttons = GameObject.FindGameObjectsWithTag("Button");              //Pega todos os GameObjects com a tag "Bot�o" na cena
        animator = gameObject.GetComponent<Animator>();
        for (int i = 0; i < doorSidesCorrection.Length; i++)
        {
            doorSidesCorrection[i].SetActive(false);
        }
    }

    private void Start()                                                    //No primeiro frame
    {
        for(int i = 0;i < doorSidesCorrection.Length;i++)
        {
            doorSidesCorrection[i].SetActive(false);
        }

        pTC = GetComponent<PublicTriggerController>();
        switch (doorDirection)                                              //Muda o funcionamento do script de acordo com o que est� escrito
        {
            case "LEFT":
                {
                    doorType(1, true, false, 2, 2, 3);
                    doorOpenAnimation = "OpenDoorRight";
                    doorCloseAnimation = "CloseDoorRight";
                }
                break;

            case "RIGHT":
                {
                    doorType(2, false, true, 1, 4, 5);
                    doorOpenAnimation = "OpenDoorLeft";
                    doorCloseAnimation = "CloseDoorLeft";
                }
                break;

            case "FB":
                {
                    doorType(0, false, false, 0, 0, 1);
                    doorOpenAnimation = "OpenDoor";
                    doorCloseAnimation = "CloseDoor";
                }
                break;

        }

        foreach (GameObject button in Buttons)                              //Para cada bot�o no Array com todos os bot�es
        {
            if(!isAnd)
            {
                if (button.GetComponent<Button_Controller>().conectionId == this.conectionId)
                {
                    ConnectedButton = button;                       //O bot�o conectado � o bot�o encontrado  
                }
            }
            else
            {
                if (button.GetComponent<Button_Controller>().conectionId != null &&
                            (button.GetComponent<Button_Controller>().buttonType == "AND" || button.GetComponent<Button_Controller>().buttonType == "FAND") &&
                            button.GetComponent<Button_Controller>().conectionId == this.conectionId)
                {
                    ConnectedAndButtons.Add(button);                       //O bot�o conectado � o bot�o encontrado
                }
            }            
        }

        chosenDoorCollider.isTrigger = false;                                 //A colis�o � solida novamente
        isClosed = true;
    }


    private void Update()                                                   //A cada frame  
    {
        if (isAnd)
        {
            bool canOpen = true;
            for (int i = 0; i < ConnectedAndButtons.Count; i++)
            {
                if (!ConnectedAndButtons[i].GetComponent<Button_Controller>().pisou) canOpen = false;
            }

            if (canOpen)
            {
                open();
            }
            else if (!isClosed) close();
        }
        else
        {
            if (ConnectedButton != null && ConnectedButton.GetComponent<Button_Controller>().pisou)
            {
                open();
            }
            else if(!isClosed) close();
        }
    }

    private void doorType(int a, bool sideA, bool sideB, int b, int c ,int d)
    {
        chosenDoorCollider = doorCollider[a];
        if (!sideA && !sideB) doorSidesCorrection[4].SetActive(true);
        else
        {
            doorSidesCorrection[0].SetActive(sideA);
            doorSidesCorrection[2].SetActive(sideA);
            doorSidesCorrection[1].SetActive(sideB);
            doorSidesCorrection[3].SetActive(sideB);
        }
        colliders[c].isTrigger = false;
        colliders[d].isTrigger = false;
    }

    private void open()
    {
        for (int i = 0; i < doorSidesCorrection.Length; i++)
        {
            doorSidesCorrection[i].SetActive(false);
        }
        animator.Play(doorOpenAnimation);              //Muda o sprite do port�o para "Aberto"
        isClosed = false;
        pTC.publicTrigger = true;
        chosenDoorCollider.isTrigger = true;           //Muda a colis�o do port�o para trigger, "N�o s�lido"
    }

    private void close()
    {
        animator.Play(doorCloseAnimation);
        isClosed = true;
        pTC.publicTrigger = false;
        chosenDoorCollider.isTrigger = false;                                 //A colis�o � solida novamente
    }
    
}
