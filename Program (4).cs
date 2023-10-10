using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace piano
{
    class Program
    {
        static double[][] Octaves =
        {
            //                 ДО       ДО#      РЕ       РЕ#      МИ      ФА,МИ#    ФА#     СОЛЬ     СОЛЬ#     ЛЯ       ЛЯ#      СИ
            new double [] {   32.70,   34.65,   36.71,   38.89,   41.20,   43.65,   46.25,   49.00,   51.91,   55.00,   58.27,   61.74 }, // Контроктава
            new double [] {   65.41,   69.30,   73.42,   77.78,   82.41,   87.31,   92.50,   98.00,  103.83,  110.00,  116.54,  123.47 }, // Большая октава
            new double [] {  130.81,  138.59,  146.83,  155.56,  164.81,  174.61,  185.00,  196.00,  207.65,  220.00,  233.08,  246.94 }, // Малая октава 
            new double [] {  261.63,  277.18,  293.66,  311.13,  329.63,  349.23,  369.99,  392.00,  415.30,  440.00,  466.16,  493.88 }, // Первая октава
            new double [] {  523.25,  554.37,  587.33,  622.25,  659.26,  698.46,  739.99,  783.99,  830.61,  880.00,  932.33,  987.77 }, // Вторая октава
            new double [] { 1046.50, 1108.73, 1174.66, 1244.51, 1318.51, 1396.91, 1479.98, 1567.98, 1661.22, 1760.00, 1864.66, 1975.53 }, // Третья октава
            new double [] { 2093.00, 2217.46, 2349.32, 2489.02, 2637.02, 2793.83, 2959.96, 3135.96, 3322.44, 3520.00, 3729.31, 3951.07 }, // Четвёртая октава
            new double [] { 4186.01, 4434.92, 4698.64, 5587.65, 5274.04, 5587.65, 5919.91, 6271.93, 6644.88, 7040.00, 7458.62, 7902.13 }, // Пятая октава
        };
        static double[] curOctave = selOctave(ConsoleKey.F5);
        static int curOctaveNum = 4;

        static double[] selOctave(ConsoleKey cKey)
        {
            int curOctaveNum = (int)cKey - 112;  // Чтобы перейти к номеру массива октавы от 0 до 7, т.к.код F1=112
            return Octaves[curOctaveNum];
        }

        static void playSound(ConsoleKey key)
        {
            char[] keys     = {'A','W','S','E','D','R','F','T','G','Y','H','U','J'};
            int[]  noteNums = { 0,  1,  2,  3,  4,  5,  5,  6,  7,  8,  9,  10, 11};
            char curKey = (char)key;

            for (int i=0; i<=12; i++)
                if (curKey==keys[i])
                {
                    int freq = (int)Math.Round(curOctave[noteNums[i]]);
                    if (freq < 37) freq = 37; // т.к. Console.Beep() не может воспроизводить звук с частотой < 37 Гц
                    Console.SetCursorPosition(10 + i * 2, 20 - (i % 2));
                    Console.Write(keys[i]);

                    Console.Beep(freq, 200);

                    Console.SetCursorPosition(10 + i * 2, 20 - (i % 2));
                    Console.Write(" ");
                    break;
                }
        }

        static void showOctav()
        {
            string[] octNames = { "F1: Контроктава",
                                  "F2: Большая октава",
                                  "F3: Малая октава",
                                  "F4: Первая октава",
                                  "F5: Вторая октава",
                                  "F6: Третья октава",
                                  "F7: Четвёртая октава",
                                  "F8: Пятая октава"
                                 };
            Console.Clear();
            Console.WriteLine("Выбор октавы: F1..F8:\n" +
                              "Выход       : Esc\n"); 
            for (int i = 0; i < 8; i++)
            {
                if (i == curOctaveNum) Console.Write("+ ");
                else Console.Write("  ");
                Console.WriteLine(octNames[i]);
            }
        }

        static void Main(string[] args)
        {
            ConsoleKeyInfo txt;
            showOctav();

            do
            {
                txt = Console.ReadKey(true);

                switch (txt.Key)
                {
                    // F1-F8 - выбор октавы
                    case ConsoleKey.F1: // Контроктава
                    case ConsoleKey.F2: // Большая октава
                    case ConsoleKey.F3: // Малая октава
                    case ConsoleKey.F4: // Первая октава
                    case ConsoleKey.F5: // Вторая октава
                    case ConsoleKey.F6: // Третья октава
                    case ConsoleKey.F7: // Четвертая октава
                    case ConsoleKey.F8: // Пятая октава
                        curOctave = selOctave(txt.Key);
                        showOctav();
                        break;

                    // Выбор номера ноты
                    case ConsoleKey.A: // ДО
                    case ConsoleKey.W: // ДО диез
                    case ConsoleKey.S: // РЕ
                    case ConsoleKey.E: // РЕ диез
                    case ConsoleKey.D: // МИ
                    case ConsoleKey.F: // МИ#
                    case ConsoleKey.R: // ФА
                    case ConsoleKey.T: // ФА#
                    case ConsoleKey.G: // СОЛЬ
                    case ConsoleKey.Y: // СОЛЬ#
                    case ConsoleKey.H: // ЛЯ
                    case ConsoleKey.U: // ЛЯ#
                    case ConsoleKey.J: // СИ
                        playSound(txt.Key);
                        break;
                }
            } while (txt.Key != ConsoleKey.Escape);
        }
    }
}