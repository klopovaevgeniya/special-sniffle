using System.Xml.Serialization;

namespace notebook
{
    // Класс для хранения в списке
    public class tNode
    {
        public string name;         // Хранит в зависимости от "уровня": дату / название заметки / одно из описаний заметки
        public List<tNode> content; // Хранит в зависимости от "уровня": список заметок в дате / список описаний в заметке
    }

    class Program
    {
        static List<tNode> notes = new List<tNode>(); // список дат

        static int dateNum = 0;                       // Номер текущей даты
        static int noteNum = 0;                       // Номер текущей заметки
        const string statusInWork = "В работе";     // статус заметки
        const string statusComplete = "Выполнено";    // статус заметки
        static int detailVisible = 0;                 // признак "видимости" описания заметки 
        const string fileName = "notebook.xml";       // имя файла для хранения данных

        // Метод для добавления даты в ежедневник со списком заметок и их описаний
        static void addOneDate(string _date, string[] _delo, string[][] _defs)
        {
            // Добавляем дату
            tNode oneNote = new tNode();
            oneNote.name = _date;
            oneNote.content = new List<tNode>();

            // Добавляем заметку
            int noteNum = 0;
            foreach (string delo in _delo)
            {
                oneNote.content.Add(new tNode());
                oneNote.content[noteNum].name = delo;
                oneNote.content[noteNum].content = new List<tNode>();

                // Добавляем описания к заметке
                int defsNum = 0;
                foreach (string defs in _defs[noteNum])
                {
                    oneNote.content[noteNum].content.Add(new tNode());
                    oneNote.content[noteNum].content[defsNum].name = defs;
                    defsNum++;
                };
                noteNum++;
            };
            notes.Add(oneNote);
        }

        // Метод для очистки заданной части экрана от А(lx,ty) до B(rx,dy) заданным цветом
        static void clearArea(int lx, int ty, int rx, int dy, ConsoleColor color)
        {
            int oldX = Console.CursorLeft;
            int oldY = Console.CursorTop;
            ConsoleColor oldColor = Console.BackgroundColor;

            string blankStr = "";
            for (int i = lx; i <= rx; i++)
                blankStr += " ";

            Console.SetCursorPosition(lx, ty);
            Console.BackgroundColor = color;
            for (int i = ty; i <= dy; i++)
                Console.WriteLine(blankStr);

            Console.SetCursorPosition(oldX, oldY);
            Console.BackgroundColor = oldColor;
        }

        // Метод для показа одной даты и всех заметок в ней        
        static void showNote(int dateNum)
        {
            clearArea(0, 0, 40, 14, ConsoleColor.Black);
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Выбрана дата: " + notes[dateNum].name + "\n------------------------");
            int deloCounter = 0;
            foreach (tNode delo in notes[dateNum].content)
            {
                if (delo.content[2].name == statusComplete)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Gray;

                if (deloCounter == noteNum) Console.Write("-> ");
                else Console.Write("   ");
                Console.WriteLine((deloCounter + 1).ToString() + "." + delo.name);
                deloCounter++;
            }
        }

        // Метод для очистки части экрана, на которой отображается описание заметки
        static void clearDetails()
        {
            clearArea(0, 15, 60, Console.WindowHeight - 2, ConsoleColor.Black);
            detailVisible = 0;
        }

        // Метод для отображения описания заметки
        static void showDetails()
        {
            clearDetails();
            Console.SetCursorPosition(0, 15);
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Описание заметки:\n-----------------");
            Console.WriteLine("Название заметки: " + notes[dateNum].content[noteNum].name);
            Console.WriteLine("Срок исполнения : " + notes[dateNum].content[noteNum].content[0].name);
            Console.WriteLine("Содержание      : " + notes[dateNum].content[noteNum].content[1].name);
            Console.Write("Статус          : ");

            if (notes[dateNum].content[noteNum].content[2].name == statusComplete)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(notes[dateNum].content[noteNum].content[2].name);

            Console.ForegroundColor = ConsoleColor.Gray;
            detailVisible = 1;
        }

        // Метод для вывода "легенды" - описания клавиш, задействованных в программе
        static void showLegend()
        {
            Console.SetCursorPosition(70, 0); Console.Write("┌───────────────────────────────────┐");
            Console.SetCursorPosition(70, 1); Console.Write("│ Выбор даты         :  стрелки ← → │");
            Console.SetCursorPosition(70, 2); Console.Write("│ Выбор заметки      :  стрелки ↑ ↓ │");
            Console.SetCursorPosition(70, 3); Console.Write("│ Описание заметки   :  Enter ◄┘    │");
            Console.SetCursorPosition(70, 4); Console.Write("│ Статус заметки     :  F3          │");
            Console.SetCursorPosition(70, 5); Console.Write("│ Добавить заметку   :  F4          │");
            Console.SetCursorPosition(70, 6); Console.Write("│                                   │");
            Console.SetCursorPosition(70, 7); Console.Write("│ Выход из программы :  Esc         │");
            Console.SetCursorPosition(70, 8); Console.Write("└───────────────────────────────────┘");
        }

        // Метод для изменения статуса заметки "В работе" / "Выполнено"
        static void changeDeloStatus()
        {
            if (notes[dateNum].content[noteNum].content[2].name == statusComplete)
                notes[dateNum].content[noteNum].content[2].name = statusInWork;
            else
                notes[dateNum].content[noteNum].content[2].name = statusComplete;

            if (detailVisible == 1) showDetails();
        }

        // Метод для добавления новой заметки
        static void addNewNote()
        {
            // Готовим настройки
            clearDetails();
            Console.SetCursorPosition(0, 15);
            Console.CursorVisible = true;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Добавление заметки:\n-------------------");

            // Вводим данные для заметки. Статус для новой заметки всегда будет "В работе"
            Console.Write("Дата заметки (ДД.ММ.ГГГГ) : "); string _noteDate = Console.ReadLine();
            Console.Write("Название заметки          : "); string _noteName = Console.ReadLine();
            Console.Write("Срок исполнения           : "); string _notePeriod = Console.ReadLine();
            Console.Write("Содержание                : "); string _noteContent = Console.ReadLine();

            // Проверяем корректность введенной даты заметки 
            DateTime chkDate;
            if (!DateTime.TryParse(_noteDate, out chkDate))
            {
                // Если дата введена неправильно, то отменяем добавление заметки
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nОшибка при вводе даты. Заметка не добавлена.\n" +
                                  "Нажмите любую клавишу для продолжения...");
                Console.ReadKey(true);
                Console.ForegroundColor = ConsoleColor.Gray;
                clearDetails();
            }

            // Если дата введена правильно
            else
            {
                // Проверяем, есть уже такая дата или нет
                int i;
                for (i = 0; i < notes.Count(); i++)
                    // Если такая дата уже есть, то запоминаем ее номер в списке дат
                    if (notes[i].name == _noteDate)
                    {
                        dateNum = i;
                        break;
                    }

                // Если такой даты нет, то добавляем новую дату в список и запоминаем ее номер
                if (i == notes.Count())
                {
                    notes.Add(new tNode());
                    notes[notes.Count() - 1].name = _noteDate;
                    notes[notes.Count() - 1].content = new List<tNode>();
                    dateNum = notes.Count() - 1;
                }

                // Добавляем новую заметку в дату
                notes[dateNum].content.Add(new tNode());
                int _deloN = notes[dateNum].content.Count() - 1;

                notes[dateNum].content[_deloN].name = _noteName;
                notes[dateNum].content[_deloN].content = new List<tNode>();

                // Добавляем описание заметки
                notes[dateNum].content[_deloN].content.Add(new tNode());
                notes[dateNum].content[_deloN].content[0].name = _notePeriod;
                notes[dateNum].content[_deloN].content.Add(new tNode());
                notes[dateNum].content[_deloN].content[1].name = _noteContent;
                notes[dateNum].content[_deloN].content.Add(new tNode());
                notes[dateNum].content[_deloN].content[2].name = statusInWork;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nЗаметка успешно добавлена.\n" +
                                  "Нажмите любую клавишу для продолжения...");
            }

            noteNum = 0;
            Console.CursorVisible = false;
            Console.ReadKey(true);
            Console.ForegroundColor = ConsoleColor.Gray;
            clearDetails();
        }

        // Метод перехода к следующей дате в списке по нажатию "вправо"
        // работает "по кругу"
        static void nextNote()
        {
            if (dateNum < notes.Count() - 1) dateNum++;
            else dateNum = 0;
            noteNum = 0;
            clearDetails();
        }

        // Метод перехода к предыдущей дате в списке по нажатию "влево"
        // работает "по кругу"
        static void prevNote()
        {
            if (dateNum > 0) dateNum--;
            else dateNum = notes.Count() - 1;
            noteNum = 0;
            clearDetails();
        }

        // Метод перехода к следующей заметке в списке по нажатию "вниз"
        // работает "по кругу"
        static void nextDelo()
        {
            if (noteNum < notes[dateNum].content.Count() - 1) noteNum++;
            else noteNum = 0;
            clearDetails();
        }

        // Метод перехода к предыдущей заметке в списке по нажатию "вверх"
        // работает "по кругу"
        static void prevDelo()
        {
            if (noteNum > 0) noteNum--;
            else noteNum = notes[dateNum].content.Count() - 1;
            clearDetails();
        }

        // Метод для сохранения данных файл (исп. сериализация)
        static void saveToFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<tNode>));
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, notes);
            }
        }

        // Метод для загрузки данных из файла (исп. сериализация)
        static void loadFromFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<tNode>));
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                notes = (List<tNode>)serializer.Deserialize(stream);
            }
        }

        // Метод для первичного наполнения списка дат
        static void setDefaults()
        {
            // Формируем заметки и их описания для нескольких дат
            // для первичного наполнения "Ежедневника" и добавляем их в список дат
            {
                string[] delo1 = { "Полить цветы",
                                   "Купить продукты" };
                string[][] defs1 = { new string[] { "10.10.2023, до 12-00", "Все цветы в комнатах и на балконе", statusInWork },
                                     new string[] { "10.10.2023, до 18-00", "Молоко, Хлеб, Колбаса, Торт", statusComplete } };
                addOneDate("10.10.2023", delo1, defs1);
            }

            {
                string[] delo1 = { "Купить подарки",
                                   "Купить цветы",
                                   "Поздравить маму" };
                string[][] defs1 = { new string[] { "11.10.2023, до 15-00", "Большой набор косметики и духи", statusInWork },
                                     new string[] { "11.10.2023, до 17-00", "Букет из белых роз, с доставкой", statusComplete },
                                     new string[] { "11.10.2023, после 19-00", "Подарить подарки и цветы", statusInWork } };
                addOneDate("11.10.2023", delo1, defs1);
            }

            {
                string[] delo1 =   { "Выучить лекцию по Философии",
                                     "Сделать практическую работу по ОАиП",
                                     "Разместить программу на GitHub",
                                     "Скачать лекции с ClassRoom"};
                string[][] defs1 = { new string[] { "16.10.2023, к 8-30", "И.Кант. Глава 1. О пространстве", statusInWork },
                                     new string[] { "18.10.2023, до 22-00", "Пр.р. Консольный калькулятор", statusInWork },
                                     new string[] { "18.10.2023, до 23-59", "Разместить проект в репозитории", statusInWork },
                                     new string[] { "15.10.2023, к 12-00", "Лекции по И. Канту по Философии", statusInWork } };
                addOneDate("12.10.2023", delo1, defs1);
            }
        }

        // Главная функция программы
        static void Main(string[] args)
        {
            if (File.Exists(fileName))
                loadFromFile();
            else
                setDefaults();

            // Готовим настройки
            ConsoleKeyInfo choice;
            Console.CursorVisible = false;
            Console.Clear();

            // Показываем "легенду"
            showLegend();

            do
            {
                // Выводим текущую дату и заметки в ней
                showNote(dateNum);
                choice = Console.ReadKey(true);

                // Проверяем нажатую клавишу и вызываем соответствующие методы
                switch (choice.Key)
                {
                    case ConsoleKey.LeftArrow:   // Предыдущая дата ("влево", "по кругу")
                        prevNote();
                        break;
                    case ConsoleKey.RightArrow:  // Следующая дата ("вправо", "по кругу")
                        nextNote();
                        break;
                    case ConsoleKey.UpArrow:     // Предыдущая заметка ("вверх", "по кругу")
                        prevDelo();
                        break;
                    case ConsoleKey.DownArrow:   // Следующая заметка ("вниз", "по кругу")
                        nextDelo();
                        break;
                    case ConsoleKey.Enter:       // Показать описание заметки 
                        showDetails();
                        break;
                    case ConsoleKey.F3:          // Статус заметки
                        changeDeloStatus();
                        break;
                    case ConsoleKey.F4:          // Добавить заметку
                        addNewNote();
                        break;
                }

            } while (choice.Key != ConsoleKey.Escape); // Выход по клавише "Esc"

            saveToFile();
        } // main()
    }
}