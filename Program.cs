

int number;
do
{
    Console.Clear();

    Console.WriteLine("Выберите действие");
    Console.WriteLine("1. Сложение");
    Console.WriteLine("2. Вычетание");
    Console.WriteLine("3. Умножение");
    Console.WriteLine("4. Деление");
    Console.WriteLine("5. Возведение в степень");
    Console.WriteLine("6. Квадратный корень числа");
    Console.WriteLine("7. Процент от числа");
    Console.WriteLine("8. Факториал");
    Console.WriteLine("9. Выйти из калькулятора");

    Console.Write("--------> ");
    number = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine();

    if (number == 1)
    {
        Console.Write("Введите А: ");
        double a = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите B: ");
        double b = Convert.ToDouble(Console.ReadLine());
        double res = a + b;
        Console.WriteLine("Сумма А+B=" + res);
    }

    if (number == 2)
    {
        Console.Write("Введите А: ");
        double a = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите B: ");
        double b = Convert.ToDouble(Console.ReadLine());
        double res = a - b;
        Console.WriteLine("Разность А-B=" + res);
    }

    if (number == 3)
    {
        Console.Write("Введите А: ");
        double a = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите B: ");
        double b = Convert.ToDouble(Console.ReadLine());
        double res = a * b;
        Console.WriteLine("Произведение А*B=" + res);
    }

    if (number == 4)
    {
        Console.Write("Введите А: ");
        double a = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите B: ");
        double b = Convert.ToDouble(Console.ReadLine());
        double res = a / b;
        Console.WriteLine("Частное А/B=" + res);

    }

    if (number == 5)
    {
        Console.Write("Введите основание: ");
        double a = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите показатель степени: ");
        double b = Convert.ToDouble(Console.ReadLine());
        double res = Math.Pow(a, b);
        Console.WriteLine(a + " в степени " + b + " = " + res);
    }

    if (number == 6)
    {
        Console.Write("Введите подкоренное выражение: ");
        double a = Convert.ToDouble(Console.ReadLine());
        double res = Math.Sqrt(a);
        Console.WriteLine("Квадратный корень из " + a + " = " + res);
    }

    if (number == 7)
    {
        Console.Write("Введите число: ");
        double a = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите %: ");
        double b = Convert.ToDouble(Console.ReadLine());
        double res = a / 100 * b;
        Console.WriteLine(b + "% от " + a + " = " + res);
    }

    if (number == 8)
    {
        Console.Write("Введите число: ");
        int a = Convert.ToInt32(Console.ReadLine());
        int f = 1;
        for (int i = 1; i <= a; i++) f = f * i;
        Console.WriteLine("Результат: " + a + "! = " + f);
    }


    Console.WriteLine("Нажмите любую клавишу для продолжения...");
    Console.ReadKey();

} while (number != 9);


