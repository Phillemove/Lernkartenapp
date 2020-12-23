using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.Support;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels
{
    public class ImportViewModel
    {
        public RelayCommand chooseData { get; }
        public RelayCommand importData { get; }
        public string FileName { get; set; }
        private BoxViewModel bvm;
        public Boolean RadioButton1IsChecked { get; set; }
        public Boolean RadioButton2IsChecked { get; set; }
        public ImportViewModel()
        {
            chooseData = new RelayCommand(() => chooseDataMethod());
            importData = new RelayCommand(() => importDataMethod());

        }

        private void importDataMethod()
        {
            if (RadioButton1IsChecked)
            {
                SaveCards.hardSave(this.bvm);
            }
            else if (RadioButton2IsChecked)
            {
                SaveCards.SaveCardsToFile(this.bvm);
            }
            else
            {
                MessageBox.Show("Leider nichts ausgewählt, somit kein Import möglich"); // Vielleicht etwas eleganter mit Auswahl oder so. 
            }
        }

        private void chooseDataMethod()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML-Files|*.xml";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show(ofd.FileName);
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(ofd.FileName);
            this.bvm = new BoxViewModel();
            foreach(XmlNode node in doc.DocumentElement)
            {
                Wrapper.CardViewModel card = readOwnFormatNode(node);
                this.bvm.Enqueue(card);
            }
            foreach(Wrapper.CardViewModel card in this.bvm)
            {
                if(card.AnswerPic != null)
                {
                    string picName = "Test" + ".jpg"; // Name muss noch definiert werden + Überprüfen auf vorhandensein?
                    File.Copy(card.AnswerPic, Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName);
                    card.AnswerPic = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName;
                }
                if (card.QuestionPic != null)
                {
                    string picName = "Test" + ".jpg"; // Name muss noch definiert werden + Überprüfen auf vorhandensein?
                    File.Copy(card.QuestionPic, Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName);
                    card.QuestionPic = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName;
                }

            }

            
            
        }

        public static Wrapper.CardViewModel readOwnFormatNode(XmlNode node)
        {
            Wrapper.CardViewModel card = new Wrapper.CardViewModel();

            foreach (XmlNode child in node)
            {
                switch (child.Name) // Muss noch ausdetailliert werden im Bereich Category und StasticCollection
                {
                    case "Question":
                        card.Question = child.InnerText;
                        break;
                    case "Answer":
                        card.Answer = child.InnerText;
                        break;
                    case "QuestionPic":
                        card.QuestionPic = child.InnerText;
                        break;
                    case "AnswerPic":
                        card.AnswerPic = child.InnerText;
                        break;
                    case "Category":
                        card.Category = new Category(child.InnerText); // Ich glaube nicht ganz korrekt. Immer eine Neue? Oder kann ich eine vorhandene verwenden? Muss das noch abgefragt werden?
                        break;
                    case "StatisticCollection":   //Statistic Collection muss dann immer passend dafür angelegt werden? Ja! Und auch tiefergehend mit allen folgenden Nodes.... Viel Aufwand
                        card.StatisticCollection = new StatisticCollection();
                        foreach(XmlNode statNode in node)
                        {
                            Statistic stat = new Statistic();
                            foreach(XmlNode statDet in statNode) //Wie die Erstellung einzelner Statisticobjekte?
                            {
                                switch (statDet.Name)
                                {
                                    case "Timestamp":
                                        stat.Timestamp = Convert.ToDateTime(statDet.InnerText);
                                        break;
                                    case "SuccessfullAnswer":
                                        stat.SuccessfullAnswer = Convert.ToBoolean(statDet.InnerText);
                                        break;
                                    case "CurrentBoxNumber":
                                        //stat.CurrentBoxNumber = statDet.InnerText;    //Noch zu bearbeiten aufgrund der Enum Problematik
                                        break;
                                }
                            }
                        }
                        break;
                }
            };
            return card;
            
        }

    }

    
}
