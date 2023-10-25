using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ca_csCakes
{
    // Класс "Элемент меню"
    public class tMenuItem
    {
        public string itemName;
        public float itemPrice;
        private bool itemSelected;
        public tOneMenu subMenu;

        public tMenuItem(string itName, float itPrice, bool isSelected)
        {
            itemName     = itName;
            itemPrice    = itPrice;
            itemSelected = isSelected;
        }
        public tMenuItem(string itName, tOneMenu sMenu, bool isSel) : this(itName, -1, isSel)
        { subMenu = sMenu; }

        public void selectItem()   { itemSelected = true; }
        public void unselectItem() { itemSelected = false; }
        public bool selected() { return itemSelected; }
    }

    // Класс "Меню" 
    public class tOneMenu
    {
        public string menuHeader;
        public tMenuItem[] items;
        public tOneMenu(string hdr, tMenuItem[] it)
        {
            menuHeader = hdr;
            items      = it;
        }
    }

    // Класс "Полное меню"
    public static class fullMenu
    {
        public static tOneMenu Menu =

            new tOneMenu(
                "Заказ тортов в кондитерской \"МЕЧТА СЛАСТЁНЫ\"\n" +
                "Любые торты на Ваш выбор!\n" +
                "Выберите параметр торта:\n" +
                "--------------------------------------------",
                new tMenuItem[]
                {
                    new tMenuItem(
                        "Форма торта",       // Название пункта главного меню
                        new tOneMenu(
                            "ВИД ФОРМЫ ТОРТА\n"+           // Заголовок подменю
                            "Для выхода нажмите Escape\n"+
                            "Выберите пункт из меню:\n"+
                            "-------------------------",
                            new tMenuItem[]  // Пункты подменю
                            {
                                new tMenuItem("Круг"         ,500, true),
                                new tMenuItem("Квадрат"      ,500, false),
                                new tMenuItem("Прямоугольник",500, false),
                                new tMenuItem("Сердечко"     ,700, false)
                            }
                        ),
                        true),

                    new tMenuItem(
                        "Размер торта",      // Название пункта главного меню
                        new tOneMenu(
                            "РАЗМЕР ТОРТА\n"+              // Заголовок подменю
                            "Для выхода нажмите Escape\n"+
                            "Выберите пункт из меню:\n"+
                            "-------------------------",
                            new tMenuItem[]  // Пункты подменю
                            {
                                new tMenuItem("Маленький (диаметр 16 см, 8 порций)" , 1000, true),
                                new tMenuItem("Средний (диаметр 18 см, 10 порций)", 1200, false),
                                new tMenuItem("Большой (диаметр 28 см, 24 порции)", 2000, false)
                            }
                        ),
                        false),

                    new tMenuItem(
                        "Вкус коржей",       // Название пункта главного меню
                        new tOneMenu(
                            "ВКУС КОРЖЕЙ ТОРТА\n"+         // Заголовок подменю
                            "Для выхода нажмите Escape\n"+
                            "Выберите пункт из меню:\n"+
                            "--------------------------",
                            new tMenuItem[]  // Пункты подменю
                            {
                                new tMenuItem("Ванильный"  , 100, true),
                                new tMenuItem("Шоколадный" , 100, false),
                                new tMenuItem("Карамельный", 150, false),
                                new tMenuItem("Ягодный"    , 200, false),
                                new tMenuItem("Кокосовый"  , 250, false)
                            }
                        ),
                        false),

                    new tMenuItem(
                        "Количество коржей", // Название пункта главного меню
                        new tOneMenu(
                            "КОЛИЧЕСТВО КОРЖЕЙ ТОРТА\n"+   // Заголовок подменю
                            "Для выхода нажмите Escape\n"+
                            "Выберите пункт из меню:\n"+
                            "-------------------------",
                            new tMenuItem[]  // Пункты подменю
                            {
                                new tMenuItem("1 корж" , 200, true),
                                new tMenuItem("2 коржа", 400, false),
                                new tMenuItem("3 коржа", 600, false),
                                new tMenuItem("4 коржа", 800, false)
                            }
                        ),
                        false),

                    new tMenuItem(
                        "Глазурь",           // Название пункта главного меню
                        new tOneMenu(
                            "ТИП ГЛАЗУРИ ТОРТА\n"+         // Заголовок подменю
                            "Для выхода нажмите Escape\n"+
                            "Выберите пункт из меню:\n"+
                            "--------------------------",
                            new tMenuItem[]  // Пункты подменю
                            {
                                new tMenuItem("Шоколадная", 150, true),
                                new tMenuItem("Ягодная"   , 150, false),
                                new tMenuItem("Кремовая"  , 150, false)
                            }
                        ),
                        false),

                    new tMenuItem(
                        "Декор",             // Название пункта главного меню
                        new tOneMenu(
                            "ВИД ДЕКТОРА ТОРТА\n"+         // Заголовок подменю
                            "Для выхода нажмите Escape\n"+
                            "Выберите пункт из меню:\n"+
                            "-------------------------",
                            new tMenuItem[]  // Пункты подменю
                            {
                                new tMenuItem("Шоколад", 100, true),
                                new tMenuItem("Крем"   , 100, false),
                                new tMenuItem("Бизе"   , 150, false),
                                new tMenuItem("Драже"  , 150, false),
                                new tMenuItem("Ягоды"  , 200, false)
                            }
                        ),
                        false),

                    new tMenuItem("Конец заказа",-2, false)
                     }
                );
        
        private static void clearMenuArea()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 20; i++)
                Console.WriteLine("                                                ");
            Console.SetCursorPosition(0, 0);
        }
        public static void showMenu(tOneMenu curMenu)
        {
            clearMenuArea();
            Console.WriteLine(curMenu.menuHeader);
            for (int i = 0; i < curMenu.items.Length; i++)
                Console.WriteLine( (curMenu.items[i].selected() ? "─►" : "  ") + 
                                   curMenu.items[i].itemName +
                                   (curMenu.items[i].itemPrice > 0 ? " - "+ curMenu.items[i].itemPrice.ToString()+" р.": "")
                                  );
        }
    }

    // Класс "Заказ торта"
    public class tCakeOrder : Object
    {
        tOneMenu curMenu;
        int curItem;
        public bool stopProgram = false;
        float[]  orderParamsPrices = { 0,0,0,0,0,0 };
        string[] orderParamsNames  = { "", "", "", "", "", "" };
        const string orderFileName = "История заказов.txt";

        public tCakeOrder(tOneMenu initMenu)
        { 
            curMenu = initMenu;
            curItem = getSelectedIndex(fullMenu.Menu);
            showOrder();
        }

        public void nextItem()
        {
            curMenu.items[curItem].unselectItem();
            if (curItem == curMenu.items.Length - 1)
                curItem = 0;
            else
                curItem++;
            curMenu.items[curItem].selectItem();
        }

        public void prevItem()
        {
            curMenu.items[curItem].unselectItem();
            if (curItem == 0)
                curItem = curMenu.items.Length - 1;
            else
                curItem--;
            curMenu.items[curItem].selectItem();
        }

        public void showMenu()
        {
            fullMenu.showMenu(curMenu);
        }

        private void showOrder()
        {
            Console.SetCursorPosition(52, 0);
            Console.WriteLine("Текущий заказ");
            Console.SetCursorPosition(52, 1);
            Console.WriteLine("-------------");

            float TotSumm = 0;
            for(int i=0; i<fullMenu.Menu.items.Length-1; i++)
            {
                Console.SetCursorPosition(52, i + 2);
                Console.Write((fullMenu.Menu.items[i].itemName+": "+
                              orderParamsNames[i]).PadRight(50)+
                              " - "+orderParamsPrices[i].ToString().PadLeft(5) + " р.");
                TotSumm += orderParamsPrices[i];
            }
            Console.SetCursorPosition(52, 10);
            Console.Write("Итого на сумму:     " + TotSumm.ToString().PadLeft(5) + " р.");
        }

        private int getSelectedIndex(tOneMenu someMenu)
        {
            int res = -1;
            for (int i = 0; i < someMenu.items.Length; i++)
                if (someMenu.items[i].selected())
                {
                    res = i;
                    break;
                }
            return res;
        }

        public void selectItem()
        {
            if (curMenu.items[curItem].itemPrice == -2)
            // это пункт главного меню "Конец заказа"
            {
                stopProgram = true;
                return;
            }

            if (curMenu.items[curItem].subMenu != null)
            // это когда нажат Enter и мы - в главном меню
            {
                curMenu = curMenu.items[curItem].subMenu;
                curItem = getSelectedIndex(curMenu);
            }
            else
            // это когда нажат Enter и мы - в подменю
            {
                orderParamsPrices[getSelectedIndex(fullMenu.Menu)] = curMenu.items[curItem].itemPrice;
                orderParamsNames[getSelectedIndex(fullMenu.Menu)]  = curMenu.items[curItem].itemName;
                showOrder();
            }
        }

        public void returnToMainMenu()
        {
            curMenu = fullMenu.Menu;
            curItem = getSelectedIndex(fullMenu.Menu);
        }

        public void saveOrder()
        {
            try
            {
                StreamWriter sw = new StreamWriter(orderFileName, true);
                sw.WriteLine("Заказ от "+DateTime.Now);
                sw.Write("\tЗаказ:");

                float TotSumm = 0;
                for (int i = 0; i < fullMenu.Menu.items.Length - 1; i++)
                {
                    sw.Write("\t"+ (fullMenu.Menu.items[i].itemName + ": " +
                             orderParamsNames[i]).PadRight(50) +
                             " - " + orderParamsPrices[i].ToString().PadLeft(5) + " р.\n\t");
                    TotSumm += orderParamsPrices[i];
                }
                sw.WriteLine("Цена:\t" + TotSumm.ToString() + " р.\n");
                sw.Close();
            }
            catch (Exception e)
            {
                Console.SetCursorPosition(0, 14);
                Console.WriteLine("Заказ не сохранен в истории заказов!\nПри сохранении заказа в файл возникла ошибка");
                Console.ReadKey(true);
                Console.SetCursorPosition(0, 14);
                Console.WriteLine("                                    \n                                            ");
            }
        }
    }

    // Главная программа
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = false;
            ConsoleKeyInfo choice;
            tCakeOrder curOrder = new tCakeOrder(fullMenu.Menu); ;

            do
            {
                do
                {
                    curOrder.showMenu();
                    choice = Console.ReadKey(true);

                    // Проверяем нажатую клавишу и вызываем соответствующие методы
                    switch (choice.Key)
                    {
                        case ConsoleKey.UpArrow:     // Предыдущий пункт меню ("вверх", "по кругу")
                            curOrder.prevItem();
                            break;

                        case ConsoleKey.DownArrow:   // Следующий пункт меню ("вниз", "по кругу")
                            curOrder.nextItem();
                            break;

                        case ConsoleKey.Enter:       // Выбрать пункт меню
                            curOrder.selectItem();
                            break;

                        case ConsoleKey.Escape:       // Вернуться в главное меню
                            curOrder.returnToMainMenu();
                            break;
                    }
                } while (!curOrder.stopProgram); // Конец заказа

                curOrder.saveOrder();

                Console.SetCursorPosition(0, 14);
                Console.Write("Спасибо за заказ! Если Вы хотите сделать еще один заказ, нажмите Escape");
                choice = Console.ReadKey(true);
                Console.SetCursorPosition(0, 14);
                Console.Write("                                                                       ");

                if (choice.Key == ConsoleKey.Escape)
                {
                    curOrder.stopProgram = false;
                    curOrder = null;
                    curOrder = new tCakeOrder(fullMenu.Menu);
                }

            } while (!curOrder.stopProgram);

            Console.WriteLine("\n\nПрограмма завершена...");
            Console.ReadKey();
        }
    }

}