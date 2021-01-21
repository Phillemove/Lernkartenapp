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
        public static void SaveCardsToFile(BoxViewModel box, Boolean copied = false)
        {
            string saveDirectory = @"..\..\..\Lernkarten";
            string pictureDirectory = @"..\..\..\Lernkarten\content\";
            System.IO.Directory.CreateDirectory(saveDirectory);
            System.IO.Directory.CreateDirectory(pictureDirectory);
            ArrayList categorys = new ArrayList();
            BoxCollectionViewModel bc = new BoxCollectionViewModel();

            foreach (CardViewModel card in box) // Jede Karte aus der übergbenen Box wird durchgegangen
            {
                if(card.QuestionPic != null && !copied)    // Wenn ein Foto vorhanden ist und es einen anderen Datenspeicherort hat, wird die Datei in unsere Struktur kopiert
                {
                    string newPicPath = ImportViewModel.RandomString() + ".jpg";
                    File.Copy(card.QuestionPic, pictureDirectory + newPicPath);  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                }
                if (card.AnswerPic != null && !copied)   // Wenn ein Foto vorhanden ist und es einen anderen Datenspeicherort hat, wird die Datei in unsere Struktur kopiert
                {
                    string newPicPath = ImportViewModel.RandomString() + ".jpg";
                    File.Copy(card.AnswerPic, pictureDirectory + newPicPath);  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                }
                if (categorys.Contains(card.Category))
                {
                    //BoxViewModel current;
                    foreach(BoxViewModel test in bc)
                    {
                        if (test.Peek().Category.Equals(card.Category))
                        {
                            test.Enqueue(card);
                        }
                    }

                    //BoxViewModel current = (BoxViewModel)bc.Where(cat => cat.Peek().Category == card.Category); // Die Categorybox wird aus der BoxCollection gezogen und die Carte dieser hinzugefügt.
                    //current.Enqueue(card);
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
                    MessageBox.Show(item.Name);
                    Category cat = new Category(item.Name);
                    //BoxViewModel current = (BoxViewModel)bc.Where(cat => cat.Peek().Category.Name == item.Name);
                    foreach (BoxViewModel curBox in bc)
                    {
                        if(curBox.Peek().Category.Name.Contains(item.Name))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(saveDirectory + @"\" + item.Name + ".xml"); // Da der Name der Datei der Kategorie entspricht, funktioniert dies hier so
                            foreach (XmlNode node in doc.DocumentElement)
                            {
                                curBox.Enqueue(ImportViewModel.readOwnFormatNode(node));   // Jede Karte wird in Form von einer XmlNode eingelesen, zu einer Karte gemacht und zurück gegeben
                            }
                        }
                    }
                    //BoxViewModel current = (BoxViewModel)bc.Where(cate => cate.Peek().Answer == "Jaa");
                    
                }
                catch
                {
                    MessageBox.Show("Keine Kategorie davon vorhanden");
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
                            writer.WriteElementString("Category", filename);    // Kategorie wird auf jeden Fall geschrieben
                            if (card.QuestionPic != null) { writer.WriteElementString("QuestionPic", card.QuestionPic); }// Wenn kein FragenBild vorhanden ist, wird nichts geschrieben
                            if (card.AnswerPic != null) { writer.WriteElementString("AnswerPic", card.AnswerPic); }// Wenn kein AntwortBild vorhanden ist, wird nichts geschrieben
                            if (card.StatisticCollection != null) { 
                                writer.WriteStartElement("StatisticCollection"); 
                                if (card.StatisticCollection != null)   // Wenn es an dieser Stelle noch keine StatisticCollection gibt, wird hier auch nichts rein geschrieben
                                {
                                    foreach(Statistic stat in card.StatisticCollection)
                                    {
                                        writer.WriteStartElement("Statistic");   // Beginn eines Statistic Blocks
                                            /*if(stat.Timestamp != null)
                                            {
                                                xmlWriter.WriteElementString("Timestamp", stat.Timestamp.ToString());
                                            }
                                            if (stat.SuccessfullAnswer != null)
                                            {
                                                xmlWriter.WriteElementString("SuccessfullAnswer", stat.SuccessfullAnswer.ToString());
                                            }
                                            if (stat.CurrentBoxNumber != Boxnumber.None)
                                            {
                                                xmlWriter.WriteElementString("CurrentBoxNumber", stat.CurrentBoxNumber.ToString());
                                            }*/
                                            writer.WriteElementString("Timestamp", stat.Timestamp.ToString());
                                            writer.WriteElementString("SuccessfullAnswer", stat.SuccessfullAnswer.ToString());
                                            writer.WriteElementString("CurrentBoxNumber", stat.CurrentBoxNumber.ToString());
                                        writer.WriteEndElement(); // Ende eines Statistic Blocks
                                    }
                                }
                                writer.WriteEndElement(); // Ende der StatisticCollection
                            }
                            //writer.WriteElementString("LastBox", box.Bn.ToString());  //Sollte eher über das letzte Statisticobjekt erfolgen. 
                        writer.WriteEndElement(); // Ende der Card
                    }
                writer.WriteEndElement(); // Ende der Kategoriebox
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }
}
