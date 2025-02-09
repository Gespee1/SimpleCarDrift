
using System;
using System.Data;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        // Информация о владении авто
        public bool[] carPurchases = new bool[53];
        public int[] carColors = new int[53];
        public int[] carRims = new int[53];
        public int selectedCar;

        // Статы игрока
        public int highscore = 0;
        public int money = 1000;
        public long lastRewardGotTime = 0;

        // Ежедневные награды
        public bool[] dailyRewardGot = new bool[6];

        // Настройки
        public float music;
        public float sound;
        public bool firstRideDone,
                    secondRideDone,
                    disableAds,
                    isDay;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
            openLevels[1] = true;
            /////////

            music = 0.1f;
            sound = 0.1f;

            carPurchases[0] = true;
            isDay = true;
        }
    }
}
