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

    /*
     * This Class Handels all the opportunities to Save a CardViewModel, 
     * BoxViewModel or BoxCollectionViewModel to the FileSystem. 
     * It can only write it to the FileSystem or load previously the cards, 
     * which are allready saved in the FileSystem
     */
    class SaveCards
    {
        private static readonly string saveDirectory = 
            @"..\..\..\Lernkarten\";
        private static readonly string pictureDirectory = 
            @"..\..\..\Lernkarten\content\";

        /*
         * This Method gets a BoxViewModel and Save this to the Filesystem. 
         * It creates a .xml File with the naming of the category.  
         */
        public static void SaveBoxToFileSystem(BoxViewModel box)
        {
            System.IO.Directory.CreateDirectory(saveDirectory);
            System.IO.Directory.CreateDirectory(pictureDirectory);
            string filename = box.Peek().Category.Name;
            XmlTextWriter writer = new XmlTextWriter(
                saveDirectory + @"\" + filename + ".xml",
                System.Text.Encoding.UTF8);
            // So the .xml File is more readable and every
            //  Element get an own Line and is intended
            writer.Formatting = Formatting.Indented;    
            writer.WriteStartDocument();    
            writer.WriteComment(filename);
            writer.WriteStartElement("Cards"); 
            foreach(CardViewModel card in box)
            {
                writer.WriteStartElement("Card");
                if (card.Question != null) { 
                    writer.WriteElementString("Question", 
                        card.Question); 
                }
                if (card.Answer != null) 
                { 
                    writer.WriteElementString("Answer", 
                        card.Answer); 
                }
                if (card.QuestionPic != null) 
                { 
                    writer.WriteElementString("QuestionPic", 
                        card.QuestionPic); 
                }
                if (card.AnswerPic != null) 
                { 
                    writer.WriteElementString("AnswerPic", 
                        card.AnswerPic); 
                }
                if (card.StatisticCollection != null) { 
                    writer.WriteStartElement("StatisticCollection"); 
                    if (card.StatisticCollection != null)
                    {
                        foreach(Statistic stat in card.StatisticCollection)
                        {
                            writer.WriteStartElement("Statistic");
                            writer.WriteElementString("Timestamp", 
                                stat.Timestamp.ToString());
                            writer.WriteElementString("SuccessfullAnswer", 
                                stat.SuccessfullAnswer.ToString());
                            writer.WriteElementString("CurrentBoxNumber", 
                                stat.CurrentBoxNumber.ToString());
                            writer.WriteEndElement();
                            writer.Flush();
                        }
                    }
                writer.WriteEndElement();
                }
            writer.WriteEndElement();
            writer.Flush();
            }
            writer.WriteEndElement(); 
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
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
            BoxCollectionViewModel bc = new BoxCollectionViewModel();
            CategoryViewModel defaultCat = 
                new CategoryViewModel(new Category("default"));


            foreach (BoxViewModel box in bcvm)
            {
                foreach (CardViewModel card in box)
                {
                    if (card.Category == null)
                    {
                        card.Category = defaultCat.category;
                        ccvm.Add(defaultCat);
                    }
                    // If there is a Picture, which isn't in our Filesystem, 
                    // it will be copyd to it and the Card gets the new Name
                    if (card.QuestionPic != null && 
                        card.QuestionPic.Contains(@"\"))    
                    {
                        string newPicPath = 
                            ImportViewModel.RandomString() + ".jpg";
                        File.Copy(card.QuestionPic, 
                            pictureDirectory + newPicPath);
                        card.QuestionPic = newPicPath;
                    }
                    // If there is a Picture, which isn't in our Filesystem, 
                    // it will be copyd to it and the Card gets the new Name
                    if (card.AnswerPic != null && 
                        card.AnswerPic.Contains(@"\"))   
                    {
                        string newPicPath = 
                            ImportViewModel.RandomString() + ".jpg";
                        File.Copy(card.AnswerPic, pictureDirectory +
                            newPicPath);
                        card.AnswerPic = newPicPath;
                    }
                    /* If categorys allready contains the category of the card, 
                    * the card will be enqueued to the correspondingly 
                    * BoxViewModel
                    */
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
                        bc.Add(newBox);
                    }
                }
            }
            return bc;
        }
        
        /*
         * This Method gets a CategoryViewModel and a CardViewModel. 
         * It will load all Cards which the corresponds to 
         * the CategoryViewModel, put them in a BoxViewModel,
         * put the CardViewModel also in the BoxViewModel and calls 
         * SaveBoxToFileSystem() to Save the Cards to the FileSystem.
         */
        public static void SaveAdditionalCard(
            CategoryViewModel cat, CardViewModel card)
        {
            BoxViewModel bvm = LoadExistingCards(cat);
            bvm.Enqueue(card);
            SaveBoxToFileSystem(bvm);
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
            CategoryViewModel cat, BoxViewModel box)
        {
            BoxViewModel bvm = LoadExistingCards(cat);
            foreach(CardViewModel card in box)
            {
                bvm.Enqueue(card);
            }
            SaveBoxToFileSystem(bvm);

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
            foreach(BoxViewModel box in newBCVM)
            {
                SaveBoxToFileSystem(box);
            }
        }

        /*
         * This Method gets a CategoryViewModel and Load all the Cards out of
         * the File, which has the Name of the CategoryViewModel.
         * So it give a BoxViewModel with all the loaded Cards back.
         * If there are no Cards in the Filesystem, 
         * the Method returns an empty BoxViewModel and shows a hint.
         */
        public static BoxViewModel LoadExistingCards(CategoryViewModel cat)
        {
            BoxViewModel bvm = new BoxViewModel();
            try
            { 
                XmlDocument doc = new XmlDocument();
                // This is possible, because the name of the
                // File is the name of the category
                doc.Load(saveDirectory + cat.Name + ".xml"); 
                foreach (XmlNode node in doc.DocumentElement)
                {
                    CardViewModel card = 
                        ImportViewModel.ReadOwnFormatNode(node);
                    card.Category = cat.category;
                    bvm.Enqueue(card);
                }
            } catch
            {
                MessageBox.Show(
                    "Es konnten keine Karten aus " +
                    "dem Dateisystem geladen werden");
            }
            return bvm;
            
        }
    }
}
