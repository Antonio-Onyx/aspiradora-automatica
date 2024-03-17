using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class Grafica{

    public List<Vertice> grafica = new List<Vertice>();
	public List<Vertice> camino = new List<Vertice>();
	private Vertice verticeActual, verticeDestino;
	
	//Agrega un v�rtice a la lista de v�rtices de la gr�fica.
    public void AgregarVertice(Vertice nuevoVertice) {
        if(!grafica.Contains(nuevoVertice)){
			grafica.Add(nuevoVertice);
		}
    }

	//Aplica el Algoritmo de A*
	public bool AStar(Vertice inicio, Vertice final) {
		//Completar
		List<Vertice> abiertos = new List<Vertice>(); //Lista de nodos que aun no se han explorado
		List<Vertice> cerrados = new List<Vertice>(); //Lista de nodos ya explorados

		abiertos.Add(inicio);

		while(abiertos.Count > 0){ //mientras la lista de abiertos no sea vacia / mientras no hayamos explorado todo
			Vertice actual;
			actual = abiertos[0];
			if(actual == final){
				reconstruirCamino(inicio, final);
				return true;
			}
			abiertos.RemoveAt(0);
			cerrados.Add(actual);

			foreach(Vertice vecino in actual.vecinos){
				if(cerrados.Contains(vecino)){
					continue;
				}

				//f(n) = g(n) + h(n)
				//g(n)
				float costoG = actual.g + distancia(actual, vecino);

				if(!abiertos.Contains(vecino) || costoG < vecino.g){
					vecino.padre = actual;
					vecino.g = costoG;
					vecino.h = distancia(vecino, final);
					vecino.f = vecino.g + vecino.h;

					if(!abiertos.Contains(vecino)){
						abiertos.Add(vecino);
					}
				}
			}

		}

		return false;
    }

	//Auxiliar que reconstruye el camino de A*
	public void reconstruirCamino(Vertice inicio, Vertice final) {
		camino.Clear();
		camino.Add(final);
		//Completar
		verticeActual = final.camino;
		while(verticeActual.id !=  inicio.id){
			camino.Add(verticeActual);
			verticeActual = verticeActual.camino;
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
		int indice = 0;

		for(int i = 0; i < l.Count; i++){
			if(l[i].f < menorValor){
				menorValor = l[i].f;
				indice = i;
			}
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
