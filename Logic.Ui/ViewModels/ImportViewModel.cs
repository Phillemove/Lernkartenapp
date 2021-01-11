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
        //private readonly string saveDirectory = @"..\..\..\Lernkarten\";  // Soweit derzeit nicht nötig. Nur für ggf. zu erstellenden Ordnern, wobei die grundlegend vorhanden sein sollten
        private readonly string pictureDirectory = @"..\..\..\Lernkarten\content\";
        public Boolean RadioButtonNewCatIsChecked { get; set; }
        public Boolean RadioButtonExistentCatIsChecked { get; set; }
        public ImportViewModel()
        {
            chooseData = new RelayCommand(() => chooseDataMethod());
            importData = new RelayCommand(() => importDataMethod());

        }

        /*
         * Diese Methode speichert die Dateien entweder unter einem neuen Kategorienamen ab oder fügt diese zu einer
         * bestehenden Kategorie hinzu
         */
        private void importDataMethod()
        {
            if (RadioButtonNewCatIsChecked)
            {
                MessageBox.Show("Also eine neue Kategorie soll angelegt werden, soso");
                //SaveCards.hardSave(this.bvm);   // Wichtig! Überprüfen, ob es schon solch eine Kategorie gibt. Wenn ja, werden die Karten der anderen Kategorie hinzugefügt.
            }
            else if (RadioButtonExistentCatIsChecked)
            {
                MessageBox.Show("Ja da sollten wohl noch ein paar Karten zur Kategorie dazu.");
                //SaveCards.SaveCardsToFile(this.bvm);
            }
            else
            {
                MessageBox.Show("Leider nichts ausgewählt, somit kein Import möglich"); // Vielleicht etwas eleganter mit Auswahl oder so. 
            }
        }
        /*
         * Diese Methode ist zum auswählen der Datei da. Damit kann man aus seiner Dateistruktur eine .xml Datei auswählen, welche dann eingelesen und importiert werden soll.
         */
        private void chooseDataMethod()
        {
            OpenFileDialog ofd = new OpenFileDialog();  // Ein OpenFileDialog wird erstellt, durch das die Datei ausgewählt weden kann
            ofd.Filter = "XML-Files|*.xml";     // Begrenzung der angezeigten Dateien auf .xml Dateien
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) // Wenn die Auswahl ohne Problme von statten ging
            {
                MessageBox.Show(ofd.FileName);  // Anzeigen des Dateinamen (Nur erst mal Intern zur Kontrolle)
            }
            string filepath = Path.GetDirectoryName(ofd.FileName);
            XmlDocument doc = new XmlDocument();    // Ein neues XmlDocument wird erstellt, in das dann die zu importierende Datei geladen wird.
            doc.Load(ofd.FileName);
            this.bvm = new BoxViewModel();  // Ein BoxViewModel, in das die zu importierenden Karten geladen werden sollen
            foreach(XmlNode node in doc.DocumentElement)
            {
                CardViewModel card = readOwnFormatNode(node);
                this.bvm.Enqueue(card);
            }
            foreach(CardViewModel card in this.bvm) // DIese Schleife kopiert ggf. die Bilder an den neuen Ort und speichert den neuen Dateipfad
            {
                if(card.AnswerPic != null)
                {
                    string picName = "Test" + ".jpg"; // Name muss noch definiert werden + Überprüfen auf vorhandensein?
                    //File.Copy(card.AnswerPic, Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName);
                    File.Copy(filepath + @"\content\" + card.AnswerPic, pictureDirectory + picName);
                    //card.AnswerPic = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName;
                    card.AnswerPic = pictureDirectory + picName;
                }
                if (card.QuestionPic != null)
                {
                    string picName = "Test2" + ".jpg"; // Name muss noch definiert werden + Überprüfen auf vorhandensein?
                    //File.Copy(card.QuestionPic, Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName);
                    File.Copy(filepath + @"\content\" + card.QuestionPic, pictureDirectory + picName);
                    //card.QuestionPic = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content\" + picName;
                    card.QuestionPic = pictureDirectory + picName;
                }
            } 
        }
        /*
         * Diese Methode kann unser eigenes Xml-Schema lesen und aufgrund dessen neue Karten erstellen.
         * Übergeben werden muss hier eine XmlNode und Rückgabe wird durch eine Karte realisiert.
         */
        public static CardViewModel readOwnFormatNode(XmlNode node)
        {
            CardViewModel card = new CardViewModel();   // Erstellung der Karte, welche zurückgegeben wird.

            foreach (XmlNode child in node)
            {
                /*
                 * In diesem Switch-Case wird immer geschaut, welche Node gerade vorhanden ist und der dementsprechende Wert dann der passenden Kartenstelle zugeschrieben.
                 */
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
                        card.StatisticCollection = new StatisticCollection();   // Wird noch zu einer StatisticCollectionViewModel
                        /*
                         * Durch diese Schleife wird jedes Statistic Object einzeln durchgegangen.
                         */
                        foreach(XmlNode statNode in node) 
                        {
                            Statistic stat = new Statistic(); // Es wird eine neue Statistik angelegt
                            /*
                             * Durch diese Schleife wird jede Node innerhalb einer Statistik-Node durchgegangen.
                             * Hier verbergen sich die dementsprechenden Werte, welche zum erstellen der Statistik anlagen.
                             */
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
                            card.StatisticCollection.Add(stat); // Das gerade erzeugte Statistic-Objekt wird der Collection hinzugefügt.
                        }
                        break;
                }
            };
            return card;
            
        }

    }

    
}
