using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
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
            /*
            foreach (CardViewModel card in box)
            {
                if (!categorys.Contains(card.Category))
                {
                    categorys.Add(card.Category);
                }
            }
            foreach (var item in categorys)
            {
                XmlReader xmlReader = XmlReader.Create(saveDirectory + (string)item + ".xml");
                while (xmlReader.Read())
                {

                }
                MessageBox.Show((string)item);
            }
            */
            

            
        }

        static void SaveCardsToFile(BoxCollectionViewModel bcvm)
        {

        }
    }
}
