using System;
public struct Interval
{
    private readonly float min;
    private readonly float max;

    private static readonly Random random = new Random();

    public float Min => min;
    public float Max => max;

    public Interval(int minValue, int maxValue)
    {
        int tempMin = minValue;
        int tempMax = maxValue;

        if (tempMin > tempMax)
        {
            Console.WriteLine($"[Interval] Некорректные данные: Min ({minValue}) > Max ({maxValue}). Значения поменяны местами.");
            int swap = tempMin;
            tempMin = tempMax;
            tempMax = swap;
        }

        if (tempMin == tempMax)
        {
            Console.WriteLine($"[Interval] Некорректные данные: Min и Max равны ({tempMin}). Max увеличен на 10.");
            tempMax += 10;
        }

        if (tempMin < 0)
        {
            Console.WriteLine($"[Interval] Некорректные данные: Min ({tempMin}) < 0. Установлено значение 0.");
            tempMin = 0;
        }
        if (tempMax < 0)
        {
            Console.WriteLine($"[Interval] Некорректные данные: Max ({tempMax}) < 0. Установлено значение 0.");
            tempMax = 0;
        }

        this.min = tempMin;
        this.max = tempMax;
    }

    public float Get()
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}






