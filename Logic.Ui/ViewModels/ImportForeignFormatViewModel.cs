using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ImportForeignFormatViewModel : INotifyPropertyChanged
    {
        public CategoryCollectionViewModel MyCategoryCollection { get; set; }
        public Boxnumber ImportBox { get; }
        public BoxCollectionViewModel BoxCollection { get; }

        public CategoryViewModel SelectedCategory { get; set; }
        public BoxViewModel TempBox { get; set; }

        public RelayCommand CloseWindow { get; }
        public RelayCommand SelectFile { get; }
        public RelayCommand FileImport { get; }
        public RelayCommand CardImport { get; }
        public RelayCommand CardClear { get; }
        public String ImportLog { get; set; }
        public String ErrorMsg { get; set; }
        public String ImportFile { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        public ImportForeignFormatViewModel(
            CategoryCollectionViewModel ccvm, 
            Boxnumber importbox, 
            BoxCollectionViewModel bcvm)
        {
            CloseWindow = new RelayCommand(param => Close(param));
            SelectFile = new RelayCommand(() => FileChoose());
            FileImport = new RelayCommand(() => Import());
            CardImport = new RelayCommand(() => ImportCards());
            CardClear = new RelayCommand(() => ClearCards());
            this.BoxCollection = bcvm;
            this.MyCategoryCollection = ccvm;
            this.ImportBox = importbox;
            this.TempBox = new BoxViewModel();

            ImportLog = "";
            ErrorMsg = "";
            ImportFile = "";
        }

        // Gets Called to inform the view of changes
        // in Properties which are bound in the view
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, 
                new PropertyChangedEventArgs(propertyName));
        }

        // Imports the Cards saved in the TempBox, clears Tempbox and view Fields
        private void ImportCards()
        {
            foreach (CardViewModel card in TempBox)
            {
                BoxCollection.storeCard(card,ImportBox);
            }
            Support.SaveCards.SaveBoxToFileSystem(TempBox);
            ClearCards();
        }

        // Clears TempBox, Log and Filechoose Field
        private void ClearCards()
        {
            TempBox.Clear();
            ClearFile();
            ClearLog();
        }

        // Clears Log and informs the view about the change
        private void ClearLog()
        {
            ImportLog = "";
            OnPropertyChanged("ImportLog");
        }

        // Closes this window
        private void Close(object param)
        {
            Window window = (Window)param;
            window.Close();
        }

        // Clears Error Message and informs the view about change
        private void ClearError()
        {
            ErrorMsg = "";
            OnPropertyChanged("ErrorMsg");
        }

        // Shows given error message in Error field and informs the view
        private void ShowError(String msg)
        {
            ErrorMsg = msg;
            OnPropertyChanged("ErrorMsg");
        }

        // Clears the filechoose field and informs the view
        private void ClearFile()
        {
            ImportFile = "";
            OnPropertyChanged("ImportFile");
        }

        // Displays given Filename in the filechoose field and informs the view
        private void ShowFile(String msg)
        {
            ImportFile = msg;
            OnPropertyChanged("ImportFile");
        }

        // Strips/Replaces common HTML Elements from given String
        // TODO: Does work for testet Import Files, but needs further
        // improvements for future imports most likely
        private String StripHTML(String str)
        {
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("<br>","\n");
            str = str.Replace("<br />", "\n");
            str = str.Replace("<div>", "");
            str = str.Replace("</div>", "");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&amp;", "&");
            str = Regex.Replace(str, "<img[^>]+>", "");

            return str;
        }

        // Adds given question and answer to log
        private void AddCardToLog(String question,String answer)
        {
            ImportLog = ImportLog + "Frage:\n" + 
                question + "\n\nAntwort:\n" + answer + "\n---------\n";
        }

        // Method to import CoboCards
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
                        String title = StripHTML(
                            reader.GetAttribute("title"));
                        String content = StripHTML(
                            reader.GetAttribute("content"));
                        if (title != null && content != null)
                        {
                            CardViewModel card = new CardViewModel();
                            card.StatisticCollection = 
                                new StatisticCollectionViewModel();
                            card.Question = title;
                            card.Answer = content;
                            card.Category = SelectedCategory;
                            TempBox.Add(card);
                            AddCardToLog(title, content);
                        }
                    }
                }
                OnPropertyChanged("ImportLog");
            }
            else
            {
                ShowError("XML File ist kein valider CoboCards Export");
            }
        }

        // Imports Anki txt Export Files
        // Remark: txt File cant be checked if its really anki export
        // So log needs to be checked if imported questions make sense
        private void ImportAnkiTxt()
        {
            StreamReader stream = new StreamReader(ImportFile);
            while (!stream.EndOfStream)
            {
                String line = stream.ReadLine();
                String[] values = line.Split('\t');
                if (values[0] != "" && values[1] != "")
                {
                    String question = StripHTML(values[0]);
                    String answer = StripHTML(values[1]);
                    if (question != "" && answer != "" && 
                        answer != "\"\"" && question != "\"\"")
                    {
                        CardViewModel card = new CardViewModel();
                        card.StatisticCollection = 
                            new StatisticCollectionViewModel();
                        card.Question = question;
                        card.Answer = answer;
                        card.Category = SelectedCategory;
                        TempBox.Add(card);
                        AddCardToLog(question, answer);
                    }
                }
            }
            OnPropertyChanged("ImportLog");
        }

        // Displays File choose Dialog
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

        // Imports Cards from chosen file, checks Filetype to know
        // which method to call
        // TODO: Future Improvements could be a better File verification
        // like XML check against DTD if more XML Formats than CoboCards
        // needs to be supportet
        private void Import()
        {
            if (ImportFile != "")
            {
                if (SelectedCategory != null)
                {
                    if (ImportFile.EndsWith(".txt"))
                    {
                        ImportAnkiTxt();
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
