using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml;

namespace De.HsFlensburg.ClientApp101.Logic.Ui.Support
{

    /*
     * This Class Handels all the opportunities to Save a CardViewModel, 
     * BoxViewModel or BoxCollectionViewModel to the FileSystem. 
     * It can only write it to the FileSystem or load previously the cards, 
     * which are allready saved in the FileSystem
     */
    class SaveCards
    {
        private static readonly string saveDirectory =
            ModelViewModel.saveDirectory;
        private static readonly string pictureDirectory =
            ModelViewModel.savePicDirectory;
        private readonly static Random random = new Random();
        // The stringlength for the Method RandomString()
        private readonly static int randPicNameLength = 10;
        /*
         * This Method gets a BoxViewModel and Save this to the Filesystem. 
         * It creates a .xml File with the naming of the category.  
         */
        public static void SaveBoxToFileSystem(
            BoxViewModel boxVM,
            CategoryCollectionViewModel ccvm)
        {
            System.IO.Directory.CreateDirectory(saveDirectory);
            System.IO.Directory.CreateDirectory(pictureDirectory);
            CardViewModel interrimsCard = boxVM.Peek();
            string filename;
            if(interrimsCard.Category != null)
            {
                filename = interrimsCard.Category.Name;
            } else
            {
                filename = GetDefaultCat(ccvm).Name;
            }
            WriteXMLFile(saveDirectory,boxVM,filename);
        }

        /*
         * This Method gets a BoxCollectionViewModel and 
         * CategoryCollectionViewModel and Sort all Cards in the
         * BCVM by theyre Category and put them in a new Box.
         * All the Boxes with cards of the same Category
         * are then the return.
         */
        public static BoxCollectionViewModel SortCardsFromBoxCollection(
            BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm)
        {
            ArrayList categorys = new ArrayList();
            BoxCollectionViewModel bcVM = new BoxCollectionViewModel();
            foreach (BoxViewModel boxVM in bcvm)
            {
                foreach (CardViewModel card in boxVM)
                {
                    if (card.Category == null)
                    {
                        card.Category = GetDefaultCat(ccvm);
                    }
                    if (categorys.Contains(card.Category))
                    {
                        foreach (BoxViewModel bvm in bcVM)
                        {
                            if (bvm.Peek().Category.Equals(card.Category))
                            {
                                bvm.Enqueue(card);
                            }
                        }
                    }
                    /*
                     * If the category isn't in category, it will create 
                     * a new BoxViewModel for the category, add the category 
                     * of the card to categorys and put the card 
                     * in the new BoxViewModel
                     */
                    else
                    {
                        categorys.Add(card.Category);
                        BoxViewModel newBox = new BoxViewModel();
                        newBox.Enqueue(card);
                        bcVM.Add(newBox);
                    }
                }
            }
            return bcVM;
        }

        /*
         * This Method gets a CategoryViewModel and a CardViewModel. 
         * It will load all Cards which the corresponds to 
         * the CategoryViewModel, put them in a BoxViewModel,
         * put the CardViewModel also in the BoxViewModel and calls 
         * SaveBoxToFileSystem() to Save the Cards to the FileSystem.
         */
        public static void SaveAdditionalCard(
            CategoryViewModel catVM,
            CardViewModel card,
            CategoryCollectionViewModel ccvm)
        {
            BoxViewModel boxVM = LoadExistingCards(catVM);
            boxVM.Enqueue(card);
            SaveBoxToFileSystem(boxVM, ccvm);
        }

        /*
         * This Method gets a CategoryViewModel and a BoxViewModel. 
         * It will load all Cards which the corresponds to the 
         * CategoryViewModel, put them in a BoxViewModel, put the 
         * CardViewModels from the BoxViewModel also in the new 
         * BoxViewModel and calls SaveBoxToFileSystem() to 
         * Save the Cards to the FileSystem.
         */
        public static void SaveAdditionalCardBox(
            CategoryViewModel catVM,
            BoxViewModel boxVM,
            CategoryCollectionViewModel ccvm)
        {
            BoxViewModel boxExist = LoadExistingCards(catVM);
            foreach (CardViewModel card in boxExist)
            {
                boxVM.Enqueue(card);
            }
            SaveBoxToFileSystem(boxVM, ccvm);

        }

        /*
         * This Method gets a BoxCollectionViewModel and 
         * a CategoryCollectionViewModel. It calls SortCardsFromBoxCollection()
         * with the BoxCollectionViewModel and CategoryCollectionViewModel. 
         * With this new BoxCollectionViewModel it calls 
         * SaveBoxToFileSystem() for every Box to save them to the FileSystem.
         */
        public static void SaveBoxCollectionsToFilesystem(
            BoxCollectionViewModel bcvm, CategoryCollectionViewModel ccvm)
        {
            BoxCollectionViewModel newBCVM =
                SortCardsFromBoxCollection(bcvm, ccvm);
            foreach (BoxViewModel boxVM in newBCVM)
            {
                SaveBoxToFileSystem(boxVM, ccvm);
            }
        }

        /*
         * This Method gets a CategoryViewModel and Load all the Cards out of
         * the File, which has the Name of the CategoryViewModel.
         * So it give a BoxViewModel with all the loaded Cards back.
         * If there are no Cards in the Filesystem, 
         * the Method returns an empty BoxViewModel and shows a hint.
         */
        public static BoxViewModel LoadExistingCards(CategoryViewModel catVM)
        {
            BoxViewModel boxVM = new BoxViewModel();

            XmlDocument xmlDoc = new XmlDocument();
            // This is possible, because the name of the
            // File is the name of the category
            if(catVM != null)
            {
                if(File.Exists(saveDirectory + catVM.Name + ".xml"))
                {
                    xmlDoc.Load(saveDirectory + catVM.Name + ".xml");
                    foreach (XmlNode node in xmlDoc.DocumentElement)
                    {
                        CardViewModel card = LoadCards.ReadOwnFormatNode(node);
                        card.Category = catVM;
                        boxVM.Enqueue(card);
                    }
                }
            }
            return boxVM;
        }

        /* 
        * defaultCat is for this moment, where the correct Categorys
        * doesn't work correctly in every classes. If everything works 
        * correctly, the defaultCat isn't necessary
        */
        private static CategoryViewModel GetDefaultCat(
            CategoryCollectionViewModel ccvm)
        {
            
            CategoryViewModel defaultCat = ccvm.Where(
                catVM => catVM.Name == "default").FirstOrDefault();
            if (defaultCat == null)
            {
                defaultCat = new CategoryViewModel(new Category("default"));
                ccvm.Add(defaultCat);
                ccvm.SaveCategorys();
            }
            return defaultCat;
        }

        /*
         * This Method gets a DateTime Object and convert it to a 
         * unix-double value, which can be stored in the FileSystem.
         * Source: https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp
         * helpful answer from: Steven Penny
         * 12.02.2021
         */
        public static double DateTimeStampToUnixTime(DateTime DateTimeStamp)
        {
            double unix = ((DateTimeOffset)DateTimeStamp).ToUnixTimeSeconds();
            return unix;
        }

        /*
         * This Method write the xmlFile by using our own xml format
         */
        public static void WriteXMLFile(
            string filePath,
            BoxViewModel boxVM,
            string catName,
            Boolean exportStat = true)
        {
            string testfile = filePath + catName + ".xml";
            XmlTextWriter xmlWriter = new XmlTextWriter(
                        testfile, System.Text.Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteComment(catName);
            xmlWriter.WriteStartElement("Cards");
            foreach(CardViewModel card in boxVM)
            {
                xmlWriter.WriteStartElement("Card");
                if (card.Question != null)
                {
                    xmlWriter.WriteElementString("Question",
                        card.Question);
                }
                if (card.Answer != null)
                {
                    xmlWriter.WriteElementString("Answer",
                        card.Answer);
                }
                if (card.QuestionPic != null)
                {
                    CheckQuestionPic(card, xmlWriter, filePath);
                }
                if (card.AnswerPic != null)
                {
                    CheckAnswerPic(card, xmlWriter, filePath);
                }
                xmlWriter.WriteStartElement("StatisticCollection");
                if (exportStat && card.StatisticCollection != null)
                {
                    WriteStatistic(card, xmlWriter);
                }
                xmlWriter.WriteEndElement();
                
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        /*
         * This Method writes the Statistic XmlElements for every
         * StatisticViewModel-Element
         */
        private static void WriteStatistic(
            CardViewModel card,
            XmlTextWriter xmlWriter)
        {
            foreach (StatisticViewModel stat in card.StatisticCollection)
            {
                xmlWriter.WriteStartElement("Statistic");
                xmlWriter.WriteElementString("Timestamp",
                    SaveCards.DateTimeStampToUnixTime(
                        stat.Timestamp).ToString());
                xmlWriter.WriteElementString("SuccessfullAnswer",
                    stat.SuccessfulAnswer.ToString());
                xmlWriter.WriteElementString("CurrentBoxNumber",
                    stat.CurrentBoxNumber.ToString());
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        /*
         * This Method cipy the AnswerPic if necessary and write the
         * AnswerPic-XmlElement
         */
        private static void CheckAnswerPic(
            CardViewModel card,
            XmlTextWriter xmlWriter,
            string filePath)
        {
            if (card.AnswerPic.Contains(@"\"))
            {
                card.AnswerPic = CopyPic(card.AnswerPic);
                xmlWriter.WriteElementString("AnswerPic", card.AnswerPic);
            }
            else if (filePath.Contains("Export"))
            {
                string fileName = CopyExportPic(filePath, card.AnswerPic);
                xmlWriter.WriteElementString("AnswerPic", fileName);
            }
            else
            {
                xmlWriter.WriteElementString("AnswerPic", card.AnswerPic);
            }
        }

        /*
         * This Method copy the QuestionPic if necessary and write the
         * QuestionPic-XmlElement
         */
        private static void CheckQuestionPic(
            CardViewModel card,
            XmlTextWriter xmlWriter,
            string filePath)
        {
            if (card.QuestionPic.Contains(@"\"))
            {
                card.QuestionPic = CopyPic(card.QuestionPic);
                xmlWriter.WriteElementString("QuestionPic", card.QuestionPic);
            }
            else if (filePath.Contains("Export"))
            {
                string fileName = CopyExportPic(filePath, card.QuestionPic);
                xmlWriter.WriteElementString("QuestionPic", fileName);
            }
            else
            {
                xmlWriter.WriteElementString("QuestionPic", card.QuestionPic);
            }
        }

        /*
         * This Method copy the Pics from a setted path to our Folder
         * and returns the new Filename. The Method is setted very flexible
         * so it can be called from any other Class.
         */
        public static string CopyPic(string path)
        {
            System.IO.Directory.CreateDirectory(pictureDirectory);
            string newPicName = RandomString() + ".jpg";
            File.Copy(path, pictureDirectory + newPicName);
            return newPicName;
        }

        /*
         * This Method copy the Pics from the own format to the selected
         * export filedirectory. Return value is the filename of the new 
         */
        private static String CopyExportPic(
            string filePath,
            string file)
        {
            string picNew = RandomString() + ".jpg";
            string pathSavePicture = filePath +
                @"content\" + picNew;
            while(File.Exists(pathSavePicture))
            {
                picNew = RandomString() + ".jpg";
                pathSavePicture = filePath +
                @"content\" + picNew;
            }
            File.Copy(pictureDirectory + file, pathSavePicture);
            return picNew;
        }

        /*
        * This Method creates a random String for the Filenames and give
        * this back. The idea and the most of the code is
        * from the following page:
        * https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        * The Website call was on: 22.01.2021 16:33MEZ
        */
        public static string RandomString()
        {
            // This is the Range and possible Chars for the String
            const string chars =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";
            return new string(Enumerable.Repeat(chars, randPicNameLength)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
