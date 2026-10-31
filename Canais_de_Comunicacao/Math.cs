namespace Canais_de_Comunicacao;

public static class ChannelMath
{
    public static double Nyquist(double B, double N)
    {
        var C  = 2*B * Math.Log2(N);
        return C;
    }
    
    public static double Shannon(double B, double SNR)
    {
        var C  = B * Math.Log2(1 + SNR);
        return C;
    }
    
    public static double DbToLinear(double snrDb) => Math.Pow(10, snrDb / 10.0);
}