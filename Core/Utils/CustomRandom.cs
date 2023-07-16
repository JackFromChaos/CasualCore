[System.Serializable]
public class CustomRandom
{
    public long seed;
    private const long multiplier = 25214903917;
    private const long addend = 11;
    private const long mask = (1L << 48) - 1;
    
    public CustomRandom(long seed)
    {
        this.seed = (seed ^ multiplier) & mask;
    }

    protected int Next(int bits)
    {
        seed = (seed * multiplier + addend) & mask;
        return (int)(seed >> (48 - bits));
    }

    public int NextInt()
    {
        return Next(31);
    }

    public int NextInt(int min, int max)
    {
        return NextInt(max - min) + min;
    }
    public int NextInt(int n)
    {
        if (n <= 0)
            throw new System.ArgumentException("n must be positive");

        if ((n & -n) == n)
            return (int)((n * (long)Next(31)) >> 31);

        int bits, val;
        do
        {
            bits = Next(31);
            val = bits % n;
        } while (bits - val + (n - 1) < 0);
        return val;
    }

    public float NextFloat()
    {
        return Next(24) / ((float)(1 << 24));
    }
}