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
        public CategoryViewModel Class { get; set; }

        public BoxViewModel bvm;
        private static Random random = new Random();
        private readonly static int randPicNameLength = 10;
        private OpenFileDialog ofd;
        private static string filepath;

        public CategoryCollectionViewModel myModelViewModel { get; set; }

        public String newClassName { get; set; }

        private readonly static string pictureDirectory = @"..\..\..\Lernkarten\content\";
        public Boolean RadioButtonNewCatIsChecked { get; set; }
        public Boolean RadioButtonExistentCatIsChecked { get; set; }
        public ImportViewModel()
        {
            chooseData = new RelayCommand(() => chooseDataMethod());
            importData = new RelayCommand(() => importDataMethod());

        }

        public ImportViewModel(CategoryCollectionViewModel categorys)
        {
            chooseData = new RelayCommand(() => chooseDataMethod());
            importData = new RelayCommand(() => importDataMethod());
            myModelViewModel = categorys;
        }
        /*
         * Diese Methode speichert die Dateien entweder unter einem neuen Kategorienamen ab oder fügt diese zu einer
         * bestehenden Kategorie hinzu
         */
        private void importDataMethod()
        {
            if(!RadioButtonNewCatIsChecked && !RadioButtonExistentCatIsChecked)
            {
                MessageBox.Show("Leider nichts ausgewählt, somit kein Import möglich"); // Vielleicht etwas eleganter mit Auswahl oder so.
            }
            else
            {
                
                foreach (CardViewModel card in this.bvm)
                {
                    if (card.AnswerPic != null)
                    {
                        string newFile = copyPic(card.AnswerPic);
                        card.AnswerPic = newFile;
                    }
                    if (card.QuestionPic != null)
                    {
                        string newFile = copyPic(card.QuestionPic);
                        card.QuestionPic = newFile;
                    }
                }
                if (RadioButtonNewCatIsChecked) 
                {
                    Category cat = new Category(System.IO.Path.GetFileNameWithoutExtension(ofd.FileName));
                    foreach (CardViewModel card in this.bvm)
                    {
                        card.Category = cat; // Zuweisung der Kategorie einer jeden Karte aus Dteiname
                    }
                    myModelViewModel.categoryCollection.Add(cat);
                    SaveCards.hardSave(this.bvm); 
                }
                else 
                {
                    Category cat = new Category(Class.Name);
                    foreach (CardViewModel card in this.bvm)
                    {
                        card.Category = cat;    // Zuweisung der Kategorie einer jeden Karte aus vorhandenen Kategorien
                    }
                    SaveCards.SaveCardsToFile(this.bvm, myModelViewModel); 
                };
            }
        }
        /*
         * Diese Methode ist zum auswählen der Datei da. Damit kann man aus seiner Dateistruktur eine .xml Datei auswählen, welche dann eingelesen und importiert werden soll.
         */
        private void chooseDataMethod()
        {
            ofd = new OpenFileDialog();  // Ein OpenFileDialog wird erstellt, durch das die Datei ausgewählt weden kann
            ofd.Filter = "XML-Files|*.xml";     // Begrenzung der angezeigten Dateien auf .xml Dateien
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) // Wenn die Auswahl ohne Problme von statten ging
            {
                //MessageBox.Show(System.IO.Path.GetFileNameWithoutExtension(ofd.FileName));  // Anzeigen des Dateinamen (Nur erst mal Intern zur Kontrolle)
                filepath = Path.GetDirectoryName(ofd.FileName);
                XmlDocument doc = new XmlDocument();    // Ein neues XmlDocument wird erstellt, in das dann die zu importierende Datei geladen wird.
                doc.Load(ofd.FileName);
                this.bvm = new BoxViewModel();  // Ein BoxViewModel, in das die zu importierenden Karten geladen werden sollen
                this.bvm.Bn = Boxnumber.None;
                foreach (XmlNode node in doc.DocumentElement)
                {
                    CardViewModel card = readOwnFormatNode(node);
                    this.bvm.Enqueue(card);
                }
            }
            else
            {
                MessageBox.Show("Die Auswahl hat leider nicht geklappt");
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
                    case "StatisticCollection":
                        card.StatisticCollection = new StatisticCollection();   // Wird noch zu einer StatisticCollectionViewModel
                        /*
                         * Durch diese Schleife wird jedes Statistic Object einzeln durchgegangen.
                         */
                        foreach(XmlNode statNode in child) 
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
                                        switch(statDet.InnerText)
                                        {
                                            case "None":
                                                stat.CurrentBoxNumber = Boxnumber.None;
                                                break;
                                            case "Box1":
                                                stat.CurrentBoxNumber = Boxnumber.Box1;
                                                break;
                                            case "Box2":
                                                stat.CurrentBoxNumber = Boxnumber.Box2;
                                                break;
                                            case "Box3":
                                                stat.CurrentBoxNumber = Boxnumber.Box3;
                                                break;
                                            case "Box4":
                                                stat.CurrentBoxNumber = Boxnumber.Box4;
                                                break;
                                            case "Box5":
                                                stat.CurrentBoxNumber = Boxnumber.Box5;
                                                break;
                                        }
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

        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, randPicNameLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string copyPic(string currentPath)
        {
            string randName = RandomString() + ".jpg";
            File.Copy(filepath + @"\content\" + currentPath, pictureDirectory +  randName);
            return (randName);
        }


    }

    
}
