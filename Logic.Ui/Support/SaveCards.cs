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
        public static void SaveCardsToFile(BoxViewModel box)
        {

            //System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App");
            //System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content");
            
            //string saveDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App";
            //string pictureDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content";
            string saveDirectory = @"..\..\..\Lernkarten";
            string pictureDirectory = @"..\..\..\Lernkarten\content";
            System.IO.Directory.CreateDirectory(saveDirectory);
            System.IO.Directory.CreateDirectory(pictureDirectory);
            ArrayList categorys = new ArrayList();
            BoxCollectionViewModel bc = new BoxCollectionViewModel();
            //BoxViewModel loadedBVM = new BoxViewModel();
            //BoxViewModel newBVM = new BoxViewModel();

            foreach (CardViewModel card in box) // Jede Karte aus der übergbenen Box wird durchgegangen
            {
                if(card.QuestionPic != null && !card.QuestionPic.Contains(pictureDirectory))    // Wenn ein Foto vorhanden ist und es einen anderen Datenspeicherort hat, wird die Datei in unsere Struktur kopiert
                {
                    File.Copy(card.QuestionPic, pictureDirectory + @"\NeuesQuestionBild.jpg");  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                }
                if (card.AnswerPic != null && !card.AnswerPic.Contains(pictureDirectory))   // Wenn ein Foto vorhanden ist und es einen anderen Datenspeicherort hat, wird die Datei in unsere Struktur kopiert
                {
                    File.Copy(card.AnswerPic, pictureDirectory + @"\NeuesAnswerBild.jpg");  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                }
                if (categorys.Contains(card.Category))
                {
                    BoxViewModel current = (BoxViewModel)bc.Where(cat => cat.Peek().Category == card.Category); // Die Categorybox wird aus der BoxCollection gezogen und die Carte dieser hinzugefügt.
                    current.Enqueue(card); // Muss in der BoxViewModel noch behoben werden
                }
                else
                {
                    categorys.Add(card.Category);   // Neue Kategorie wird erstellt und der ArrayList hinzugefügt
                    BoxViewModel newBox = new BoxViewModel();   // Eine neue Box wird für die Kategorie erstellt
                    newBox.Enqueue(card); // Muss in der BoxViewModel noch behoben werden
                    bc.Add(newBox); // Die neu erstellte Box wird der BoxCollection hinzugefügt.
                }
            }
            /*
             *  Diese Schleife itteriert jetzt über alle Kategorien, welche in der ArrayList sind und schaut, ob es dazu schon passende Dateien gefüllt mit Karten gibt, lädt
             *  diese gegebenenfalls und fügt sie der von uns vorher erstellten Box hinzu.
             */ 
            foreach (Category item in categorys)
            {
                try
                {
                    BoxViewModel current = (BoxViewModel)bc.Where(cat => cat.Peek().Category.Name == item.Name);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(saveDirectory + @"\" + item + ".xml"); // Da der Name der Datei der Kategorie entspricht, funktioniert dies hier so
                    foreach (XmlNode node in doc.DocumentElement)
                    {
                        current.Enqueue(ImportViewModel.readOwnFormatNode(node));   // Jede Karte wird in Form von einer XmlNode eingelesen, zu einer Karte gemacht und zurück gegeben
                    }
                }
                catch
                {
                    //Weiß noch nicht, ob was passieren soll, eigentlich nämlich nicht
                };
            };
            foreach (BoxViewModel currentBox in bc)    //passende Variablennamen überlegen
            {
                hardSave(currentBox);   // Jede Box wird über die Methode hardSave abgespeichert in der Dateistruktur
            }
        }

            
        
        /*
         * Diese Methode bekommt eine BoxCollection übergeben und soll diese abspeichern. Dazu wird Box für Box durchgegangen und der Methode SaveCardsToFile(BoxViewModel) übergeben.
         * Dort wird dann die Box dementsprechend abgespeichert. Hierbei wird davon ausgegangen, dass bei übergabe einer ganzen Collection die einzelnen Boxen schon sortiert sind und 
         * man sich darüber vorher keine Gedanken machen muss.
         */
        static void SaveCardsToFile(BoxCollectionViewModel bcvm)
        {
            foreach (BoxViewModel box in bcvm)
            {
                SaveCardsToFile(box);
            }
        }

        /*
         * Diese Methode bekommt eine Box übergeben, welche Sie dann nach unserem Schema abspeichert. 
         */
        public static void hardSave(BoxViewModel box)
        {
            string saveDirectory = @"..\..\..\Lernkarten";
            //string saveDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App";
            string filename = box.Peek().Category.Name; // Der Filename wird aus einer der Dateien gelesen. Dabei handelt es sich um die Kategorienamen
            XmlTextWriter writer = new XmlTextWriter(saveDirectory + @"\" + filename + ".xml", System.Text.Encoding.UTF8);  // Die Datei wird durch den Writer erstellt. Name ist dabei "Kategorie".xml
            writer.Formatting = Formatting.Indented;
            writer.WriteComment(filename);  // Als Kommentar wird der Dateiname, also die Kategorie geschrieben
            writer.WriteName(filename); // Name der Datei ist auch die Kategorie
            writer.WriteStartDocument();    // Beginnen des Schreibens in die Datei
            writer.WriteStartElement("Cards");  // Das Startelement ist ein Cards Element
                foreach(CardViewModel card in box)  // Für jede Karte werden nun folgende Nodes + Inhalt geschrieben
                {
                    writer.WriteStartAttribute("Card"); // Einleitung über eine Card Node
                        writer.WriteElementString("Question", card.Question != null ? card.Question : null);    // Wenn keine Frage vorhanden ist, wird nichts geschrieben
                        writer.WriteElementString("Answer", card.Answer != null ? card.Answer : null);// Wenn keine Antwort vorhanden ist, wird nichts geschrieben
                        writer.WriteElementString("Category", filename);    // Kategorie wird auf jeden Fall geschrieben
                        writer.WriteElementString("QuestionPic", card.QuestionPic != null ? card.QuestionPic : null);// Wenn kein FragenBild vorhanden ist, wird nichts geschrieben
                        writer.WriteElementString("AnswerPic", card.AnswerPic != null ? card.AnswerPic : null);// Wenn kein AntwortBild vorhanden ist, wird nichts geschrieben
                        writer.WriteStartAttribute("StatisticCollection");
                            if (card.StatisticCollection != null)   // Wenn es an dieser Stelle noch keine StatisticCollection gibt, wird hier auch nichts rein geschrieben
                            {
                                foreach(Statistic stat in card.StatisticCollection)
                                {
                                    writer.WriteStartAttribute("Statistic");
                                        writer.WriteElementString("Timestamp", stat.Timestamp != null ? stat.Timestamp.ToString() : null);
                                        writer.WriteElementString("SuccessfullAnswer", stat.SuccessfullAnswer ? stat.SuccessfullAnswer.ToString() : null);
                                        //writer.WriteElementString("CurrentBoxNumber",stat.CurrentBoxNumber != null ? stat.CurrentBoxNumber.ToString() : null);    //Derzeit aufgrund der Enum Problematik nicht möglich
                                    writer.WriteEndAttribute();
                                }
                            }
                        writer.WriteEndAttribute();
                    writer.WriteEndAttribute();
                }
            writer.WriteEndAttribute();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }
}
