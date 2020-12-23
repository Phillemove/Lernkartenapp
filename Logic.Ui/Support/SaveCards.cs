using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
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
        static void SaveCardsToFile(BoxViewModel box)
        {

            System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App");
            System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content");
            string saveDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App";
            string pictureDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content";
            ArrayList categorys = new ArrayList();
            BoxCollectionViewModel bc = new BoxCollectionViewModel();
            //BoxViewModel loadedBVM = new BoxViewModel();
            //BoxViewModel newBVM = new BoxViewModel();

            foreach (CardViewModel card in box)
            {
                if(card.QuestionPic != null && !card.QuestionPic.Contains(pictureDirectory))
                {
                    File.Copy(card.QuestionPic, pictureDirectory + @"\NeuesQuestionBild.jpg");  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                }
                if (card.AnswerPic != null && !card.AnswerPic.Contains(pictureDirectory))
                {
                    File.Copy(card.AnswerPic, pictureDirectory + @"\NeuesAnswerBild.jpg");  // Der Dateiname muss noch geändert werden. Wie, entscheidet die Kartenetsellung
                }
                if (categorys.Contains(card.Category))
                {
                    bc.Where(cat => cat.Peek().Category == card.Category).Add(card); // Muss in der BoxViewModel noch behoben werden
                }
                else
                {
                    categorys.Add(card.Category);
                    BoxViewModel newBox = new BoxViewModel();
                    newBox.Add(card); // Muss in der BoxViewModel noch behoben werden
                    bc.Add(newBox);
                }
            }
            foreach (Category item in categorys)
            {
                try
                {
                    BoxViewModel current = (BoxViewModel)bc.Where(cat => cat.Peek().Category.Name == item.Name);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(saveDirectory + @"\" + item + ".xml");
                    foreach (XmlNode node in doc.DocumentElement)
                    {
                        CardViewModel card = new CardViewModel();
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
                                    /*case "StatisticCollection":   //Statistic Collection muss dann immer passend dafür angelegt werden? Ja! Und auch tiefergehend mit allen folgenden Nodes.... Viel Aufwand
                                        card.StatisticCollection = child.Name;
                                        break;*/
                            }
                        };
                        current.Add(card);

                    }
                }
                catch
                {
                    //Weiß noch nicht, ob was passieren soll, eigentlich nämlich nicht
                };
            };
            foreach (BoxViewModel pup in bc)    //passende Variablennamen überlegen
            {
                hardSave(pup);
            }
        }

            
        

        static void SaveCardsToFile(BoxCollectionViewModel bcvm)
        {

            BoxViewModel current = new BoxViewModel();
            foreach(BoxViewModel box in bcvm)
            {
                foreach(CardViewModel card in box)
                {
                    current.Add(card);
                }
            }
            SaveCardsToFile(current);
        }


        private static void hardSave(BoxViewModel box)
        {
            string saveDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App";
            string filename = box.Peek().Category.Name;
            XmlTextWriter writer = new XmlTextWriter(saveDirectory + @"\" + filename + ".xml", System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteComment(filename);
            writer.WriteName(filename);
            writer.WriteStartDocument();
            writer.WriteStartElement("Cards");
            foreach(CardViewModel card in box)
            {
                writer.WriteStartAttribute("Card");
                writer.WriteElementString("Question", card.Question != null ? card.Question : null);
                writer.WriteElementString("Answer", card.Answer != null ? card.Answer : null);
                writer.WriteElementString("Category", filename);
                writer.WriteElementString("QuestionPic", card.QuestionPic != null ? card.QuestionPic : null);
                writer.WriteElementString("AnswerPic", card.AnswerPic != null ? card.AnswerPic : null);
                if(card.StatisticCollection != null)
                {
                    foreach(Statistic stat in card.StatisticCollection)
                    {
                        writer.WriteStartAttribute("Statistic");
                        writer.WriteElementString("Timestamp", stat.Timestamp != null ? stat.Timestamp.ToString() : null);
                        writer.WriteElementString("SuccessfullAnswer", stat.SuccessfullAnswer != null ? stat.SuccessfullAnswer.ToString() : null);
                        //writer.WriteElementString("CurrentBoxNumber",stat.CurrentBoxNumber != null ? stat.CurrentBoxNumber.ToString() : null);    //Derzeit aufgrund der Enum Problematik nicht möglich
                        writer.WriteEndAttribute();
                    }
                }
                writer.WriteEndAttribute();
            }
            writer.WriteEndAttribute();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }
}
