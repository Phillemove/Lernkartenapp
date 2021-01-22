using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Support
{
    class SaveCards
    {
        private static readonly string saveDirectory = @"..\..\..\Lernkarten";
        private static readonly string pictureDirectory = @"..\..\..\Lernkarten\content\";

        /*
         * Diese Methode bekommt eine Box übergeben, welche Sie dann nach unserem Schema abspeichert. 
         */
        public static void hardSave(BoxViewModel box)
        {
            string saveDirectory = @"..\..\..\Lernkarten";
            string filename = box.Peek().Category.Name; // Der Filename wird aus einer der Dateien gelesen. Dabei handelt es sich um die Kategorienamen
            XmlTextWriter writer = new XmlTextWriter(saveDirectory + @"\" + filename + ".xml", System.Text.Encoding.UTF8);  // Die Datei wird durch den Writer erstellt. Name ist dabei "Kategorie".xml
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();    // Beginnen des Schreibens in die Datei
                writer.WriteComment(filename);  // Als Kommentar wird der Dateiname, also die Kategorie geschrieben
                //writer.WriteName(filename); // Name der Datei ist auch die Kategorie
                writer.WriteStartElement("Cards");  // Das Startelement ist ein Cards Element
                    foreach(CardViewModel card in box)  // Für jede Karte werden nun folgende Nodes + Inhalt geschrieben
                    {
                        writer.WriteStartElement("Card"); // Einleitung über eine Card Node
                            if (card.Question != null) { writer.WriteElementString("Question", card.Question); }    // Wenn keine Frage vorhanden ist, wird nichts geschrieben
                            if (card.Answer != null) { writer.WriteElementString("Answer", card.Answer); }// Wenn keine Antwort vorhanden ist, wird nichts geschrieben
                            //writer.WriteElementString("Category", filename);    // Kategorie wird auf jeden Fall geschrieben
                            if (card.QuestionPic != null) { writer.WriteElementString("QuestionPic", card.QuestionPic); }// Wenn kein FragenBild vorhanden ist, wird nichts geschrieben
                            if (card.AnswerPic != null) { writer.WriteElementString("AnswerPic", card.AnswerPic); }// Wenn kein AntwortBild vorhanden ist, wird nichts geschrieben
                            if (card.StatisticCollection != null) { 
                                writer.WriteStartElement("StatisticCollection"); 
                                if (card.StatisticCollection != null)   // Wenn es an dieser Stelle noch keine StatisticCollection gibt, wird hier auch nichts rein geschrieben
                                {
                                    foreach(Statistic stat in card.StatisticCollection)
                                    {
                                        writer.WriteStartElement("Statistic");   // Beginn eines Statistic Blocks
                                            writer.WriteElementString("Timestamp", stat.Timestamp.ToString());
                                            writer.WriteElementString("SuccessfullAnswer", stat.SuccessfullAnswer.ToString());
                                            writer.WriteElementString("CurrentBoxNumber", stat.CurrentBoxNumber.ToString());
                                        writer.WriteEndElement(); // Ende eines Statistic Blocks
                                    }
                                }
                                writer.WriteEndElement(); // Ende der StatisticCollection
                            }
                        writer.WriteEndElement(); // Ende der Card
                    }
                writer.WriteEndElement(); // Ende der Kategoriebox
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }


        public static BoxCollectionViewModel SortCardsFromBoxCollection(BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm)
        {
            System.IO.Directory.CreateDirectory(saveDirectory);
            System.IO.Directory.CreateDirectory(pictureDirectory);
            ArrayList categorys = new ArrayList();
            BoxCollectionViewModel bc = new BoxCollectionViewModel();
            CategoryViewModel defaultCat = new CategoryViewModel(new Category("default"));


            foreach (BoxViewModel box in bcvm)
            {
                foreach (CardViewModel card in box) // Jede Karte aus der übergbenen Box wird durchgegangen
                {
                    if (card.Category == null)
                    {
                        card.Category = defaultCat.category;
                        ccvm.Add(defaultCat);
                    }
                    if (card.QuestionPic != null && card.QuestionPic.Contains(@"\"))    // Wenn ein Foto vorhanden ist und es einen anderen Datenspeicherort hat, wird die Datei in unsere Struktur kopiert
                    {
                        string newPicPath = ImportViewModel.RandomString() + ".jpg";
                        File.Copy(card.QuestionPic, pictureDirectory + newPicPath);  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                        card.QuestionPic = newPicPath;
                    }
                    if (card.AnswerPic != null && card.AnswerPic.Contains(@"\"))   // Wenn ein Foto vorhanden ist und es einen anderen Datenspeicherort hat, wird die Datei in unsere Struktur kopiert
                    {
                        string newPicPath = ImportViewModel.RandomString() + ".jpg";
                        File.Copy(card.AnswerPic, pictureDirectory + newPicPath);  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                        card.AnswerPic = newPicPath;
                    }
                    if (categorys.Contains(card.Category))
                    {
                        foreach (BoxViewModel bvm in bc)
                        {
                            if (bvm.Peek().Category.Equals(card.Category))
                            {
                                bvm.Enqueue(card);
                            }
                        }
                    }
                    else
                    {
                        categorys.Add(card.Category);   // Neue Kategorie wird erstellt und der ArrayList hinzugefügt
                        BoxViewModel newBox = new BoxViewModel();   // Eine neue Box wird für die Kategorie erstellt
                        newBox.Enqueue(card); // Muss in der BoxViewModel noch behoben werden
                        bc.Add(newBox); // Die neu erstellte Box wird der BoxCollection hinzugefügt.
                    }
                }

            }

            return bc;
        }

        public static void SaveAdditionalCard(CategoryViewModel cat, CardViewModel card)
        {
            BoxViewModel bvm = LoadExistingCards(cat);
            bvm.Enqueue(card);
            hardSave(bvm);
        }

        public static void SaveAdditionalCardBox(CategoryViewModel cat, BoxViewModel box)
        {
            BoxViewModel bvm = LoadExistingCards(cat);
            foreach(CardViewModel card in box)
            {
                bvm.Enqueue(card);
            }
            hardSave(bvm);

        }

        public static void SaveBoxCollectionsToFilesystem(BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm)
        {
            BoxCollectionViewModel newBCVM = SortCardsFromBoxCollection(bcvm, ccvm);
            foreach(BoxViewModel box in newBCVM)
            {
                hardSave(box);
            }
        }

        public static BoxViewModel LoadExistingCards(CategoryViewModel cat)
        {
            BoxViewModel bvm = new BoxViewModel();
            XmlDocument doc = new XmlDocument();
            doc.Load(saveDirectory + @"\" + cat.Name + ".xml"); // Da der Name der Datei der Kategorie entspricht, funktioniert dies hier so
            foreach (XmlNode node in doc.DocumentElement)
            {
                CardViewModel card = ImportViewModel.readOwnFormatNode(node);
                card.Category = cat.category;
                bvm.Enqueue(card);
            }

            return bvm;
        }
    }
}
