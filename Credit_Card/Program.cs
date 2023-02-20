using System;

namespace Credit_Card
{
    class Program
    {
        public delegate void CreditCardEventHandler();

        public class CreditCard
        {
            public delegate void CreditCardEventHandler();
            public string NumberCard { get; set; }
            public string Name { get; set; }
            public DateTime ExpirationDate { get; set; }
            public string Pin { get; set; }
            public double CreditLimit { get; set; }
            public double Balance { get; private set; }

            public event CreditCardEventHandler FundsAdded;
            public event CreditCardEventHandler FundsSpent;
            public event CreditCardEventHandler UseCredit;
            public event CreditCardEventHandler TargetBalanceReached;
            public event CreditCardEventHandler PinChanged;

            public CreditCard(string Numbercard, string cardholderName, DateTime expirationDate, double creditLimit)
            {
                NumberCard = Numbercard;
                Name = cardholderName;
                ExpirationDate = expirationDate;
                CreditLimit = creditLimit;
                Balance = 0;
            }

            public void Init()
            {
                Console.Write("Введите номер карты: ");
                NumberCard = Console.ReadLine();

                Console.Write("Введите ФИО владельца:");
                Name = Console.ReadLine();

                Console.Write("Введите срок действия карты (Пример: 14.07.2024): ");
                string date = Console.ReadLine();
                ExpirationDate.ToString(date);

                Console.Write("Введите PIN: ");
                Pin = Console.ReadLine();

                Console.Write("Введите кредитный лимит: ");
                CreditLimit = Convert.ToDouble(Console.ReadLine());

                Console.Write("Введите ваш баланс: ");
                Balance = Convert.ToDouble(Console.ReadLine());
            }

            public void Refill()
            {
                Console.Write("Введите сумму для пополнения счета -> ");
                double dopsum = Convert.ToDouble(Console.ReadLine());

                Balance += dopsum;
                Console.WriteLine($"Ваш текущий баланс: {Balance}");
                FundsAdded?.Invoke();
            }

            public void Expend()
            {
                Console.Write("Введите сколько денег вы хотите снять -> ");
                double dopmin = Convert.ToDouble(Console.ReadLine());
                if (Balance < dopmin || Balance == 0)
                {
                    Console.WriteLine($"Недостаточно денег\n Ваш баланс составляет: {Balance}");

                    Console.Write("Введите сумму которую вы хотите снять с кредитного лимита -> ");
                    double dopsum = Convert.ToDouble(Console.ReadLine());

                    if (CreditLimit == 0 && CreditLimit < dopsum)
                    {
                        Console.WriteLine($"Вы потратили весь кредитный лимит");
                        return;
                    }
                    CreditLimit -= dopsum;
                }
                else
                {
                    Balance -= dopmin;
                    Console.WriteLine($"Ваш текущий баланс: {Balance}");
                    FundsSpent?.Invoke();
                }

                if (Balance < 0)
                {
                    Console.WriteLine($"Вы использовали кредитные деньги на сумму {Balance * -1}.");
                    UseCredit?.Invoke();
                }
            }

            public void DesiredAmount()
            {
                Console.Write("Введите сумму которую хотите достичь -> ");
                double desired = Convert.ToDouble(Console.ReadLine());

                if (Balance == desired || Balance > desired)
                {
                    Console.WriteLine($"Вы достигли желаемой суммы\n Желаемая сумма: {desired}\t Ваш баланс {Balance}");
                    return;
                }
                Console.WriteLine($"Вы не достигли желаемой суммы\n Желаемая сумма: {desired}\t Ваш баланс {Balance}");
            }
            public void ChangePin()
            {
                Console.WriteLine($"Ваш текущий пароль {Pin}");
                Console.Write($"Ведите новый пароль -> ");
                Pin = Console.ReadLine();
            }

        }
        static void Main(string[] args)
        {
            CreditCard card = new CreditCard("1234567890123456", "Иван Иванов", DateTime.Now.AddYears(2), 10000);

            card.FundsAdded += OnFundsAdded;
            card.FundsSpent += OnFundsSpent;
            card.UseCredit += OnUseCredit;
            card.TargetBalanceReached += OnTargetBalanceReached;
            card.PinChanged += OnPinChanged;

            card.Init();

            while (true)
            {
                Console.WriteLine("1. Пополнить счет");
                Console.WriteLine("2. Снять деньги");
                Console.WriteLine("3. Проверить баланс");
                Console.WriteLine("4. Изменить PIN");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите действие -> ");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        card.Refill();
                        break;
                    case 2:
                        card.Expend();
                        break;
                    case 3:
                        card.DesiredAmount();
                        break;
                    case 4:
                        card.ChangePin();
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Неправильный выбор");
                        break;
                }
            }
        }

        static void OnFundsAdded()
        {
            Console.WriteLine("Счет пополнен");
        }

        static void OnFundsSpent()
        {
            Console.WriteLine("Деньги списаны со счета");
        }

        static void OnUseCredit()
        {
            Console.WriteLine("Вы использовали кредитные деньги");
        }

        static void OnTargetBalanceReached()
        {
            Console.WriteLine("Вы достигли желаемой суммы");
        }

        static void OnPinChanged()
        {
            Console.WriteLine("PIN изменен");
        }

    }
}

