using System.Collections.Generic;

namespace DormApplication
{
    class InfoClass
    {
        private string _btnName;
        private string _category;
        private List<string> _lst;

        public string BtnName { get { return _btnName; } }
        public string Category { get { return _category; } }
        public List<string> Lst { get { return _lst; } }

        public InfoClass(string BtnName, string Category, List<string> Lst)
        {
            _btnName = BtnName;
            _category = Category;
            _lst = Lst;
        }

        public static InfoClass[] Default()
        {
            InfoClass[] info = new InfoClass[5];
            string BtnName;
            string Category;

            //Для кнопки btnEnterPerson
            List<string> Lst_0 = new List<string>();
            BtnName = "btnEnterPerson";
            Category = "Заселение";
            Lst_0.Add("Ввод данных заселяющегося");
            Lst_0.Add("Активация лицевого счета");
            Lst_0.Add("Прикрепление к комнате");
            info[0] = new InfoClass(BtnName, Category, Lst_0);

            //Для кнопки btnTarif
            List<string> Lst_1 = new List<string>();
            BtnName = "btnTarif";
            Category = "Тарифы";
            Lst_1.Add("Добавление тарифа");
            Lst_1.Add("Изменение стоимости проживания");
            info[1] = new InfoClass(BtnName, Category, Lst_1);

            //Для кнопки btnPay - Оплата
            List<string> Lst_2 = new List<string>();
            BtnName = "btnPay";
            Category = "Оплата проживания";
            Lst_2.Add("Ввод данных о проведенном платеже");
            info[2] = new InfoClass(BtnName, Category, Lst_2);

            //Для кнопки btnReport - Отчеты
            List<string> Lst_3 = new List<string>();
            BtnName = "btnReport";
            Category = "Отчеты";
            Lst_3.Add("Просмотр загруженности комнат");
            Lst_3.Add("Печать задолженности проживающих");
            info[3] = new InfoClass(BtnName, Category, Lst_3);

            //Для кнопки btnQuit - Выселение
            List<string> Lst_4 = new List<string>();
            BtnName = "btnQuit";
            Category = "Выселение";
            Lst_4.Add("Смена комнаты");
            Lst_4.Add("Выселение");
            info[4] = new InfoClass(BtnName, Category, Lst_4);

            return info;
        }
    }
}
