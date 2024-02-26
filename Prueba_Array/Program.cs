// See89 https://aka.ms/new-console-template for more information
using static System.Runtime.InteropServices.JavaScript.JSType;

Console.WriteLine("Hello, World!");
List<string> lista_palabras= new List<string>() { "casa#fuente",
"comida",
"perro"};


separar_palabras_y_guardalas(lista_palabras);

static void separar_palabras_y_guardalas(List<string> palabras) {

    foreach (string palabra in palabras) {

        if (palabra.Contains("#")) { 
        
        
        string primer_palabra=palabra.Substring(0,palabra.IndexOf("#"));

            Console.WriteLine(primer_palabra);


         
        
        }
    
        
    
    
    }





}