using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Support
{
    class LoadCards
    {
        private static readonly string saveDirectory =
            @"..\..\..\Lernkarten\";

        public static void LoadCategoryFromFileSystem(BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm, CategoryViewModel cvm) // CategoryViewModel muss übergeben werden
        {
            XmlDocument doc = new XmlDocument();
            var fileName = saveDirectory + cvm.Name + ".xml";
            if (File.Exists(fileName))
            {
                doc.Load(saveDirectory + cvm.Name + ".xml");
                foreach (XmlNode node in doc.DocumentElement)
                {
                    CardViewModel card = ReadOwnFormatNode(node);
                    Boxnumber bn = CheckCardBoxnumber(card);
                    foreach (BoxViewModel boxi in bcvm)
                    {
                        if (boxi.Bn.Equals(bn))
                        {
                            boxi.Enqueue(card);
                        }
                    }
                }
            }
        }

        public static void LoadAllCategorysFromFileSystem(BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm)
        {
            XmlDocument doc = new XmlDocument();
            foreach(CategoryViewModel cat in ccvm)
            {
                LoadCategoryFromFileSystem(bcvm, ccvm, cat);
            }

        }

        public static CardViewModel ReadOwnFormatNode(XmlNode node)
        {
            // Creating the card, which is going to be given back
            CardViewModel card = new CardViewModel();

            foreach (XmlNode child in node)
            {
                /*
                 * This Switch-Case looks what kind the actual
                 * Node is and creates the equivalent part of the card
                 */
                switch (child.Name)
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
                    case "StatisticCollection":
                        card.StatisticCollection = new StatisticCollectionViewModel();
                        /*
                         * loop to go throug every Statistic
                         */
                        foreach (XmlNode statNode in child)
                        {
                            StatisticViewModel stat = ReadStat(statNode);

                            card.StatisticCollection.Add(stat);
                        }
                        break;
                }
            };
            return card;

        }

        private static StatisticViewModel ReadStat(XmlNode statNode)
        {
            // Creates a new Statistic
            StatisticViewModel stat = new StatisticViewModel();
            /*
             * loop for the details of every StatisticObject
             */
            foreach (XmlNode statDet in statNode)
            {
                switch (statDet.Name)
                {
                    case "Timestamp":
                        stat.Timestamp = Convert.ToDateTime(statDet.InnerText);
                        break;
                    case "SuccessfullAnswer":
                        stat.SuccessfulAnswer =
                            Convert.ToBoolean(statDet.InnerText);
                        break;
                    case "CurrentBoxNumber":
                        switch (statDet.InnerText)
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
            return stat;
        }

        private static Boxnumber NextBox(Boxnumber bn)
        {
            Boxnumber ret;

            switch(bn)
            {
                case Boxnumber.Box1:
                    ret = Boxnumber.Box2;
                    break;
                case Boxnumber.Box2:
                    ret = Boxnumber.Box3;
                    break;
                case Boxnumber.Box3:
                    ret = Boxnumber.Box4;
                    break;
                case Boxnumber.Box4:
                    ret = Boxnumber.Box5;
                    break;
                default:
                    ret = Boxnumber.Box5;
                    break;
            }
            return ret;
        }

        private static Boxnumber PrevBox(Boxnumber bn)
        {
            Boxnumber ret;

            switch (bn)
            {
                case Boxnumber.Box5:
                    ret = Boxnumber.Box4;
                    break;
                case Boxnumber.Box4:
                    ret = Boxnumber.Box3;
                    break;
                case Boxnumber.Box3:
                    ret = Boxnumber.Box2;
                    break;
                case Boxnumber.Box2:
                    ret = Boxnumber.Box1;
                    break;
                default:
                    ret = Boxnumber.Box1;
                    break;
            }
            return ret;
        }

        private static Boxnumber CheckCardBoxnumber(CardViewModel card)
        {
            Boxnumber bn;
            StatisticViewModel svm = card.StatisticCollection.LastOrDefault();
           
            if (svm != null)
            {
                if (svm.SuccessfulAnswer)
                {
                    bn = NextBox(svm.CurrentBoxNumber);
                }
                else
                {
                    bn = PrevBox(svm.CurrentBoxNumber);
                }
            }
            else
            {
                bn = Boxnumber.Box1;
            }
            return bn;
        }


    }
}
