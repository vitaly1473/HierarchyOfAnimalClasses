using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using Microsoft.Extensions.DependencyInjection;// добавили, предварительно установив через Проект/Управление пакетами NuGet
/*Вся основная функциональность внедрения зависимостей в .NET расположена в пакете Microsoft.Extensions.DependencyInjection. 
 Стоит отметить, что в проект консольного приложения, а также в ряд других типов проектов этот пакет по умолчанию НЕ установлен. 
 Поэтому нам надо предварительно установить через Nuget данный пакет.*/

namespace HierarchyOfAnimalClasses
{
    // интерфейс для фабрики животных
    public interface IAnimalFactory
    {
        Cat CreateCat(string name, int age, int ears, int paws, int tail);
        Bird CreateBird(string name, int age, string wings, string tail);
    }
    //  фабрика животных
    public class AnimalFactory : IAnimalFactory
    {
        public Cat CreateCat(string name, int age, int ears, int paws, int tail)
        {
            return new Cat { Name = name, Age = age, Ears = ears, Paws = paws, Tail = tail };
        }

        public Bird CreateBird(string name, int age, string wings, string tail)
        {
            return new Bird { Name = name, Age = age, Wings = wings, Tail = tail };
        }       
    }

    //интерфейс для получения имя, возраст, методы Sound, Info 
    internal interface IAnimal
    {
        string Name { get; set; }
        int Age { get; set; }
        void Sound();
        void Info();
      
    }
    // интерфейс для записи в файл
    internal interface IOutput
    {
        void OutputFile();
    }

    // абстрактный класс Animal, наследует интерфейсы IAnimal, IOutput
    public abstract class Animal : IAnimal, IOutput
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public abstract void Sound();
        public abstract void Info();
      
        //метод для получения информации 
        protected abstract string GetInfo();

        public void OutputFile()
        {
            string fileName = $"{Name} info.txt";
            string info = GetInfo();

            try
            {
                File.WriteAllText(fileName, info);
                Console.WriteLine($"Информация записана в файл {fileName}"); // пишем в папку проекта bin/debag
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи.");
            }
        }       
    }

    // класс Bird, наследует от класса Animal
     public class Bird : Animal
    {
        public string Wings { get; set; }
        public string Tail { get; set; }

        public override void Sound()
        {
            //Console.WriteLine("Звук: каню...каню...");
            Console.WriteLine("Звук: фьють...фьють...");
        }
        public override void Info()
        {
            Console.WriteLine($"Имя {Name}, лет {Age}, {Wings}, Хвост: {Tail}");
        }
        protected override string GetInfo()
        {
            return $"Имя: {Name}\nВозраст: {Age}\n {Wings}\nХвост: {Tail}";
        }
    }
    //класс Cat, наследует от класса Animal
    public class Cat : Animal
    {
        public int Ears { get; set; }
        public int Paws { get; set; }
        public int Tail { get; set; }
       

        public override void Sound()
        {
            Console.WriteLine("Звук: Мяу!");
        }
        public override void Info()
        {
            Console.WriteLine($"Вид {Name}, лет {Age}, Уши: {Ears}, Лапы: {Paws}, Хвост: {Tail}");
        }
        protected override string GetInfo()
        {
            return $"Имя: {Name}\nВозраст: {Age} \nУшей: {Ears}\nЛап: {Paws} \nХвост: {Tail}";
        }
    }

    internal class Program
    {
        static void Test1()
        {
            // создание объектов животных
            Cat cat = new Cat { Name = "Сибирская", Age = 2, Ears = 2, Paws = 4, Tail = 1 };
            Bird bird = new Bird
            {
                Name = "Канюк (Сарыч)",
                Age = 2,
                Tail = "Хвост с частыми узкими поперечными полосами, по краю иногда более широкая темная полоса.",
                Wings = "У летящей птицы снизу широких крыльев видна светлая полоса"
            };

            cat.Info();
            cat.Sound();
            cat.OutputFile();

            bird.Info();
            bird.Sound();
            bird.OutputFile();
        }
        static void Main(string[] args)
        {            
             // Test1();          

            // 2 способ: 
            Console.WriteLine("Метод внедрения зависимостей. Используем принципы внедрения зависимостей (Dependency Injection) между классами: \n");
            
            var services2 = new ServiceCollection().AddTransient<IAnimalFactory, AnimalFactory>() .BuildServiceProvider();

            // получаем фабрику
            var animalFactory = services2.GetService<IAnimalFactory>();

            // создаем объекты с помощью фабрики
            var cat3 = animalFactory.CreateCat("Британская короткошерстная", 2, 2, 4, 1);
            var bird3 = animalFactory.CreateBird("Соловей", 2, "Крылышки маленькие", "Хвост серый");

            // выводим динамически на экран или запись в файл
            while (true)
            {
                Console.WriteLine("Выберите способ вывода информации:");
                Console.WriteLine("1 - Вывод на консоль");
                Console.WriteLine("2 - Запись в файл");
                Console.WriteLine("3 - Выход");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("---------------------------- \n");
                        cat3.Info();
                        cat3.Sound();
                        Console.WriteLine("---------------------------- \n");
                        bird3.Info();
                        bird3.Sound();
                        break;

                    case "2":
                        Console.WriteLine("---------------------------- \n");
                        cat3.OutputFile();
                        bird3.OutputFile();
                        break;

                    case "3":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор, попробуйте еще раз.");
                        break;
                }
                Console.ReadLine();
            }               
        }
    }
}
// 197 строк кода
