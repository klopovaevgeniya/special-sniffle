using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ca_csExplorer
{
    // *** Класс вывода сообщений
    public static class tMessage
    {
        public static void showMessage(int lx, int ty, string msg, char delim, ConsoleColor color, int width)
        {
            string[] src_rows = msg.Split(delim);
            string[] res_rows = new string[] { };

            for (int i = 0; i < src_rows.Count(); i++)
                if (src_rows[i].Length <= width)
                    res_rows = res_rows.Append(src_rows[i]).ToArray();
                else
                {
                    string[] tmp = src_rows[i].Split(' ');
                    string s = "";
                    for (int j = 0; j < tmp.Count(); j++)
                        if (s.Length + tmp[j].Length <= width)
                            s += tmp[j]+" ";
                        else
                        {
                            res_rows = res_rows.Append(s).ToArray();
                            s = tmp[j]+" ";
                        }
                    res_rows = res_rows.Append(s + " ").ToArray();
                }

            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < res_rows.Count(); i++) {
                Console.SetCursorPosition(lx, ty + i);
                Console.Write(res_rows[i]);
            }
            Console.ReadKey();
            Console.ForegroundColor = oldColor;
            for (int i = 0; i < res_rows.Count(); i++) { 
                Console.SetCursorPosition(lx, ty + i);
                Console.Write("".PadLeft(width, ' '));
            }
        }
    }

    // --- Класс tObjInfo - описание одного объекта в списке
    public class tObjInfo
    {
        public string name;
        public string size;
        public string freeSpace_or_Date;
        public string Label_or_Type;
        public string objType;

        public tObjInfo(string _name, string _size, string _freeSpace_or_Date, string _Label_or_Type, string _objType)
        {
            name              = _name;
            size              = _size;
            freeSpace_or_Date = _freeSpace_or_Date;
            Label_or_Type     = _Label_or_Type;
            objType = _objType;
        }
    }

    // --- Класс tListInfo - описание списка объектов
    public class tListInfo
    {
        public string hdr ="";
        public int[] colWidth = {};
        public tObjInfo[] objects = new tObjInfo[] {};
    }

    // --- Класс tExplorer - управление списком объектов
    public static class tExplorer
    {
        public static string curPath = "";
        static string oldPath = "";
        public static tListInfo curList;
        public static int curItem;
        public const int leftMargin = 85;
        public const int topMargin  = 12;

        // Метод формирования списка объектов
        public static void makeList(string _Path)
        {
            string h;
            string[] colName;
            curList = null;
            curList = new tListInfo();

            if (_Path == "") // Список дисков
            {
                h = "Список дисков";
                colName = new string[] { "Имя диска    ", "    Объем (байт)",
                                         "  Свободно (байт)", "  Метка тома" };
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo d in drives)
                    if (d.IsReady) {
                        tObjInfo tmpObj = new tObjInfo(d.Name.Substring(0, 2),
                                                       (d.TotalSize).ToString(),
                                                       (d.TotalFreeSpace).ToString(),
                                                       d.VolumeLabel,
                                                       "drive");
                        curList.objects = curList.objects.Append(tmpObj).ToArray();
                    }
            }

            else // Список папок и файлов
            {
                h = "Папка: " + _Path;
                colName = new string[] { "          Название            ", "    Размер (байт)",
                                         "    Дата созд./изм.  ", "   Тип " };
                DirectoryInfo dir = new DirectoryInfo(_Path);
                FileInfo[] allfiles = dir.GetFiles();
                DirectoryInfo[] allfolders = dir.GetDirectories();

                foreach (DirectoryInfo d in allfolders) {
                    string dt = d.LastWriteTimeUtc.ToString();
                    if (d.LastWriteTimeUtc.Hour < 10) dt = dt.Replace(" ", " 0");
                    tObjInfo tmpObj = new tObjInfo(d.Name, " ", dt, "Папка","folder");
                    curList.objects = curList.objects.Append(tmpObj).ToArray();
                }
                foreach (FileInfo f in allfiles) {
                    string dt = f.LastWriteTimeUtc.ToString();
                    if (f.LastWriteTimeUtc.Hour < 10) dt = dt.Replace(" ", " 0");
                    tObjInfo tmpObj = new tObjInfo(f.Name, f.Length.ToString(), dt, f.Extension,"file");
                    curList.objects = curList.objects.Append(tmpObj).ToArray();
                }
            }

            foreach (string s in colName) {
                curList.colWidth = curList.colWidth.Append(s.Length).ToArray();
                curList.hdr += s;
            }
            curList.hdr = h.PadLeft(curList.hdr.Length / 2 + h.Length / 2, ' ') + "\n" +
                           "".PadRight(curList.hdr.Length + 2, '-') + "\n  " + curList.hdr;
            //curList.hdr = "  " + curList.hdr;
        }

        // Метод вывода одного объекта списка
        private static void showListItem(int itemNum)
        {
            string s = curList.objects[itemNum].name;
            if (s.Length > curList.colWidth[0] - 2) s = s.Substring(0, curList.colWidth[0] - 5) + "...";
            Console.SetCursorPosition(2, itemNum + 3);
            Console.Write(s.PadRight(curList.colWidth[0], ' '));
            Console.Write(curList.objects[itemNum].size.PadLeft(curList.colWidth[1], ' '));
            Console.Write(curList.objects[itemNum].freeSpace_or_Date.PadLeft(curList.colWidth[2], ' '));
            Console.Write(("  " + curList.objects[itemNum].Label_or_Type).PadRight(curList.colWidth[3], ' '));
            Console.WriteLine();
        }

        // Метод выввода списка объектов
        public static void showList()
        {
            if (oldPath != curPath) {
                oldPath = curPath;
                curItem = 0;
            }
            curList = null;
            makeList(curPath);

            Console.Clear();
            Console.WriteLine(curList.hdr);
            for (int i = 0; i < curList.objects.Length; i++)
                showListItem(i);

            showActions();
        }

        // Метод выбора объекта из списка
        public static bool selObject()
        {
            bool res = false;
            if ((curList.objects[curItem].objType == "folder") || 
                (curList.objects[curItem].objType == "drive"))
                try {
                    Directory.GetFiles(curPath + curList.objects[curItem].name + "\\");
                    curPath += curList.objects[curItem].name + "\\";
                    res = true;
                }
                catch (Exception ex)
                {
                    Console.Beep(550, 30);
                    tMessage.showMessage(leftMargin, topMargin, "ОШИБКА!\n"+
                                                                ex.Message+"\n"+
                                                                "Нажмите любую клавишу...", '\n',
                                                                ConsoleColor.Red,30);
                    showListItem(curItem);
                }
            else
                try {
                    Process.Start(new ProcessStartInfo { FileName = curPath + curList.objects[curItem].name,
                                                         UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    Console.Beep(550, 30);
                    tMessage.showMessage(leftMargin, topMargin, "ОШИБКА!\n" +
                                                                ex.Message + "\n" +
                                                                "Нажмите любую клавишу...", '\n',
                                                                ConsoleColor.Red,30);
                    showListItem(curItem);
                }
            return res;
        }

        // Метод выхода или возврата на предыдущий уровень папки
        public static bool upLevel()
        {
            bool res = false;
            if (curPath == "")
                res = true;
            else {
                string[] tmp = curPath.Split('\\');
                curPath = "";
                for (int i = 0; i < tmp.Length - 2; i++)
                    curPath += tmp[i] + "\\";
            }
            return res;
        }

        public static void showActions()
        {
            Console.SetCursorPosition(leftMargin, 0); Console.Write("Возможные действия:");
            Console.SetCursorPosition(leftMargin, 2); Console.Write("Перемещение       : ↓ ↑ Home End");
            Console.SetCursorPosition(leftMargin, 3); Console.Write("Выбор             : ◄─┘");
            Console.SetCursorPosition(leftMargin, 4); Console.Write(tExplorer.curPath != "" ? "Возврат           : Escape":"Выход           : Escape");
            Console.SetCursorPosition(leftMargin, 6); Console.Write(tExplorer.curPath != "" ? "Создать файл      : Shift-F4" : "");
            Console.SetCursorPosition(leftMargin, 7); Console.Write(tExplorer.curPath != "" ? "Создать папку     : F7" : "");
            Console.SetCursorPosition(leftMargin, 8); Console.Write(tExplorer.curPath != "" ? "Удалить файл/папку: F8" : "");
        }
    }

    // --- Класс tNavigator для управления навигацией
    public static class tNavigator
    {
        private static void showPtr()
        {
            Console.CursorVisible = false;
            if (tExplorer.curItem < 2) Console.SetCursorPosition(0, tExplorer.curItem);
            Console.SetCursorPosition(0, tExplorer.curItem + 3);
            Console.Write("->");
        }

        private static void hidePtr()
        {
            Console.SetCursorPosition(0, tExplorer.curItem + 3);
            Console.Write("  ");
        }

        public static void nextItem()
        {
            hidePtr();
            if (tExplorer.curItem < tExplorer.curList.objects.Count() - 1) tExplorer.curItem++;
            else tExplorer.curItem = 0;
            showPtr();
        }

        public static void prevItem()
        {
            hidePtr();
            if (tExplorer.curItem > 0) tExplorer.curItem--;
            else tExplorer.curItem = tExplorer.curList.objects.Count() - 1;
            showPtr();
        }

        public static void firstItem()
        {
            hidePtr();
            tExplorer.curItem = 0;
            showPtr();
        }

        public static void lastItem()
        {
            hidePtr();
            tExplorer.curItem = tExplorer.curList.objects.Count() - 1;
            showPtr();
        }

    }

    public static class tFileDirManager
    {
        // Метод создания папки
        public static bool createFolder()
        {
            bool res = false;
            string newFolder = "";
            if (tExplorer.curPath != "") // Если не список дисков
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(tExplorer.leftMargin, tExplorer.topMargin);
                Console.Write("Введите имя папки:");
                Console.SetCursorPosition(tExplorer.leftMargin, tExplorer.topMargin + 1);
                newFolder = Console.ReadLine();
                Console.CursorVisible = false;

                try {
                    Directory.CreateDirectory(tExplorer.curPath + "\\" + newFolder);
                    res = true;
                }
                catch (Exception ex)
                {
                    Console.Beep(550, 30);
                    tMessage.showMessage(tExplorer.leftMargin, tExplorer.topMargin,
                                         "ОШИБКА!                   \nПапка не может быть создана!\n" +
                                         ex.Message + "\n" +
                                         "Нажмите любую клавишу...", '\n',
                                         ConsoleColor.Red,30);
                }
            }
            return res;
        }

        // Метод создания файла
        public static bool createFile()
        {
            bool res = false;
            string newFile = "";
            if (tExplorer.curPath != "") // Если не список дисков
            {
                Console.CursorVisible = true;
                Console.SetCursorPosition(tExplorer.leftMargin, tExplorer.topMargin);
                Console.Write("Введите имя файла:");
                Console.SetCursorPosition(tExplorer.leftMargin, tExplorer.topMargin + 1);
                newFile = Console.ReadLine();
                Console.CursorVisible = false;

                if (File.Exists(tExplorer.curPath + "\\" + newFile)) {
                    Console.Beep(550, 30);
                    tMessage.showMessage(tExplorer.leftMargin, tExplorer.topMargin,
                                         "ОШИБКА!\nТакой файл уже существует!\n" +
                                         "Нажмите любую клавишу...", '\n',
                                         ConsoleColor.Red,30);
                }
                else
                    try {
                        FileStream fs = File.Create(tExplorer.curPath + "\\" + newFile);
                        fs.Close();
                        res = true;
                    }
                    catch (Exception ex)
                    {
                        Console.Beep(550, 30);
                        tMessage.showMessage(tExplorer.leftMargin, tExplorer.topMargin,
                                             "ОШИБКА!                   \nФайл не может быть создан!\n" +
                                             ex.Message + "\n" +
                                             "Нажмите любую клавишу...", '\n',
                                             ConsoleColor.Red,30);
                    }
            }
            return res;
        }

        // Метод удаления файла или папки
        public static bool delFileDir()
        {
            bool res = false;
            string objName = tExplorer.curPath + tExplorer.curList.objects[tExplorer.curItem].name;
            string objType = tExplorer.curList.objects[tExplorer.curItem].objType;

            if (objType == "file") // Если файл
                try {
                    File.Delete(objName);
                    res = true;
                }
                catch (Exception ex)
                {
                    Console.Beep(550, 30);
                    tMessage.showMessage(tExplorer.leftMargin, tExplorer.topMargin,
                                         "ОШИБКА!\nФайл не может быть удален!\n" +
                                         ex.Message +"\n"+
                                         "Нажмите любую клавишу...", '\n',
                                         ConsoleColor.Red,30);
                }

            if (objType == "folder") // Если папка
                try {
                    Directory.Delete(objName, false);
                    res = true;
                }
                catch (Exception ex)
                {
                    Console.Beep(550, 30);
                    tMessage.showMessage(tExplorer.leftMargin, tExplorer.topMargin,
                                         "ОШИБКА!\nПапка не может быть удалена!\n" +
                                         ex.Message + "\n" +
                                         "Нажмите любую клавишу...", '\n',
                                         ConsoleColor.Red,30);
                }
            return res;
        }
    }

    // --- Главная программа
    class Program
    {
        static void Main(string[] args)
        {
            bool stop1, stop2;
            ConsoleKeyInfo choice;
            stop1 = false;

            do // Главный цикл программы
            {
                tExplorer.showList();
                tNavigator.firstItem();
                stop2 = false;

                do // Цикл одного списка объектов
                {
                    choice = Console.ReadKey(true);
                    switch (choice.Key)
                    {
                        // Перемещение по списку объектов
                        case ConsoleKey.UpArrow:
                            tNavigator.prevItem();
                            break;
                        case ConsoleKey.DownArrow:
                            tNavigator.nextItem();
                            break;
                        case ConsoleKey.Home:
                            tNavigator.firstItem();
                            break;
                        case ConsoleKey.End:
                            tNavigator.lastItem();
                            break;

                        // Навигация по папкам
                        case ConsoleKey.Enter:
                            stop2 = tExplorer.selObject();
                            break;
                        case ConsoleKey.Escape:
                            stop2 = true;
                            stop1 = tExplorer.upLevel();
                            break;

                        // Управление папками и файлами
                        case ConsoleKey.F4:
                            if (choice.Modifiers == ConsoleModifiers.Shift)
                                stop2 = tFileDirManager.createFile();
                            break;
                        case ConsoleKey.F7:
                            stop2 = tFileDirManager.createFolder();
                            break;
                        case ConsoleKey.F8:
                            stop2 = tFileDirManager.delFileDir();
                            break;
                    }
                } while (!stop2);

            } while (!stop1);
        } // Конец Main
    } // Конец класса Program
}