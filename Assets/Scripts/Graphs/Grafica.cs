using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grafica{

    public List<Vertice> grafica = new List<Vertice>();
	public List<Vertice> camino = new List<Vertice>();
	private Vertice verticeActual, verticeDestino;

	private List<Vertice> abiertos = new List<Vertice>(); //Lista de nodos que aun no se han explorado
	private List<Vertice> cerrados = new List<Vertice>(); //Lista de nodos ya explorados

	//Agrega un v�rtice a la lista de v�rtices de la gr�fica.
    public void AgregarVertice(Vertice nuevoVertice) {
        if(!grafica.Contains(nuevoVertice)){
			grafica.Add(nuevoVertice);
		}
    }

	//Aplica el Algoritmo de A*
	public bool AStar(Vertice inicio, Vertice final) {
		//Completar
		return true;
    }

	//Auxiliar que reconstruye el camino de A*
	public void reconstruirCamino(Vertice inicio, Vertice final) {
		camino.Clear();
		camino.Add(final);
		//Completar
		verticeActual = final.camino;
		while(verticeActual.id !=  inicio.id){
			camino.Add(verticeActual);
		}
		camino.Add(inicio); 
	}

	float distancia(Vertice a, Vertice b) {
		//Completar distancia entre dos vectores utilizando Vector3.distance()
		return Vector3.Distance(a.posicion, b.posicion);
	}

	int menorF(List<Vertice> l) {
		//Coompletar
		float menorValor = l[0].f;
		int contador = 0;
		int indice = 0;

		for(int i = 0; i < l.Count; i++){
			if(l[i].f < menorValor){
				menorValor = l[i].f;
				indice = contador;
			}
			contador++;
		}
		return indice;
	}

	//M�todo que da una representaci�n escrita de la gr�fica.
	public string toString() {
		string aux = "\nG:\n";
		foreach (Vertice v in grafica) {
			aux += v.toString() + "\n";
		}
		return aux;
	}

}
