
int number;
do
{
    Console.Clear();

    Console.WriteLine("Выберите программу");
    Console.WriteLine("1. Угадай число");
    Console.WriteLine("2. Таблица умножения");
    Console.WriteLine("3. Вывод делителей числа");
    Console.WriteLine("4. Выход из меню");

    Console.Write("--------> ");
    number = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine();

    if (number == 1)
    {
        Random rand = new Random();
        int i = rand.Next(101);

        Console.WriteLine("Компьютер загадал число от 0 до 100. Попробуй отгадать его");

        int a;
        do
        {
            Console.WriteLine("Введите число:");
            a = Convert.ToInt32(Console.ReadLine());

            if (a > i)
                Console.WriteLine("Надо число меньше");
            if (a < i)
                Console.WriteLine("Надо число больше");
        }
        while (a != i);

        Console.WriteLine("Молодец!\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    if (number == 2)
    {
        int[,] mt = new int[10, 10];
        for (int b = 0; b <= 9; b++)
            for (int j = 0; j <= 9; j++)
                mt[b, j] = (b + 1) * (j + 1);

        for (int b = 0; b <= 9; b++)
        {
            for (int j = 0; j <= 9; j++)
                Console.Write(mt[b, j] + "\t");
            Console.WriteLine("\n");
        }
        Console.ReadKey();

    }


    if (number == 3)
    {
        Console.Write(" Введите число: ");
        int x = Convert.ToInt32(Console.ReadLine());
        Console.Write(" Делители числа " + x + ": 1, ");

        for (int c = 2; c < x; c++)
            if (x % c == 0)
                Console.Write(c + " , ");

        Console.WriteLine(x);
        Console.ReadKey();
    }

}
while (number != 4);