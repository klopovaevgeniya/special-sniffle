using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ca_csKbdTrainer
{
    // Пользовательский тип одной записи в списке таблицы рекордов
    struct tRecordData
    {
        public string userName;
        public float cps;
        public float cpm;
    }

    // Класс "Таблица рекордов"
    static class tRecordTable
    {
        const string fileName = "records.json";
        public static tRecordData curUser = new tRecordData();
        static List<tRecordData> records = new List<tRecordData>();

        // Метод загрузки списка таблицы рекордов из JSON-файла
        public static void LoadRecTable()
        {
            if (File.Exists(fileName)) {
                string s = File.ReadAllText(fileName);
                records = JsonConvert.DeserializeObject<List<tRecordData>>(s);
            }
        }

        // Метод сохранения списка таблицы рекордов в JSON-файл
        public static void SaveRecTable()
        {
            string s = JsonConvert.SerializeObject(records.ToList());
            File.WriteAllText(fileName, s);
        }

        // Метод сортировки списка таблицы рекордов
        public static void SortRecTable()
        {
            tRecordData tmp;
            for(int i=0; i<records.Count()-1; i++)
                for (int j=i+1; j<records.Count(); j++)
                    if (records[i].cpm < records[j].cpm)
                    {
                        tmp = records[i];
                        records[i] = records[j];
                        records[j] = tmp;
                    }
        }

        // Метод добавления записи в список таблицы рекордов
        public static void AddRec(tRecordData d)
        {
            records.Add(d);
        }

        // Метод вывода на экран списка таблицы рекордов
        public static void ShowRecTable()
        {
            bool isCurUser;
            Console.Clear();
            Console.WriteLine("Таблица рекордов:\n-----------------");
            for (int i = 0; i < records.Count(); i++)
            {
                if (curUser.userName == records[i].userName &&  // Конечно, есть очень маленькая вероятность одновременного совпадения в результатах
                    curUser.cpm == records[i].cpm &&            // имени, кол-ва симв./мин. и кол-ва симв./сек. и лучше бы добавить в tRecordData
                    curUser.cps == records[i].cps) {            // например, уникальный ID, но, по условию задачи, ID в пользовательский тип не входит
                    isCurUser = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else 
                    isCurUser = false;

                Console.WriteLine(records[i].userName.PadRight(20) + "\t" +
                                  string.Format("{0:f3}", records[i].cpm) + " симв./мин.\t" +
                                  string.Format("{0:f3}", records[i].cps) + " симв./сек." +
                                  (isCurUser ? "  <--- Ваш результат" : ""));
                Console.ResetColor();
            }
        }
    }


    // Класс работы с текстом
    class tTrainer
    {
        string fileName = "texts.json";
        List<string> texts = new List<string>();
        int curTextNum = 0;
        public int curSymb = 0;
        public bool stop = false;

        public void getText()
        {
            if (File.Exists(fileName)) {
                string s = File.ReadAllText(fileName);
                texts = JsonConvert.DeserializeObject<List<string>>(s);
            }
            else
            {
                texts.Add("За последние несколько лет компьютер становится всё в большей степени " +
                          "неотъемлемой частью почти каждого человека. Использование ПК не только " +
                          "существенно облегчает интелектуальный труд и помогает решать сложнейшие " +
                          "задачи всех уровней жизнедеятельности человека, но и способствует развитию " +
                          "информационных технологий науки и техники, коренным образом изменяя наше " +
                          "сознание. В век компьютеров, глобальных сетей и телекомуникаций каждый " +
                          "человек, столкнувшись с этим миром, постепенно, с большим трудом, методом " +
                          "проб и ошибок становится квалифицированным пользователем, применяя накопленные " +
                          "знания в решении каждодневных больших и малых вопросов и проблем.");
            }
            Random r = new Random();
            curTextNum = r.Next(0, texts.Count()-1);
            curSymb = 0;
        }

        public void showText(char ch)
        {
            if (ch == texts[curTextNum][curSymb]) curSymb++;

            Console.SetCursorPosition(0, 0);
            Console.Write(texts[curTextNum]);
            if (curSymb > 0) {
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(texts[curTextNum].Substring(0, curSymb));
                Console.ResetColor();
            }

            if (curSymb == texts[curTextNum].Length)
                stop = true;
        }
    }

    // Класс таймер
    static class tTimer
    {
        public const int testTime = 60;
        public static Stopwatch myTimer = new Stopwatch();
        public static Thread myTimerTread = new Thread(new ThreadStart(showTimer));
        public static int timerRow;
        public static int leftTime = testTime;

        public static void showTimer()
        {
            int elapsedTime;
            while (true)
            {
                if (myTimer.IsRunning)
                {
                    int row = Console.CursorTop;
                    int col = Console.CursorLeft;
                    Console.CursorVisible = false;

                    elapsedTime = (int)myTimer.ElapsedMilliseconds / 1000;
                    leftTime = testTime - elapsedTime > 0 ? testTime - elapsedTime : 0;
                    Console.SetCursorPosition(0, timerRow);
                    Console.Write(("Печатайте текст...  У Вас осталось: " + 
                                   leftTime.ToString() + " сек.").PadRight(40));

                    Console.SetCursorPosition(col, row);
                    Console.CursorVisible = true;
                    Thread.Sleep(500);
                }
            }
        }
    }

    // Главная программа
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo choice;
            tTrainer Trainer = new tTrainer();
            tTimer.myTimerTread.Start();

            do
            {
                Console.Clear();
                Console.Write("Введите имя для таблицы рекордов: ");
                tRecordTable.curUser.userName = Console.ReadLine();

                Console.Clear();
                Trainer.getText();
                Trainer.showText((char)0);

                int row = Console.CursorTop + 2;
                Console.Write("\n--------------------------------------------------------------" +
                              "\nКак только будете готовы, нажмите Enter");
                choice = Console.ReadKey(true);
                if (choice.Key != ConsoleKey.Enter) break;

                tTimer.timerRow = row;
                Trainer.stop = false;

                tTimer.myTimer.Start();
                Console.SetCursorPosition(0, 0);

                do {
                    if (tTimer.leftTime == 0)
                        Trainer.stop = true;

                    while(Console.KeyAvailable) {
                        choice = Console.ReadKey(true);
                        Trainer.showText(choice.KeyChar);
                    }
                } while (!Trainer.stop);
                tTimer.myTimer.Stop();

                Console.SetCursorPosition(0, row);
                Console.Write("Тестирование завершено. Нажмите Enter для продолжения...");
                do { choice = Console.ReadKey(true); } while (choice.Key != ConsoleKey.Enter);


                tRecordTable.curUser.cps = (float)(Trainer.curSymb / (tTimer.myTimer.ElapsedMilliseconds/1000.0));
                tRecordTable.curUser.cpm = tRecordTable.curUser.cps * 60;
                tTimer.myTimer.Reset();

                tRecordTable.LoadRecTable();
                tRecordTable.AddRec(tRecordTable.curUser);
                tRecordTable.SortRecTable();
                tRecordTable.SaveRecTable();

                tRecordTable.ShowRecTable();
                Console.Write("\nЕсли Вы хотите попробовать еще раз, нажмите Enter");
                choice = Console.ReadKey(true);

            } while (choice.Key == ConsoleKey.Enter);

            tTimer.myTimerTread.Abort();

        } // end Main()
    } // end Program
} // end namespace