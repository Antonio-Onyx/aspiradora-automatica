using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ComportamientoAutomatico : MonoBehaviour {


    //Enum para los estados
    public enum State {
        MAPEO,
        DFS,
        ASTARCARGA,
        CONTINUARMAPEANDO,
        CARGANDO
    }

    private State currentState;
    private Sensores sensor;
	private Actuadores actuador;
	private Mapa mapa;
    private Vertice verticeActual, verticeDestino;
    public bool fp = true, look;
    public bool pila = true, path = false;
    public Vector3 destino;
    public Vertice inicio, final;
    private Grafica grafica;
    public int s;
    public Vertice actualCamino;
    public int indiceCamino = 0;
    public List<Vertice> camino = new List<Vertice>();
    GameObject basura;


    void Start(){
        SetState(State.DFS);
        sensor = GetComponent<Sensores>();
		actuador = GetComponent<Actuadores>();
        mapa = GetComponent<Mapa>();
        mapa.ColocarNodo(0);
        mapa.popStack(out verticeActual);
        destino = new Vector3(0.0f, 0.0f, 0.0f);
        inicio = verticeActual;
        final = verticeActual;
    }


    void FixedUpdate() {
        //si la bateria esta por debajo del 40%, se cambia al estado ASTARCARGA
        if(sensor.Bateria() < 30){
            currentState = State.ASTARCARGA;
        }
        switch (currentState) {
            case State.MAPEO:
            UpdateMAPEO();
            break;
            case State.DFS:
            UpdateDFS();
            break;
            case State.ASTARCARGA:
            UpdateAstarCarga();
            break;
            case State.CONTINUARMAPEANDO:
            continuarMapeando();
            break;
            case State.CARGANDO:
            cargando();
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
        //con la variable fp controlaremos si debemos sacar un vertice de la pila, los tres vertices que coloca nuestra
        //aspiradora al frente (2), a la izquierda (1) y la derecha (3)
        if (fp){ 
            if (!mapa.popStack(out verticeDestino)) { //con cada popStack() sacamos el nodo
                SetState(State.DFS);                  //y con SetState() cambiamos el estado  DFS y se llama a la funcion UpdateDFS()
                return;                               //asi colocando los nodos
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

        //esto lo usamos para limpiar basura
        if(sensor.TocandoBasura()){
            Debug.Log("Se esta tocando la basura");
            actuador.Limpiar(sensor.GetBasura());
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
        //si hay espacio libre al frente
        if(sensor.FrenteLibre()){
            //colocamos un nodo en frente de la pila
            mapa.ColocarNodo(2);
        }
        //se cambia el estado a mapeo
        SetState(State.MAPEO);
    }

    void UpdateAstarCarga(){
        Debug.Log("de regeso con A*");
        //si la lista de camino esta vacia
        if(camino.Count == 0){
            //se busca el camino a la base de carga utilizando A*
            if(mapa.mapa.AStar(verticeActual,mapa.baseCarga)){
                //se guarda la lista de camino en la variable camino
                camino = mapa.mapa.camino;
            } else {
                Debug.Log("No hay camino");
            }
        } else {
            //si no se ha llegado al final del camino
            if(indiceCamino != camino.Count){
                //si la distancia al siguiente vertice es mayor a 0.04f
                if(Vector3.Distance(sensor.Ubicacion(), camino[indiceCamino].posicion) >= 0.04f){//si no se ha llegado al vertice
                    //se gira hacia el siguiente vertice del camino
                    transform.LookAt(camino[indiceCamino].posicion);
                    //se avanza hacia el siguiente vertice del camino
                    actuador.Adelante();
                } else {
                    //si ya se llego al vertice
                    //se actualiza el vertice actual al que sigue en el camino
                    actualCamino = camino[indiceCamino];
                    indiceCamino++;
                }
            } else {
                //aqui metemos un if para checar el nivel de bateria
                //se cambia el estado a continuar mapeando
                SetState(State.CARGANDO);
            }
        }
    }

    void cargando(){
        Debug.Log("CARGANDO");
        //si el nivel de bateria es mayor al 90%
        if(sensor.Bateria() > 90){
            SetState(State.CONTINUARMAPEANDO);
        }
    }

    void continuarMapeando(){
        if(indiceCamino != -1){//si todavia no hemos llegado al vertice en el que nos quedamos
            if(indiceCamino == camino.Count){
                indiceCamino--;
            }

            if(Vector3.Distance(sensor.Ubicacion(), camino[indiceCamino].posicion) >= 0.04f){
                transform.LookAt(camino[indiceCamino].posicion);
                actuador.Adelante();
            } else {
                actualCamino = camino[indiceCamino];
                indiceCamino--;
            }
        } else {
            //se reinicia el indice del vertice actual en la lista de camino
            indiceCamino = 0;
            //vaciamos la lista de camino
            camino = new List<Vertice>();
            //se reinicia el vertice actual
            actualCamino = null;
            //se cambia el estado a DFS
            SetState(State.MAPEO);
        }
    }



    // Función para cambiar de estado
    void SetState(State newState) {
        currentState = newState;
    }

}
