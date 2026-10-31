using System.Globalization;
using Canais_de_Comunicacao;

CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

Console.WriteLine("Iniciando Experimentos");


Console.WriteLine("-----------------------------------------------------------");
Tests.Experimento1();
Console.WriteLine("-----------------------------------------------------------");
Tests.Experimento2();
Console.WriteLine("-----------------------------------------------------------");
Tests.Experimento3();
Console.WriteLine("-----------------------------------------------------------");
Tests.Experimento4();
Console.WriteLine("-----------------------------------------------------------");
Console.WriteLine("Finalizando Experimentos - clique qualquer tecla para fechar");
Console.ReadKey();// impede o console de fechar