using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class StatisticWindowViewModel
    {
        private bool _showDataGrid;
        public RelayCommand ShowHide { get; }
        public StatisticCollectionViewModel StatisticCollectionVM { get; set; }
        public int RightAnswer { get; set; }
        public bool ShowDataGrid
        {
            get { return _showDataGrid; }
        }
        public StatisticWindowViewModel(StatisticCollectionViewModel statisticCollectionVM)
        {
            
            ShowHide = new RelayCommand(() => ShowHideMethod());
            StatisticCollectionVM = statisticCollectionVM;
            RightAnswer = 0;
            _showDataGrid = true;
        }

        private void ShowHideMethod()
        {
            if (_showDataGrid == true)
            {
                _showDataGrid = false;
                
            } else
            {
                _showDataGrid = true;
                
            }
        }

    }
}
