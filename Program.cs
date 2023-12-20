using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ca_csTaskManager
{
    enum tProcessActions
    {
        none = 0,
        killOneProc = 1,
        killAllProc = 2
    }

    static class tProcess
    {
        static Process[] processList;
        static int curItem = 0;

        const int nameWidth = 40;
        const int idWidth = 5;
        const int memWidth = 14;
        const int priorWidth = 12;
        const int leftMargin = 85;

        public static void killOneProcess()
        {
            processList[curItem].Kill();
            processList[curItem].WaitForExit();
        }

        public static void killAllProcesses()
        {
            string procName = processList[curItem].ProcessName;
            Process[] tmpProcList = Process.GetProcessesByName(procName);
            foreach (Process p in tmpProcList)
            {
                try
                {
                    p.Kill();
                    p.WaitForExit();
                    do { Thread.Sleep(100); } while (!p.HasExited);
                }
                catch { }
            }
        }

        public static void initProcess()
        {
            processList = null;
            processList = Process.GetProcesses();

            sortProcessList();
            showProcessList();
            firstItem();
        }

        public static void sortProcessList()
        {
            Process tmp = new Process();
            for (int i = 0; i < processList.Length - 1; i++)
                for (int j = i + 1; j < processList.Length; j++)
                    if (string.Compare(processList[i].ProcessName, processList[j].ProcessName) > 0)
                    {
                        tmp = processList[i];
                        processList[i] = processList[j];
                        processList[j] = tmp;
                    }
        }

        public static tProcessActions showOneProcess()
        {
            tProcessActions res = tProcessActions.none;
            bool stop2 = false;

            Console.Clear();
            Console.WriteLine("Процесс: " + processList[curItem].ProcessName + "\n" +
                              "".PadLeft(50, '-'));
            try
            {
                Console.WriteLine("Использование диска:              " + processList[curItem].VirtualMemorySize64.ToString() + "\n" +
                                  "Приоритет:                        " + processList[curItem].BasePriority.ToString() + "\n" +
                                  "Класс приоритета:                 " + processList[curItem].PriorityClass + "\n" +
                                  "Время использования процесса:     " + processList[curItem].TotalProcessorTime.ToString() + "\n" +
                                  "Все время использования:          " + processList[curItem].UserProcessorTime.ToString() + "\n" +
                                  "Использование оперативной памяти: " + processList[curItem].PrivateMemorySize64.ToString() + "\n" +
                                  "".PadLeft(50, '-') +
                                  "\nСтатус = " + processList[curItem].Threads[0].ThreadState + "\n" +
                                  "".PadLeft(50, '-') +
                                  "\nD         - завершить процесс\n" +
                                  "Delete    - завершить все процессы с этим именем"
                                 );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Процесс недоступен для отображения");
                Console.WriteLine("Причина: " + ex.Message);
                Console.WriteLine("".PadLeft(50, '-'));
            }
            finally
            {
                Console.WriteLine("Backspace - вернуться к списку процессов");
            }

            ConsoleKeyInfo ch;
            do
            {
                ch = Console.ReadKey(true);
                switch (ch.Key)
                {
                    case ConsoleKey.D:
                        res = tProcessActions.killOneProc;
                        stop2 = true;
                        break;
                    case ConsoleKey.Delete:
                        res = tProcessActions.killAllProc;
                        stop2 = true;
                        break;
                    case ConsoleKey.Backspace:
                        res = tProcessActions.none;
                        stop2 = true;
                        break;
                }
            } while (!stop2);

            return res;
        }

        public static void showProcessList()
        {
            int totWidth = nameWidth + idWidth + memWidth + priorWidth + 2;

            Console.Clear();
            string s = "Диспетчер задач".PadLeft(totWidth / 2 + 7, ' ').PadRight(totWidth, ' ');
            Console.WriteLine(s);
            Console.WriteLine("".PadLeft(totWidth, '-'));

            Console.WriteLine("Название  ".PadRight(nameWidth / 2).PadLeft(nameWidth / 2).PadLeft(nameWidth) +
                              "ID".PadRight(idWidth / 2).PadLeft(idWidth / 2).PadLeft(idWidth) +
                              "Память".PadRight(memWidth / 2).PadLeft(memWidth / 2).PadLeft(memWidth) +
                              "Приоритет".PadLeft(priorWidth + 2));

            foreach (Process theprocess in processList)
            {
                Console.WriteLine("  " + theprocess.ProcessName.PadRight(nameWidth) +
                                  (theprocess.Id.ToString()).PadLeft(idWidth) +
                                  (Math.Round(theprocess.PrivateMemorySize64 / 1048576.0, 2).ToString() + " Мб").PadLeft(memWidth) +
                                  theprocess.BasePriority.ToString().PadLeft(priorWidth));
            }

            Console.SetCursorPosition(leftMargin, 0); Console.Write("Возможные действия:");
            Console.SetCursorPosition(leftMargin, 2); Console.Write("Перемещение : ↓ ↑ Home End");
            Console.SetCursorPosition(leftMargin, 3); Console.Write("Выбор       : ◄─┘");
            Console.SetCursorPosition(leftMargin, 4); Console.Write("Выход       : Escape");
        }

        private static void showPtr()
        {
            Console.CursorVisible = false;
            if (curItem < 2) Console.SetCursorPosition(0, curItem);
            Console.SetCursorPosition(0, curItem + 3);
            Console.Write("->");
        }

        private static void hidePtr()
        {
            Console.SetCursorPosition(0, curItem + 3);
            Console.Write("  ");
        }

        public static void nextItem()
        {
            hidePtr();
            if (curItem < processList.Count() - 1) curItem++;
            else curItem = 0;
            showPtr();
        }

        public static void prevItem()
        {
            hidePtr();
            if (curItem > 0) curItem--;
            else curItem = processList.Count() - 1;
            showPtr();
        }

        public static void firstItem()
        {
            hidePtr();
            curItem = 0;
            showPtr();
        }

        public static void lastItem()
        {
            hidePtr();
            curItem = processList.Count() - 1;
            showPtr();
        }

        public static void showCurItem()
        {
            hidePtr();
            showPtr();
        }
    }


    // --- Главная программа
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo choice;
            bool stop = false;

            tProcess.initProcess();

            do
            {// Главный цикл программы
                choice = Console.ReadKey(true);

                switch (choice.Key)
                {
                    // Перемещение по списку процессов
                    case ConsoleKey.UpArrow:
                        tProcess.prevItem();
                        break;
                    case ConsoleKey.DownArrow:
                        tProcess.nextItem();
                        break;
                    case ConsoleKey.Home:
                        tProcess.firstItem();
                        break;
                    case ConsoleKey.End:
                        tProcess.lastItem();
                        break;

                    case ConsoleKey.Escape: // Выход
                        stop = true; ;
                        break;

                    case ConsoleKey.Enter:  // Подробно о процессе
                        tProcessActions curAction = tProcess.showOneProcess();
                        switch (curAction)
                        {
                            case tProcessActions.killOneProc:
                                tProcess.killOneProcess();
                                tProcess.initProcess();
                                break;
                            case tProcessActions.killAllProc:
                                tProcess.killAllProcesses();
                                tProcess.initProcess();
                                break;
                            case tProcessActions.none:
                                tProcess.showProcessList();
                                tProcess.showCurItem();
                                break;
                        }
                        break;
                }
            } while (!stop);

        } // Конец Main
    } // Конец класса Program
}
