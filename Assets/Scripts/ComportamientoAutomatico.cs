using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ComportamientoAutomatico : MonoBehaviour {


    //Enum para los estados
    public enum State {
        MAPEO,
        DFS
    }

    private State currentState;
    private Sensores sensor;
	private Actuadores actuador;
	private Mapa mapa;
    private Vertice verticeActual, verticeDestino;
    public bool fp = true, look;
    public Vector3 destino;


    void Start(){
        SetState(State.DFS);
        sensor = GetComponent<Sensores>();
		actuador = GetComponent<Actuadores>();
        mapa = GetComponent<Mapa>();
        mapa.ColocarNodo(0);
        mapa.popStack(out verticeActual);
        destino = new Vector3(0.0f, 0.0f, 0.0f);
        //mapa.setPreV(anterior);
    }


    void FixedUpdate() {
        switch (currentState) {
            case State.MAPEO:
            UpdateMAPEO();
            break;
            case State.DFS:
            UpdateDFS();
            break;
        }
    }

    // Funciones de actualizacion especificas para cada estado
    /**
     * PASOS PARA EL DFS
     * 1.- Colocar un vértice (meterlo a la pila 'ColocarNodo' ya lo mete a la pila
     * 2.- Sacar de la pila, e intentar poner mas vértices
     * 3.- Hacer backtrack al siguiente vértice en la pila
     * 4.- Repetir hasta vaciar la pila
     */
    void UpdateMAPEO() {
        if (fp){
            if (!mapa.popStack(out verticeDestino)) {
                SetState(State.DFS);
                return;
            }
            destino = verticeDestino.posicion;
            mapa.setPreV(verticeDestino);
            fp = false;
        }
        if (verticeDestino.padre.id == verticeActual.id) {
            if (!look) {
                transform.LookAt(destino);
                look = true;
            }
            if (Vector3.Distance(sensor.Ubicacion(), destino) >= 0.04f) {
                actuador.Adelante();
            } else {
                verticeActual = verticeDestino;
                fp = true;
                look = false;
                SetState(State.DFS);
            }
        } else {
            Debug.Log(Vector3.Distance(sensor.Ubicacion(), verticeActual.padre.posicion) >= 0.04f);
            if (Vector3.Distance(sensor.Ubicacion(), verticeActual.padre.posicion) >= 0.04f){
                if (!look) {
                    transform.LookAt(verticeActual.padre.posicion);
                    look = true;
                }
                actuador.Adelante();
            } else {
                verticeActual = verticeActual.padre;
                look = false;
            }
        }
    }

    void UpdateDFS(){
        if(!sensor.FrenteLibre()){
            actuador.Detener();
        }
        if(sensor.IzquierdaLibre()){
            mapa.ColocarNodo(1);
        }
        if(sensor.DerechaLibre()){
            mapa.ColocarNodo(3);
        }
        if(sensor.FrenteLibre()){
            mapa.ColocarNodo(2);
        }
        SetState(State.MAPEO);
    }

    // Función para cambiar de estado
    void SetState(State newState) {
        currentState = newState;
    }

}
