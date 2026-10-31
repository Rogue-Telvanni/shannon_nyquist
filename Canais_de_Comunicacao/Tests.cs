using ScottPlot;

namespace Canais_de_Comunicacao;

public static class Tests
{
    public static void Experimento1()
    {
        double[] bandasKHz = [1, 5, 10]; // kHz
        double[] niveis = [2, 4, 8, 16];
        
        var plt = new Plot();
        Console.WriteLine($"{"B (kHz)",-10}{"N",-6}{"C (bit/s)",-12}");
        foreach (var B in bandasKHz)
        {
            List<double> results = []; 
            foreach (var N in niveis)
            {
                // transforma para Hz
                var result = ChannelMath.Nyquist(B * 1000, N);
                results.Add(result);
                Console.WriteLine($"{B,-10}{N,-6}{result,-12:N0}");
            }
            
            var curva = plt.Add.Scatter(niveis, [..results]);
            curva.LegendText = $"B = {B} kHz";
        }
        
        plt.Title("Experimento 1 - Capacidade de Nyquist");
        plt.XLabel("N (níveis de sinalização)");
        plt.YLabel("C (bit/s)");
        plt.ShowLegend();
        plt.SavePng("exp1_nyquist.png", 800, 500);
    }

    public static void Experimento2()
    {
        double[] SNRs = [1000, 5000, 10000, 50000];

        var plt = new Plot();
        Console.WriteLine($"{"B (kHz)",-10}{"SNR",-8}{"C (bit/s)",-12}");

        double maior = double.MinValue, menor = double.MaxValue;
        string maiorInfo = "", menorInfo = "";

        foreach (var snr in SNRs)
        {
            List<double> results = [];
            List<double> Bs = [];
            for (int B = 1; B <= 10; B++)
            {
                var result = ChannelMath.Shannon(B * 1000, snr);
                results.Add(result);
                Bs.Add(B);
                Console.WriteLine($"{B,-10}{snr,-8}{result,-12:N0}");

                if (result > maior) { maior = result; maiorInfo = $"B = {B} kHz, SNR = {snr}"; }
                if (result < menor) { menor = result; menorInfo = $"B = {B} kHz, SNR = {snr}"; }
            }

            var curva = plt.Add.Scatter(Bs, [..results]);
            curva.LegendText = $"SNR = {snr}";
        }

        Console.WriteLine($"Maior capacidade: {maior:N0} bit/s ({maiorInfo})");
        Console.WriteLine($"Menor capacidade: {menor:N0} bit/s ({menorInfo})");

        plt.Title("Experimento 2 - Capacidade de Shannon");
        plt.XLabel("B (kHz)");
        plt.YLabel("C (bit/s)");
        plt.ShowLegend();
        plt.SavePng("exp2_shannon.png", 800, 500);
    }
    
    public static void Experimento3()
    {
        const double B = 5; // kHz
        var plt = new Plot();

        Console.WriteLine($"{"B (kHz)",-10}{"SNR (dB)",-10}{"SNR (linear)",-14}{"C (bit/s)",-12}");
        List<double> results = [];
        List<double> SNRs = [];
        List<double> Dbs = [];
        for (int db = 0; db <= 40; db+= 5)
        {
            double snr = ChannelMath.DbToLinear(db);
            SNRs.Add(snr);
            Dbs.Add(db);
            
            var result = ChannelMath.Shannon(B * 1000, snr);
            results.Add(result);
            Console.WriteLine($"{B,-10}{db,-6}{snr,-6}{result,-12:N0}");
        }
        
        var curva = plt.Add.Scatter(Dbs, [..results]);
        curva.LegendText = $"B = {B} kHz";
        
        plt.Title("Experimento 3 - Influência do ruído (B = 5 kHz)");
        plt.XLabel("SNR (dB)");
        plt.YLabel("C (bit/s)");
        plt.ShowLegend();
        plt.SavePng("exp3_ruido.png", 800, 500);
    }

    public static void Experimento4()
    {
        const double B = 3; // kHz
        const double Db = 30; // db
        double[] niveis = [2, 4, 8, 16, 32, 64];
        double SNR = ChannelMath.DbToLinear(Db); // db
        double shannon = ChannelMath.Shannon(B * 1000, SNR);
        Console.WriteLine($"Limite de Shannon: {shannon:N0} bit/s");
        Console.WriteLine($"{"B (kHz)",-10}{"N",-6}{"SNR (dB)",-10}{"Nyquist (bit/s)",-18}{"Shannon (bit/s)",-18}{"Nyquist > Shannon?",-20}");
        List<double> results = [];
        List<double> maiores = [];

        foreach (var N in niveis)
        {
            var result = ChannelMath.Nyquist(B * 1000, N);
            results.Add(result);
            bool maior = result > shannon;
            if (maior) maiores.Add(N);
            
            Console.WriteLine($"{B,-10}{N,-6}{Db,-10}{result,-18:N0}{shannon,-18:N0}{(maior ? "SIM" : "não"),-20}");

            
        }
        
        Console.WriteLine(maiores.Count > 0
            ? $"Nyquist ultrapassa Shannon para N = {string.Join(", ", maiores)}"
            : "Nenhuma configuração ultrapassa o limite de Shannon.");
        
        var plt = new Plot();

        var curvaNyquist = plt.Add.Scatter(niveis, [..results]);
        curvaNyquist.LegendText = "Nyquist (canal ideal)";

        var linhaShannon = plt.Add.HorizontalLine(shannon);
        linhaShannon.LegendText = $"Limite de Shannon ({shannon:N0} bit/s)";
        linhaShannon.LinePattern = LinePattern.Dashed;

        plt.Title("Experimento 4 - Nyquist vs Shannon (B = 3 kHz, SNR = 30 dB)");
        plt.XLabel("N (níveis de sinalização)");
        plt.YLabel("C (bit/s)");
        plt.ShowLegend();
        plt.SavePng("exp4_nyquist_vs_shannon.png", 800, 500);

    }
}