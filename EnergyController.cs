using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public List<GameObject> Sensores = new List<GameObject>();
    [SerializeField] BoxCollider2D colisao;
    [SerializeField] string id = "teste";


    // Start is called before the first frame update
    void Start()
    {
        colisao = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) { 
            checkSensor();
        }
    }

    public void setId(string id)
    {
        this.id = id;
    }

    public string getId() { return id; }

    public bool searchEnergy(GameObject obj)
    {
        for(int i = 0; i < Sensores.Count; i++)
        {
            if (Sensores[i].GetComponent<SensorTrigger>().GetTarget() == obj)
            {
                print("Vejo de volta, meu id �: " + getId());
                return true;
            }
        }
        return false;
    }

    void checkSensor()
    {
        for(int i = 0; i<15; i++)
        {
            for (int j = 0; j < Sensores.Count; j++)
            {
                SensorTrigger sensorTrigger = Sensores[j].GetComponent<SensorTrigger>();
                EnergyController other;
                GameObject target = sensorTrigger.GetTarget();
                if (target != null)
                {
                    switch (sensorTrigger.GetTarget().tag)
                    {
                        case "Button":
                            Debug.Log("Vejo um bot�o");
                            this.id = target.GetComponent<Button_Controller>().conectionId;
                            break;

                        case "Energia":
                            Debug.Log("Vejo outra energia");
                            other = target.GetComponent<EnergyController>();
                            if (other.searchEnergy(this.gameObject))                                            //tamb�m me v�
                            {
                                if (other.getId() != null)
                                {
                                    this.id = other.getId();
                                }                                //o Id dele � o meu
                            }
                            break;

                        case "Door":
                            Debug.Log("Vejo um port�o");
                            target.GetComponentInParent<Door_Controller>().conectionId = this.id;
                            break;

                        default:
                            Debug.Log("N�o vejo nada");
                            break;
                    }
                }
                else print("N�o vejo nada");

            }
        }
        }
}
