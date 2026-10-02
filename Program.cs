using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        // Iniciar cronómetro para medir el tiempo de ejecución
        Stopwatch sw = Stopwatch.StartNew();

        string rutaArchivo = "datos_grafo.txt";
        Dictionary<string, List<string>> grafo = CargarGrafoDesdeTxt(rutaArchivo);

        // Mostrar la reportería requerida
        MostrarReporteria(grafo);

        sw.Stop();
        Console.WriteLine("\n----------------------------------------");
        Console.WriteLine($"[Rendimiento] Tiempo de ejecución: {sw.ElapsedMilliseconds} ms ({sw.ElapsedTicks} ticks)");
        Console.WriteLine("----------------------------------------");
    }

    static Dictionary<string, List<string>> CargarGrafoDesdeTxt(string ruta)
    {
        Dictionary<string, List<string>> grafo = new Dictionary<string, List<string>>();

        if (!File.Exists(ruta))
        {
            Console.WriteLine($"[Error] No se encontró el archivo '{ruta}'. Asegúrate de crearlo en la carpeta del proyecto.");
            return grafo;
        }

        string[] lineas = File.ReadAllLines(ruta);
        foreach (string linea in lineas)
        {
            if (string.IsNullOrWhiteSpace(linea)) continue;

            string[] partes = linea.Split(',');
            if (partes.Length == 2)
            {
                string origen = partes[0].Trim();
                string destino = partes[1].Trim();

                // Agregar nodo origen si no existe
                if (!grafo.ContainsKey(origen))
                    grafo[origen] = new List<string>();

                // Agregar nodo destino si no existe
                if (!grafo.ContainsKey(destino))
                    grafo[destino] = new List<string>();

                // Como es un grafo no dirigido, añadimos la conexión en ambas direcciones
                if (!grafo[origen].Contains(destino))
                    grafo[origen].Add(destino);

                if (!grafo[destino].Contains(origen))
                    grafo[destino].Add(origen);
            }
        }

        Console.WriteLine("[Éxito] Grafo cargado correctamente desde el archivo de texto.");
        return grafo;
    }

    static void MostrarReporteria(Dictionary<string, List<string>> grafo)
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine("         REPORTERÍA DEL GRAFO (C#)      ");
        Console.WriteLine("========================================");

        if (grafo.Count == 0)
        {
            Console.WriteLine("El grafo está vacío o no se pudo cargar.");
            return;
        }

        int totalVertices = grafo.Count;
        int totalAristas = 0;

        foreach (var par in grafo)
        {
            totalAristas += par.Value.Count;
        }
        // En un grafo no dirigido, cada arista se cuenta dos veces (ida y vuelta)
        totalAristas = totalAristas / 2;

        Console.WriteLine($"Total de Vértices (Nodos): {totalVertices}");
        Console.WriteLine($"Total de Aristas (Conexiones): {totalAristas}\n");

        Console.WriteLine("Listas de Adyacencia (Conexiones por nodo):");
        foreach (var par in grafo)
        {
            Console.WriteLine($"   Nodo [{par.Key}] -> Conectado con: {string.Join(", ", par.Value)}");
        }
        Console.WriteLine("========================================");
    }
}
