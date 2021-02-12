using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Support
{
    class LoadCards
    {
        private static readonly string saveDirectory =
            ModelViewModel.saveDirectory;

        /*
         * This Method gets a BoxCollectionViewModel and the Category,
         * which should be loaded to the System. Every Card will be putted
         * in the correct Box.
         */
        public static void LoadCategoryFromFileSystem(
            BoxCollectionViewModel bcvm,
            CategoryViewModel catVM)
        {
            XmlDocument xmlDoc = new XmlDocument();
            var fileName = saveDirectory + catVM.Name + ".xml";
            if (File.Exists(fileName))
            {
                xmlDoc.Load(saveDirectory + catVM.Name + ".xml");
                foreach (XmlNode node in xmlDoc.DocumentElement)
                {
                    CardViewModel card = ReadOwnFormatNode(node);
                    Boxnumber boxNumber = CheckCardBoxnumber(card);
                    card.Category = catVM;
                    foreach (BoxViewModel boxVM in bcvm)
                    {
                        if (boxVM.Bn.Equals(boxNumber))
                        {
                            boxVM.Enqueue(card);
                        }
                    }
                }
            }
        }


        /*
         * This Method gets the BoxCollectionViewModel and the 
         * CategoryCollectionViewModel and load all possible Categorys,
         * which contains Cards.
         */
        public static void LoadAllCategorysFromFileSystem(
            BoxCollectionViewModel bcvm,
            CategoryCollectionViewModel ccvm)
        {
            foreach(CategoryViewModel catVM in ccvm)
            {
                LoadCategoryFromFileSystem(bcvm, catVM);
            }
        }

        /*
         * This Method reads the xml Nodes and creates cards by our selfe sheme
         */
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
                        card.StatisticCollection = 
                            new StatisticCollectionViewModel();
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

        /*
         * This Method gets an XmlNode with Statistic Nodes, create out of
         * it a Statistic Object and give this Object back.
         */
        public static StatisticViewModel ReadStat(XmlNode statNode)
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
                        stat.Timestamp = UnixTimeStampToDateTime(
                            Convert.ToDouble(statDet.InnerText));
                        break;
                    case "SuccessfullAnswer":
                        stat.SuccessfulAnswer =
                            Convert.ToBoolean(statDet.InnerText);
                        break;
                    case "CurrentBoxNumber":
                        switch (statDet.InnerText)
                        {
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

        /*
         * This Method receives a Boxnumber and give, if necessary, the 
         * next higher Boxnumber back. 
         */
        private static Boxnumber NextBox(Boxnumber boxNumber)
        {
            Boxnumber retBn;

            switch(boxNumber)
            {
                case Boxnumber.Box1:
                    retBn = Boxnumber.Box2;
                    break;
                case Boxnumber.Box2:
                    retBn = Boxnumber.Box3;
                    break;
                case Boxnumber.Box3:
                    retBn = Boxnumber.Box4;
                    break;
                case Boxnumber.Box4:
                    retBn = Boxnumber.Box5;
                    break;
                default:
                    retBn = Boxnumber.Box5;
                    break;
            }
            return retBn;
        }

        /*
         * This Method receives a Boxnumber and give, if necessary, the
         * next lower Boxnumber back.
         */
        private static Boxnumber PrevBox(Boxnumber boxNumber)
        {
            Boxnumber retBn;

            switch (boxNumber)
            {
                case Boxnumber.Box5:
                    retBn = Boxnumber.Box4;
                    break;
                case Boxnumber.Box4:
                    retBn = Boxnumber.Box3;
                    break;
                case Boxnumber.Box3:
                    retBn = Boxnumber.Box2;
                    break;
                case Boxnumber.Box2:
                    retBn = Boxnumber.Box1;
                    break;
                default:
                    retBn = Boxnumber.Box1;
                    break;
            }
            return retBn;
        }

        /*
         * This Method gets a CardViewModel and give the Boxnumber back, which
         * is needed. If the CardViewModel doesn't contain a 
         * StatisticViewModel, the Boxnumber is Box1.
         */
        private static Boxnumber CheckCardBoxnumber(CardViewModel card)
        {
            Boxnumber retBn;
            StatisticViewModel statVM = 
                card.StatisticCollection.LastOrDefault();
           
            if (statVM != null)
            {
                if (statVM.SuccessfulAnswer)
                {
                    retBn = NextBox(statVM.CurrentBoxNumber);
                }
                else
                {
                    retBn = PrevBox(statVM.CurrentBoxNumber);
                }
            }
            else
            {
                retBn = Boxnumber.Box1;
            }
            return retBn;
        }

        /*
         * This Method gets a unixTimeStamp and convert it to a DateTime
         * Object, which fits to the actual System DateTime Format.
         * Source:https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
         * Author: user6269864
         * 11.02.2021
         */
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            Console.WriteLine(unixTimeStamp);
            System.DateTime dtDateTime = new DateTime(
                1970, 1, 1, 0, 0, 0, 0,
                System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            Console.WriteLine(dtDateTime);
            return dtDateTime;
        }


    }
}
