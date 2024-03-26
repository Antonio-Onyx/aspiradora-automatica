using System;
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

		inicio.g = 0;
		inicio.h = distancia(inicio, final);
		inicio.f = inicio.h;

		abiertos.Add(inicio); //agregamos el nodo inicial a abiertos

		while(abiertos.Count > 0){ //mientras la lista de abiertos no sea vacia / mientras no hayamos explorado todo
			int i = menorF(abiertos);
			Vertice actual;
			actual = abiertos[i]; //nuestro nodo actual es el primero en la lista de abiertos
			if(actual.id == final.id){
				reconstruirCamino(inicio, final);
				return true;
			}
			abiertos.RemoveAt(i);//una vez que ya pasamos por nuestro nodo visitado lo pasamos a cerrados
			cerrados.Add(actual);

			foreach(Vertice vecino in actual.vecinos){
				if(cerrados.IndexOf(vecino) > -1){
					continue; //si el un vecino del nodo actual esta la lista de cerrados, nos saltamos esta iteracion
				}
				if(abiertos.IndexOf(vecino) == -1){
					abiertos.Add(vecino);
					vecino.camino = actual;
					vecino.g = actual.g + 1;
					vecino.h = distancia(actual,final);
					vecino.f = vecino.g + vecino.h;
				}
			}
		}
		return true;
    }

	//Auxiliar que reconstruye el camino de A*
	public void reconstruirCamino(Vertice inicio, Vertice final) {
		camino.Clear();
		camino.Add(final);
		
		//Completar
		var p = final.camino;
		while(p.id !=  inicio.id){
			camino.Insert(0, p);
			p = p.camino;
		}
		camino.Insert(0,inicio); 

		//imprimir el contenido de la lista camino en la consola
		string aux = "";
		foreach(Vertice v in camino){
			aux += v.id.ToString() + ",";
		}
	}

	float distancia(Vertice a, Vertice b) {
		//Completar distancia entre dos vectores utilizando Vector3.distance()
		//float distanciaX = a.posicion.x - b.posicion.x;
		//float distanciaY = a.posicion.y - b.posicion.y;
		//float distanciaZ = a.posicion.z - b.posicion.z;
		//float distancia = (float)Math.Sqrt(distanciaX * distanciaX + distanciaY * distanciaY + distanciaZ * distanciaZ);
		return Vector3.Distance(a.posicion, b.posicion);
	}

	int menorF(List<Vertice> l) {
		//Coompletar
		float menorValor = l[0].f;
		int indice = 0;
		int c = 0;

		for(int i = 0; i < l.Count; i++){
			if(l[i].f < menorValor){
				menorValor = l[i].f;
				indice = c;
			}
			c++;
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
