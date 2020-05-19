using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressBarDB
{
    public class MyProgressBar
    {

        #region Свойства

        BackgroundWorker worker;

        /// <summary>
        /// Наибольшее значение прогресс бара
        /// </summary>
        public int maxProgressBar { get; private set; }
        /// <summary>
        /// Значение прогресс бара
        /// </summary>
        public int valueProgressBar { get; private set; }

        #endregion

        #region События

        public delegate void Start(int iteration);
        public event Start WorkMethod;
        public event Action WorkCompleted;

        /// <summary>
        /// Запуск прогресс бара
        /// </summary>
        public void StartMethod()
        {
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            WorkCompleted();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            valueProgressBar = e.ProgressPercentage;

        }

        /// <summary>
        /// Метод, который выполняется в работе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            // Количество операций (например, выгрузка из базы данных по 1 элементу)
            for (int i = 0; i < maxProgressBar; i++)
            {
                WorkMethod(i);
                worker.ReportProgress(i + 1, String.Format($"{i + 1}"));
            }
        }


        #endregion



        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="maxProgressBar">Максимальное значение прогресс бара</param>
        public MyProgressBar(int maxProgressBar)
        {
            this.maxProgressBar = maxProgressBar;

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }
    }
}
