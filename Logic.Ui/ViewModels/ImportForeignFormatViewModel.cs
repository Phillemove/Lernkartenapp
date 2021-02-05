using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ImportForeignFormatViewModel : INotifyPropertyChanged
    {
        public CategoryCollectionViewModel MyCategoryCollection { get; set; }
        public BoxViewModel ImportBox { get; }

        public RelayCommand CloseWindow { get; }
        public RelayCommand SelectFile { get; }
        public String ErrorMsg { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        public ImportForeignFormatViewModel(CategoryCollectionViewModel ccvm, BoxViewModel importbox)
        {
            CloseWindow = new RelayCommand(param => Close(param));
            SelectFile = new RelayCommand(() => FileChoose());
            this.MyCategoryCollection = ccvm;
            this.ImportBox = importbox;
            ErrorMsg = "";
        }

        // Für Änderungen der Fehlermeldung
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }



        private void Close(object param)
        {
            // Save Box
            Window window = (Window)param;
            window.Close();
        }

        private void ClearError()
        {
            ErrorMsg = "";
            OnPropertyChanged("ErrorMsg");
        }
        private void ShowError(String msg)
        {
            ErrorMsg = msg;
            OnPropertyChanged("ErrorMsg");
        }

        private void FileChoose()
        {
            ClearError();
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {
                ShowError("Keine Datei ausgewählt");
            }
        }
    }
}
