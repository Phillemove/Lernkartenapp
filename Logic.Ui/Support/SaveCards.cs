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

            System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App");
            System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content");
            string saveDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App";
            string pictureDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content";
            ArrayList categorys = new ArrayList();
            BoxCollectionViewModel bc = new BoxCollectionViewModel();
            //BoxViewModel loadedBVM = new BoxViewModel();
            //BoxViewModel newBVM = new BoxViewModel();

            foreach (Wrapper.CardViewModel card in box)
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
                    BoxViewModel current = (BoxViewModel)bc.Where(cat => cat.Peek().Category == card.Category);
                    current.Enqueue(card); // Muss in der BoxViewModel noch behoben werden
                }
                else
                {
                    categorys.Add(card.Category);
                    BoxViewModel newBox = new BoxViewModel();
                    newBox.Enqueue(card); // Muss in der BoxViewModel noch behoben werden
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
                        current.Enqueue(ImportViewModel.readOwnFormatNode(node));
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
                foreach(Wrapper.CardViewModel card in box)
                {
                    current.Enqueue(card);
                }
            }
            SaveCardsToFile(current);
        }


        public static void hardSave(BoxViewModel box)
        {
            string saveDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App";
            string filename = box.Peek().Category.Name;
            XmlTextWriter writer = new XmlTextWriter(saveDirectory + @"\" + filename + ".xml", System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteComment(filename);
            writer.WriteName(filename);
            writer.WriteStartDocument();
            writer.WriteStartElement("Cards");
            foreach(Wrapper.CardViewModel card in box)
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
                        writer.WriteElementString("SuccessfullAnswer", stat.SuccessfullAnswer ? stat.SuccessfullAnswer.ToString() : null);
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
