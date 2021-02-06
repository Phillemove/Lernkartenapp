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
        public CategoryViewModel SelectedCategory { get; set; }
        public BoxViewModel TempBox { get; set; }

        public RelayCommand CloseWindow { get; }
        public RelayCommand SelectFile { get; }
        public RelayCommand FileImport { get; }
        public RelayCommand CardImport { get; }
        public String ErrorMsg { get; set; }
        public String ImportFile { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        public ImportForeignFormatViewModel(CategoryCollectionViewModel ccvm, BoxViewModel importbox)
        {
            CloseWindow = new RelayCommand(param => Close(param));
            SelectFile = new RelayCommand(() => FileChoose());
            FileImport = new RelayCommand(() => Import());
            CardImport = new RelayCommand(() => ImportCards());
            this.MyCategoryCollection = ccvm;
            this.ImportBox = importbox;
            this.TempBox = new BoxViewModel();
            CardViewModel card = new CardViewModel();
            card.StatisticCollection = new StatisticCollectionViewModel();
            card.Question = "Title";
            card.Answer = "Answer";
            TempBox.Add(card);


            ErrorMsg = "";
            ImportFile = "";
        }

        // Für Änderungen der Fehlermeldung
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void ImportCards()
        {

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
        private void ClearFile()
        {
            ImportFile = "";
            OnPropertyChanged("ImportFile");
        }
        private void ShowFile(String msg)
        {
            ImportFile = msg;
            OnPropertyChanged("ImportFile");
        }

        private String StripHTML(String str)
        {
            str = str.Replace("<br>","\n");

            return str;
        }

        private void ImportCoboCards()
        {
            var reader = System.Xml.XmlReader.Create(ImportFile);
            if (reader.ReadToFollowing("CoboCards"))
            {
                // Ist Cobo Cards Export
                while (reader.ReadToFollowing("card"))
                {
                    if (reader.HasAttributes)
                    {
                        String title = StripHTML(reader.GetAttribute("title"));
                        String content = StripHTML(reader.GetAttribute("content"));
                        if (title != null && content != null)
                        {

                            CardViewModel card = new CardViewModel();
                            card.StatisticCollection = new StatisticCollectionViewModel();
                            card.Question = title;
                            card.Answer = content;
                            TempBox.Add(card);
                            OnPropertyChanged("TempBox");
                            Console.WriteLine(card.Answer);
                            //show cards?
                            // Save Box
                            // Exclude Picture only answers
                        }
                    }
                }
            }
            else
            {
                ShowError("XML File ist kein valider CoboCards Export");
            }
        }


        private void FileChoose()
        {
            ClearError();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CoboCards Export|*.xml| Ansi txt-Export|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                
                if (dialog.FileName.EndsWith(".txt"))
                {
                    ShowFile(dialog.FileName);
                    ShowError("txt nicht implementiert");
                }
                else if (dialog.FileName.EndsWith(".xml"))
                {
                    ShowFile(dialog.FileName);
                }
                else
                {
                    ShowError("Nur .txt und .xml Dateien erlaubt");
                }
            }
            else
            {
                ShowError("Keine Datei ausgewählt");
            }
        }

        private void Import()
        {
            if (ImportFile != "")
            {
                if (SelectedCategory != null)
                {
                    if (ImportFile.EndsWith(".txt"))
                    {

                    }
                    else if (ImportFile.EndsWith(".xml"))
                    {
                        ImportCoboCards();
                    }
                    else
                    {
                        ShowError("Kein valides Dateiformat");
                    }
                }
                else
                {
                    ShowError("Keine Kategorie ausgewählt");
                }
            }
            else
            {
                ShowError("Keine Datei ausgewählt");
            }
        }


    }
}
