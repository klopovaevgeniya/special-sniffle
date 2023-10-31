using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ca_csModelEditor
{
    // Класс модель
    public class tCarModel
    {
        const int modelFieldsCount = 5;
        public string brand;
        public string model;
        public string color;
        public int prodYear;
        public int price;

        public tCarModel() { }
        public tCarModel(string carBrand, string carModel, int carPrice, int carYear, string carColor)
        {
            brand = carBrand;
            model = carModel;
            color = carColor;
            prodYear = carYear;
            price = carPrice;
        }
    }

    // Класс редактор моделей
    public class tModelEditor
    {
        public tCarModel[] models;
        string fileName = "";

        // Метод загрузки данных из текстового файла
        private void loadTxt()
        {
            if (File.Exists(fileName))
            {
                string[] tmp = File.ReadAllText(fileName).Replace("\r","").Split('\n');
                int modelCount = tmp.Length / 5;

                string _brand;
                string _model;
                int _price;
                int _prodYear;
                string _color;

                for (int i=1; i<=tmp.Length/5; i++)
                {
                    _brand = tmp[(i-1)*5];
                    _model = tmp[(i-1)*5+1];
                    _price    = Convert.ToInt32(tmp[(i-1)*5+2]);
                    _prodYear = Convert.ToInt32(tmp[(i-1)*5+3]);
                    _color = tmp[(i-1)*5+4];

                    tCarModel tmpModel = new tCarModel(_brand, _model, _price, _prodYear, _color);
                    models = models.Append(tmpModel).ToArray();
                }
            }
        }

        // Метод сохранения данных в текстовый файл
        private void saveTxt()
        {
            string tmp = "";
            for (int i = 0; i < models.Length; i++)
            {
                tmp += models[i].brand + "\n";
                tmp += models[i].model + "\n";
                tmp += models[i].price.ToString() + "\n";
                tmp += models[i].prodYear.ToString() + "\n";
                tmp += models[i].color + (i < models.Length - 1 ? "\n" : "");
            }
            File.WriteAllText(fileName, tmp);
        }

        // Метод загрузки данных из XML-файла
        private void loadXML()
        {
            XmlSerializer xml = new XmlSerializer(typeof(tCarModel[]));
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                models = (tCarModel[])xml.Deserialize(fs);
                fs.Close();
            }
        }

        // Метод сохранения данных в XML-файл
        private void saveXML()
        {
            XmlSerializer xml = new XmlSerializer(typeof(tCarModel[]));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, models);
                fs.Close();
            }
        }

        // Метод сохранения данных в JSON-файл
        private void saveJSON()
        {
            string s = JsonConvert.SerializeObject(models.ToList());
            File.WriteAllText(fileName, s);
        }

        // Метод загрузки данных из JSON-файла
        private void loadJSON()
        {
            string s = File.ReadAllText(fileName);
            models = JsonConvert.DeserializeObject<tCarModel[]>(s);
        }

        // Метод загрузки данных из файла
        public void loadFile()
        {
            models = new tCarModel[] { };
            string ext = Path.GetExtension(fileName).ToUpper();
            if (ext == ".TXT")
                loadTxt();
            else if (ext == ".JSON")
                loadJSON();
            else if (ext == ".XML")
                loadXML();
        }

        // Метод сохранения данных в файл
        public void saveFile()
        {
            string ext = Path.GetExtension(fileName); ;
            if (ext.ToUpper() == ".TXT")
                saveTxt();
            else if (ext.ToUpper() == ".JSON")
                saveJSON();
            else if (ext.ToUpper() == ".XML")
                saveXML();
            models = null;
        }

        // Метод ввода имени файла
        public void getFileName(string mode)
        {
            Console.Clear();
            Console.CursorVisible = true;
            string res = "";
            if (mode == "load")
                Console.WriteLine("Введите путь до файла (вместе с именем), который Вы хотите открыть:\n" +
                                  "-------------------------------------------------------------------");
            else if (mode == "save")
            Console.WriteLine("Введите путь до файла (вместе с названием), куда Вы хотите сохранить данные:\n" +
                              "----------------------------------------------------------------------------");
            res = Console.ReadLine();
            res = res.TrimStart(' ').TrimEnd(' ');
            fileName = res;
        }

        // Метод отображения содержимого файла
        public void showContent()
        {
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine("Сохранить файл в одном из трёх форматов (txt, json, xml) - F1. Закрыть программу - Escape\n" +
                              "-----------------------------------------------------------------------------------------");
            for (int i=0; i<models.Length; i++)
            {
                Console.WriteLine(models[i].brand);
                Console.WriteLine(models[i].model);
                Console.WriteLine(models[i].price.ToString());
                Console.WriteLine(models[i].prodYear.ToString());
                Console.WriteLine(models[i].color);
            }
        }

    }


    // Главная программа
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo choice;
            tModelEditor editor = new tModelEditor();

            do
            {
                editor.getFileName("load");
                editor.loadFile();
                editor.showContent();

                do { choice = Console.ReadKey(true);
                } while ((choice.Key != ConsoleKey.F1) && (choice.Key != ConsoleKey.Escape));

                if (choice.Key == ConsoleKey.F1)
                {
                    editor.getFileName("save");
                    editor.saveFile();
                }

            } while (choice.Key != ConsoleKey.Escape);

            Console.WriteLine("\n\nПрограмма завершена...\nНажмите любую клавишу...");
            Console.ReadKey(true);

        } // end Main()
    }
}